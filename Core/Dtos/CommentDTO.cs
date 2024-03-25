using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos;
public class CommentDTO
{
    public Guid Id { get; set; }    
    public int Rate { get; set; }   
    public string Body { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string UserName {  get; set; } = string.Empty;
}
