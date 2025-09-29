using System.ComponentModel.DataAnnotations;

namespace Api;

public class AppOptions
{
    [MinLength(1)]
    public string DBConnection { get; set; }
    
}