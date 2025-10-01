using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
public class GenreController(IGenreService genreService) : ControllerBase
{

    [Route(nameof(GetAllGenres))]
    [HttpGet]
    public async Task<ActionResult<List<GenreDTO>>> GetAllGenres()
    {
        return await genreService.GetAllGenres();
    }

    [Route(nameof(CreateGenre))]
    [HttpPost]
    public async Task<ActionResult<Genre>> CreateGenre([FromBody] CreateGenreDTO createGenreDto)
    {
        return await genreService.CreateGenre(createGenreDto);   
    }
    
    [Route(nameof(DeleteGenre))]
    [HttpDelete]
    public async Task<ActionResult> DeleteGenre([FromBody] DeleteGenreDTO deleteGenreDto)
    {
        genreService.DeleteGenre(deleteGenreDto);
        return Ok();
    }
    
    [Route(nameof(UpdateGenre))]
    [HttpPut]
    public async Task<ActionResult<GenreDTO>> UpdateGenre([FromBody]UpdateGenreDTO updateGenreDTO)
    {
        var result = await genreService.UpdateGenre(updateGenreDTO);
        return result;
        
    }
    
}