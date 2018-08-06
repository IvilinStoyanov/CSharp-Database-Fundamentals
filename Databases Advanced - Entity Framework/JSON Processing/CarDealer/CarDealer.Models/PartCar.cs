using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.Models
{
    public class PartCar
    {
        public int Part_Id { get; set; }
        public Part Part { get; set; }

        public int Car_Id { get; set; }
        public Car Car { get; set; }
    }
}