
using System.Windows.Input;
using Maui.Models;
using Maui.Services.Api;

namespace Maui.ViewModels
{
    public class LoginViewModel : BaseModelView
    {
        private IApiService _apiService;

        public LoginViewModel(IApiService apiService) 
        {
            _apiService = apiService;
            LoginCommand = new Command(async () => await LoginAsync());
        }

       

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set{ email = value; onPropertyChanged(); }
        }

        private string password = string.Empty;
        public string Password
        {
            get => password;
            set { password = value; onPropertyChanged(); }
        }

        public ICommand LoginCommand { get;}
        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await Shell.Current.DisplayAlert("Błąd", "Niepoprawne dane", "OK");
                return;
            }
            try
            {
                var dto = new LoginDto
                {
                    Email = email,
                    Password = password
                };

                var result = await _apiService.LoginAsync(dto);

                if (result.Success)
                {
                    await Shell.Current.GoToAsync("//MePage");
                }
                else
                {
                    string formatted = string.Join("\n", result.Errors.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}"));
                    await Shell.Current.DisplayAlert("Error", formatted, "OK");
                }

            }
            catch (Exception ex)
            {
            }
        }
    }
}
