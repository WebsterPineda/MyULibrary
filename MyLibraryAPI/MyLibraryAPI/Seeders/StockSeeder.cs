using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Seeders
{
    public class StockSeeder
    {
        public static IList<Models.Stock> stocksSeederList(ICollection<Models.Book> booksList)
        {
            IList<Models.Stock> stocks = new List<Models.Stock>();

            Random rnd = new Random();

            foreach (Models.Book book in booksList)
                stocks.Add(new Models.Stock()
                {
                    Book = book,
                    BookId = book.BookId,
                    Available = rnd.Next(1, 25),
                });

            return stocks;
        }
    }
}