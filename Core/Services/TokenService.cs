using Core.Infrastructure.Authentication;
using Core.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services;
public class TokenService(UserManager<ApplicationUser> userManager) : ITokenService
{
    public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }
    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return token;
    }
}
