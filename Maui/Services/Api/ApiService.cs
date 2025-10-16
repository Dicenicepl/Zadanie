using Maui.Models;
using Maui.Services.Token;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;


namespace Maui.Services.Api
{

    public class ApiService : IApiService

    {
        private readonly HttpClient _client;
        private readonly ITokenService _tokenService;

        public ApiService(ITokenService tokenService)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.1.83:5200/")
            };
            _tokenService = tokenService;
        }
        public async Task<MeDto?> GetUserAsync()
        {
            var token = await _tokenService.GetTokenAsync();
            if (token == null) return null;

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetFromJsonAsync<ApiResponse<MeDto>>("me");
            return response?.Data;
        }

        public async Task<ApiResponse<string>> LoginAsync(LoginDto dto)
        {
            var response = await _client.PostAsJsonAsync("login", dto);
            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<JsonElement>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            string? token = null;
            if (apiResponse.Success && apiResponse.Data.TryGetProperty("token", out var tokenElement))
            {
                token = tokenElement.GetString();
                if (token != null)
                    await _tokenService.SaveTokenAsync(token);
            }

            return new ApiResponse<string>
            {
                Success = apiResponse.Success,
                Message = apiResponse.Message,
                Data = token,
                Errors = apiResponse.Errors
            };
        }

        public async Task<ApiResponse<MeDto>> RegisterAsync(RegisterDto dto)
        {
            var response = await _client.PostAsJsonAsync("register", dto);
            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<MeDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            return apiResponse;
        }

        public async Task LogOut()
        {
            await _tokenService.RemoveTokenAsync();
        }
    }
}
