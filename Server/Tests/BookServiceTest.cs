using System.ComponentModel.DataAnnotations;
using Api.Controllers;
using Api.Services.Interfaces;
using DataAccess;

namespace tests;

public class BookServiceTest(IBookService bookService, MyDbContext dbContext, ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task GetAllBooks_Test()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Science Fiction",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Book",
            Pages = 100,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await bookService.GetAllBooks();
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(1, actual.Count());
        Assert.Equal(book.Title, actual.First().Title);
        Assert.Equal(book.Pages, actual.First().Pages);
    }

    [Fact]
    public async Task GetBookById_Test()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Fantasy",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Test Book",
            Pages = 250,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await bookService.GetBookById(book.Id);
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(book.Id, actual.Id);
        Assert.Equal(book.Title, actual.Title);
        Assert.Equal(book.Pages, actual.Pages);
    }

    [Fact]
    public async Task CreateBook_HappyTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Mystery",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Agatha Christie",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        var createBookDto = new CreateBookDto("Murder Mystery", 350, genre.Id, new List<string> { author.Id });

        // Act
        var result = await bookService.CreateBook(createBookDto);
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(createBookDto.Title, actual.Title);
        Assert.Equal(createBookDto.Pages, actual.Pages);
        Assert.True(actual.Id.Length > 30);
        Assert.NotNull(actual.Createdat);
        Assert.Single(actual.AuthorIDs);
    }

    [Fact]
    public async Task CreateBook_SadTest_TitleTooShort()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Fiction",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        var createBookDto = new CreateBookDto("AB", 100, genre.Id, new List<string> { author.Id });

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => bookService.CreateBook(createBookDto));
    }

    [Fact]
    public async Task CreateBook_SadTest_NoAuthors()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Fiction",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        await dbContext.SaveChangesAsync();
        
        var createBookDto = new CreateBookDto("Valid Title", 100, genre.Id, new List<string>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => bookService.CreateBook(createBookDto));
    }

    [Fact]
    public async Task CreateBook_SadTest_NotEnoughPages()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Fiction",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        var createBookDto = new CreateBookDto("Valid Title", 2, genre.Id, new List<string> { author.Id });

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => bookService.CreateBook(createBookDto));
    }

    [Fact]
    public async Task UpdateBook_HappyTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Horror",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author1 = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Original Author",
            Createdat = DateTime.UtcNow
        };
        var author2 = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "New Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author1);
        await dbContext.Authors.AddAsync(author2);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Original Title",
            Pages = 100,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author1 }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act
        var updateBookDto = new UpdateBookDto(book.Id, "Updated Title", 200, genre.Id, new List<string> { author2.Id });
        var result = await bookService.UpdateBook(updateBookDto);
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(updateBookDto.Title, actual.Title);
        Assert.Equal(updateBookDto.Pages, actual.Pages);
        Assert.Equal(book.Id, actual.Id);
        Assert.Equal(book.Createdat, actual.Createdat);
        Assert.Single(actual.AuthorIDs);
    }

    [Fact]
    public async Task UpdateBook_SadTest_TitleTooShort()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Drama",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Original Title",
            Pages = 100,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        var updateBookDto = new UpdateBookDto(book.Id, "AB", 200, genre.Id, new List<string> { author.Id });
        await Assert.ThrowsAsync<ValidationException>(() => bookService.UpdateBook(updateBookDto));
    }

    [Fact]
    public async Task UpdateBook_SadTest_NoAuthors()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Comedy",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Original Title",
            Pages = 100,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        var updateBookDto = new UpdateBookDto(book.Id, "Valid Title", 200, genre.Id, new List<string>());
        await Assert.ThrowsAsync<ValidationException>(() => bookService.UpdateBook(updateBookDto));
    }

    [Fact]
    public async Task DeleteBook_HappyTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Thriller",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Book To Delete",
            Pages = 150,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act
        bookService.DeleteBook(new DeleteBookDto(book.Id));

        // Assert
        Assert.Null(dbContext.Books.Find(book.Id));
        Assert.Empty(dbContext.Books);
    }

    [Fact]
    public async Task DeleteBook_SadTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Biography",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Author",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        
        var book = new Book
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Existing Book",
            Pages = 300,
            Createdat = DateTime.UtcNow,
            Genre = genre,
            Authors = new List<Author> { author }
        };
        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        // Act & Assert
        Assert.Throws<Exception>(() => bookService.DeleteBook(new DeleteBookDto("nonexistent-id")));
    }
}