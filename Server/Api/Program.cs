using api;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var appOptions = builder.Services.AddAppOptions(builder.Configuration);

builder.Services.AddDbContext<MyDbContext>(conf =>
{
    conf.UseNpgsql(appOptions.DBConnection);
} );

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(config => config
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(x => true));


//Just for testing purposes
app.MapGet("/", ([FromServices] MyDbContext dbContext) =>
{
    var myAuthor = new Author
        { Id = Guid.NewGuid().ToString(), Name = "Alex", Books = new List<Book>(), Createdat = DateTime.UtcNow };
    dbContext.Authors.Add(myAuthor);
    dbContext.SaveChanges();
    var objects = dbContext.Authors.ToList();
    return objects;
});

app.Run();
