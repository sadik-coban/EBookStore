using Business.Features.Comments;
using Business.Features.Orders;
using Business.Features.Products;
using Core.Entities;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.Models;
using X.PagedList;

namespace Web.Controllers;
public class HomeController(IProductsService productsService, ICommentsService commentsService, IOrdersService ordersService) : BaseController
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

    public async Task<IActionResult> Product(Guid id)
    {
        var numbers = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "1" },
            new SelectListItem { Value = "2", Text = "2" },
            new SelectListItem { Value = "3", Text = "3" },
            new SelectListItem { Value = "4", Text = "4" },
            new SelectListItem { Value = "5", Text = "5" }
        };

        // Store the list in ViewBag
        ViewBag.Rating = new SelectList(numbers, "Value", "Text");
        var product = await productsService.GetProductByIdMain(id,UserId!.Value, User.IsInRole("Administrators"));
        return View(product);
    }

    [Authorize]
    public async Task<IActionResult> AddComment(CommentViewModel model)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        await commentsService.CreateCommentAsync(model.ProductId, UserId!.Value, model.Body, model.Rating);
        return RedirectToAction(nameof(Product), new { id = model.ProductId });
    }
    [Authorize(Roles = "Administrators")]
    public async Task<IActionResult> EnableComment(Guid id,string? returnUrl)
    {
        await commentsService.EnableCommentAsync(id);
        return Redirect($"{(returnUrl ?? "/")}");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    [Authorize]
    public async Task<IActionResult> Orders()
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        var orders = await ordersService.GetAllOrdersMain(UserId!.Value);
        return View(orders);
    }
    [Authorize]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        var result = await ordersService.CancelOrder(id);
        if (result < 0)
        {
            return RedirectToAction(nameof(Orders)); //Normalde api olsa hata dönderilmesini sağlarım fakat mvc ve burada temp data ile uyarı döndermeye uğraşmak istemedim
        }
        return RedirectToAction(nameof(Orders));
    }
}
