using Application.Models;
using Application.Models.DTO;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterDto dto);
        Task<ApiResponse> LoginAsync(LoginDto dto);

        Task<UserDto> GetUserAsync(string email);

    }
}
