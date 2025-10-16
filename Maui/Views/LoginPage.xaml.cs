using Maui.ViewModels;

namespace Maui.Views;

public partial class LoginPage : ContentPage
{

    public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
    
}