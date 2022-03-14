using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Login(Login user)
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
                        Token token = Auth.TokenGenerator.Generate(usr.UserId, usr.RoleId);

                        return Ok(token);
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