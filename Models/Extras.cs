using System;
using System.ComponentModel.DataAnnotations;

namespace HorasExtras.Models;

public class Extras
{
    [Key]
    public int ExtraId { get; set; }
    public DateTime Day { get; set; }
    
}
