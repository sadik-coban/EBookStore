using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class ProductDetailViewModel
{
    public Guid Id {  get; set; }
    public string Image { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DiscountRate { get; set; }
    public decimal DiscountedPrice { get; set; }
    public Guid PublisherId { get; set; }
    public string PublisherName {  get; set; } = string.Empty;
    public IEnumerable<KeyNameDTO> Catalogs { get; set; } = new HashSet<KeyNameDTO>();
    public IEnumerable<KeyNameDTO> Authors { get; set; } = new HashSet<KeyNameDTO>();
    public IEnumerable<CommentDTO> Comments { get; set; } = new HashSet<CommentDTO>();
    public bool IsInFavorites { get; set; }
}
