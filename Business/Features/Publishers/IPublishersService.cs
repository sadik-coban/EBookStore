using Core.Dtos;
using Core.Entities;
using Core.Enums;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace Business.Features.Publishers;
public interface IPublishersService
{
    Task<IPagedList<Publisher>> GetPublisherListAsync(string? keywords = null, bool withDeleted = false, bool withDisabled = true, OrderBy orderBy = OrderBy.DateDescending, int pageNumber = 1, int pageSize = 10, Func<IQueryable<Publisher>, IIncludableQueryable<Publisher, object?>>? include = null);
    Task<Publisher> GetPublisherById(Guid id, bool withDeleted = false, bool withDisabled = true);
    Task<int> CreatePublisherAsync(PublisherInputModel publisherInput, Guid userId);
    Task<int> UpdatePublisherAsync(PublisherInputModel publisherInput, Guid publisherId);
    Task<int> DeletePublisherAsync(Guid id);
    Task<ICollection<KeyNameDTO>> GetKeyNameListAsync();
}
