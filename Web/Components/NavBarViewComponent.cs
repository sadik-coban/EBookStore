using Business.Features.Catalogs;
using Business.Features.Favorites;
using Business.Features.Products;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace QuasarShop.Components
{
    public class NavBarViewComponent(
            ICatalogsService catalogsService,
            IProductsService productsService,
            IFavoritesService favoritesService
            ) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = Guid.Parse(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            return View(new NavbarViewModel
            {
                Catalogs = await catalogsService.GetKeyNameListAsync(),
                FavoriteCount = User.Identity.IsAuthenticated & User.IsInRole("Administrators") ? await favoritesService.GetFavoriteCount(userId) : 0,
                //ShoppingCartItemCount = User.Identity.IsAuthenticated ? await productsService.GetShoppingCartCount(userId) : 0
            });
        }
    }
}
