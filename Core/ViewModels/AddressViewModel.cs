using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels;
public class AddressViewModel
{
    public Guid Id { get; set; }    
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
