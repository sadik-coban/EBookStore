using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;

public class CommentViewModel
{
    public Guid ProductId { get; set; }
    [Display(Name = "Puan")]
    public int Rating { get; set; }
    [Display(Name = "Yorum")]
    public string Body { get; set; } = string.Empty;
    public SelectList SelectList { get; set; } = null!;
}

