using Core.Entities;
using Core.ViewModels;
using Core.Infrastructure.Base.RepositoriesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dtos;
using System.Linq.Expressions;
using X.PagedList;

namespace Business.Features.Orders;
public class OrdersService(IRepositoryBase<Order> orderRepository) : IOrdersService
{
    public async Task<IPagedList<OrderViewModel>> GetAllOrdersMain(Guid? userId = null, int pageNumber = 1, int pageSize = 10)
    {
        Expression<Func<Order, bool>>? predicate = userId == null ? null : p => p.CustomerId == userId;
        return await orderRepository.GetListAsync(query => query.Select(p => new OrderViewModel
        {
            Id = p.Id,
            Address = new AddressDTO
            {
                Name = p.CustomerAddress.Name,
                Text = p.CustomerAddress.Text,  
            },
            Status = p.Status,
            CargoTrackingNumber = p.CargoTrackingNumber,
            DateCreated = p.DateCreated,
            OrderDetails = p.OrderDetails.Select(q => new OrderDetailDTO
            {
                
                Product = new ProductShoppingCartItemDTO
                {
                    Id = q.Product.Id,
                    Name = q.Product.Name,
                    Image = q.Product.Image,
                    DiscountedPrice = q.Product.DiscountedPrice,
                    Price = q.Product.Price
                },
                Quantity = q.Quantity,
                Price = q.Price,
                DiscountRate = q.DiscountRate
            }),
        }), predicate,orderBy: query => query.OrderByDescending(p => p.DateCreated),include: null,pageNumber: pageNumber,pageSize: pageSize,withDeleted: false, asNoTracking: true);
    }
    public async Task<int> CancelOrder(Guid id)
    {
        var order = await orderRepository.GetAsync(p => p.Id == id);
        if(order.Status == Core.Enums.DeliveryStatus.New)
        {
            order.Status = Core.Enums.DeliveryStatus.Cancelled;
            var result = await orderRepository.UpdateAsync(order);
            return result;
        }
        return -1;
    }
    public async Task<int> UpdateStatusToOnDelivery(Guid id, string cargoTrackingNumber)
    {
        var result = await orderRepository.ExecuteUpdateAsync(p => p.Id == id, s => s.SetProperty(p => p.CargoTrackingNumber, cargoTrackingNumber).SetProperty(p => p.Status, Core.Enums.DeliveryStatus.OnDelivery));
        return result;
    }
    public async Task<int> UpdateStatusToShipped(Guid id)
    {
        var result = await orderRepository.ExecuteUpdateAsync(p => p.Id == id, s => s.SetProperty(p => p.Status, Core.Enums.DeliveryStatus.Shipped));
        return result;
    }
}

