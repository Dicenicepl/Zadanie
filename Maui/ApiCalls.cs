using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Maui
{
    internal class ApiCalls
    {
        private readonly HttpClient _client;
        private string _token = string.Empty;

        public ApiCalls()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _client = new HttpClient (handler)
            { 
                BaseAddress = new Uri("http://192.168.1.83:5200/")//Emulator nie widzi localhosta serwera, dlatego trzeba podac adres lokalny hosta
            };
        }

        public async Task<string?> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            var dto = new { FirstName = firstName, LastName = lastName, Email = email, Password = password, ConfirmPassword = password };
            var response = await _client.PostAsJsonAsync("register", dto);
            if (response.IsSuccessStatusCode)
            {
                return "Success";

            }
            return HttpErrorInfo(await response.Content.ReadAsStringAsync());

        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var dto = new { Email = email, Password = password };
            var response = await _client.PostAsJsonAsync("login", dto);
            if (!response.IsSuccessStatusCode)
            {
                return HttpErrorInfo(await response.Content.ReadAsStringAsync());
            }

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            _token = result["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return string.Empty;
        }

        public async Task<Dictionary<string, string>?> GetUserAsync()
        {
            var response = await _client.GetAsync("me");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        }
        private string HttpErrorInfo(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);

                if (!doc.RootElement.TryGetProperty("errors", out var errors))
                    return "Unknown error occurred.";

                var messages = new List<string>();

                foreach (var property in errors.EnumerateObject())
                {
                    string field = property.Name;
                    string text = string.Join("\n", property.Value.EnumerateArray().Select(e => e.GetString()));
                    messages.Add($"{field}: {text}");
                }

                return string.Join("\n\n", messages);
            }
            catch (Exception ex)
            {
                return $"Unexpected error format: {ex.Message}";
            }
        }

    }
}
