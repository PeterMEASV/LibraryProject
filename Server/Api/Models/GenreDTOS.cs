using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

public record CreateGenreDTO([Length(3,30)]String Name);

public record GenreDTO(string Id, string Name, DateTime? Createdat);

public record DeleteGenreDTO([MinLength(30)]string Id);

public record UpdateGenreDTO([MinLength(30)]string Id, [Length(3,30)]String Name);