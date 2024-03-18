using Core.Infrastructure.Base.EntitiesBase;
using Core.Infrastructure.Base.EntitiesBase.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Order : Entity, IAuditableEntity
{
    public Guid CustomerId { get; set; }
    public DateTime DateCreated { get; set; }
    public string? CargoTrackingNumber { get; set; }

    public Customer Customer { get; set; } = null!;
    public ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();

    [NotMapped]
    public decimal GrandTotal => OrderDetails.Sum(p => p.LineTotal);
}
