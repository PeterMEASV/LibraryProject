using Api.Controllers;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services.Interfaces;

public interface IBookService
{
    Task<ActionResult<List<BookDto>>> GetAllBooks();
    Task<ActionResult<BookDto>> CreateBook(CreateBookDto createBookDto);
    Task<ActionResult<BookDto>> UpdateBook(UpdateBookDto updateBookDto);
    void DeleteBook(DeleteBookDto deleteBookDto);
    Task<ActionResult<BookDto>> GetBookById(string id);
}