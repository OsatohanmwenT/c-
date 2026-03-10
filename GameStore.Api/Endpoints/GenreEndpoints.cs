using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{
    public static RouteGroupBuilder MapGenreEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext db) =>
        {
            List<GenreDto> genres = await db.Genres
                .Select(genre => new GenreDto(genre.Id, genre.Title))
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(genres);
        });

        return group;
    }
}
