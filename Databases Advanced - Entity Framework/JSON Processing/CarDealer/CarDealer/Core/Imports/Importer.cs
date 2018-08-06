using CarDealer.App.Core.Contracts;
using CarDealer.Data;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarDealer.App.Core.Imports
{
    public abstract class Importer : IImporter
    {
        private CarDealerContext carDealerContext;
        private Random random;

        public Importer(CarDealerContext carDealerContext)
        {
            this.carDealerContext = carDealerContext;
            this.random = new Random();
        }

        protected CarDealerContext Context => carDealerContext;

        public abstract void Import();

        protected void SeedDatabase(Car[] cars, Customer[] customers, Part[] parts, Supplier[] supplies)
        {
            ImportSupplies(supplies);

            AssignSupplierToPart(parts, supplies);

            ImportCars(cars);

            ImportPartCars(cars);

            ImportCustomers(customers);

            ImportSales(customers);

        }

        private void ImportSales(Customer[] customers)
        {
            var discounts = new decimal[] { 0, 0.05M, 0.1M, 0.15M, 0.2M, 0.3M, 0.4M, 0.5M };
            var carsCount = this.Context.Cars.Count();
            var customersCount = this.Context.Customers.Count();

            var sales = new List<Sale>();

            for (int i = 0; i < customers.Length; i++)
            {
                var sale = new Sale
                {
                    Discount = discounts[random.Next(0, discounts.Length - 1)],
                    Car_Id = random.Next(1, carsCount),
                    Customer_Id = random.Next(1, customersCount)
                };

                sales.Add(sale);
            }

            this.Context.Sales.AddRange(sales);
            this.Context.SaveChanges();
        }

        private void ImportCustomers(Customer[] customers)
        {
            this.Context.Customers.AddRange(customers);

            this.Context.SaveChanges();
        }

        private void ImportPartCars(Car[] cars)
        {
            var partsCars = new List<PartCar>();

            var allPartsCount = this.Context.Parts.Count();

            var minPartsPerCar = 10;
            var maxPartsPerCar = 20;

            foreach (var car in cars)
            {
                var partsPerCarCount = random.Next(minPartsPerCar, maxPartsPerCar);

                var randomPartIdsPerCar = new List<int>();

                for (int i = 0; i < partsPerCarCount; i++)
                {
                    var randomPartId = random.Next(1, allPartsCount);
                    if (randomPartIdsPerCar.Contains(randomPartId))
                    {
                        i--;
                        continue;
                    }
                    randomPartIdsPerCar.Add(randomPartId);

                    var partCar = new PartCar
                    {
                        Part_Id = randomPartId,
                        Car_Id = car.Id
                    };

                    partsCars.Add(partCar);
                }
            }

            this.Context.PartsCars.AddRange(partsCars);
            this.Context.SaveChanges();
        }

        private void ImportSupplies(Supplier[] supplies)
        {
            this.Context.Supplies.AddRange(supplies);

            this.Context.SaveChanges();
        }

        private void ImportCars(Car[] cars)
        {
            this.Context.Cars.AddRange(cars);

            this.Context.SaveChanges();
        }

        private void AssignSupplierToPart(Part[] parts, Supplier[] supplies)
        {
            var supplierCount = Context.Supplies.Count();

            for (int i = 0; i < parts.Length; i++)
            {
                var supplier = random.Next(1, supplierCount);

                parts[i].Supplier_Id = supplier;
            }

            this.Context.Parts.AddRange(parts);
            this.Context.SaveChanges();
        }
    }
}
