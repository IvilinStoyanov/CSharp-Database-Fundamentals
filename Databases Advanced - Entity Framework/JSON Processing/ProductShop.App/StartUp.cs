namespace ProductShop.App
{
    using AutoMapper;

    using Data;
    using Models;
    using Newtonsoft.Json;
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = config.CreateMapper();

            var context = new ProductShopContext();

            var jasonStringUsers = File.ReadAllText("../../../Json/users.json");

            var deserializedUsers = JsonConvert.DeserializeObject<User[]>(jasonStringUsers);

            List<User> users = new List<User>();

            foreach (var user in deserializedUsers)
            {
                if(IsValid(user))
                {
                    users.Add(user);
                }
               
            }

            context.AddRange(users);
            context.SaveChanges();
        }

        public static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);

            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, result, true);
        }


        }
    }
}
