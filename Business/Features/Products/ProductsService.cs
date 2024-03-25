using Core.Data;
using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Infrastructure.Base.EntitiesBase;
using Core.Infrastructure.Base.RepositoriesBase;
using Core.Services.Abstract;
using Core.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using X.PagedList;

namespace Business.Features.Products;
public class ProductsService(IRepositoryBase<Product> productsRepository, IRepositoryBase<Catalog> catalogsRepository,IRepositoryBase<Author> authorsRepository,IImageService imageService) : IProductsService
{
    public async Task<IPagedList<Product>> GetProductListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Product>, IIncludableQueryable<Product, object?>>? include = null)
    {
        Expression<Func<Product, bool>>? predicate = null;
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderByFunc;

        switch (orderBy)
        {
            case OrderBy.DateAscending:
                orderByFunc = query => query.OrderBy(p => p.DateCreated);
                break;
            case OrderBy.DateDescending:
                orderByFunc = query => query.OrderByDescending(p => p.DateCreated);
                break;
            case OrderBy.NameAscending:
                orderByFunc = query => query.OrderBy(p => p.Name);
                break;
            case OrderBy.NameDescending:
                orderByFunc = query => query.OrderByDescending(p => p.Name);
                break;
            default:
                orderByFunc = query => query.OrderByDescending(p => p.DateCreated);
                break;
        }
        if (keywords != null)
        {
            var searchKeywords = Regex.Split(keywords.ToLower(), @"\s+").ToList();
            predicate = p => searchKeywords.Any(q => p.Name.ToLower().Contains(q));
        }
        if (!withDisabled)
        {
            Expression<Func<Product, bool>>? predicateDisabled = p => p.Enabled == true;
            if (predicate == null)
            {
                predicate = predicateDisabled;
            }
            else
            {
                predicate = predicate.AndAlso(predicateDisabled);
            }
        }
        
        return await productsRepository.GetListAsync(predicate, orderByFunc, include, pageNumber, pageSize, withDeleted);
    }
    public async Task<Product> GetProductById(Guid id, bool withDeleted = false, bool withDisabled = true, Func<IQueryable<Product>, IIncludableQueryable<Product, object?>>? include = null)
    {
        return await productsRepository.GetAsync(p => p.Id == id, withDeleted: withDeleted,include: include);
    }
    public async Task<int> CreateProductAsync(ProductInputModel productInput, Guid userId)
    {

        try
        {
            string toBase64String;
            if (productInput.Image is not null)
            {
                Image image = await imageService.ResizeImageAsync(productInput.Image.OpenReadStream(), new Size(800, 600));
                toBase64String = image.ToBase64String(JpegFormat.Instance);
            }
            else
            {
                toBase64String = string.Empty;
            }

            var catalogs = await catalogsRepository.GetListAsync(predicate: p => productInput.Catalogs.Any(q => q == p.Id),orderBy:null, include: null,withDeleted: false, asNoTracking: false);
            var authors = await authorsRepository.GetListAsync(predicate: p => productInput.Authors.Any(q => q == p.Id), orderBy: null, include: null, withDeleted: false, asNoTracking: false);
            var product = new Product
            {
                Name = productInput.Name,
                Enabled = productInput.Enabled,
                DiscountRate = int.Parse(productInput.DiscountRate),
                Description = productInput.Description,
                Price = decimal.Parse(productInput.Price, CultureInfo.CreateSpecificCulture("tr-TR")),
                Catalogs = catalogs,
                Authors = authors,
                PublisherId = productInput.Publisher,
                Image = toBase64String,
                DateCreated = DateTime.UtcNow,
                UserId = userId,
            };
            // Attempt to save changes to the database
            var result = await productsRepository.CreateAsync(product);
            
            return result;
    }
        catch (DbUpdateException ex)
        {
            // Check if it's a duplicate key violation
            if (ex.InnerException is SqlException sqlException && (sqlException.Number == 2627 || sqlException.Number == 2601))
            {
                // Handle the duplicate key violation here
                // For example, log the error or inform the user
                return -1;
            }
            else
            {
                // Rethrow the exception if it's not a duplicate key violation
                throw;
            }
        }
    }
    public async Task<int> UpdateProductAsync(ProductInputModel productInput, Guid productId)
    {
        var product = await productsRepository.GetAsync(p => p.Id == productId, asNoTracking: false, include: p => p.Include(p => p.Catalogs).Include(p => p.Authors));
        var catalogs = await catalogsRepository.GetListAsync(predicate: p => productInput.Catalogs.Any(q => q == p.Id), orderBy: null, include: null, withDeleted: false, asNoTracking: false);
        var authors = await authorsRepository.GetListAsync(predicate: p => productInput.Authors.Any(q => q == p.Id), orderBy: null, include: null, withDeleted: false, asNoTracking: false);


        product.Catalogs.Clear();
        product.Authors.Clear();

        product.Catalogs = catalogs;
        product.Authors = authors;

        product.Name = productInput.Name;
        product.Enabled = productInput.Enabled;
        product.DiscountRate = int.Parse(productInput.DiscountRate);
        product.Description = productInput.Description;
        product.Price = decimal.Parse(productInput.Price, CultureInfo.CreateSpecificCulture("tr-TR"));
        product.PublisherId = productInput.Publisher;

        if(productInput.Image is not null)
        {
            Image image = await imageService.ResizeImageAsync(productInput.Image.OpenReadStream(), new Size(800, 600));
            var toBase64String = image.ToBase64String(JpegFormat.Instance);
            product.Image = toBase64String;
        }

        var result = await productsRepository.UpdateAsync(product);
        return result;
    }
    public async Task<int> DeleteProductAsync(Guid id)
    {
        var result = await productsRepository.ExecuteDeleteAsync(p => p.Id == id);
        return result;
    }
    
    public async Task<IPagedList<ProductListViewModel>> GetAllProductsMain(Guid? userId = null,string? keywords = null, bool withDeleted = false, bool withDisabled = false, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Product>, IIncludableQueryable<Product, object?>>? include = null, Expression<Func<Product, bool>>? predicate = null)
    {
        Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderByFunc;

        switch (orderBy)
        {
            case OrderBy.DateAscending:
                orderByFunc = query => query.OrderBy(p => p.DateCreated);
                break;
            case OrderBy.DateDescending:
                orderByFunc = query => query.OrderByDescending(p => p.DateCreated);
                break;
            case OrderBy.NameAscending:
                orderByFunc = query => query.OrderBy(p => p.Name);
                break;
            case OrderBy.NameDescending:
                orderByFunc = query => query.OrderByDescending(p => p.Name);
                break;
            default:
                orderByFunc = query => query.OrderByDescending(p => p.DateCreated);
                break;
        }
        if (keywords != null)
        {
            var searchKeywords = Regex.Split(keywords.ToLower(), @"\s+").ToList();
            Expression<Func<Product, bool>>?  predicateSearch = p => searchKeywords.Any(q => p.Name.ToLower().Contains(q));
            predicate =  predicate is null ? predicateSearch : predicate.AndAlso(predicateSearch);

        }
        if (!withDisabled)
        {
            Expression<Func<Product, bool>>? predicateDisabled = p => p.Enabled == true;
            if (predicate == null)
            {
                predicate = predicateDisabled;
            }
            else
            {
                predicate = predicate.AndAlso(predicateDisabled);
            }
        }

        return await productsRepository.GetListAsync(query => query.Select(p => new ProductListViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Image = p.Image,
            DiscountedPrice = p.DiscountedPrice,
            PublisherId = p.Publisher.Id,
            PublisherName = p.Publisher.Name,
            IsInFavorites = p.Favorites.Any(r => r.Customer.Id == userId),
        }),predicate, orderByFunc, include, pageNumber, pageSize, withDeleted);
    }

    public async Task<ProductDetailViewModel> GetProductByIdMain(Guid id, Guid userId)
    {
        return await productsRepository.GetAsync(p => p.Id == id, query => query.Select(p => new ProductDetailViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Image = p.Image,
            DiscountedPrice = p.DiscountedPrice,
            DiscountRate = p.DiscountRate,
            PublisherId = p.Publisher.Id,
            PublisherName = p.Publisher.Name,
            Description = p.Description,
            Catalogs = p.Catalogs.Select(q => new KeyNameDTO { Id = q.Id, Name = q.Name }),
            Authors = p.Authors.Select(q => new KeyNameDTO { Id = q.Id, Name = q.Name }),
            Comments = p.Comments.OrderByDescending(p => p.DateCreated).Where(p => p.Enabled || p.User.Id == userId).Select(q => new CommentDTO { Id = q.Id, Body = q.Body, Date = q.DateCreated, Rate = q.Rate, UserName = q.User.Name }),
            IsInFavorites = p.Favorites.Any(q => q.Customer.Id == userId)
        }), withDeleted: false, include: null);
    }
}



