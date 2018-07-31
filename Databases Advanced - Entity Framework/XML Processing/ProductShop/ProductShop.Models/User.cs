using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Models
{
    // Users have an id, first name (optional) and last name (at least 3 characters) and age (optional).
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product> ProductsSold { get; set; }

        public ICollection<Product> ProductsBought { get; set; }
    }
}
