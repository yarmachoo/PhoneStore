using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WEB_253503_Yarmak;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.Extensions;
using WEB_253503_Yarmak.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

string? UriData = configuration["UriData:ApiUri"];

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt=>
        opt.BaseAddress=new Uri(UriData));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.RegisterCustomServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Все, что идет дальше это формирование конвейера middleware:
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
