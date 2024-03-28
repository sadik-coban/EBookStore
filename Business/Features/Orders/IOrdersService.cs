using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Business.Features.Orders;
public interface IOrdersService
{
    Task<IPagedList<OrderViewModel>> GetAllOrdersMain(Guid? userId = null, int pageNumber = 1, int pageSize = 10);
    Task<int> CancelOrder(Guid id);
    Task<int> UpdateStatusToOnDelivery(Guid id, string cargoTrackingNumber);
    Task<int> UpdateStatusToShipped(Guid id);
}
