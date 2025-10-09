namespace Maui.Views;

public partial class Login : ContentPage
{
    private readonly ApiCalls _apiCalls = new ApiCalls();

    public Login()
	{
		InitializeComponent();
	}
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string success = await _apiCalls.LoginAsync(EmailEntry.Text, PasswordEntry.Text);
        if (success == string.Empty)
        {
            var userData = await _apiCalls.GetUserAsync();
            if (userData != null)
                await Navigation.PushAsync(new Me(userData));
        }
        else
        {
            ErrorLabel.Text = success;
        }
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Register());
    }
}