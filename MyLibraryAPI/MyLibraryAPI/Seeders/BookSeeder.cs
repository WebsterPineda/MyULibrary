using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Seeders
{
    public class BookSeeder
    {
        public static IList<Models.Book> booksSeederList(int userIdRegistration)
        {
            IList<Models.Book> books = new List<Models.Book>();

            books.Add(new Models.Book()
            {
                Author = "Stephen King",
                Genre = "Novel",
                PublishedYear = 1977,
                Title = "The Shining",
                CreatedBy = userIdRegistration
            });
            books.Add(new Models.Book()
            {
                Author = "Stephen King",
                Genre = "Novel",
                PublishedYear = 1986,
                Title = "IT",
                CreatedBy = userIdRegistration
            });
            books.Add(new Models.Book()
            {
                Author = "Stephen King",
                Genre = "Novel",
                PublishedYear = 1974,
                Title = "Carrie",
                CreatedBy = userIdRegistration
            });
            books.Add(new Models.Book()
            {
                Author = "Edgar Allan Poe",
                Genre = "Terror",
                PublishedYear = 1843,
                Title = "The black cat",
                CreatedBy = userIdRegistration
            });

            return books;
        }
    }
}