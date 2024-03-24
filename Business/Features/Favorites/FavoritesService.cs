using Core.Entities;
using Core.Infrastructure.Base.RepositoriesBase;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Business.Features.Favorites;
public class FavoritesService(IRepositoryBase<Favorite> favoriteRepository, IRepositoryBase<Customer> customerRepository) : IFavoritesService
{
    public async Task AddToFavorites(Guid productId, Guid userId)
    {
        //var user = await customerRepository.GetAsync(p => p.Id == userId,asNoTracking: false);
        await favoriteRepository.CreateAsync(new Favorite { ProductId = productId, CustomerId = userId });
    }

    public async Task<int> GetFavoriteCount(Guid userId)
    {
        return await favoriteRepository.CountAsync(p => p.CustomerId == userId);
    }

    public async Task<IPagedList<ProductListViewModel>> GetFavoriteProducts(Guid userId,int pageNumber = 1, int pageSize = 10)
    {
        return await favoriteRepository.GetListAsync(query => query.Select(p =>
        new ProductListViewModel
        {
            Id = p.Product.Id,
            Image = p.Product.Image,
            Name = p.Product.Name,
            Price = p.Product.DiscountedPrice,
            DiscountedPrice = p.Product.DiscountedPrice,
            PublisherId = p.Product.Publisher.Id,
            PublisherName = p.Product.Publisher.Name,
            IsInFavorites = true,
        }
        ), p => p.CustomerId == userId && p.Product.Enabled,orderBy: query => query.OrderByDescending(p => p.Product.DateCreated),include: null,withDeleted:false,asNoTracking: true, pageNumber: pageNumber, pageSize: pageSize);
    }

    public async Task RemoveFromFavorites(Guid productId, Guid userId)
    {
        await favoriteRepository.ExecuteDeleteAsync(p => p.ProductId == productId && p.CustomerId == userId);
    }

}
