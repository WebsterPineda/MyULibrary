using System;
using System.Collections.Generic;
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
        public IHttpActionResult Get()
        {
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    ICollection<Book> bookList = context.Books.ToList();
                    if (bookList == null)
                        return Content(HttpStatusCode.NotFound, new
                        {
                            message = "Can't retrieve books, there's no data yet!"
                        });
                    return Ok(bookList);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    message = "Something went wrong.",
                    detailedError = e.Message,
                });
            }
        }
    }
}