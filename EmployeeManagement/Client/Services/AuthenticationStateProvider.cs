using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
namespace EmployeeManagement.Client.Services
{


    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly NavigationManager _navigationManager;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly ObjectStore LocalStorage;
        public JwtAuthenticationStateProvider(ObjectStore localStorage,NavigationManager navigationManager)
        {

            LocalStorage = localStorage;
            _navigationManager = navigationManager;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                await SetTokenAsync(string.Empty);
                _navigationManager.NavigateTo("/");
               return new AuthenticationState(new ClaimsPrincipal());
            }

            var claims = ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }

        public async Task LoginAsync(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            await SetTokenAsync(token);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async Task LogoutAsync()
        {
            await SetTokenAsync(string.Empty);
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }

        private async Task<string> GetTokenAsync()
        {
            return await LocalStorage.GetDataAsync<string>("Jwt");
        }

        private async Task SetTokenAsync(string token)
        {
            await LocalStorage.StoreDataAsync("Jwt", token);
            
        }

        private Claim[] ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes, _serializerOptions);
            foreach (var pair in keyValuePairs)
            {
                var value = pair.Value;
                if (value is JsonElement element)
                {
                    if (element.ValueKind == JsonValueKind.String)
                    {
                        claims.Add(new Claim(pair.Key, element.GetString()));
                    }
                    else if (element.ValueKind == JsonValueKind.True || element.ValueKind == JsonValueKind.False)
                    {
                        claims.Add(new Claim(pair.Key, element.GetBoolean().ToString()));
                    }
                }
            }

            return claims.ToArray();
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

     
    }
}
