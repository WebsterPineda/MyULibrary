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
            try
            {
                using (LibraryContext context = new LibraryContext())
                {
                    if (check.StudentId != 0)
                    {
                        var studentCheckedout = context.CheckOuts
                            .Where(c => c.StudentId == check.StudentId && c.Returned == false)
                            .Select(c => new
                            {
                                c.Book.Author,
                                c.Book.Title,
                                c.Book.Genre,
                                c.Book.PublishedYear
                            }).ToList();
                        if (studentCheckedout.Count == 0)
                        {
                            string Message = (check.User != null) ? "You don't have any book checked out."
                                : "Student doesn't have pending books.";
                            return Ok(new
                            {
                                Message
                            });
                        }
                        return Ok(studentCheckedout);
                    }
                    var checksOut = context.CheckOuts.Where(c => c.Returned == false).ToList();

                    if (checksOut.Count == 0)
                        return Ok(new { Messsage = "All books are at Library."});

                    List<CheckOutViewModel> list = new List<CheckOutViewModel>();
                    foreach (var item in checksOut)
                        list.Add(new CheckOutViewModel()
                        {
                            CheckOutId = item.CheckOutId,
                            Author = item.Book.Author,
                            Since = item.CheckedOutMoment,
                            Genre = item.Book.Genre,
                            StudentName = item.User.FirstName + " " + item.User.LastName,
                            Title = item.Book.Title,
                            PublishedYear = item.Book.PublishedYear,
                        });

                    return Ok(list);
                }
            }catch (Exception e)
            {
                string Message = "Something went wrong!";
                string DetailedError = e.Message;
                Exception ex = e.InnerException;
                while (ex != null)
                {
                    DetailedError += " => " + ex.Message;
                    ex = ex.InnerException;
                }
                return Content(HttpStatusCode.InternalServerError, new
                {
                    Message,
                    DetailedError
                });
            }
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

        [HttpPut]
        public IHttpActionResult ReturnBook(int id)
        {
            try
            {
                using(LibraryContext context = new LibraryContext())
                {
                    var checkout = context.CheckOuts.SingleOrDefault(c => c.CheckOutId == id && c.Returned == false);
                    if (checkout == null)
                        return Content(HttpStatusCode.NotFound, new
                        {
                            Message = "Can't retrieve checkout data, maybe student already has returned book. try again!"
                        });
                    checkout.Returned = true;

                    context.SaveChanges();
                    return Ok(new
                    {
                        Message = "Book was returned Succesfully."
                    });
                }
            }catch (Exception e)
            {
                string Message = "Something went wrong!";
                string DetailedError = e.Message;
                Exception ex = e.InnerException;
                while (ex != null)
                {
                    DetailedError += " => " + ex.Message;
                    ex = ex.InnerException;
                }
                return Content(HttpStatusCode.InternalServerError, new
                {
                    Message,
                    DetailedError
                });
            }
        }
    }
}