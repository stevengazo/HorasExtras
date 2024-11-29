

using System;
using System.ComponentModel.DataAnnotations;

namespace HorasExtras.Models;

public class Employee
{
    [Key]
    public string EmployeeId { get; set; } 
    public string EmployeeName { get; set;}
    public string LastName { get; set; }
    public string SecondLastName { get; set; }	


}
