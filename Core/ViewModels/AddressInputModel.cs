using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class AddressInputModel
{
    [Display(Name = "Adres Adı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public string Name { get; set; } = string.Empty;
    [Display(Name = "Tam Adres")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public string Text { get; set; } = string.Empty;
}
