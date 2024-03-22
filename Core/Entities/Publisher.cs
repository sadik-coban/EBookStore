using Core.Infrastructure.Base.EntitiesBase.Abstract;
using Core.Infrastructure.Base.EntitiesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Publisher : AuditableEntity, ISoftDeleteableEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsSoftDeleted { get; set; }
    public ICollection<Product> Products = new HashSet<Product>();
}

