using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Author.Models
public class APIResponse
{
    public APIResponse() { }
    public bool IsSuccess { get; set; } = true;
    public string? Message { get; set; }
    public List<Error>? Errors { get; set; }
    public object? Result { get; set; }
}