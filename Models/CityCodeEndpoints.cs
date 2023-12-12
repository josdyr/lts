using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using lts.Models;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Text.RegularExpressions;

namespace lts.Models;

public static class CityCodeEndpoints
{
    public static void MapCityCodeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/CityCode").WithTags(nameof(CityCode));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.CityCodes.ToListAsync();
        })
        .WithName("GetAllCityCodes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CityCode>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.CityCodes.FindAsync(id)
                is CityCode model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetCityCodeById")
        .WithOpenApi();
    }
}
