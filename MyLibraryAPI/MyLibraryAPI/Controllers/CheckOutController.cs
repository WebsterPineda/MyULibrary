using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    public class CheckOutController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetRequests(CheckOut check)
        {
            
        }

        [HttpPost]
        public IHttpActionResult PostRequest(CheckOut check)
        {
            if (!ModelState.IsValid)
                return Content(HttpStatusCode.BadRequest, new
                {
                    Message = "Something happened during request, try again."
                });
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    check.Book = context.Books.SingleOrDefault(b => b.BookId == check.BookId);
                    check.Book.Stock = context.Stocks
                        .Where(s => s.BookId == check.Book.BookId).SingleOrDefault();
                    check.User = context.Users.SingleOrDefault(usr => usr.UserId == check.StudentId);

                    if (check.Book.BookId == 0 || check.User.UserId == 0)
                    {
                        return Content(HttpStatusCode.NotFound, new
                        {
                            Message = "Cant request book at this moment, please try again later."
                        });
                    }

                    if (check.Book.Stock.Available > 0)
                        check.Book.Stock.Available -= 1;
                    else
                        return Content(HttpStatusCode.BadRequest, new
                        {
                            Message = "There's currently no stock for this book."
                        });

                    var checkout = new CheckOut()
                    {
                        BookId = check.BookId,
                        StudentId = check.StudentId,
                        Returned = false,
                        CheckedOutMoment = DateTime.Now,
                        Book = check.Book,
                        User = check.User
                    };

                    context.CheckOuts.Add(checkout);

                    context.SaveChanges();

                    return Ok(new
                    {
                        Message = "Book was checked out succesfully!"
                    });
                }
            }catch (Exception e)
            {
                string Message = "Something went wrong!";
                string DetailedError = e.Message;
                DetailedError += (e.InnerException != null) ? " - " + e.InnerException.Message : "";
                return Content(HttpStatusCode.InternalServerError, new
                {
                    Message,
                    DetailedError
                });
            }
        }
    }
}