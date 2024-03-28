using Business.Features.Addresses;
using Business.Features.Checkout;
using Business.Features.Products;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

[Authorize]
public class CheckoutController(ICheckoutService checkoutService, IAddressesService addressesService) : BaseController
{
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        var cart = new CheckoutViewModel
        {
            ShoppingCartItems = await checkoutService.GetAllCheckoutMain(UserId!.Value)
        };
        return View(cart);
    }

    public async Task<IActionResult> AddToShoppingCart(Guid id, int? quantity, string? returnUrl)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        await checkoutService.AddToShoppingCart(id, UserId!.Value, quantity ?? 1);
        return Redirect(returnUrl ?? "/");
    }

    public async Task<IActionResult> RemoveFromShoppingCart(Guid id)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        await checkoutService.RemoveFromCart(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ClearShoppingCart()
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        await checkoutService.ClearShoppingCart(UserId!.Value);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Payment()
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        ViewBag.Addresses = (await addressesService.GetKeyNameListAsync(UserId!.Value)).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        return View(new PaymentViewModel { });
    }

    [HttpPost]
    public async Task<IActionResult> Payment(PaymentViewModel model)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        var result = await checkoutService.Payment(model.AddressId, UserId!.Value);
        if(result < 0)
        {
            return View("PaymentFailed");
        }
        return View("PaymentSuccess");
    }
}
