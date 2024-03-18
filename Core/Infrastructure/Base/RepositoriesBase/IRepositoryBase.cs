using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Infrastructure.Base.RepositoriesBase;
public interface IRepositoryBase<TEntity>
{
    Task<int> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<int> ExecuteUpdateAsync(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default);
    Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<IPagedList<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        int pageNumber = 1,
        int pageSize = 10,
        bool withDeleted = false);

    Task<IPagedList<TResult>> GetListAsync<TResult>(
        Func<IQueryable<TEntity>, IQueryable<TResult>> select,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        int pageNumber = 1,
        int pageSize = 10,
        bool withDeleted = false);


    Task<IPagedList<TResult>> GetListAsync<TResult>(
        Func<TEntity, TResult> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        int pageNumber = 1,
        int pageSize = 10,
        bool withDeleted = false);

    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default);


    Task<TResult> GetAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TResult>> select,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default);

    Task<TResult> GetAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Func<TEntity, TResult> selector,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default);
}
