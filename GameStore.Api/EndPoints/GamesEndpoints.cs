using System.Data.Common;
using GameStore.Api.Data;
using GameStore.Api.Dto;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;
namespace GameStore.Api.EndPoints;

public static class GamesEndpoints
{
    const string GetGameRoute = "GetGame";

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){
        var group = app.MapGroup("/games")
                       .WithParameterValidation();

        //game
        group.MapGet("/", async (GameStoreContext context) =>
        await context.Games
            .Include(g=>g.Genre)
                .Select(game => game.ToSummaryDto())
                    .AsNoTracking()
                        .ToListAsync()
                            );

        //game/1 
        group.MapGet("/{id}", async (int id , GameStoreContext context) => 
            {
                Game? game = await context.Games.FindAsync(id);

                return game != null ?
                    Results.Ok(game.ToDetailsDto()) : 
                    Results.NotFound();
            })
            .WithName(GetGameRoute);

        
        //ADD games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext context) => {
            
            Game game = newGame.ToEntity();        

            context.Games.Add(game);
            await context.SaveChangesAsync();
            
            return Results.CreatedAtRoute(
                    GetGameRoute,
                        new {id = game.Id},
                            game.ToDetailsDto()
                            );
        });


        //UPDATE games
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame,GameStoreContext context) => {

            var existingGame = await context.Games.FindAsync(id);

            if(existingGame is null) return Results.NotFound();

            context.Entry(existingGame)
                .CurrentValues
                    .SetValues(
                        updatedGame.ToEntity(id)
                        );

            await context.SaveChangesAsync();

            return Results.NoContent();
        });


        //DELETE games
        group.MapDelete("/{id}", async (int id,GameStoreContext context) => {
            await context.Games
                .Where(g => g.Id == id)
                    .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group;
    }
            
}
