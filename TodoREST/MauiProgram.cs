﻿using CommunityToolkit.Maui;
using TodoREST.Services;
using TodoREST.ViewModels;
using TodoREST.Views;

namespace TodoREST;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// register services:
		builder.Services.AddSingleton<IHttpsClientHandlerService, HttpsClientHandlerService>();
		builder.Services.AddSingleton<IRestService, RestService>();
		builder.Services.AddSingleton<ITodoService, TodoService>();

        // register views:
        builder.Services.AddSingleton<AuthenticatePage>();
        builder.Services.AddSingleton<TodoListPage>();
		builder.Services.AddTransient<TodoItemPage>();

        // register ViewModels:
        builder.Services.AddSingleton<AuthenticateViewModel>();
        builder.Services.AddSingleton<TodoListViewModel>();
        builder.Services.AddTransient<TodoItemViewModel>();

        return builder.Build();
	}
}
