using System.ComponentModel.DataAnnotations;
using Api.Controllers;
using Api.Services.Classes;
using Api.Services.Interfaces;
using DataAccess;


namespace tests;

public class AuthorServiceTest(IAuthorService authorService, MyDbContext dbContext, ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task GetAllAuthors_Test()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        dbContext.Authors.AddAsync(author);
        dbContext.SaveChangesAsync();

        // Act
        var result = await authorService.GetAllAuthors();
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(author.Id, actual.First().Id);
        Assert.Equal(author.Name, actual.First().Name);
        ;
    }

    [Fact]
    public async Task GetAuthorById_Test()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await authorService.GetAuthorById(author.Id);
        var actual = result.Value;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(author.Id, actual.Id);
        Assert.Equal(author.Name, actual.Name);
        ;
    }

    [Fact]
    public async Task CreateAuthor_HappyTest()
    {
        // Arrange
        var createAuthorDTO = new CreateAuthorDTO("Frank Sinatra");
        
        // Act
        var result = await authorService.CreateAuthor(createAuthorDTO);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(createAuthorDTO.Name, result.Name);
        Assert.True(result.Id.Length > 30);
        Assert.NotNull(result.Createdat);
    }

    [Fact]
    public async Task CreateAuthor_SadTest()
    {
        // Arrange
        var createAuthorDTO = new CreateAuthorDTO("");
        
        // Act & Assert?
        await Assert.ThrowsAsync<ValidationException>(async () => await authorService.CreateAuthor(createAuthorDTO));
    }
    
    [Fact]
    public async Task DeleteAuthor_HappyTest()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        // Act
        authorService.DeleteAuthor(new DeleteAuthorDTO(author.Id));
        
        // Assert
        var authorsList = authorService.GetAllAuthors().Result.Value;
        Assert.Equal(authorsList, new List<AuthorDto>());
    }
    
    [Fact]
    public async Task DeleteAuthor_SadTest()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };

        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        // Act & Assert
        Assert.Throws<Exception>(() => authorService.DeleteAuthor(new DeleteAuthorDTO("1234567890")));
    }
    
    [Fact]
    public async Task UpdateAuthor_HappyTest()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        //Act
        var updatedAuthor = new UpdateAuthorDTO(author.Id, "Frank Sinatra",[]);
        await authorService.UpdateAuthor(updatedAuthor);
        
        // Assert
        var updatedAuthorDto = await authorService.GetAuthorById(author.Id);
        Assert.Equal(updatedAuthorDto.Value.Name, updatedAuthor.Name);
    }
    
    [Fact]
    public async Task UpdateAuthor_SadTest()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test",
            Createdat = DateTime.UtcNow
        };
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        
        //Act & Assert
        var updatedAuthor = new UpdateAuthorDTO(author.Id, "", null);
        await Assert.ThrowsAsync<ValidationException>(() => authorService.UpdateAuthor(updatedAuthor));
    }   
}