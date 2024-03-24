using Core.Entities;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Business.Features.Favorites;
public interface IFavoritesService
{
    Task AddToFavorites(Guid productId, Guid userId);
    Task<int> GetFavoriteCount(Guid userId);
    Task<IPagedList<ProductListViewModel>> GetFavoriteProducts(Guid userId, int pageNumber = 1, int pageSize = 10);
    Task RemoveFromFavorites(Guid productId, Guid userId);
}
