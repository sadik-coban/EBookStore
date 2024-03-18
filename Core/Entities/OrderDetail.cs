using Core.Infrastructure.Base.EntitiesBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class OrderDetail : Entity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int DiscountRate { get; set; }

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;

    [NotMapped]
    public decimal DiscountedPrice => Price - (Price * DiscountRate / 100.0m);

    [NotMapped]
    public decimal LineTotal => DiscountedPrice * Quantity;
}
