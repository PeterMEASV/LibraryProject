using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

public record CreateBookDto([Length(3, 50)]string Title, [Range(3, 5000)]int Pages, string GenreId, [MinLength(1)]List<String> AuthorIDs);

public record BookShortDto(string Id, string Title);

public record BookDto(string Id, string Title, int Pages, string Genre, DateTime? Createdat, List<AuthorShortDto> AuthorIDs);

public record UpdateBookDto(string Id, [Length(3, 50)]string Title, [Range(3, 5000)]int Pages, string GenreId, [MinLength(1)]List<String> AuthorIDs);

public record DeleteBookDto(string Id);