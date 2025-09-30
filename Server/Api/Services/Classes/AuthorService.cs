using Api.Controllers;
using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Classes;

public class AuthorService(MyDbContext dbContext) : IAuthorService
{
    public async Task<Author> CreateAuthor(CreateAuthorDTO createAuthorDto)
    {
        var NewAuthor = new Author()
        {
            Id = Guid.NewGuid().ToString(),
            Name = createAuthorDto.Name,
            Books = null,
            Createdat = DateTime.UtcNow
        };
        dbContext.Authors.Add(NewAuthor);
        dbContext.SaveChanges();
        return NewAuthor;
    }

    public async Task<ActionResult<List<Author>>> GetAllAuthors()
    {
        return dbContext.Authors.ToList();
        
    }
}