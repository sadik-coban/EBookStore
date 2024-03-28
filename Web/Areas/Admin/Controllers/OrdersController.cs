using Business.Features.Catalogs;
using Business.Features.Orders;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Administrators")]
public class OrdersController(IOrdersService ordersService) : Controller
{
    public async Task<IActionResult> Index(int? pageNumber)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<OrderViewModel> list;
        if (pageNumber == null)
        {
            list = await ordersService.GetAllOrdersMain();
        }
        else
        {
            list = await ordersService.GetAllOrdersMain(pageNumber: pageNumber.Value);
        }
        return View(list);
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