using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Favorite
{
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }

    public Customer? Customer { get; set; }
    public Product? Product { get; set; }
}
