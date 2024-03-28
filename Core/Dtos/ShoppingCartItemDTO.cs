using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos;
public class ShoppingCartItemDTO
{
    public Guid Id { get; set; }    
    public int Quantity { get; set; }
    public ProductShoppingCartItemDTO Product { get; set; } = null!;

    [NotMapped]
    public decimal LineTotal => Quantity * Product!.DiscountedPrice;

    [NotMapped]
    public decimal BaseLineTotal => Quantity * Product!.Price;
}
