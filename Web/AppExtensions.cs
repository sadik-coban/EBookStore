using Business.Features.Catalogs;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;
using System;
using Core.Data;
using Core.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Web;


public static class AppExtensions
{
    public static IApplicationBuilder SeedIdentity(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        context.Database.Migrate();

        roleManager.CreateAsync(new ApplicationRole { Name = "Administrators" }).Wait();
        roleManager.CreateAsync(new ApplicationRole { Name = "ProductAdministrators" }).Wait();
        roleManager.CreateAsync(new ApplicationRole { Name = "OrderAdministrators" }).Wait();
        roleManager.CreateAsync(new ApplicationRole { Name = "Members" }).Wait();

        var user = new Manager
        {
            UserName = configuration.GetValue<string>("Security:DefaultUser:UserName"),
            Email = configuration.GetValue<string>("Security:DefaultUser:UserName"),
            Name = configuration.GetValue<string>("Security:DefaultUser:Name"),
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, configuration.GetValue<string>("Security:DefaultUser:Password")).Wait();
        userManager.AddToRoleAsync(user, "Administrators").Wait();
        var claimResult = userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, configuration.GetValue<string>("Security:DefaultUser:Name"))).Result;

        return builder;
    }
}
