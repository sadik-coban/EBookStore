using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Options;
public class EmailOptions
{
    public const string Email = "Email";
    public string Server { get; set; } = string.Empty;
    public int Port { get; set; }   
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Security {  get; set; }       
}

