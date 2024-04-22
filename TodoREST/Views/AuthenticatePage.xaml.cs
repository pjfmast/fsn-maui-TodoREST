using TodoREST.ViewModels;

namespace TodoREST.Views;

public partial class AuthenticatePage : ContentPage
{
	public AuthenticatePage(AuthenticateViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
	}
}