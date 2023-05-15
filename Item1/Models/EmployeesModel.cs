using System.ComponentModel.DataAnnotations;

namespace Item1.Models
{
    public class Employees
    {
        [Key]
        public int EmployeeID {get; set;}
        [Required]
        public string FirstName {get; set;}
        [Required]
        public string LastName {get; set;}
        [Required]
        public int Age {get; set;}
    }
}