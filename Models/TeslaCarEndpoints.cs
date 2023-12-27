using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Azure.Messaging.WebPubSub;
using System.Text.RegularExpressions;
using Tesla;
using Newtonsoft.Json;

namespace lts.Models;

public static class TeslaCarEndpoints
{
    public static void MapTeslaCarEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TeslaCar").WithTags(nameof(TeslaCar));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.TeslaCars.ToListAsync();
        })
        .WithName("GetAllTeslaCars")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<TeslaCar>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.TeslaCars.FindAsync(id)
                is TeslaCar model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTeslaCarById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<NotFound, NoContent>> (int id, TeslaCar car, ApplicationDbContext db) =>
        {
            var foundModel = await db.TeslaCars.FindAsync(id);

            if (foundModel is null)
            {
                return TypedResults.NotFound();
            }

            foundModel.Model = car.Model;
            foundModel.SerialNumber = car.SerialNumber;
            foundModel.Location = car.Location;
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        })
        .WithName("UpdateTeslaCar")
        .WithOpenApi();

        group.MapPost("/", async (TeslaCar car, ApplicationDbContext db) =>
        {
            string pattern = @"^(TS-|T3-|TX-|TY-|TC-)\d{5}-[A-Za-z]{2}$";
            bool isValid = Regex.IsMatch(car.SerialNumber, pattern);
            if (isValid is false)
            {
                return Results.BadRequest("Serial number is not valid. Correct format could be: TC-00001-RG");
            }
            string serializedCar = JsonConvert.SerializeObject(car);
            Console.WriteLine(serializedCar);

            db.TeslaCars.Add(car);
            await db.SaveChangesAsync();

            var connectionString = "Endpoint=https://wps-communication.webpubsub.azure.com;AccessKey=uAwDEt4WQXFXaXSEWqKIwkZLIqZNnhjNeTOjE+WHYLY=;Version=1.0;";
            var hub = "Hub";

            // Either generate the token or fetch it from server or fetch a temp one from the portal
            var serviceClient = new WebPubSubServiceClient(connectionString, hub);
            await serviceClient.SendToAllAsync(serializedCar);

            return TypedResults.Created($"/api/TeslaCar/{car.Id}", car);
        })
        .WithName("CreateTeslaCar")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok<TeslaCar>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            if (await db.TeslaCars.FindAsync(id) is TeslaCar car)
            {
                db.TeslaCars.Remove(car);
                await db.SaveChangesAsync();
                return TypedResults.Ok(car);
            }

            return TypedResults.NotFound();
        })
        .WithName("DeleteTeslaCar")
        .WithOpenApi();
    }
}
