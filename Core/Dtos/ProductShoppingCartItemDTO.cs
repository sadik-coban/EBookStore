using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos;
public class ProductShoppingCartItemDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Image { get; set; }
    public decimal DiscountedPrice { get; set; }
    public decimal Price { get; set; }  
}
