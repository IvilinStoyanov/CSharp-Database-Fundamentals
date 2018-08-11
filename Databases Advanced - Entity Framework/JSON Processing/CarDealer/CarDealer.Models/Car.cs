﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CarDealer.Models
{
    public class Car
    {
        public Car()
        {
            this.PartCars = new HashSet<PartCar>();
        }

        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public ICollection<PartCar> PartCars { get; set; }

        public ICollection<Sale> Sales { get; set; }

        [NotMapped]
        public decimal Price => this.PartCars.Select(pc => pc.Part.Price).Sum();
    }
}

