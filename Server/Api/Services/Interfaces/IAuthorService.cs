using Api.Controllers;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces;

public interface IAuthorService
{
    Task<Author> CreateAuthor(CreateAuthorDTO createAuthorDto);
    Task<ActionResult<List<Author>>> GetAllAuthors();
}