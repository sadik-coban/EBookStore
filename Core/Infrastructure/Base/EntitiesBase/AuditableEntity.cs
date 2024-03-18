using Core.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Base.EntitiesBase;
public class AuditableEntity : Entity
{
    public bool Enabled { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public ApplicationUser? User { get; set; }
}
