using Api.Services.Interfaces;
using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
public class BookController(IBookService bookService) : ControllerBase
{

    [Route(nameof(GetAllBooks))]
    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAllBooks()
    {
        return await bookService.GetAllBooks();
        
    }

    [Route(nameof(GetBookById))]
    [HttpGet]
    public async Task<ActionResult<BookDto>> GetBookById(String id)
    {
        return await bookService.GetBookById(id);
    }
    
    [Route(nameof(CreateBook))]
    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var result = await bookService.CreateBook(createBookDto);
        return result;
    }
    
    [Route(nameof(DeleteBook))]
    [HttpDelete]
    public async Task<ActionResult> DeleteBook([FromBody] DeleteBookDto deleteBookDto)
    {
        bookService.DeleteBook(deleteBookDto);
        return Ok();
    }

    [Route(nameof(UpdateBook))]
    [HttpPut]
    public async Task<ActionResult<BookDto>> UpdateBook([FromBody] UpdateBookDto updateBookDto)
    {
        return await bookService.UpdateBook(updateBookDto);
    }
}