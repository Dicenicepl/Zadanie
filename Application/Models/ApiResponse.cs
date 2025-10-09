namespace Application.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]>? Errors { get; set; } = null;
        public object? Data { get; set; } = null;

        public static ApiResponse Ok(object? data = null, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static ApiResponse Fail(string message, Dictionary<string, string[]>? errors = null) =>
            new() { Success = false, Message = message, Errors = errors };
    }
}
