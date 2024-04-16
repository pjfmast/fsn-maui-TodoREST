using System.Windows.Input;
using TodoREST.Models;
using TodoREST.Services;

namespace TodoREST.ViewModels;

[QueryProperty(nameof(TodoItem), "TodoItem")]
public class TodoItemViewModel : BaseViewModel
{
    public ICommand SaveTodoCommand { get; }
    public ICommand DeleteTodoCommand { get; }
    public ICommand CancelTodoCommand { get; }

    private readonly ITodoService _todoService;
    private readonly TodoListViewModel _todoListViewModel;


    public TodoItemViewModel(ITodoService todoService, TodoListViewModel todoListViewModel)
    {
        _todoService = todoService;
        _todoListViewModel = todoListViewModel;

        SaveTodoCommand = new Command(async () => await SaveTodo());
        DeleteTodoCommand = new Command(async () => await DeleteTodo());
        CancelTodoCommand = new Command(async () => await CancelTodo());
    }

    private bool _isNewItem;
    private bool IsNewItem(TodoItem todoItem)
    {
        if (string.IsNullOrWhiteSpace(todoItem.Name) && string.IsNullOrWhiteSpace(todoItem.Notes))
            return true;
        return false;
    }


    private TodoItem _todoItem;

    public TodoItem TodoItem
    {
        get => _todoItem;
        set
        {
            _isNewItem = IsNewItem(value);
            _todoItem = value;
            Title = _isNewItem ? "Add new todo item" : "Edit todo item";
            OnPropertyChanged();
        }
    }


    private async Task SaveTodo()
    {
        await _todoService.SaveTaskAsync(TodoItem, _isNewItem);
        await _todoListViewModel.RefreshTodoItems();
        await Shell.Current.GoToAsync("..");
    }

    private async Task DeleteTodo()
    {
        await _todoService.DeleteTaskAsync(TodoItem);
        await _todoListViewModel.RefreshTodoItems();
        await Shell.Current.GoToAsync("..");
    }

    async Task CancelTodo()
    {
        await Shell.Current.GoToAsync("..");
    }

}