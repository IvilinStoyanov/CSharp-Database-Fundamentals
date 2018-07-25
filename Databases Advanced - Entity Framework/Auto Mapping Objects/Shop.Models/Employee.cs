using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Employee
    {
        /*
          Each employee should have properties:
          first name, last name, salary, birthday and address.
          Only first name, last name and salary are required 
        */

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? Birthday { get; set; }

        public string Address { get; set; }



    }
}
