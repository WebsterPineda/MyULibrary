using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web.Http;

using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    public class BookController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(Book bookfilter)
        {
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    if (bookfilter == null)
                        bookfilter = new Book();
                    string authorFilter = "%" + (bookfilter.Author ?? "") + "%";
                    string titleFilter = "%" + (bookfilter.Title ?? "") + "%";
                    string genreFilter = "%" + (bookfilter.Genre ?? "") + "%";
                    var bookList = context.Books.Where(x => SqlFunctions.PatIndex(authorFilter, x.Author) > 0
                    && SqlFunctions.PatIndex(titleFilter, x.Title) > 0 && SqlFunctions.PatIndex(genreFilter, x.Genre) > 0)
                        .Select(x => new 
                        {
                            x.BookId,
                            x.Author,
                            x.Genre,
                            x.Title,
                            x.PublishedYear
                        }).ToList();
                    if (bookList == null || bookList.Count == 0)
                    {
                        return Content(HttpStatusCode.NotFound, new
                        {
                            message = "Can't retrieve books, no data has been found.",
                        });
                    }
                    return Ok(bookList);
                }
            }
            catch (Exception e)
            {
                string detailedError = e.Message;
                detailedError += (e.InnerException != null) ? " - " + e.InnerException.Message : "";
                return Content(HttpStatusCode.InternalServerError, new
                {
                    message = "Something went wrong.",
                    detailedError
                });
            }
        }

        [HttpGet]
        public IHttpActionResult GetBook(int id)
        {
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    var book = context.Books.Select(x => new
                        {
                            x.BookId,
                            x.Author,
                            x.Title,
                            x.Genre,
                            Stock = x.Stock.Available
                        }).SingleOrDefault(x => x.BookId == id);
                    if (book == null)
                        return Content(HttpStatusCode.NotFound, new
                        {
                            message = "The requested book can't be found."
                        });
                    return Ok(book);
                }
            }catch (Exception e)
            {
                string message = "Something went wrong!";
                string detailedError = e.Message;
                detailedError += (e.InnerException != null) ? " - " + e.InnerException.Message : "";
                return Content(HttpStatusCode.InternalServerError, new
                {
                    message,
                    detailedError
                });
            }
        }
    }
}