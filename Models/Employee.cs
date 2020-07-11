using System.ComponentModel.DataAnnotations;

namespace PayrollApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public double Salary { get; set; }

        public bool Status { get; set; }
    }
}
