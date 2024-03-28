using Business.Features.Favorites;
using Business.Features.Products;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Web.Controllers;
public class FavoritesController(IFavoritesService favoritesService) : BaseController
{
    [Authorize]
    public async Task<IActionResult> Index(int? pageNumber)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }
        IPagedList<ProductListViewModel> list;
        if (pageNumber == null)
        {
            list = await favoritesService.GetFavoriteProducts(userId: UserId!.Value, pageSize: 8);
        }
        else
        {
            list = await favoritesService.GetFavoriteProducts(userId: UserId!.Value,pageNumber: pageNumber!.Value, pageSize: 8);
        }
        return View(list);
    }
    [Authorize]
    public async Task<IActionResult> AddToFavorites(Guid id, string? returnUrl)
    {
        if(User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        await favoritesService.AddToFavorites(id, UserId!.Value);
        return Redirect($"{(returnUrl ?? "/")}");
    }

    [Authorize]
    public async Task<IActionResult> RemoveFromFavorites(Guid id, string? returnUrl)
    {
        if (User.IsInRole("Administrators"))
        {
            return Redirect("/Home");
        }
        await favoritesService.RemoveFromFavorites(id, UserId!.Value);
        return Redirect($"{(returnUrl ?? "/")}");
    }
}
