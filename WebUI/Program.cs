using Application.DTOs.Request.Account;
using Application.Extension;
using Application.Extensions;
using Application.Interfaces;
using Application.Services;
using Application.Validators.Account;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebUI;
using WebUI.StateManagementServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<CustomHttpHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
builder.Services.AddScoped<AvailableUserState>();
builder.Services.AddScoped<SelectedUserState>();
builder.Services.AddTransient<CustomHttpHandler>();
builder.Services.AddTransient<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddTransient<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton<ReceiverChangeState>();

builder.Services.AddScoped<IAccountRepository, AccountService>();
builder.Services.AddScoped<IChatRepository, ChatService>();

builder.Services.AddHttpClient<IAccountRepository, AccountService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7201");
});

builder.Services.AddHttpClient<IChatRepository, ChatService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7201");
});

builder.Services.AddHttpClient("WebUI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7201");
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7201") });

await builder.Build().RunAsync();