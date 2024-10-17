using GameStore.Api.Data;
using GameStore.Api.EndPoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration["DefaultConnectionString"];//UserSecrets

builder.Services.AddDbContext<GameStoreContext>(
    options => options.UseSqlServer(connString));

var app = builder.Build();    

app.MapGamesEndpoints();

app.MapGenresEndpoints();

await app.DbMigrateAsync();

app.Run();

