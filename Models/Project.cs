using System;
using System.ComponentModel.DataAnnotations;

namespace HorasExtras.Models;

public class Project
{
    [Key]
    [Required]
    public string ProjectId { get; set; } = Guid.NewGuid().ToString();
    public string? ProjectName { get; set;} 
    public string? Type { get; set;}  

    public Project(string Name, string Type )
    {
        this.ProjectId = Guid.NewGuid().ToString();
        this.ProjectName = Name;    
        this.Type = Type;
    }
    public Project()
    {
        
    }

}
