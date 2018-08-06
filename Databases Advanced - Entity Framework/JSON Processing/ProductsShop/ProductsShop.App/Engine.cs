namespace ProductsShop.App
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProductsShop.App.DTOs.JsonExport;
    using ProductsShop.App.DTOs.Shared;
    using ProductsShop.App.Exports;
    using ProductsShop.App.Imports;
    using ProductsShop.Data;
    using System;
    using System.IO;
    using System.Linq;

    public class Engine
    {
        private const string ProductsInRangeSellerExportFilePathJson
            = "../../../Exports/Json/ProductsInRange.json";

        private const string SuccessfullySoldProductsExportFilePathJson
            = "../../../Exports/Json/SuccessfullySoldProducts.json";

        private const string CategoriesByProductsCountExportFilePathJson
            = "../../../Exports/Json/CategoriesByProductsCount.json";

        private const string UsersAndProductsExportFilePathJson
            = "../../../Exports/Json/UsersAndProducts.json";

        private JsonExporter jsonExporter;

        public Engine()
        {
            this.jsonExporter = new JsonExporter();

        }

        public Engine(JsonExporter jsonExporter)
        {
            this.jsonExporter = jsonExporter;

        }

        public void Run()
        {
            ResetDatabase(new JSONImporter(new ProductsShopContext()));

            // this.JsonExportProductsInRange(500, 1000); // Query 1 - Products In Range
            // this.JsonExportSuccessfullySoldProducts(); // Query 2 - Successfully Sold Products
            // this.JsonExportCategoriesByProductsCount(); // Query 3 - Categories By Products Count
            // this.JsonExportUsersAndProducts(); // Query 4 - Users and Products
        }

        private void JsonExportProductsInRange(decimal minPrice, decimal maxPrice)
        {
            ProductsInRangeSellerDto[] products = null;
            using (var context = new ProductsShopContext())
            {
                products = context.Products
                    .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                    .Select(p => new ProductsInRangeSellerDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        SellerFullName = (p.Seller.FirstName == null)
                            ? p.Seller.LastName
                            : $"{p.Seller.FirstName} {p.Seller.LastName}"
                    })
                    .OrderBy(p => p.Price)
                    .ToArray();
            }

            var filePath = ProductsInRangeSellerExportFilePathJson;

            this.jsonExporter.Export(filePath, products);
        }

        private void JsonExportSuccessfullySoldProducts()
        {
            SuccessfullySoldProductsSellerDto[] sellers = null;
            using (var context = new ProductsShopContext())
            {
                sellers = context.Users
                    .Where(u => u.SoldProducts.Count > 0)
                    .Select(u => new SuccessfullySoldProductsSellerDto()
                    {
                        SellerFirstName = u.FirstName,
                        SellerLastName = u.LastName,
                        SoldProducts = u.SoldProducts
                            .Select(sp => new SuccessfullySoldProductsProductDto()
                            {
                                BuyerFirstName = sp.Buyer.FirstName,
                                BuyerLastName = sp.Buyer.LastName,
                                Price = sp.Price,
                                ProductName = sp.Name
                            })
                    })
                    .ToArray();
            }

            this.jsonExporter.Export(SuccessfullySoldProductsExportFilePathJson, sellers);
        }

        private void JsonExportCategoriesByProductsCount()
        {
            CategoriesByProductsCountDto[] categories = null;
            using (var context = new ProductsShopContext())
            {
                categories = context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new CategoriesByProductsCountDto()
                    {
                        Name = c.Name,
                        ProductsCount = c.ProductsCategories.Count,
                        AveragePrice = c.ProductsCategories
                            .Select(pc => pc.Product.Price)
                            .Average(),
                        TotalPriceSum = c.ProductsCategories
                            .Select(pc => pc.Product.Price)
                            .Sum()
                    })
                    .ToArray();
            }

            this.jsonExporter.Export(CategoriesByProductsCountExportFilePathJson, categories);
        }

        private void JsonExportUsersAndProducts()
        {
            using (var context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.SoldProducts.Any())
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age ?? 0,
                        soldProducts = new
                        {
                            count = u.SoldProducts.Count,
                            products = u.SoldProducts.Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                        }
                    })
                    .OrderByDescending(u => u.soldProducts.count)
                    .ThenBy(u => u.lastName)
                    .ToArray();

                var jsonReady = new
                {
                    usersCount = users.Length,
                    users = users
                };

                this.jsonExporter.Export(UsersAndProductsExportFilePathJson, jsonReady);
            }
        }

        private static void ResetDatabase(Importer importer)
        {
            using (var context = new ProductsShopContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }

            importer.Import();
        }
    }
}
