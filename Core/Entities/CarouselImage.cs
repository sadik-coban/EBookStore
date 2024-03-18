using Core.Infrastructure.Base.EntitiesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class CarouselImage : AuditableEntity
{
    public string Image { get; set; } = string.Empty;
    public DateTime? DateFirst { get; set; }
    public DateTime? DateEnd { get; set; }
}
