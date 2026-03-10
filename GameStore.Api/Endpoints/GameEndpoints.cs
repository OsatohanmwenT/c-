using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithTags("Games");

        group.MapGet("/", async (GameStoreContext db) =>
        {
            List<GameDto> games = await db.Games.Include(game => game.Genre).Select(game => new GameDto(
                game.Id,
                game.Name,
                game.Genre!.Title,
                game.Price,
                game.ReleaseDate
            )).AsNoTracking().ToListAsync();

            return Results.Ok(games);
        })
        .WithSummary("Get all games")
        .WithDescription("Returns a list of all games including their genre.");

        group.MapGet("/{id}", async (int id, GameStoreContext db) =>
        {
            Game? game = await db.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            ));
        })
        .WithName(GetGameEndpointName)
        .WithSummary("Get a game by ID")
        .WithDescription("Returns the details of a specific game by its ID.");

        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext db) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            db.Games.Add(game);
            await db.SaveChangesAsync();

            GameDetailsDto gameDetails = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDetails.Id }, gameDetails);
        })
        .WithSummary("Create a new game")
        .WithDescription("Creates a new game entry and returns its details.");

        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext db) =>
        {
            var existingGame = await db.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.Price = updatedGame.Price;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithSummary("Update a game")
        .WithDescription("Updates the details of an existing game by its ID.");

        group.MapDelete("/{id}", async (int id, GameStoreContext db) =>
        {
            var game = await db.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }

            db.Games.Remove(game);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithSummary("Delete a game")
        .WithDescription("Deletes a game by its ID.");

        return group;
    }
}
