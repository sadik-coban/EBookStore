using Business.Features.Catalogs.Enums;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Business.Features.Catalogs;
public interface ICatalogsService
{
    Task<IPagedList<Catalog>> GetCatalogListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderByCatalog orderBy = OrderByCatalog.DateDescending, int pageNumber = 1, int pageSize = 10);
    Task<Catalog> GetCatalogById(Guid id, bool withDeleted = false, bool withDisabled = true);
    Task<int> CreateCatalogAsync(Catalog catalog, Guid userId);
    Task<int> UpdateCatalogAsync(Catalog catalog);
    Task<int> DeleteCatalogAsync(Guid id);
}
