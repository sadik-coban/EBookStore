using Business.Features.Favorites;
using Business.Features.Products;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Web.Controllers;

[Authorize(Roles = "Members")]
public class FavoritesController(IFavoritesService favoritesService) : BaseController
{
    public async Task<IActionResult> Index(int? pageNumber)
    {
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

    public async Task<IActionResult> AddToFavorites(Guid id, string? returnUrl)
    {
        await favoritesService.AddToFavorites(id, UserId!.Value);
        return Redirect($"{(returnUrl ?? "/")}");
    }

    public async Task<IActionResult> RemoveFromFavorites(Guid id, string? returnUrl)
    {
        await favoritesService.RemoveFromFavorites(id, UserId!.Value);
        return Redirect($"{(returnUrl ?? "/")}");
    }
}
