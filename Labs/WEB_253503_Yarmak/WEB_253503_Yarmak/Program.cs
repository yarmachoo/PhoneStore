using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using WEB_253503_Yarmak;
using WEB_253503_Yarmak.API.Data;
using WEB_253503_Yarmak.API.HelperClasses;
using WEB_253503_Yarmak.API.Services.Authentication;
using WEB_253503_Yarmak.Authorization;
using WEB_253503_Yarmak.Extensions;
using WEB_253503_Yarmak.Services.FileService;
using WEB_253503_Yarmak.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


//builder.Services.Configure<UriData>(builder.Configuration.GetSection("UriData"));

string connection = builder.Configuration.GetConnectionString("Default");
string? UriData = configuration["UriData:ApiUri"];

builder.Services.AddHttpClient<IProductService, ApiProductService>(opt=>
        opt.BaseAddress=new Uri(UriData));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.RegisterCustomServices();
builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
opt.BaseAddress = new Uri($"{UriData}"));

builder.Services.AddScoped<ITokenAccessor, KeycloakTokenAccessor>();

var keycloakData =
    builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
        CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer()
    .AddOpenIdConnect(options =>
    {
        options.Authority =
        $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
        options.ClientId = keycloakData.ClientId;
        options.ClientSecret = keycloakData.ClientSecret;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add("openid");
        options.SaveTokens = true;
        //позволяет обращаться к локальному Keycloak по http
        options.RequireHttpsMetadata = false;
        options.MetadataAddress =
        $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
        options.CallbackPath = "/signin-oidc";
    });

builder.Services.AddScoped<IAuthService, KeucloakAuthService>();

builder.Services.AddHttpContextAccessor();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();





//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//    })
//    .AddCookie()
//    .AddOpenIdConnect(options =>
//    {
//        options.Authority = $"{keycloakData.Host}/realms/{keycloakData.Realm}";
//        options.ClientId = keycloakData.ClientId;
//        options.ClientSecret = keycloakData.ClientSecret;
//        options.ResponseType = OpenIdConnectResponseType.Code;
//        options.SaveTokens = true;
//        options.RequireHttpsMetadata = false;
//        options.GetClaimsFromUserInfoEndpoint = true;

//        options.Scope.Clear();
//        options.Scope.Add("openid");
//        options.Scope.Add("profile");
//        options.Scope.Add("email");

//        options.ClaimActions.MapJsonKey("avatar", "avatar", "string");

//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            NameClaimType = "preferred_username",
//            RoleClaimType = "roles"
//        };
//    });