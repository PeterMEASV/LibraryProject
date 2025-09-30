using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class AuthorController(IAuthorService authorService) : ControllerBase
{

    [Route(nameof(GetAllAuthors))]
    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAllAuthors()
    {
        var result = await authorService.GetAllAuthors();
        return result;
    }

    [Route(nameof(CreateAuthor))]
    [HttpPut]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] CreateAuthorDTO createAuthorDto)
    {
        var result = await authorService.CreateAuthor(createAuthorDto);
        return result;
    }
    
    
}