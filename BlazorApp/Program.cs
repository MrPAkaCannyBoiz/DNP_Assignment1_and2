using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var address = new Uri("http://localhost:5122");
// TODO: refactor into scrutor for assembly scanning
builder.Services.AddScoped(sp => new HttpClient 
    { 
        BaseAddress = address
    });
builder.Services.AddScoped<ICommentService, HttpsCommentService>();
builder.Services.AddScoped<IPostService, HttpsPostService>();   
builder.Services.AddScoped<IUserService, HttpsUserService>();
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();
builder.Services.AddHttpClient<HttpsPostService>(c =>
{
    c.BaseAddress = address;
});
builder.Services.AddHttpClient<HttpsUserService>(c =>
{
    c.BaseAddress = address;
});
builder.Services.AddHttpClient<HttpsCommentService>(c =>
{
    c.BaseAddress = address;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
