using Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Checkout;
public interface ICheckoutService
{
    Task<int> AddToShoppingCart(Guid productId, Guid userId, int quantity);
    Task<int> ClearShoppingCart(Guid userId);
    Task<ICollection<ShoppingCartItemDTO>> GetAllCheckoutMain(Guid userId);
    Task<int> RemoveFromCart(Guid id);
    Task<int> Payment(Guid addressId, Guid userId);
}
