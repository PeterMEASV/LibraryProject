namespace Api.Controllers;

public record CreateBookDto(string Title, int Pages, string GenreId, List<String> AuthorIDs);

public record BookShortDto(string Id, string Title);

public record BookDto(string Id, string Title, int Pages, string Genre, DateTime? Createdat, List<AuthorShortDto> AuthorIDs);

public record UpdateBookDto(string Id, string Title, int Pages, string GenreId, List<String> AuthorIDs);

public record DeleteBookDto(string Id);