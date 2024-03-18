using Core.Entities;
using Core.Infrastructure.Authentication;
using Core.Services.Abstract;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Features.Account;
public class AccountService(SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager,
             IEmailSender<ApplicationUser> emailService) : IAccountService
{
    public async Task<SignInResult> Login(LoginViewModel model)
    {
        var result = await signInManager
            .PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

        return result;
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel model, ClaimsPrincipal User)
    {
        var user = await userManager.GetUserAsync(User);
        var result = await userManager.ChangePasswordAsync(user!, model.CurrentPassword, model.NewPassword);
        return result;
    }

    public async Task<IdentityResult> Register(Customer user, string url, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.GivenName, user.Name),
            };

            await userManager.AddToRoleAsync(user, "Members");
            await userManager.AddClaimsAsync(user, claims);

            await emailService.SendConfirmationLinkAsync(user,user.Email,url);
            return result;
        }
        else
        {
            return result;
        }
    }

    public async Task<IdentityResult> ConfirmEmail(Guid id, string token)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, isPersistent: false);
            return result;
        }
        return result;  
    }

    public async Task<bool> IsEmailAlreadyUsed(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        return user is not null ? true : false;
    }

    public async Task<IdentityResult> ResetPassword(ApplicationUser user, string url)
    {
        if(user is null) 
        {
            return IdentityResult.Failed(new IdentityError() { Description = "User Not Found" });
        }
        await emailService.SendPasswordResetLinkAsync(user, user.Email,url);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> CreateNewPassword(CreateNewPasswordViewModel model)
    {
        var user = await userManager.FindByIdAsync(model.Id.ToString());
        var result = await userManager.ResetPasswordAsync(user!, model.Token, model.NewPassword);
        return result;
    }

    public async Task<ApplicationUser> FindByNameAsync(string userName)
    {
        var user = await userManager.FindByNameAsync(userName);
        return user;
    }

}
