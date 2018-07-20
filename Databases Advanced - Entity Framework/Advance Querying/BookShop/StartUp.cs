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

        // P05
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                               .Where(b => b.BookCategories
                               .Any(c => categories
                               .Contains(c.Category.Name.ToLower())))
                               .Select(t => t.Title)
                               .OrderBy(x => x)
                               .ToArray();
                           
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

        // P12 
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var totalProfit = context.Categories
                                     .Select(c => new
                                     {
                                         Name = c.Name,
                                         totalProfit = c.CategoryBooks
                                                        .Select(cb => cb.Book.Price * cb.Book.Copies)
                                                        .Sum()
                                     })
                                     .OrderByDescending(c => c.totalProfit)
                                     .ThenByDescending(c => c.Name)
                                     .Select(c => $"{c.Name} ${c.totalProfit}");

            return string.Join(Environment.NewLine, totalProfit);
        }

        // P13
        public static string GetMostRecentBooks(BookShopContext context)
             => "--" + string.Join($"{Environment.NewLine}--", context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    BookCount = c.CategoryBooks
                        .Select(cb => cb.Book)
                        .Count(),
                    TopThreeString = string.Join(Environment.NewLine, c.CategoryBooks
                        .Select(cb => cb.Book)
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .Select(b => b.ReleaseDate == null
                            ? $"{b.Title} ()"
                            : $"{b.Title} ({b.ReleaseDate.Value.Year})"))
                })
                .OrderBy(c => c.Name)
                .Select(c => $"{c.Name}{Environment.NewLine}{c.TopThreeString}"));


        // P14
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                               .Where(b => b.ReleaseDate != null && b.ReleaseDate.Value.Year < 2010)
                               .ToArray();

            // Increase books price by 5
            int increasePriceOfBooks = 5;

            foreach (var book in books)
            {
                book.Price += increasePriceOfBooks;
            }

            context.SaveChanges();
        }

        // P15
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToBeRemoved = context.Books
                                          .Where(b => b.Copies < 4200)
                                          .ToArray();

            var removedBooks = booksToBeRemoved.Length;

            context.Books.RemoveRange(booksToBeRemoved);
            context.SaveChanges();

            return removedBooks;
        }

    }
}

