using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorasExtras.Models;

public class Extras
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ExtraId { get; set; }
    public DateTime Day { get; set; }
    public TimeSpan EntryHour { get; set; }
    public TimeSpan ExitHour { get; set; }
    public TimeSpan TotalDuration { get; set; }

    
    public int EmployeeId { get; set; } 
    public Employee Employee{ get; set; }

    
    public string ProjectId { get; set; }
    public Project Project{ get; set; }
}
