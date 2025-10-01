using Api.Controllers;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces;

public interface IAuthorService
{
    Task<Author> CreateAuthor(CreateAuthorDTO createAuthorDto);
    Task<ActionResult<List<AuthorDto>>> GetAllAuthors();
    void DeleteAuthor(DeleteAuthorDTO deleteAuthorDto);
    Task<AuthorDto> UpdateAuthor(UpdateAuthorDTO updateAuthorDto);
}