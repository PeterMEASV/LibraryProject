using System.ComponentModel.DataAnnotations;
using DataAccess;

namespace Api.Controllers;

public record CreateAuthorDTO([Length(3, 50)]string Name);

public record DeleteAuthorDTO([MinLength(30)]string Id);

public record AuthorId(string Id);

public record UpdateAuthorDTO(string Id, [Length(3, 50)]string Name, List<String> BookIds);

public record AuthorShortDto(string Id, string Name);

public record AuthorDto(string Id, string Name, DateTime? Createdat, List<BookShortDto> Books);