using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class ProductListViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Image { get; set; } = string.Empty;
    public decimal DiscountedPrice { get; set; }
    public Guid PublisherId { get; set; }
    public string PublisherName { get; set; } = string.Empty;
    public bool IsInFavorites { get; set; }    
}
