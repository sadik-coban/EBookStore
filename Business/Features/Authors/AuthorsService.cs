using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Infrastructure.Base.RepositoriesBase;
using Core.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using X.PagedList;

namespace Business.Features.Authors;
public class AuthorsService(IRepositoryBase<Author> authorsRepository) : IAuthorsService
{
    public async Task<IPagedList<Author>> GetAuthorListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending,int pageNumber = 1, int pageSize = 10, Func<IQueryable<Author>, IIncludableQueryable<Author, object?>>? include = null)
    {
        Expression<Func<Author, bool>>? predicate = null;
        Func<IQueryable<Author>, IOrderedQueryable<Author>>? orderByFunc;

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
            Expression<Func<Author, bool>>? predicateDisabled = p => p.Enabled == true;
            if(predicate == null)
            {
                predicate = predicateDisabled;
            }
            else
            {
                predicate = predicate.AndAlso(predicateDisabled);
            }
        }
        return await authorsRepository.GetListAsync(predicate, orderByFunc, include, pageNumber, pageSize, withDeleted);
    }
    public async Task<Author> GetAuthorById(Guid id, bool withDeleted = false, bool withDisabled = true)
    {
        return await authorsRepository.GetAsync(p => p.Id == id, withDeleted: withDeleted);
    }
    public async Task<int> CreateAuthorAsync(AuthorInputModel catalogInput,Guid userId)
    {
        try
        {
            var catalog = new Author
            {
                Name = catalogInput.Name,
                Enabled = catalogInput.Enabled,
                DateCreated = DateTime.UtcNow,
                UserId = userId,
            };
            // Attempt to save changes to the database
            var result = await authorsRepository.CreateAsync(catalog);
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
    public async Task<int> UpdateAuthorAsync(AuthorInputModel catalogInput, Guid catalogId)
    {
        var result = await authorsRepository.ExecuteUpdateAsync(p => p.Id == catalogId, s => s.SetProperty(p => p.Name, catalogInput.Name).SetProperty(p => p.Enabled,catalogInput.Enabled));
        return result;
    }
    public async Task<int> DeleteAuthorAsync(Guid id)
    {
        var result = await authorsRepository.ExecuteDeleteAsync(p => p.Id == id);
        return result;
    }

    public async Task<ICollection<KeyNameDTO>> GetKeyNameListAsync()
    {
        return await authorsRepository.GetListAsync(query => query.Select(p => new KeyNameDTO
        {
            Id = p.Id,
            Name = p.Name,
        }), p => p.Enabled, query => query.OrderBy(p => p.Name), include: null, withDeleted: false, asNoTracking: true);
    }
}
