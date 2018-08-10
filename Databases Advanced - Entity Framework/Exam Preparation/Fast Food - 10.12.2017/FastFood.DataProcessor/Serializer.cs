using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Export;
using FastFood.Models.Enums;
using Newtonsoft.Json;
using System.Xml;

namespace FastFood.DataProcessor
{
    public class Serializer
    {
        public static string ExportOrdersByEmployee(FastFoodDbContext context, string employeeName, string orderType)
        {
            var orderTypeAsEnum = Enum.Parse<OrderType>(orderType);

            var employee = context.Employees
                               .ToArray()
                                .Where(x => x.Name == employeeName)
                                .Select(x => new
                                {
                                    Name = x.Name,
                                    Orders = x.Orders
                                    .Where(s => s.Type == orderTypeAsEnum)
                                    .Select(c => new
                                    {
                                        Customer = c.Customer,
                                        Items = c.OrderItems
                                                 .Select(i => new
                                                 {
                                                     Name = i.Item.Name,
                                                     Price = i.Item.Price,
                                                     Quantity = i.Quantity
                                                 })
                                                 .ToArray(),
                                        TotalPrice = c.TotalPrice
                                    })
                                    .OrderByDescending(t => t.TotalPrice)
                                    .ThenByDescending(i => i.Items.Length)
                                    .ToArray(),
                                    TotalMade = x.Orders.Where(t => t.Type == orderTypeAsEnum)
                                                        .Sum(tm => tm.TotalPrice)
                                })
                                .FirstOrDefault();

            var jsonString = JsonConvert.SerializeObject(employee, Newtonsoft.Json.Formatting.Indented);

            return jsonString;

        }

        public static string ExportCategoryStatistics(FastFoodDbContext context, string categoriesString)
        {
            var categoriesArray = categoriesString.Split(',');

            var categories = context.Categories
                                    .Where(c => categoriesArray
                                        .Any(s => s == c.Name))
                                    .Select(s => new CategoryDto
                                    {
                                        Name = s.Name,
                                        MostPopularItemDto =
                                         s.Items
                                         .Select(x => new MostPopularItemDto
                                         {
                                             Name = x.Name,
                                             TimesSold = x.OrderItems.Sum(i => i.Quantity),
                                             TotalMade = x.OrderItems.Sum(i => i.Item.Price * i.Quantity)
                                         })
                                         .OrderByDescending(x => x.TotalMade)
                                         .ThenByDescending(x => x.TimesSold)
                                         .FirstOrDefault()
                                    }).
                                    OrderByDescending(x => x.MostPopularItemDto.TotalMade)
                                    .ThenByDescending(x => x.MostPopularItemDto.TimesSold)
                                    .ToArray();

            StringBuilder sb = new StringBuilder();

            var xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var xmlSerializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("Categories"));

            xmlSerializer.Serialize(new StringWriter(sb), categories, xmlNamespaces);

            return sb.ToString();
        }
    }
}