using System.ComponentModel.DataAnnotations;
using Api.Controllers;
using Api.Services.Interfaces;
using DataAccess;

namespace tests;

public class GenreServiceTest(IGenreService genreService, MyDbContext dbContext, ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task GetAllGenres_Test()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await genreService.GetAllGenres();
        var actual = result.Value;

        // Assert
        Assert.Equal(actual.Count(), 1);
        Assert.Equal(actual.First().Name, genre.Name);
    }

    [Fact]
    public async Task GetGenreById_Test()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await genreService.GetGenreById(genre.Id);
        var actual = result.Value;

        // Assert
        Assert.Equal(actual.Name, genre.Name);
    }

    [Fact]
    public async Task CreateGenre_HappyTest()
    {
        // Arrange
        var genre = new CreateGenreDTO("Romance");

        // Act
        var result = await genreService.CreateGenre(genre);
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(genre.Name, actual.Name);
        Assert.True(actual.Id.Length > 30);
        Assert.NotNull(actual.Createdat);
    }

    [Fact]
    public async Task CreateGenre_SadTest()
    {
        // Arrange
        var genre = new CreateGenreDTO("");

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => genreService.CreateGenre(genre));
    }

    [Fact]
    public async Task UpdateGenre_HappyTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        await dbContext.SaveChangesAsync();
        
        // Act
        var genreUpdate = new UpdateGenreDTO(genre.Id, "Frank");
        var result = await genreService.UpdateGenre(genreUpdate);
        var actual = result.Value;
        
        // Assert
        Assert.NotNull(actual);
        Assert.Equal(genreUpdate.Name, actual.Name);
        Assert.Equal(genre.Id, actual.Id);
        Assert.Equal(genre.Createdat, actual.Createdat);
    }
    
    [Fact]
    public async Task UpdateGenre_SadTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Genres.AddAsync(genre);
        await dbContext.SaveChangesAsync();
        
        // Act & Assert
        var genreUpdate = new UpdateGenreDTO(genre.Id, "b");
        await Assert.ThrowsAsync<ValidationException>(() => genreService.UpdateGenre(genreUpdate));
    }

    [Fact]
    public void DeleteGenre_HappyTest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        dbContext.Genres.Add(genre);
        dbContext.SaveChanges();

        // Act
        genreService.DeleteGenre(new DeleteGenreDTO(genre.Id));

        // Assert
        Assert.Null(dbContext.Genres.Find(genre.Id));
        Assert.Empty(dbContext.Genres);
    }
}