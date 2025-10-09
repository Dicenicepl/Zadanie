
namespace Maui.Views;

public partial class Register : ContentPage
{
    private readonly ApiCalls _apiCalls = new ApiCalls();

    public Register()
	{
		InitializeComponent();
	}
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            ErrorLabel.Text = "Passwords do not match.";
            return;
        }

        var result = await _apiCalls.RegisterAsync(
            FirstNameEntry.Text,
            LastNameEntry.Text,
            EmailEntry.Text,
            PasswordEntry.Text
        );

        if (result == "Success")
            await Navigation.PushAsync(new Login());
        else
            ErrorLabel.Text = result;
    }
}