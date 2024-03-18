using Core.Data;
using Core.Infrastructure.Authentication;
using Core.Options;
using Core;
using Business;
using Web;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var emailSettings = builder.Configuration.GetSection(EmailOptions.Email).Get<EmailOptions>();

builder.Services.AddBusinessServices();
builder.Services.AddCoreServices(builder.Configuration, emailSettings);
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(config =>
{
    config.SignIn.RequireConfirmedEmail = true;
    config.User.RequireUniqueEmail = true;
    config.Password.RequireDigit = builder.Configuration.GetValue<bool>("Security:Password:RequireDigit");
    config.Password.RequiredLength = builder.Configuration.GetValue<int>("Security:Password:RequiredLength");
    config.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Security:Password:RequiredUniqueChars");
    config.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Security:Password:RequireLowercase");
    config.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Security:Password:RequireUppercase");
    config.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Security:Password:RequireNonAlphanumeric");

    config.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>("Security:Lockout:MaxFailedAccessAttempts");
    config.Lockout.DefaultLockoutTimeSpan = builder.Configuration.GetValue<TimeSpan>("Security:Lockout:DefaultLockoutTimeSpan");
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
     name: "areas",
     pattern: "{area:exists}/{controller=Dashboards}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.SeedIdentity();

app.Run();
