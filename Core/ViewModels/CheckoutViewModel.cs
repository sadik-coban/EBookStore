using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class CheckoutViewModel
{
    public IEnumerable<ShoppingCartItemDTO> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItemDTO>(); 
    public decimal GrandTotal => ShoppingCartItems.Sum(p => p.LineTotal);
    public decimal BaseGrandTotal => ShoppingCartItems.Sum(p => p.BaseLineTotal);
    public decimal Earning => BaseGrandTotal - GrandTotal;
}

