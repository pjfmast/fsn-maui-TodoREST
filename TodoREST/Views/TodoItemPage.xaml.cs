using TodoREST.Models;
using TodoREST.ViewModels;

namespace TodoREST.Views;

[QueryProperty(nameof(TodoItem), "TodoItem")]
public partial class TodoItemPage : ContentPage
{
    public TodoItemPage(TodoItemViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}