using Core.Infrastructure.Base.EntitiesBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class ShoppingCartItem : Entity
{
    public Guid CustomerId { get; set; }    
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public Customer Customer { get; set; } = null!;
    public Product Product { get; set; } = null!;

    [NotMapped]
    public decimal LineTotal => Quantity * Product!.DiscountedPrice;

    [NotMapped]
    public decimal BaseLineTotal => Quantity * Product!.Price;
}
