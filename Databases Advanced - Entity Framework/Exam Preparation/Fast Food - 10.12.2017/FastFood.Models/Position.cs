﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class Position
    {
        // Id – integer, Primary Key
        // Name – text with min length 3 and max length 30 (required, unique)
        // Employees – Collection of type Employee

        public Position()
        {
            this.Employees = new List<Employee>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)] // TODO: make this property unique
        public string Name { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}