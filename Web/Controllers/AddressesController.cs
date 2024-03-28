using Business.Features.Addresses;
using Business.Features.Catalogs;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Authorize]
public class AddressesController(IAddressesService addressesService) : BaseController
{
    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        return View(await addressesService.GetAllCustomerAddressesMain(UserId!.Value));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AddressInputModel address)
    {
        if (ModelState.IsValid)
        {
            var result = await addressesService.CreateCustomerAddressAsync(address, UserId!.Value);
            if (result < 0)
            {
                ModelState.AddModelError("", "Bu katalog zaten oluşturulmuş");
                return View(address);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(address);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var address = await addressesService.GetCustomerAddressByIdMain(id);
        return View(new AddressInputModel { Name = address.Name, Text = address.Text });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, AddressInputModel address)
    {
        if (ModelState.IsValid)
        {
            var result = await addressesService.UpdateCustomerAddressAsync(address, id);
            if (result < 0)
            {
                ModelState.AddModelError("", "Güncellemede hata ile karşılaşıldı");
                return View(address);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(address);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        await addressesService.DeleteCustomerAddressAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
