using System;
using System.ComponentModel.DataAnnotations;

namespace HorasExtras.Models;

public class Project
{
    [Key]
    public string ProjectId { get; set; }
    public string ProjectName { get; set;}  

}
