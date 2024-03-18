using Core.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Manager : ApplicationUser
{
    public ICollection<CarouselImage> CarouselImages { get; set; } = new HashSet<CarouselImage>();
    public ICollection<Catalog> Catalogs { get; set; } = new HashSet<Catalog>();
    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}
