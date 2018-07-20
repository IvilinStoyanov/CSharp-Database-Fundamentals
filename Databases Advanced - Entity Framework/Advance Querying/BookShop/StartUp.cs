namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // DbInitializer.ResetDatabase(db);
                // var input = int.Parse(Console.ReadLine());
                //.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var result = CountCopiesByAuthor(db);

                Console.WriteLine(result);
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

            return string.Join(Environment.NewLine, books);
        }

        // P05 - Compile time error
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

        // P07
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                 .Where(a => a.FirstName != null && a.FirstName.EndsWith(input))
                 .Select(a => a.FirstName == null ? a.LastName : $"{a.FirstName} {a.LastName}")
                 .OrderBy(n => n);

            return string.Join(Environment.NewLine, authors);
        }
        // P08
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var booksTitle = context.Books
                                    .Where(b => EF.Functions.Like(b.Title, $"%{input}%"))
                                    .Select(b => b.Title)
                                    .OrderBy(b => b)
                                    .ToList();

            return string.Join(Environment.NewLine, booksTitle);
        }

        // P09
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                               .Where(b => EF.Functions.Like(b.Author.LastName, $"{input}%"))
                               .OrderBy(x => x.BookId)
                               .Select(b => b.Author.FirstName == null
                               ? $"{b.Title} ({b.Author.LastName})"
                               : $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})");

            return string.Join(Environment.NewLine, books);
        }

        // P10 
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                               .Where(b => b.Title.Length > lengthCheck)
                               .Select(b => b.BookId)
                               .Count();
            return books;
        }

        // P11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var copies = context.Authors
                                .Select(x => new
                                {
                                    Name = x.FirstName + " " + x.LastName,
                                    Copies = x.Books
                                              .Select(b => b.Copies)
                                              .Sum()
                                })
                                .OrderByDescending(x => x.Copies)
                                .Select(a => $"{a.Name} - {a.Copies}");

            return string.Join(Environment.NewLine, copies);
        }


    }
}
