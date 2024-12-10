using GameStore.API.Data;
using GameStore.API.Dtos;
using GameStore.API.Endpoints;
using GameStore.API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddDbContext<GameStoreContext>(options =>
    options.UseMySql(connString, ServerVersion.AutoDetect(connString)));


var app = builder.Build();
app.UseHttpsRedirection();

// app.MapControllers();
app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.Run();
