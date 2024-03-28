using Business.Features.Addresses;
using Business.Features.Checkout;
using Business.Features.Products;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers;

[Authorize(Roles = "Members")]
public class CheckoutController(ICheckoutService checkoutService, IAddressesService addressesService) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var cart = new CheckoutViewModel
        {
            ShoppingCartItems = await checkoutService.GetAllCheckoutMain(UserId!.Value)
        };
        return View(cart);
    }

    public async Task<IActionResult> AddToShoppingCart(Guid id, int? quantity, string? returnUrl)
    {
        await checkoutService.AddToShoppingCart(id, UserId!.Value, quantity ?? 1);
        return Redirect(returnUrl ?? "/");
    }

    public async Task<IActionResult> RemoveFromShoppingCart(Guid id)
    {
        await checkoutService.RemoveFromCart(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ClearShoppingCart()
    {
        await checkoutService.ClearShoppingCart(UserId!.Value);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Payment()
    {
        ViewBag.Addresses = (await addressesService.GetKeyNameListAsync(UserId!.Value)).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        return View(new PaymentViewModel { });
    }

    [HttpPost]
    public async Task<IActionResult> Payment(PaymentViewModel model)
    {
        try
        {
            var result = await checkoutService.Payment(model.AddressId, UserId!.Value);
            if (result < 0)
            {
                return View("PaymentFailed");
            }
        }
        catch
        {
            return View("PaymentFailed");
        }
        return View("PaymentSuccess");
    }
}
