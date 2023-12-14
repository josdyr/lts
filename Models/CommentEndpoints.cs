using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
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
    }
}
