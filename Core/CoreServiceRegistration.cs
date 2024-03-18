using Core.Data;
using Core.Infrastructure.Authentication;
using Core.Options;
using Core.Services;
using Core.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace Core;
public static class CoreServiceRegistration
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services,IConfiguration configuration, EmailOptions emailOptions)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddMailKit(optionBuilder =>
        {
            optionBuilder.UseMailKit(new MailKitOptions()
            {
                Server = emailOptions.Server,
                Port = emailOptions.Port,
                SenderName = emailOptions.SenderName,
                SenderEmail = emailOptions.SenderEmail,

                // can be optional with no authentication 
                Account = emailOptions.Account,
                Password = emailOptions.Password,

                // enable ssl or tls
                Security = emailOptions.Security
            });
        });
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}
