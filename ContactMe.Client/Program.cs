using ContactMe.Client;
using ContactMe.Client.Services;
using ContactMe.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// add HttpClient as a service
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddScoped<ICategoryDTOService, WASMCategoryDTOService>();
builder.Services.AddScoped<IContactDTOService, WASMContactDTOService>();

await builder.Build().RunAsync();
