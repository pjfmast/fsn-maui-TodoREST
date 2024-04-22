using TodoREST.Views;

namespace TodoREST;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AuthenticatePage), typeof(AuthenticatePage));
		Routing.RegisterRoute(nameof(TodoListPage), typeof(TodoListPage));
		Routing.RegisterRoute(nameof(TodoItemPage), typeof(TodoItemPage));
	}
}
