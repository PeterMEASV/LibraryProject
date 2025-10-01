using api;
using Api;
using Api.Services.Classes;
using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adding app options
var appOptions = builder.Services.AddAppOptions(builder.Configuration);

// Adding db context
builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(appOptions.DBConnection);
} );

// Adding services
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IGenreService, GenreService>();

// Adding swagger documentation here
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Peter's Library API";
    config.Version = "0.0.1";
});

// Adding exception handler
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Adding cors stuff
builder.Services.AddCors();

var app = builder.Build();

//Controller setup
app.MapControllers();

//adding swagger documentation here
app.UseOpenApi();
app.UseSwaggerUi();


//adding the exception handler
app.UseExceptionHandler();

// allowing cors stuff
app.UseCors(config => config
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(x => true));

await app.GenerateApiClientsFromOpenApi("/../../client/src/generated-ts-client.ts");


app.Run();
