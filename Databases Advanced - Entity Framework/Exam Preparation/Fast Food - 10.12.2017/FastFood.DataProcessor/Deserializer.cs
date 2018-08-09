using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using FastFood.Data;
using FastFood.DataProcessor.Dto.Import;
using FastFood.Models;
using FastFood.Models.Enums;
using Newtonsoft.Json;

namespace FastFood.DataProcessor
{
	public static class Deserializer
	{
		private const string FailureMessage = "Invalid data format.";
		private const string SuccessMessage = "Record {0} successfully imported.";

		public static string ImportEmployees(FastFoodDbContext context, string jsonString)
		{
            StringBuilder sb = new StringBuilder();

            var deserializedEmployees = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var listEmployees = new List<Employee>();

            foreach (var employeeDto in deserializedEmployees)
            {
                if(!IsValid(employeeDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                Position position = GetPosition(context, employeeDto.Position);

                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Position = position
                };

                listEmployees.Add(employee);

                sb.AppendLine(string.Format(SuccessMessage, employee.Name));               
            }

            context.Employees.AddRange(listEmployees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportItems(FastFoodDbContext context, string jsonString)
		{
            StringBuilder sb = new StringBuilder();

            var deserializedItems = JsonConvert.DeserializeObject<ItemDto[]>(jsonString);

            var listItems = new List<Item>();

            foreach (var itemDto in deserializedItems)
            {
                if(!IsValid(itemDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var itemsExists = listItems.Any(i => i.Name == itemDto.Name);

                if(itemsExists)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var category = GetGategory(context, itemDto.Category);

                var item = new Item
                {
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Category = category
                };

                sb.AppendLine(string.Format(SuccessMessage, itemDto.Name));
                listItems.Add(item);
            }

            context.Items.AddRange(listItems);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}


        public static string ImportOrders(FastFoodDbContext context, string xmlString)
		{
            StringBuilder sb = new StringBuilder();

            var orderItemsList = new List<OrderItem>();
            var orderList = new List<Order>();

            var serializer = new XmlSerializer(typeof(OrderDto[]), new XmlRootAttribute("Orders"));
            var deserializedOrders = (OrderDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (var orderDto in deserializedOrders)
            {
                bool isValidItem = true;

                if(!IsValid(orderDto))
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                foreach (var itemDto in orderDto.OrderItems)
                {
                    if (!IsValid(itemDto))
                    {
                        sb.AppendLine(FailureMessage);
                        isValidItem = false;
                        break;

                    }
                }

                if (!isValidItem)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var employee = context.Employees.FirstOrDefault(x => x.Name == orderDto.Employee);

                if(employee == null)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var areValidItems = AreValidItems(context, orderDto.OrderItems);

                if(!areValidItems)
                {
                    sb.AppendLine(FailureMessage);
                    continue;
                }

                var date = DateTime
                    .ParseExact(orderDto.DateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var orderType = Enum.Parse<OrderType>(orderDto.Type);

                var order = new Order
                {
                    Customer = orderDto.Customer,
                    Employee = employee,
                    DateTime = date,
                    Type = orderType
                };

                orderList.Add(order);

                foreach (var itemDto in orderDto.OrderItems)
                {
                    var item = context.Items.FirstOrDefault(i => i.Name == itemDto.Name);

                    var orderItem = new OrderItem
                    {
                        Order = order,
                        Item = item,
                        Quantity = itemDto.Quantity
                    };

                    orderItemsList.Add(orderItem);
                }
                sb.AppendLine
                    ($"Order for {orderDto.Customer} on {date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)} added");
            }

            context.Orders.AddRange(orderList);
            context.SaveChanges();

            context.OrderItems.AddRange(orderItemsList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

        private static bool AreValidItems(FastFoodDbContext context, OrderItemsDto[] orderItems)
        {
            foreach (var item in orderItems)
            {
                bool itemExist = context.Items.Any(i => i.Name == item.Name);

                if (!itemExist)
                {
                    return false;
                }
            }

            return true;
        }

        private static Category GetGategory(FastFoodDbContext context, string categoryName)
        {
            var category = context.Categories.FirstOrDefault(c => c.Name == categoryName);

            if (category == null)
            {
                category = new Category
                {
                    Name = categoryName
                };

                context.Categories.Add(category);
                context.SaveChanges();
            }

            return category;
        }

        private static Position GetPosition(FastFoodDbContext context, string positionName)
        {
            var position = context.Positions.FirstOrDefault(p => p.Name == positionName);

            if(position == null)
            {
                position = new Position
                {
                    Name = positionName
                };

                context.Positions.Add(position);
                context.SaveChanges();
            }

            return position;
        }


        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResult, true);
        }
	}
}