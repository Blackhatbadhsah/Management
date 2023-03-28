using EmployeeManagement.Client.Services;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.EndPoints;
using EmployeeManagement.Shared.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Formats.Asn1;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;

namespace EmployeeManagement.Client.Pages.Account
{
    public partial class Registration
    {
        [Parameter]
        public RegisterModel RegisterModel { get; set; } = new RegisterModel() { };
        [Parameter]
        public Department DepartmentSelectedValue { get; set; }
        [Parameter]
        public EmployeeType EmployeeTypeSelectedValue { get; set; }
        [Inject]
        public ApiClientService _client { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Parameter]
        public Roles RoleSelectedValue { get; set; }
        protected override async Task OnInitializedAsync()
        {
           await  InvokeAsync(StateHasChanged);
        }
        protected override async void OnAfterRender(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("InitDatePicker");
            base.OnAfterRender(firstRender);
        }
        public async void RegisterAsync(EventArgs e)
        {
            var Data = RegisterModel;
            Data.EmployeeType = EmployeeTypeSelectedValue;
            Data.Department = DepartmentSelectedValue;
            Data.Role = RoleSelectedValue;
            await _client.CallApiAcync<AuthResponse>("POST", EndPoints.REGISTER, Data);
            _navigationManager.NavigateTo("/home");

        }


    }
}
