using System.ComponentModel.DataAnnotations;
using Api.Controllers;
using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Classes;

public class GenreService(MyDbContext dbContext) : IGenreService
{
    public async Task<ActionResult<List<GenreDTO>>> GetAllGenres()
    {
        return await dbContext.Genres.Select(g => new GenreDTO(g.Id, g.Name, g.Createdat)).ToListAsync();
    }
    
    public async Task<ActionResult<GenreDTO>> GetGenreById(String id)
    {
        var genre = await dbContext.Genres.FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null)
        {
            throw new Exception("Genre not found");
        }
        return new GenreDTO(genre.Id, genre.Name, genre.Createdat);
    }

    public async Task<ActionResult<Genre>> CreateGenre(CreateGenreDTO createGenreDto)
    {
        if (createGenreDto.Name.Length <= 3 || createGenreDto.Name.Length >= 30)
        {
            throw new ValidationException("Genre name must be between 3 and 30 characters long");
        }
        var newGenre = new Genre
        {
            Id = Guid.NewGuid().ToString(),
            Name = createGenreDto.Name,
            Createdat = DateTime.UtcNow
        };
        dbContext.Genres.Add(newGenre);
        dbContext.SaveChanges();
        return newGenre;
    }

    public void DeleteGenre(DeleteGenreDTO deleteGenreDto)
    {
        if (dbContext.Genres.Any(g => g.Id == deleteGenreDto.Id))
        {
            var genre = dbContext.Genres.FirstOrDefault(g => g.Id == deleteGenreDto.Id);
            dbContext.Genres.Remove(genre);
            dbContext.SaveChanges();
        }
        else
        {
            throw new Exception("Genre not found");
        }
    }
    
    public async Task<ActionResult<GenreDTO>> UpdateGenre(UpdateGenreDTO updateGenreDto)
    {
        if (updateGenreDto.Name.Length <= 3 || updateGenreDto.Name.Length >= 30)
        {
            throw new ValidationException("Genre name must be between 3 and 30 characters long");
        }
        var genre = await dbContext.Genres.FirstOrDefaultAsync(g => g.Id == updateGenreDto.Id);
        if (genre == null)
        {
            throw new Exception("Genre not found");
        }
        
        genre.Name = updateGenreDto.Name;
        dbContext.Genres.Update(genre);
        await dbContext.SaveChangesAsync();
        
        return new GenreDTO(genre.Id, genre.Name, genre.Createdat);
    }
}