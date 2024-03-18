using Core.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Customer : ApplicationUser
{
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    public ICollection<CustomerAddress> CustomerAddresses { get; set; } = new HashSet<CustomerAddress>();
    public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();

    [NotMapped]
    public decimal GrandTotal => ShoppingCartItems.Sum(p => p.LineTotal);
    [NotMapped]
    public decimal BaseGrandTotal => ShoppingCartItems.Sum(p => p.BaseLineTotal);
    [NotMapped]
    public decimal Earning => BaseGrandTotal - GrandTotal;

}
