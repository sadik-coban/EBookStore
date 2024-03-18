using Core.Infrastructure.Base.EntitiesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
public class Comment : AuditableEntity
{
    public Guid ProductId { get; set; }
    public int Rate { get; set; }
    public string Body { get; set; } = string.Empty;
    public ICollection<CommentImage>? CommentImages { get; set; } = new HashSet<CommentImage>();
    public Product Product { get; set; } = null!;
}
