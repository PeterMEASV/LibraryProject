using System.Data.Common;
using api;
using Api;
using Api.Services.Classes;
using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Program
{

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<AppOptions>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var appOptions = new AppOptions();
            configuration.GetSection(nameof(AppOptions)).Bind(appOptions);
            return appOptions;
        });

// Adding db context
        services.AddDbContext<MyDbContext>((services, options) => {options.UseNpgsql(services.GetRequiredService<AppOptions>().DBConnection); });

// Adding services
        services.AddControllers();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IGenreService, GenreService>();

// Adding swagger documentation here
        services.AddOpenApiDocument(config =>
        {
            config.Title = "Peter's Library API";
            config.Version = "0.0.1";
        });

// Adding exception handler
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

// Adding cors stuff
        services.AddCors();
    }

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services);
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
    }
}
