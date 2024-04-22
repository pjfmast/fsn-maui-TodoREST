﻿using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TodoREST.Models;

namespace TodoREST.Services;

public class RestService : IRestService
{
    readonly HttpClient _client;
    readonly JsonSerializerOptions _serializerOptions;
    readonly IHttpsClientHandlerService _httpsClientHandlerService;

    public List<TodoItem> Items { get; private set; }

    public RestService(IHttpsClientHandlerService service)
    {
#if DEBUG
        _httpsClientHandlerService = service;
        HttpMessageHandler handler = _httpsClientHandlerService.GetPlatformMessageHandler();
        if (handler != null)
            _client = new HttpClient(handler);
        else
            _client = new HttpClient();
#else
        _client = new HttpClient();
#endif
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<List<TodoItem>> RefreshDataAsync()
    {
        Items = [];

        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));
        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Items = JsonSerializer.Deserialize<List<TodoItem>>(content, _serializerOptions);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }

        return Items;
    }

    public async Task SaveTodoItemAsync(TodoItem item, bool isNewItem = false)
    {
        Uri uri = new(string.Format(Constants.RestUrl, string.Empty));

        try
        {
            var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync(Constants.SecureStorageAuthenticationKey);
            if (serializedLoginResponseInStorage is null) return;
            string token = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage).AccessToken;
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string json = JsonSerializer.Serialize(item, _serializerOptions);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            if (isNewItem)
                response = await _client.PostAsync(uri, content);
            else
                response = await _client.PutAsync(uri, content);

            if (response.IsSuccessStatusCode)
                Debug.WriteLine(@"\tTodoItem successfully saved.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }

    public async Task DeleteTodoItemAsync(string id)
    {
        Uri uri = new(string.Format(Constants.RestUrl, id));

        try
        {
            HttpResponseMessage response = await _client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
                Debug.WriteLine(@"\tTodoItem successfully deleted.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(@"\tERROR {0}", ex.Message);
        }
    }

    public async Task Register(RegisterModel model)
    {
        Uri uri = new(Constants.RegisterUrl);

        var result = await _client.PostAsJsonAsync(uri, model);
        if (result.IsSuccessStatusCode)
        {
            await Shell.Current.DisplayAlert("Alert", "sucessfully Register", "Ok");
        }
        else
        {
            await Shell.Current.DisplayAlert("Alert", result.ReasonPhrase, "Ok");
        }
    }


    public async Task Login(LoginModel model)
    {
        Uri uri = new(Constants.LoginUrl);

        var result = await _client.PostAsJsonAsync(uri, model);
        var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
        if (response is not null)
        {
            var serializeResponse = JsonSerializer.Serialize(
                new LoginResponse() { AccessToken = response.AccessToken, RefreshToken = response.RefreshToken, UserName = model.Email });
            await SecureStorage.Default.SetAsync(Constants.SecureStorageAuthenticationKey, serializeResponse);
        }
    }
}
