using EmployeeManagement.Shared;
using EmployeeManagement.Shared.EndPoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeManagement.Client.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ObjectStore _store;
        private JwtAuthenticationStateProvider _auth;
        private readonly NavigationManager _navigationManager;
        public ApiClientService(HttpClient httpClient, ObjectStore store, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _navigationManager = navigationManager;
            _store = store;
            _auth = new JwtAuthenticationStateProvider(_store, _navigationManager);

        }

        public async Task<T> CallApiAcync<T>(string Method, string Endpoint, object Content) where T : class
        {
            string token = await _store.GetDataAsync<string>("Jwt");
            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("bearer", token);
            


            // Store the data in local storage
            HttpResponseMessage message = new HttpResponseMessage();

            switch (Method)
            {
                case "POST":
                    message = await _httpClient.PostAsJsonAsync(Endpoint, Content);
                    break;
                case "GET":
                    message = await _httpClient.GetAsync(Endpoint +(Content!=null? "/"+ (string)Content:""));
                    break;
                case "PUT":
                    message = await _httpClient.PutAsJsonAsync(Endpoint, Content);
                    break;
                case "DELETE":
                    message = await _httpClient.DeleteAsync(Endpoint + (Content != null ? "/" + (string) Content : ""));
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (message.IsSuccessStatusCode)
            {
                string jsonResponse = await message.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? (T)new Object();
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Login(LoginModel Content)
        {


            HttpResponseMessage message = await _httpClient.PostAsJsonAsync(EndPoints.LOGIN, Content);
            if (message.IsSuccessStatusCode)
            {
                string jsonResponse = await message.Content.ReadAsStringAsync();
                AuthResponse token = JsonSerializer.Deserialize<AuthResponse>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
               
                await _auth.LoginAsync(token.Token);

                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task Logout()
        {
            await _auth.LogoutAsync();
        }
    }
}

