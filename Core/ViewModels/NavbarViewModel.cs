using Core.Dtos;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class NavbarViewModel
{
    public IEnumerable<KeyNameDTO> Catalogs { get; set; } = new HashSet<KeyNameDTO>();
    public int? FavoriteCount { get; set; } = null;
    public int? ShoppingCartItemCount { get; set; } = null;
}
