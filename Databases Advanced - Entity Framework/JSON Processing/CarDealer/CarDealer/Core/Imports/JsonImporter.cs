using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarDealer.App.Core.Imports
{
    public class JsonImporter : Importer
    {
        private const string CarJsonFilePath = "../../../JsonFiles/cars.json";
        private const string CustomerJsonFilePath = "../../../JsonFiles/customers.json";
        private const string PartsJsonFilePath = "../../../JsonFiles/parts.json";
        private const string SuppliesJsonFilePath = "../../../JsonFiles/suppliers.json";

        public JsonImporter(CarDealerContext context)
            : base(context)
        {
        }

        public override void Import()
        {
            var cars = Deserialize<Car>(CarJsonFilePath);
            var customers = Deserialize<Customer>(CustomerJsonFilePath);
            var parts = Deserialize<Part>(PartsJsonFilePath);
            var supplies = Deserialize<Supplier>(SuppliesJsonFilePath);

            base.SeedDatabase(cars, customers, parts, supplies);
        }

        protected TModel[] Deserialize<TModel>(string usersJsonFilePath)
        {
            var jsonString = File.ReadAllText(usersJsonFilePath);
            var collection = JsonConvert.DeserializeObject<IEnumerable<TModel>>(jsonString)
                .ToArray();

            return collection;
        }
    }
}
