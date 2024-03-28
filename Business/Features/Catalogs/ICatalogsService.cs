using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace Business.Features.Catalogs;
public interface ICatalogsService
{
    Task<IPagedList<Catalog>> GetCatalogListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Catalog>, IIncludableQueryable<Catalog, object?>>? include = null);
    Task<Catalog> GetCatalogById(Guid id, bool withDeleted = false, bool withDisabled = true);
    Task<int> CreateCatalogAsync(CatalogInputModel catalogInput, Guid userId);
    Task<int> UpdateCatalogAsync(CatalogInputModel catalogInput, Guid catalogId);
    Task<int> DeleteCatalogAsync(Guid id);
    Task<ICollection<KeyNameDTO>> GetKeyNameListAsync();
    Task<string> GetNameById(Guid id);
}
