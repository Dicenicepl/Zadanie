
using Maui.Models;
using Maui.Services.Api;
using System.Windows.Input;

namespace Maui.ViewModels
{
    public class RegisterViewModel : BaseModelView
    {
        private IApiService _apiService;
        public RegisterViewModel(IApiService apiService)
        {
            _apiService = apiService;
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private string email = string.Empty;
        public string Email 
        {
            get => email;
            set { email = value; onPropertyChanged(); } 
        }
        private string password = string.Empty;
        public string Password
        {
            get => password;
            set { password = value; onPropertyChanged(); }
        }
        private string confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; onPropertyChanged(); }
        }
        private string firstName = string.Empty;
        public string FirstName
        {
            get => firstName;
            set { firstName = value; onPropertyChanged(); }
        }
        private string lastName = string.Empty;
        public string LastName
        {
            get => lastName;
            set { lastName = value; onPropertyChanged(); }
        }


        public ICommand RegisterCommand { get; }

        public async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Błąd", "Niepoprawne dane", "OK");
                return;
            }
            try
            {
                var dto = new RegisterDto
                {
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = ConfirmPassword,
                    FirstName = FirstName,
                    LastName = LastName
                };

                var result = await _apiService.RegisterAsync(dto);

                if (result.Success)
                {
                    await Shell.Current.DisplayAlert("Sukces", result.Message, "OK");
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    //Zamienić to aby wyciągało błędy z tablicy Errors[]
                    var errorMsg = result.Message ?? "Wystąpił błąd";
                    await Shell.Current.DisplayAlert("Błąd", errorMsg, "OK");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
