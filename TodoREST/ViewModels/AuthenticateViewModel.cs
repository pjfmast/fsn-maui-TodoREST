using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using TodoREST.Models;
using TodoREST.Services;
using TodoREST.Views;

namespace TodoREST.ViewModels;

public partial class AuthenticateViewModel : ObservableObject
{
    [ObservableProperty]
    private RegisterModel registerModel;
    [ObservableProperty]
    private LoginModel loginModel;

    [ObservableProperty]
    private string userName;
    [ObservableProperty]
    private bool isAuthenticated;

    private readonly IRestService clientService;
    public AuthenticateViewModel(IRestService clientService)
    {
        this.clientService = clientService;
        RegisterModel = new();
        LoginModel = new();
        IsAuthenticated = false;
        GetUserNameFromSecuredStorage();
    }

    [RelayCommand]
    private async Task Register()
    {
        await clientService.Register(registerModel);
    }

    [RelayCommand]
    private async Task Login()
    {
        await clientService.Login(LoginModel);
        GetUserNameFromSecuredStorage();
    }

    [RelayCommand]
    private async Task Logout()
    {
        SecureStorage.Default.Remove(Constants.SecureStorageAuthenticationKey);
        IsAuthenticated = false;
        UserName = "Guest";
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task GoToListItems()
    {
        await Shell.Current.GoToAsync(nameof(TodoListPage));
    }


    private async void GetUserNameFromSecuredStorage()
    {
        if (!string.IsNullOrEmpty(UserName) && UserName != "Guest")
        {
            IsAuthenticated = true;
            return;
        }
        var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync(Constants.SecureStorageAuthenticationKey);
        if (serializedLoginResponseInStorage != null)
        {
            UserName = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage).UserName;
            IsAuthenticated = true;
            return;
        }
        UserName = "Guest";
        IsAuthenticated = false;
    }
}