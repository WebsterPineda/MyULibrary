using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using MyLibraryAPI.Context;
using MyLibraryAPI.Helpers;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
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
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }

        [HttpGet]
        public IHttpActionResult GetById(int id)
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
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }

        [HttpPost]
        public IHttpActionResult Create(Book book)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Content(HttpStatusCode.BadRequest, new { Message = "Bad data was received to create new book." });
                if (string.IsNullOrWhiteSpace(book.Author) || string.IsNullOrWhiteSpace(book.Genre) || string.IsNullOrWhiteSpace(book.Title)
                    || book.PublishedYear == 0)
                    return Content(HttpStatusCode.BadRequest, new { Message = "Some data of book is missed, please verify." });
                using (var context = new LibraryContext())
                {
                    context.Books.Add(book);
                    context.SaveChanges();

                    Stock stock = context.Stocks.SingleOrDefault(x => x.BookId == book.BookId);

                    if (stock == null)
                    {
                        stock = new Stock()
                        {
                            BookId = book.BookId,
                            Available = 0,
                            Book = book
                        };
                        context.Stocks.Add(stock);

                        context.SaveChanges();
                    }
                    return Ok(new { Message = "Book was created succesfully" });
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }

        [HttpPut]
        public IHttpActionResult Update([FromUri]int id, [FromBody]Book book)
        {
            if (id == 0)
                return Content(HttpStatusCode.BadRequest, new { Message = "Invalid data to update" });
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    var dbBook = context.Books.SingleOrDefault(b => b.BookId == id);
                    if (dbBook == null)
                        return Content(HttpStatusCode.NotFound, new { Message = "Book not found." });

                    if (!string.IsNullOrWhiteSpace(book.Author))
                        dbBook.Author = book.Author;
                    if (!string.IsNullOrWhiteSpace(book.Title))
                        dbBook.Title = book.Title;
                    if (!string.IsNullOrWhiteSpace(book.Genre))
                        dbBook.Genre = book.Genre;
                    if (book.PublishedYear != 0)
                        dbBook.PublishedYear = book.PublishedYear;

                    context.SaveChanges();
                    return Content(HttpStatusCode.OK, new { Message = "Book updated successfully" });
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }
    }
}