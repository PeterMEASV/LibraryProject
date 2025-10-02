using Api.Controllers;
using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Classes;

public class BookService(MyDbContext dbContext) : IBookService
{
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        var books = await dbContext.Books.Include(b => b.Authors).Include(b => b.Genre).ToListAsync();

        return books.Select(b =>
            new BookDto(
                b.Id,
                b.Title,
                b.Pages,
                b.Genre?.Name,
                b.Createdat,
                b.Authors.Select(b => new AuthorShortDto(b.Id, b.Name)).ToList()
            )
        ).ToList();
    }
    
    public async Task<ActionResult<BookDto>> GetBookById(String id)
    {
        var book = await dbContext.Books.Include(b => b.Authors).Include(b => b.Genre).FirstOrDefaultAsync(b => b.Id == id);
        
        if (book == null)
        {
            throw new Exception("Book not found");
        }
        
        return new BookDto(
            book.Id,
            book.Title,
            book.Pages,
            book.Genre?.Name,
            book.Createdat,
            book.Authors.Select(b => new AuthorShortDto(b.Id, b.Name)).ToList()
        );
    }


    public async Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto)
    {
        var authors = new List<Author>();

        foreach (var authorId in createBookDto.AuthorIDs)
        {
            var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
            if (author != null)
            {
                authors.Add(author);
            }
            else
            {
                throw new Exception("Author not found");
            }

        }

        ;

        var newBook = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = createBookDto.Title,
            Pages = createBookDto.Pages,
            Createdat = DateTime.UtcNow,
            Genre = await dbContext.Genres.FirstOrDefaultAsync(g => g.Id == createBookDto.GenreId),
            Authors = authors
        };
        dbContext.Books.Add(newBook);
        await dbContext.SaveChangesAsync();

        var returnBook = new BookDto(
            newBook.Id,
            newBook.Title,
            newBook.Pages,
            newBook.Genre.Id,
            newBook.Createdat,
            newBook.Authors.Select(b => new AuthorShortDto(b.Id, b.Name)).ToList()
        );
        return returnBook;
    }

    public async Task<ActionResult<BookDto>> UpdateBook(UpdateBookDto updateBookDto)
    {
        var authors = new List<Author>();

        foreach (var authorId in updateBookDto.AuthorIDs)
        {
            var author = await dbContext.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
            if (author != null)
            {
                authors.Add(author);
            }
            else
            {
                throw new Exception("Author not found");
            }
        }

        var book = await dbContext.Books
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(b => b.Id == updateBookDto.Id);
        
        if (book == null)
        {
            throw new Exception("Book not found");
        }

        book.Title = updateBookDto.Title;
        book.Pages = updateBookDto.Pages;
        book.Genre = await dbContext.Genres.FirstOrDefaultAsync(g => g.Id == updateBookDto.GenreId);
        book.Authors.Clear();
        
        foreach (var author in authors)
        {
            book.Authors.Add(author); 
        }


        await dbContext.SaveChangesAsync();
        return new BookDto(
            book.Id,
            book.Title,
            book.Pages,
            book.Genre.Name,
            book.Createdat,
            book.Authors.Select(b => new AuthorShortDto(b.Id, b.Name)).ToList()
        );
    }

    public void DeleteBook(DeleteBookDto deleteBookDto)
    {
        if (dbContext.Books.Any(b => b.Id == deleteBookDto.Id))
        {
            var book = dbContext.Books.FirstOrDefault(b => b.Id == deleteBookDto.Id);
            dbContext.Books.Remove(book);
            dbContext.SaveChanges();
        }
        else
        {
            throw new Exception("Book not found");
        }
    }
}