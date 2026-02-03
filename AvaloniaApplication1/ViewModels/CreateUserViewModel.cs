using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication1.ViewModels;

public partial class CreateUserViewModel:ViewModelBase
{
    
    [ObservableProperty] private User createUser;

    public CreateUserViewModel()
    {
        CreateUser = new();
    }
    
    [RelayCommand]
    public async void Create()
    {
        var _client = Http.GetHttpClient();

        var result = await _client.PostAsync("Users", new StringContent(JsonSerializer.Serialize(CreateUser), Encoding.UTF8, "application/json"));

        if (result.IsSuccessStatusCode)
        {
            CreateUser = JsonSerializer.Deserialize<User>(await result.Content.ReadAsStringAsync());
            close();
        }
    }

    private Action close;
    public void SetCloseAction(Action close)
    {
        this.close = close;
    }
}