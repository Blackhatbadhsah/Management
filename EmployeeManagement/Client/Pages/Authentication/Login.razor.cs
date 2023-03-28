using EmployeeManagement.Client.Services;
using EmployeeManagement.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EmployeeManagement.Client.Pages.Authentication
{

    public partial class Login
    {
        [Parameter]
        public LoginModel loginModel { get; set; } = new LoginModel();
        [Inject]
        public ApiClientService _client { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        // The login method

        public async Task LoginAsync(EventArgs e)
        {
            // Send the login request to the server
            bool response = await _client.Login(loginModel);
            if (response)
            {
                _navigationManager.NavigateTo("/home");
                await JSRuntime.InvokeVoidAsync("location.reload");
            }
        }

    }
}
