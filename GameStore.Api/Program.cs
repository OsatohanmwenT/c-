using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, ct) =>
    {
        document.Info.Title = "GameStore API";
        document.Info.Version = "v1";
        document.Info.Description = "REST API for managing games and genres in the GameStore.";
        return Task.CompletedTask;
    });
});

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGameEndpoints();
app.MapGenreEndpoints();

app.MigrateDb();

app.Run();
