namespace Application.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data {  get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }   

        public static ApiResponse Ok(object? data = null, string message = "") =>
            new ApiResponse { Success = true,Data = data, Message = message};

        public static ApiResponse Fail(string message = "", Dictionary<string, string[]>? errors = null) =>
            new ApiResponse { Success = false, Message = message, Errors = errors };
    }
}
