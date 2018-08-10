using System;
using System.Linq;
using FastFood.Data;

namespace FastFood.DataProcessor
{
    public static class Bonus
    {
	    public static string UpdatePrice(FastFoodDbContext context, string itemName, decimal newPrice)
	    {
            var item = context.Items
                              .Where(i => i.Name == itemName)
                              .FirstOrDefault();

            if(item == null)
            {
                return $"Item {itemName} not found!";
            }

            var itemOldPrice = item.Price;

            item.Price = newPrice;

            context.SaveChanges();

            return $"{item.Name} Price updated from ${itemOldPrice:F2} to ${newPrice:F2}";



            context.Items.

          
	    }
    }
}
