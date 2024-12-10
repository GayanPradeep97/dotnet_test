using System;
using System.Data.Common;
using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Entities;
using GameStore.API.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;


public static class GamesEndpoints

{
    const string GetGameEndPoint = "GetGames";
    private static readonly List<GameSummaryDto> games = [
    new (
        1,
        "Streey Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992,7,15)),
    new (
        2,
        "Final Fantasy XIV",
        "Roleplaying",
        59.99M,
        new DateOnly(210,9,30)),
    new (
        3,
        "FIFA 23",
        "Sports",
        69.99M,
        new DateOnly(2022,9,27)),
];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games");
        //Get /games
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.games
        .Include(game => game.Genre)
        .Select(game => game
        .ToGameSummaryDto())
        .AsNoTracking()
        .ToListAsync());

        //Get /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetGameEndPoint);



        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {

            Game game = newGame.ToEntity();
            game.Genre = await dbContext.Genres.FindAsync(newGame.GenreId);

            dbContext.games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetGameEndPoint, new { id = game.Id }, game.ToGameDetailsDto());

        }).WithParameterValidation();

        //PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
            .CurrentValues
            .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {

            await dbContext.games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;
    }

}
