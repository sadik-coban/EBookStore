using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class PaymentViewModel
{
    [Display(Name = "Kredi Kartı Numarası")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public string CreditCardNumber { get; set; } = string.Empty;
    [Display(Name = "CVV/CVC")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public int CVV {  get; set; }
    [Display(Name = "Ad Soyad")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public string FullName { get; set; } = string.Empty;
    [Display(Name = "Adres")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public Guid AddressId { get; set; } 
}
