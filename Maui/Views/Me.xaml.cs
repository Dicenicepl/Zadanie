namespace Maui.Views;

public partial class Me : ContentPage
{
    public Me(Dictionary<string, string> userData)
    {
        InitializeComponent();

        UserStack.Children.Add(new Label { Text = $"Name: {userData["firstName"]} {userData["lastName"]}" });
        UserStack.Children.Add(new Label { Text = $"Email: {userData["email"]}" });
        UserStack.Children.Add(new Label { Text = $"Created at: {userData["createdDate"]}" });
    }
}