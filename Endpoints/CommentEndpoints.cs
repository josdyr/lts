using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Messaging.WebPubSub;
using Newtonsoft.Json;
using Tesla;

namespace lts.Models;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/comment").WithTags(nameof(Comment));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Comment.ToListAsync();
        })
        .WithName("GetAllComments")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Comment>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Comment.FindAsync(id)
                is Comment model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCommentById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, Comment comment, ApplicationDbContext db) =>
        {
            var foundModel = await db.Comment.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            foundModel.CarId = comment.CarId;
            foundModel.CommentDescription = comment.CommentDescription;
            foundModel.User = comment.User;
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateComment")
        .WithOpenApi();

        group.MapPost("/", async (Comment comment, ApplicationDbContext db) =>
        {
            string serializedComment = JsonConvert.SerializeObject(comment);
            Console.WriteLine(serializedComment);
            db.Comment.Add(comment);
            await db.SaveChangesAsync();

            var connectionString = Environment.GetEnvironmentVariable("AzureWebPubSubConnectionString");
            var hub = "Hub";

            // Either generate the token or fetch it from server or fetch a temp one from the portal
            var serviceClient = new WebPubSubServiceClient(connectionString, hub);
            await serviceClient.SendToAllAsync(serializedComment);

            return TypedResults.Created($"/api/comment/{comment.Id}", comment);
        })
        .WithName("CreateComment")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok<Comment>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            if (await db.Comment.FindAsync(id) is Comment comment)
            {
                db.Comment.Remove(comment);
                await db.SaveChangesAsync();
                return TypedResults.Ok(comment);
            }

            return TypedResults.NotFound();
        })
        .WithName("DeleteComment")
        .WithOpenApi();
    }
}
