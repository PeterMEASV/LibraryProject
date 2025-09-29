using System.ComponentModel.DataAnnotations;

namespace Api;

public class AppOptions
{
    [Required]
    [MinLength(1)]
    public string DBConnection { get; set; }
    
}