using Core.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Core.Services.Abstract;
using NETCore.MailKit.Core;

namespace Core.Services;
public class EmailSender(IEmailService emailService) : IEmailSender<ApplicationUser>
{
    private string TemplateForLinks(string link)
    {
        return $"<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Document</title>\r\n</head>\r\n<body>\r\n    <a href=\"{link}\">Confirm</a>\r\n</body>\r\n</html>";
    }

    private string TemplateForCodes(string resetCode)
    {
        return $"<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Document</title>\r\n</head>\r\n<body>\r\n    {resetCode} \r\n</body>\r\n</html>";
    }

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        await emailService.SendAsync(email, $"Confirmation Email for ${user.UserName}", TemplateForLinks(confirmationLink), isHtml: true);
    }
    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        await emailService.SendAsync(email, $"Reset Code for ${user.UserName}", TemplateForCodes(resetCode), isHtml: true);
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        await emailService.SendAsync(email, $"Reset Link for ${user.UserName}", TemplateForLinks(resetLink), isHtml: true);

    }
}