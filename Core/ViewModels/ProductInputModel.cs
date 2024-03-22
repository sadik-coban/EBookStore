using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class ProductInputModel
{
    [Display(Name = "Aktif")]
    public bool Enabled { get; set; }

    [Display(Name = "Ürün Adı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [MaxLength(400, ErrorMessage = "{0} alanı en fazla {1} karakter olmalıdır!")]
    public string Name { get; set; } = string.Empty;


    [Display(Name = "Fiyat")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [RegularExpression(@"^[0-9]+(,{1}[0-9]{1,2})?$", ErrorMessage = "Lütfen geçerli bir fiyat yazınız!")]
    public string Price { get; set; } = string.Empty;


    [Display(Name = "Açıklama")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    public string Description { get; set; } = string.Empty;


    [Display(Name = "Görsel")]
    public IFormFile? Image { get; set; }

    [Display(Name = "İndirim Oranı (%)")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [RegularExpression(@"^[0-9]{1,2}$", ErrorMessage = "Lütfen geçerli bir oran yazınız!")]
    public string DiscountRate { get; set; } = string.Empty;

    [Display(Name = "Yayınevi")]
    public Guid Publisher { get; set; }
    [Display(Name = "Katalog")]
    public IEnumerable<Guid> Catalogs { get; set; } = new List<Guid>();

    [Display(Name = "Yazar")]
    public IEnumerable<Guid> Authors { get; set; } = new List<Guid>();

    [Display(Name = "Foto Galeri")]

    public string? OriginalImage { get; set; }

    public string DiscountedPrice => (decimal.Parse(Price) - (int.Parse(DiscountRate) * decimal.Parse(Price) / 100m)).ToString("f2", CultureInfo.CreateSpecificCulture("tr-TR"));

}

