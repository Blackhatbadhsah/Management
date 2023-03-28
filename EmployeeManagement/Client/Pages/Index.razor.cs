using EmployeeManagement.Client.Services;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.EndPoints;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EmployeeManagement.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public ApiClientService _client { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Parameter]
        public List<RegisterModel> dataSource { get; set; } = new List<RegisterModel>();
        protected override async Task OnInitializedAsync()
        {
            dataSource = await _client.CallApiAcync<List<RegisterModel>>("GET", EndPoints.GETUSERS,null)??new();
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("InitDatatable");
            
        }
    }
}
