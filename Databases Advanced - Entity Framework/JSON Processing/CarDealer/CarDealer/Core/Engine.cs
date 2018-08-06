using CarDealer.App.Core.Contracts;
using CarDealer.App.Core.Dtos;
using CarDealer.App.Core.Export;
using CarDealer.App.Core.Imports;
using CarDealer.Data;
using Microsoft.EntityFrameworkCore;
using System;

using System.Linq;

namespace CarDealer.App.Core
{
    public class Engine : IEngine
    {
        private const string OrderedCustomersExportFilePathJson
           = "../../../ExportedJsonFiles/ordered-customers.json";

        private const string ToyotaCarsExportFilePathJson
           = "../../../ExportedJsonFiles/toyota-cars.json";

        private const string LocalSupplierExportFilePathJson
           = "../../../ExportedJsonFiles/local-suppliers.json";

        private const string GetCarPartsExportFilePathJson
          = "../../../ExportedJsonFiles/cars-and-parts.json";

        private const string TotalSalesByCustomerExportFilePathJson
          = "../../../ExportedJsonFiles/customers-total-sales.json";

        private const string SalesWithAppliedDiscountExportFilePathJson
         = "../../../ExportedJsonFiles/sales-discounts.json";


        private readonly CarDealerContext context;
        private readonly JsonExporter jsonExporter;

        public Engine()
        {
            context = new CarDealerContext();
            jsonExporter = new JsonExporter();
        }

        public void Run()
        {
            ResetDatabase(new JsonImporter(new CarDealerContext()));

            //Query 1
            GetOrderedCustomers();
            //Query 2
            GetCarsMadeFromToyota();
            //Query 3
            GetLocalSuppliers();
            //Query 4
            GetCarParts();
            //Query 5
            GetTotalSalesByCustomer();
            //Query 6
            GetSalesWithAppliedDiscount();

        }

        private void GetSalesWithAppliedDiscount()
        {
            var saleDiscountsDTOs = this.context.Sales
                .Select(s => new SalesDiscountDto
                {
                    Car = new CarDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars
                        .Select(pc => pc.Part.Price * (1 + s.Discount))
                        .DefaultIfEmpty(0)
                        .Sum(),
                    PriceWithDiscount = s.Car.PartCars
                        .Select(pc => pc.Part.Price)
                        .DefaultIfEmpty(0)
                        .Sum(),
                })
                .ToArray();

            this.jsonExporter
                .Export(SalesWithAppliedDiscountExportFilePathJson, saleDiscountsDTOs);
        }

        private void GetTotalSalesByCustomer()
        {
            var customerTotalSalesDTOs = this.context.Customers
              .Where(c => c.Sales.Count > 0)
              .Select(c => new CustomerTotalSalesDto
              {
                  Name = c.Name,
                  BoughtCars = c.Sales.Count,
                  SpentMoney = Math.Round(
                      c.Sales
                      .Select(s => s.Car.PartCars.Sum(cp => cp.Part.Price) * (1 + s.Discount))
                      .DefaultIfEmpty(0)
                      .Sum(), 2)
              })
              .OrderByDescending(c => c.SpentMoney)
              .ThenByDescending(c => c.BoughtCars)
              .ToArray();

            this.jsonExporter
                .Export(TotalSalesByCustomerExportFilePathJson, customerTotalSalesDTOs);
        }

        private void GetCarParts()
        {
            var carPartsDTOs = this.context.Cars
               .Select(c => new
               {
                   car = new CarPartsDTO
                   {
                       Make = c.Make,
                       Model = c.Model,
                       TravelledDistance = c.TravelledDistance,
                       CarParts = c.PartCars.Select(pc => new PartDto
                       {
                           Name = pc.Part.Name,
                           Price = pc.Part.Price
                       })
                       .ToArray()
                   }
               })
               .ToArray();

            this.jsonExporter.Export(GetCarPartsExportFilePathJson, carPartsDTOs);
        }

        private void GetLocalSuppliers()
        {
            var localSupplier = context.Supplies
                 .Where(s => s.IsImporter == false)
                 .Select(s => new LocalSupplierDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     PartsCount = s.Parts.Count
                 })
                .ToArray();

            this.jsonExporter.Export(LocalSupplierExportFilePathJson, localSupplier);
        }

        private void GetCarsMadeFromToyota()
        {
            var carDTOs = this.context.Cars
                .Where(c => c.Make.Equals("Toyota"))
                .Select(c => new CarDTO
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray()
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            this.jsonExporter.Export(ToyotaCarsExportFilePathJson, carDTOs);
        }

        private void GetOrderedCustomers()
        {
            var customerDTO = context.Customers
                 .OrderBy(c => c.BirthDate)
                 .ThenBy(c => c.IsYoungDriver)
                 .Select(c => new CustomerDto
                 {
                     Id = c.Id,
                     Name = c.Name,
                     BirthDate = c.BirthDate,
                     IsYoungDriver = c.IsYoungDriver,
                     SalesCount = c.Sales.Count
                 })
                 .ToArray();

            this.jsonExporter.Export(OrderedCustomersExportFilePathJson, customerDTO);
        }

        private void ResetDatabase(JsonImporter jsonImporter)
        {
            using (var context = new CarDealerContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }

            jsonImporter.Import();
        }
    }
}
