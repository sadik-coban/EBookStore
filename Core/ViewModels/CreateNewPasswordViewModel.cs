using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;

public class CreateNewPasswordViewModel
{
    public Guid Id { get; set; }

    public string Token { get; set; } = string.Empty;

    [Display(Name = "Yeni Parola")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Display(Name = "Yeni Parola Tekrar")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "{0} ve {1} alanı aynı olmalıdır!")]
    public string NewPasswordCheck { get; set; } = string.Empty;
}
