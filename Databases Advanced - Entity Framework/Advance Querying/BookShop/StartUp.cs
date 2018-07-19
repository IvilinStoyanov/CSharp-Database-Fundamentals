namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using System;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db);
                string command = Console.ReadLine();

                string result = GetBooksByAgeRestriction(db, command);
                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum
                                                 .Parse(typeof(AgeRestriction), command, true);

            var books = context.Books
                               .Where(x => x.AgeRestriction == ageRestriction)
                               .OrderBy(x => x.Title)
                               .Select(x => x.Title)
                               .ToArray();

            return string.Join(Environment.NewLine, books);
        }
    }
}
