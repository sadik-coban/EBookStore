using Business.Features.Account;
using Core.Entities;
using Core.Services;
using Core.Services.Abstract;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NETCore.MailKit.Core;
using NuGet.Common;
using System.Security.Claims;

namespace Web.Controllers;
public class AccountController(IAccountService accountService, ITokenService tokenService) : BaseController
{
    public IActionResult AccessDenied()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View(new LoginViewModel { RememberMe = true });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var result = await accountService.Login(model);
        if (result.Succeeded)
        {
            return Redirect(model.ReturnUrl ?? "/");
        }
        else
        {
            if (result.IsLockedOut)
                ModelState.AddModelError("", "Çok fazla hatalı deneme lütfen bekleyiniz!");
            if (result.IsNotAllowed)
                ModelState.AddModelError("", "E-Posta doğrulanmadığı için giriş işlemini yapılamıyor");

            ModelState.AddModelError("", "Geçersiz kullanıcı girişi");
            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await accountService.Logout();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        await accountService.ChangePassword(model,User);
        return RedirectToAction("Index", "Home", new { area = "" });
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var user = new Customer
        {
            UserName = model.UserName,
            Name = model.Name,
            Email = model.UserName
        };
        var token = await tokenService.GenerateEmailConfirmationTokenAsync(user);

        var url = Url.Action(nameof(ConfirmEmail), "Account", new { id = user.Id, token }, Request.Scheme);

        var result = await accountService.Register(user, url, model.Password);
        if(result.Succeeded)
        {
            return View("RegisterSuccess");
        }
        else
        {
            result.Errors.ToList().ForEach(e => ModelState.AddModelError("", e.Description));
            return View(model);
        }
    }
    public async Task<IActionResult> ConfirmEmail(Guid id, string token)
    {
        var result = await accountService.ConfirmEmail(id, token);
        if (result.Succeeded)
            return RedirectToAction("Index", "Home");
        else
            return View("EmailConfirmFailed");
    }

    public async Task<IActionResult> VerifyEmail(string userName)
    {
        var isUsed = await accountService.IsEmailAlreadyUsed(userName);
        return Json(isUsed ? $"{userName} zaten kullanımdadır" : "true");
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var user = await accountService.FindByNameAsync(model.UserName);
        var token = await tokenService.GeneratePasswordResetTokenAsync(user);
        var url = Url.Action(nameof(CreateNewPassword), "Account", new { id = user.Id, token }, Request.Scheme);
        await accountService.ResetPassword(user, url);
        return View("ResetPasswordSuccess");
    }

    public IActionResult CreateNewPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewPassword(CreateNewPasswordViewModel model)
    {
        await accountService.CreateNewPassword(model);
        return RedirectToAction(nameof(Login));
    }

}

