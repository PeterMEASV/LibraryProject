using Api.Controllers;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces;

public interface IGenreService
{
    Task<ActionResult<List<GenreDTO>>> GetAllGenres();
    Task<ActionResult<Genre>> CreateGenre(CreateGenreDTO createGenreDto);
    void DeleteGenre(DeleteGenreDTO deleteGenreDto);
    Task<ActionResult<GenreDTO>> UpdateGenre(UpdateGenreDTO updateGenreDto);
    Task<ActionResult<GenreDTO>> GetGenreById(string id);
}