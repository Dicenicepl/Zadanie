
namespace Maui.Services.Token
{
    public class TokenService : ITokenService
    {
        private const string TokenKey = "bearer_token";
        public Task<string?> GetTokenAsync()
        {
            return SecureStorage.Default.GetAsync(TokenKey);
        }

        public Task RemoveTokenAsync()
        {
            SecureStorage.Default.Remove(TokenKey);
            return Task.CompletedTask;
        }

        public Task SaveTokenAsync(string token)
        {
            return SecureStorage.Default.SetAsync(TokenKey, token);
        }
    }
}
