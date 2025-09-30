using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

public record CreateAuthorDTO([Length(1, 50)]string Name);

public record DeleteAuthorDTO(string Id);

public record UpdateAuthorDTO(string Id, [Length(1, 50)]string Name, List<string> Books);