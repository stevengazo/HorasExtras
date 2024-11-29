

using System;
using System.ComponentModel.DataAnnotations;

namespace HorasExtras.Models;

public class Employee
{
    [Key]
    [Required]
    public string EmployeeId { get; set; } = Guid.NewGuid().ToString();
    public string? EmployeeName { get; set;}
    public string? LastName { get; set; }
    public string? SecondLastName { get; set; }	


}
