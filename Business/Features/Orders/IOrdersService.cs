using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Orders;
public interface IOrdersService
{
    Task<ICollection<OrderViewModel>> GetAllOrdersMain(Guid? userId = null);
    Task<int> CancelOrder(Guid id);
    Task<int> UpdateStatusToOnDelivery(Guid id, string cargoTrackingNumber);
    Task<int> UpdateStatusToShipped(Guid id);
}
