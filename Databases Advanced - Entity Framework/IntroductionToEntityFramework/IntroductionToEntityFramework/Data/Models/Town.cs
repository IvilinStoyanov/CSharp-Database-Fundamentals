using System;
using System.Collections.Generic;

namespace P02_DatabaseFirst.Data.Models
{
    public class Town
    {
        public Town()
        {
            Address = new HashSet<Address>();
        }

        public int TownId { get; set; }
        public string Name { get; set; }

        public ICollection<Address> Address { get; set; }
    }
}
