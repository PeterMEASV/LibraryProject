using Api.Controllers;
using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Classes;

public class AuthorService(MyDbContext dbContext) : IAuthorService
{
    public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
    {
        var authors = await dbContext.Authors.Include(a => a.Books).ToListAsync();

        return authors.Select(a => 
            new AuthorDto(
                a.Id,
                a.Name,
                a.Createdat,
                a.Books.Select(b => new BookShortDto(b.Id, b.Title)).ToList()
            )
        ).ToList();

    }


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
    
    public void DeleteAuthor(DeleteAuthorDTO deleteAuthorDto)
    {
        if (dbContext.Authors.Any(a => a.Id == deleteAuthorDto.Id))
        {
            var author = dbContext.Authors.FirstOrDefault(a => a.Id == deleteAuthorDto.Id);
            dbContext.Authors.Remove(author);
            dbContext.SaveChanges();
        }
        else
        {
            throw new Exception("Author not found");
        }
    }

    public async Task<AuthorDto> UpdateAuthor(UpdateAuthorDTO updateAuthorDto)
    {
        var author = await dbContext.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == updateAuthorDto.Id);

        if (author == null)
            throw new Exception("Author not found");

        
        author.Name = updateAuthorDto.Name;
        
        author.Books.Clear();
        foreach (var bookId in updateAuthorDto.BookIds)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book != null)
            {
                author.Books.Add(book);
            }
            else
            {
                throw new Exception("Book not found");
            }
            
        }

        await dbContext.SaveChangesAsync();
        return new AuthorDto(
            author.Id,
            author.Name,
            author.Createdat,
            author.Books.Select(b => new BookShortDto(b.Id, b.Title)).ToList()
        );
    }
    
    public async Task<ActionResult<AuthorDto>> GetAuthorById(String id)
    {
        var author = await dbContext.Authors
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (author == null)
        {
            throw new Exception("Author not found");
        }
        return new AuthorDto(
            author.Id,
            author.Name,
            author.Createdat,
            author.Books.Select(b => new BookShortDto(b.Id, b.Title)).ToList()
        );
    }

    
    
}