namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using System;
    using System.Globalization;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db);
                var input = Console.ReadLine()
                            .Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                var result = GetBooksByCategory(db , input);

                Console.WriteLine(string.Join(Environment.NewLine, result));
            }
        }

        // P01
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum
                                                 .Parse(typeof(AgeRestriction), command, true);

            var books = context.Books
                               .Where(b => b.AgeRestriction == ageRestriction)
                               .OrderBy(b => b.Title)
                               .Select(b => b.Title)
                               .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        // P02 
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                                     .Where(b => b.EditionType == EditionType.Gold &&
                                      b.Copies < 5000)
                                     .OrderBy(b => b.BookId)
                                     .Select(b => b.Title);


            return string.Join(Environment.NewLine, goldenBooks);
        }

        // P03
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                               .Where(x => x.Price > 40)
                               .OrderByDescending(x => x.Price)
                               .Select(x => $"{x.Title} - ${x.Price:F2}");

            return string.Join(Environment.NewLine, books);
        }

        // P04 
        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                               .Where(b => b.ReleaseDate.Value.Year != year)
                               .OrderBy(b => b.BookId)
                               .Select(b => b.Title);

            return string.Join(Environment.NewLine , books);
        }

        // P05
        public static string GetBooksByCategory(BookShopContext context, string[] input)
        {
            var books = context.Books
                .Where(b => b.BookCategories.Select(bc => bc.Category.Name.ToLower())
                .Intersect(input)
                .Any())
                .Select(b => b.Title)
                .OrderBy(t => t);
                
               return string.Join(Environment.NewLine, books);
        }

        // P06 
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var parseDate = DateTime
                    .ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                               .Where(b => b.ReleaseDate < parseDate)
                               .OrderByDescending(b => b.ReleaseDate)
                               .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}")
                               .ToArray();

            return string.Join(Environment.NewLine, books);
        }
    }
}
