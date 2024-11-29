using System;
using System.ComponentModel.DataAnnotations;

namespace HorasExtras.Models;

public class Extras
{
    [Key]
    [Required]
    public int ExtraId { get; set; }
    public DateTime Day { get; set; }
    
}
