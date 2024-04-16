using System.Collections.ObjectModel;
using System.Windows.Input;
using TodoREST.Models;
using TodoREST.Services;

namespace TodoREST.ViewModels;

public class TodoListViewModel : BaseViewModel
{
    private readonly ITodoService _todoService;

    public ObservableCollection<TodoItem> Items { get; set; } = [];
    public ICommand NewCommand { get; }

    public ICommand SelectTodoComment { get; }

    public TodoListViewModel(ITodoService service)
    {
        _todoService = service;

        Title = "List of todo items";
        NewCommand = new Command(async () => await AddItem());
        SelectTodoComment = new Command<TodoItem>(async (item) => await SelectionChanged(item));
        _ = RefreshTodoItems();
    }

    public async Task RefreshTodoItems()
    {
        Items.Clear();
        var tasks = await _todoService.GetTasksAsync();
        tasks.ForEach(Items.Add);
    }

    private async Task AddItem()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(TodoItem), new TodoItem { ID = Guid.NewGuid().ToString() } }
        };
        await Shell.Current.GoToAsync(nameof(Views.TodoItemPage), navigationParameter);
    }

    private async Task SelectionChanged(TodoItem todoItem)
    {
        if (todoItem == null) return;

        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(TodoItem), todoItem }
        };
        await Shell.Current.GoToAsync(nameof(Views.TodoItemPage), navigationParameter);
    }
}