using Maui.Models;


namespace Maui.Services.Api
{
    public interface IApiService
    {
        Task<ApiResponse<string>> LoginAsync(LoginDto dto);
        Task<ApiResponse<MeDto>> RegisterAsync(RegisterDto dto);
        Task<MeDto?> GetUserAsync();
        Task LogOut();
    }
}
