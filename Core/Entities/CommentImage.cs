using Core.Infrastructure.Base.EntitiesBase;

namespace Core.Entities;
public class CommentImage : Entity
{
    public Guid CommentId { get; set; }
    public string Image { get; set; } = string.Empty;
    public Comment Comment { get; set; } = null!;
}
