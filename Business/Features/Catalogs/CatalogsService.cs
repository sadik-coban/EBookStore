using Business.Features.Catalogs.Enums;
using Core.Data;
using Core.Entities;
using Core.Extensions;
using Core.Infrastructure.Base.RepositoriesBase;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Business.Features.Catalogs;
public class CatalogsService(IRepositoryBase<Catalog> catalogsRepository) : ICatalogsService
{
    public async Task<IPagedList<Catalog>> GetCatalogListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderByCatalog orderBy = OrderByCatalog.DateDescending,int pageNumber = 1, int pageSize = 10)
    {
        Expression<Func<Catalog, bool>>? predicate = null;
        Func<IQueryable<Catalog>, IOrderedQueryable<Catalog>>? orderByFunc;

        switch (orderBy)
        {
            case OrderByCatalog.DateAscending:
                orderByFunc = query => query.OrderBy(p => p.DateCreated);
                break;
            case OrderByCatalog.DateDescending:
                orderByFunc = query => query.OrderByDescending(p => p.DateCreated);
                break;
            case OrderByCatalog.NameAscending:
                orderByFunc = query => query.OrderBy(p => p.Name);
                break;
            case OrderByCatalog.NameDescending:
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
        if(!withDisabled)
        {
            Expression<Func<Catalog, bool>>? predicateDisabled = p => p.Enabled == true;
            if(predicate == null)
            {
                predicate = predicateDisabled;
            }
            else
            {
                predicate = predicate.AndAlso(predicateDisabled);
            }
        }
        return await catalogsRepository.GetListAsync(predicate, orderByFunc, q => q.Include(p => p.User), pageNumber, pageSize, withDeleted);
    }
    public async Task<Catalog> GetCatalogById(Guid id, bool withDeleted = false, bool withDisabled = true)
    {
        return await catalogsRepository.GetAsync(p => p.Id == id, withDeleted: withDeleted);
    }
    public async Task<int> CreateCatalogAsync(Catalog catalog,Guid userId)
    {
        try
        {
            catalog.DateCreated = DateTime.UtcNow;
            catalog.UserId = userId;
            // Attempt to save changes to the database
            var result = await catalogsRepository.CreateAsync(catalog);
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
    public async Task<int> UpdateCatalogAsync(Catalog catalog)
    {
        var result = await catalogsRepository.ExecuteUpdateAsync(p => p.Id == catalog.Id,s => s.SetProperty(p => p.Name, catalog.Name));
        return result;
    }
    public async Task<int> DeleteCatalogAsync(Guid id)
    {
        var result = await catalogsRepository.ExecuteDeleteAsync(p => p.Id == id);
        return result;
    }
}
