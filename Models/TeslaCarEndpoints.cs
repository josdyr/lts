using Microsoft.AspNetCore.Http.HttpResults;
namespace lts.Models;

public static class TeslaCarEndpoints
{
    public static void MapTeslaCarEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TeslaCar");

        group.MapGet("/", () =>
        {
            return new[] { new TeslaCar() };
        })
        .WithName("GetAllTeslaCars");

        group.MapGet("/{id}", (int id) =>
        {
            return new TeslaCar { Id = id };
        })
        .WithName("GetTeslaCarById");

        group.MapPut("/{id}", (int id, TeslaCar input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateTeslaCar");

        group.MapPost("/", (TeslaCar model) =>
        {
            return TypedResults.Created($"/api/TeslaCars/{model.Id}", model);
        })
        .WithName("CreateTeslaCar");

        group.MapDelete("/{id}", (int id) =>
        {
            return TypedResults.Ok(new TeslaCar { Id = id });
        })
        .WithName("DeleteTeslaCar");
    }
}
