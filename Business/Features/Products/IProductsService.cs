using Core.Entities;
using Core.Enums;
using Core.Services;
using Core.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Business.Features.Products;
public interface IProductsService
{
    Task<IPagedList<Product>> GetProductListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Product>, IIncludableQueryable<Product, object?>>? include = null);
    Task<Product> GetProductById(Guid id, bool withDeleted = false, bool withDisabled = true, Func<IQueryable<Product>, IIncludableQueryable<Product, object?>>? include = null);
    Task<int> CreateProductAsync(ProductInputModel productInput, Guid userId);
    Task<int> UpdateProductAsync(ProductInputModel productInput, Guid productId);
    Task<int> DeleteProductAsync(Guid id);

    //Optimised
    Task<IPagedList<ProductListViewModel>> GetAllProductsMain(Guid? userId = null ,string? keywords = null, bool withDeleted = false, bool withDisabled = false, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Product>, IIncludableQueryable<Product, object?>>? include = null);
}
