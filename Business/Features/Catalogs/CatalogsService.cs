using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Infrastructure.Base.RepositoriesBase;
using Core.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using X.PagedList;
using Core.Dtos;
using Microsoft.EntityFrameworkCore.Query;

namespace Business.Features.Catalogs;
public class CatalogsService(IRepositoryBase<Catalog> catalogsRepository) : ICatalogsService
{
    public async Task<IPagedList<Catalog>> GetCatalogListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending,int pageNumber = 1, int pageSize = 10, Func<IQueryable<Catalog>, IIncludableQueryable<Catalog, object?>>? include = null)
    {
        Expression<Func<Catalog, bool>>? predicate = null;
        Func<IQueryable<Catalog>, IOrderedQueryable<Catalog>>? orderByFunc;

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
        return await catalogsRepository.GetListAsync(predicate, orderByFunc, include, pageNumber, pageSize, withDeleted);
    }
    public async Task<Catalog> GetCatalogById(Guid id, bool withDeleted = false, bool withDisabled = true)
    {
        return await catalogsRepository.GetAsync(p => p.Id == id, withDeleted: withDeleted);
    }
    public async Task<int> CreateCatalogAsync(CatalogInputModel catalogInput,Guid userId)
    {
        try
        {
            var catalog = new Catalog
            {
                Name = catalogInput.Name,
                Enabled = catalogInput.Enabled,
                DateCreated = DateTime.UtcNow,
                UserId = userId,
            };
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
    public async Task<int> UpdateCatalogAsync(CatalogInputModel catalogInput, Guid catalogId)
    {
        var result = await catalogsRepository.ExecuteUpdateAsync(p => p.Id == catalogId, s => s.SetProperty(p => p.Name, catalogInput.Name).SetProperty(p => p.Enabled,catalogInput.Enabled));
        return result;
    }
    public async Task<int> DeleteCatalogAsync(Guid id)
    {
        var result = await catalogsRepository.ExecuteDeleteAsync(p => p.Id == id);
        return result;
    }

    public async Task<ICollection<KeyNameDTO>> GetKeyNameListAsync()
    {
        return await catalogsRepository.GetListAsync(query => query.Select(p=> new KeyNameDTO
        {
            Id = p.Id,
            Name = p.Name,
        }),p => p.Enabled, query => query.OrderBy(p => p.Name), include: null, withDeleted: false, asNoTracking: true);
    }

    public async Task<string> GetNameById(Guid id)
    {
        return await catalogsRepository.GetAsync(p => p.Id == id, query => query.Select(p => p.Name), withDeleted:false);
    }
}
