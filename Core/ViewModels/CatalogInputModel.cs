using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class CatalogInputModel
{
    [Display(Name = "Aktif")]
    public bool Enabled { get; set; }

    [Display(Name = "Katalog Adı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [MaxLength(400, ErrorMessage = "{0} alanı en fazla {1} karakter olmalıdır!")]
    public string Name { get; set; } = string.Empty;
}
