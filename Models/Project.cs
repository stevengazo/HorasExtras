using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorasExtras.Models;

public class Project
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public string? Type { get; set; }
 public ICollection<Extras> ExtraExtras { get; set; }
    public Project(string Name, string Type)
    {
        
        this.ProjectName = Name;
        this.Type = Type;
    }
    public Project()
    {

    }
     
}
