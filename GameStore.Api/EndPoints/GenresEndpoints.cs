using System;
using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.EndPoints;

public static class GenresEndpoints
{
     public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app){
        var group = app.MapGroup("/genres")
                       .WithParameterValidation();

        group.MapGet("/", async (GameStoreContext context) =>
            await context.Genres
                .Select(genre => genre.ToDto())
                    .AsNoTracking()
                        .ToListAsync()
                );
                
    return group;    
    }
}
