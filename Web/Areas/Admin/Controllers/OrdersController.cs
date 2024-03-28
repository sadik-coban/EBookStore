using Business.Features.Orders;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class OrdersController(IOrdersService ordersService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var orders = await ordersService.GetAllOrdersMain();
        return View(orders);
    }

    public IActionResult UpdateStatusToOnDelivery()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> UpdateStatusToOnDelivery(OrderInputModel model,Guid id)
    {
        var result = await ordersService.UpdateStatusToOnDelivery(id, model.CargoTrackingNumber);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> UpdateStatusToShipped(Guid id)
    {
        var result = await ordersService.UpdateStatusToShipped(id);
        return RedirectToAction(nameof(Index));
    }
}