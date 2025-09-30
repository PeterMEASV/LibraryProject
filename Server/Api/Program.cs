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
builder.Services.AddScoped<IAuthorService, AuthorService>();

// Adding swagger documentation here
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();

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



app.Run();
