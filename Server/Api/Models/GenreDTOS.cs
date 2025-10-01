namespace Api.Controllers;

public record CreateGenreDTO(String Name);

public record GenreDTO(string Id, string Name, DateTime? Createdat);

public record DeleteGenreDTO(string Id);

public record UpdateGenreDTO(string Id, String Name);