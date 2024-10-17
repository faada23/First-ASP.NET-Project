using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static async Task DbMigrateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        await dataContext.Database.MigrateAsync();
    }
}
 