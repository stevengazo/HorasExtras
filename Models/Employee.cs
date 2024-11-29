

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorasExtras.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? LastName { get; set; }
    public string? SecondLastName { get; set; }
    public ICollection<Extras> ExtraExtras { get; set; }

    public Employee()
    {
        
    }
    public Employee(string name, string lastName, string secondLastName)
    {
        this.EmployeeName = name;
        this.LastName = lastName;
        this.SecondLastName = secondLastName;
    }

}
