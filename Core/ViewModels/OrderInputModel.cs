using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class OrderInputModel
{
    [Display(Name = "Kargo Takip Numarası")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public string CargoTrackingNumber { get; set; } = string.Empty;
}
