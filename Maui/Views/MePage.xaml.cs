using Maui.ViewModels;

namespace Maui.Views;

public partial class MePage : ContentPage
{
    public MePage(MeViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as MeViewModel;
        if (vm != null)
            await vm.LoadUserAsync();
    }
}