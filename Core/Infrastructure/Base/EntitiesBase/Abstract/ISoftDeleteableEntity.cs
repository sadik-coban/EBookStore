using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Base.EntitiesBase.Abstract;
public interface ISoftDeleteableEntity
{
    public bool IsSoftDeleted { get; set; }
}
