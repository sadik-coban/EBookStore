using Core.Dtos;
using Core.Entities;
using Core.Infrastructure.Base.RepositoriesBase;

namespace Business.Features.Checkout;
public class CheckoutService(IRepositoryBase<ShoppingCartItem> shoppingCartRepository, IRepositoryBase<Order> orderRepository) : ICheckoutService
{
    public async Task<int> AddToShoppingCart(Guid productId, Guid userId, int quantity)
    {
        ShoppingCartItem item;
        try {
            item = await shoppingCartRepository.GetAsync(p => p.CustomerId == userId && p.ProductId == productId, include: null, false, asNoTracking: true);
        }
        catch(ArgumentNullException)
        {
            item = new ShoppingCartItem { ProductId = productId, CustomerId = userId, Quantity = quantity };
            return await shoppingCartRepository.CreateAsync(item);
        }
        item.Quantity += quantity;
        if (item.Quantity < 1)
        {
            item.Quantity = 1;
        }
        return await shoppingCartRepository.UpdateAsync(item);
    }

    public async Task<int> ClearShoppingCart(Guid userId)
    {
        var result = await shoppingCartRepository.ExecuteDeleteAsync(p => p.CustomerId == userId);
        return result;
    }

    public async Task<ICollection<ShoppingCartItemDTO>> GetAllCheckoutMain(Guid userId)
    {
        return await shoppingCartRepository.GetListAsync(query => query.Select(p => new ShoppingCartItemDTO
        {
            Id = p.Id,  
            Quantity = p.Quantity,
            Product = new ProductShoppingCartItemDTO
            {
                Id = p.Product.Id,
                Name = p.Product.Name,
                Image = p.Product.Image,
                DiscountedPrice = p.Product.DiscountedPrice,
                Price = p.Product.Price
            }
        }), p => p.CustomerId == userId,orderBy: null, include: null, withDeleted: false, asNoTracking: true);
    }

    public async Task<int> RemoveFromCart(Guid id)
    {
        var result = await shoppingCartRepository.ExecuteDeleteAsync(p => p.Id == id);
        return result;
    }

    public async Task<int> Payment(Guid addressId, Guid userId)
    {
        var count = await shoppingCartRepository.CountAsync(p => p.CustomerId == userId);
        if (count == 0)
        {
            return -1;
        }
        var orderDetails = new List<OrderDetail>();
        var shoppingCart = await shoppingCartRepository.GetListAsync(query => query.Select(p => new
        {
            ProductId = p.Product.Id,
            Quantity = p.Quantity,
            Price = p.Product.Price,
            DiscountRate = p.Product.DiscountRate
        }), p => p.CustomerId == userId, orderBy: null, include: null, withDeleted: false, asNoTracking: true);
        foreach (var shoppingCartItem in shoppingCart)
        {
            orderDetails.Add(new OrderDetail { ProductId = shoppingCartItem.ProductId, Quantity = shoppingCartItem.Quantity, Price = shoppingCartItem.Price, DiscountRate = shoppingCartItem.DiscountRate });

        }
        var order = new Order
        {
            CustomerId = userId,
            CustomerAddressId = addressId,
            DateCreated = DateTime.UtcNow,
            OrderDetails = orderDetails
        };
        await shoppingCartRepository.ExecuteDeleteAsync(p => p.CustomerId == userId);
        return await orderRepository.CreateAsync(order);
    }
}
