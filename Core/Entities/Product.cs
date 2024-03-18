
using Core.Infrastructure.Base.EntitiesBase;
using Core.Infrastructure.Base.EntitiesBase.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;
public class Product : AuditableEntity, ISoftDeleteableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsSoftDeleted { get; set; }
    public string Image { get; set; } = string.Empty;

    public ICollection<Catalog> Catalogs { get; set; } = new HashSet<Catalog>();
    public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
    public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();

    [NotMapped]
    public decimal DiscountedPrice { get; set; }
}
