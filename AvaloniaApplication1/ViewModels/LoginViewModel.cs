using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Utils;
using AvaloniaApplication1.VmTools;

namespace AvaloniaApplication1.ViewModels;

public class LoginViewModel : BaseVM
{
    private HttpClient _client;
    private string _baseUri;
    private LoginData _loginData;
    
    private bool _isCheck = false;

    public LoginData LoginData
    {
        get => _loginData;
        set
        {
            if (Equals(value, _loginData)) return;
            _loginData = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginButton { get; set; }
    public LoginViewModel()
    {
        _baseUri = Http.GetHttpClient().BaseAddress.ToString();
        _client = new HttpClient();

        LoginButton = new CommandVM(() =>
        {
            CheckLogin();
        });
    }

    private async void CheckLogin()
    {
        if (string.IsNullOrWhiteSpace(LoginData?.Username) || 
            string.IsNullOrWhiteSpace(LoginData?.Password))
        {
            Console.WriteLine("Login or password is empty");
            _isCheck = false;
            return;
        }

        try
        {
            var url = $"{_baseUri}/auth/login";
            
            var json = JsonSerializer.Serialize(LoginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                _isCheck = true;
                Console.WriteLine("Login success");
                
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {responseBody}");
            }
            else
            {
                _isCheck = false;
                Console.WriteLine($"Login failed. Status: {response.StatusCode}");
                
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {error}");
            }
        }
        catch (Exception ex)
        {
            _isCheck = false;
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}