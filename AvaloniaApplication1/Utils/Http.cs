using System;
using System.Net.Http;
using System.Net.Http.Headers;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Utils;

public class Http
{
    static HttpClient client;
    public static HttpClient GetHttpClient()
    {
        if (client == null)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7081/");
        }

        if (ActiveUser.Token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActiveUser.Token);
        }

        return client;
    }
}