using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class AuthorController(IAuthorService authorService) : ControllerBase
{

    [Route(nameof(GetAllAuthors))]
    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
    {
        return await authorService.GetAllAuthors();
        
    }

    [Route(nameof(CreateAuthor))]
    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] CreateAuthorDTO createAuthorDto)
    {
        var result = await authorService.CreateAuthor(createAuthorDto);
        return result;
    }

    [Route(nameof(DeleteAuthor))]
    [HttpDelete]
    public async Task<ActionResult> DeleteAuthor([FromBody] DeleteAuthorDTO deleteAuthorDto)
    {
        authorService.DeleteAuthor(deleteAuthorDto);
        return Ok();
    }

    [Route(nameof(UpdateAuthor))]
    [HttpPut]
    public async Task<ActionResult<AuthorDto>> UpdateAuthor([FromBody] UpdateAuthorDTO updateAuthorDto)
    {
        var result = await authorService.UpdateAuthor(updateAuthorDto);
        return result;

    }
    
    
}