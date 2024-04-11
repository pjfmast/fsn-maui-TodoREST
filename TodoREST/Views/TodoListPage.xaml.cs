using TodoREST.ViewModels;

namespace TodoREST.Views;

public partial class TodoListPage : ContentPage
{
    public TodoListPage(TodoListViewModel todoListViewModel)
    {
        InitializeComponent();
        BindingContext = todoListViewModel;
    }
}
