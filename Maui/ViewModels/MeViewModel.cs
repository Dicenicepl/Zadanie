
using System.Windows.Input;
using Maui.Services.Api;
using Maui.Models;


namespace Maui.ViewModels
{
    public class MeViewModel : BaseModelView
    {
        private readonly IApiService _apiService;

        public MeViewModel(IApiService apiService)
        {
            _apiService = apiService;

            RefreshCommand = new Command(async () => await LoadUserAsync());

            LogOutCommand = new Command(async () => await LogOutAsync());
        }

        private MeDto? _user;
        public MeDto? User
        {
            get => _user;
            set { _user = value; onPropertyChanged(); }
        }

        public ICommand RefreshCommand { get; }

        public async Task LoadUserAsync()
        {
            try
            {
                var user = await _apiService.GetUserAsync();
                if (user != null)
                {
                    User = user;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Błąd", "Nie udało się pobrać danych", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Błąd", ex.Message, "OK");
            }
        }
        public ICommand LogOutCommand { get; }
        public async Task LogOutAsync()
        {
            await _apiService.LogOut();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
