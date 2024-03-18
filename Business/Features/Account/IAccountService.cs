using Core.Entities;
using Core.Infrastructure.Authentication;
using Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Business.Features.Account;
public interface IAccountService
{
    Task<SignInResult> Login(LoginViewModel model);
    Task Logout();
    Task<IdentityResult> ChangePassword(ChangePasswordViewModel model, ClaimsPrincipal User);
    Task<IdentityResult> Register(Customer user, string url, string password);
    Task<IdentityResult> ConfirmEmail(Guid id, string token);
    Task<bool> IsEmailAlreadyUsed(string userName);
    Task<IdentityResult> ResetPassword(ApplicationUser user, string url);
    Task<IdentityResult> CreateNewPassword(CreateNewPasswordViewModel model);
    Task<ApplicationUser> FindByNameAsync(string userName);
}
