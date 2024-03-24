using Business.Features.Products;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;
using X.PagedList;

namespace Web.Controllers;
public class HomeController(IProductsService productsService/*, ILogger<HomeController> _logger*/) : BaseController
{
    public async Task<IActionResult> Index(int? pageNumber, string? keywords)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<ProductListViewModel> list;
        if (pageNumber == null)
        {
            list = await productsService.GetAllProductsMain(userId: UserId,keywords:keywords, pageSize: 8);
        }
        else
        {
            list = await productsService.GetAllProductsMain(userId: UserId,keywords:keywords, pageNumber: pageNumber!.Value, pageSize: 8);
        }
        return View(list);
    }

    public async Task<IActionResult> Catalog(Guid id, int? pageNumber)
    {
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<ProductListViewModel> list;
        if (pageNumber == null)
        {
            list = await productsService.GetAllProductsMain(userId: UserId, pageSize: 8, predicate: p => p.Catalogs.Any(q => q.Id == id));
        }
        else
        {
            list = await productsService.GetAllProductsMain(userId: UserId, pageNumber: pageNumber!.Value, pageSize: 8, predicate: p => p.Catalogs.Any(q => q.Id == id));
        }
        return View(list);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
