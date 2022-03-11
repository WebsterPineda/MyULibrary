using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    public class LoginController : ApiController
    {
        public IHttpActionResult Get(Login user)
        {
            if (!ModelState.IsValid)
                return Content(HttpStatusCode.BadRequest, new
                {
                    Message = "Some required data are not provided"
                });
            try
            {
                using(LibraryContext context = new LibraryContext())
                {
                    var usr = context.Users
                        .SingleOrDefault(x => x.Email == user.Email && x.Active == true);

                    if (usr == null)
                        return Content(HttpStatusCode.NotFound, new
                        {
                            Message = "User doesn't exist."
                        });

                    if (Helpers.PasswordUtil.PasswordVerify(user.Password, usr.Password))
                    {
                        User person = new User()
                        {
                            Email = usr.Email,
                            FirstName = usr.FirstName,
                            LastName = usr.LastName,
                            RoleId = usr.RoleId,
                            Active = usr.Active,
                            TempPassword = usr.TempPassword
                        };
                        return Ok(person);
                    }
                    return Content(HttpStatusCode.Conflict, new
                    {
                        Message = "Password is invalid"
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