using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Base.EntitiesBase.Abstract;
public interface IAuditableEntity
{
    DateTime DateCreated { get; }
}
