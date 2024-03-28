using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Dtos;
public class OrderDetailDTO
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int DiscountRate { get; set; }

    public ProductShoppingCartItemDTO Product { get; set; } = null!;

    [NotMapped]
    public decimal DiscountedPrice => Price - (Price * DiscountRate / 100.0m);

    [NotMapped]
    public decimal LineTotal => DiscountedPrice * Quantity;
}
