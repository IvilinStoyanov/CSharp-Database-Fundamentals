﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetClinic.Models
{
   public class Vet
    {
        public Vet()
        {
            this.Procedures = new List<Procedure>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Profession  { get; set; }

        [Required]
        [Range(22, 65)]
        public int Age { get; set; }

        [Required]
        [RegularExpression("^([+359|0])+([0-9]{9})$")]
        public string PhoneNumber { get; set; }

        public ICollection<Procedure> Procedures  { get; set; }
    }
}
