using Core.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Base.EntitiesBase;
public abstract class Entity
{
    public Guid Id { get; set; }
}
