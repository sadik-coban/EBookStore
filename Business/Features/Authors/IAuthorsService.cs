using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace Business.Features.Authors;
public interface IAuthorsService
{
    Task<IPagedList<Author>> GetAuthorListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Author>, IIncludableQueryable<Author, object?>>? include = null);
    Task<Author> GetAuthorById(Guid id, bool withDeleted = false, bool withDisabled = true);
    Task<int> CreateAuthorAsync(AuthorInputModel catalogInput, Guid userId);
    Task<int> UpdateAuthorAsync(AuthorInputModel catalogInput, Guid catalogId);
    Task<int> DeleteAuthorAsync(Guid id);
    Task<ICollection<KeyNameDTO>> GetKeyNameListAsync();
}
