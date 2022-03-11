using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    public class UserController : ApiController
    {
        public IHttpActionResult GetUsers()
        {
            try
            {
                using(LibraryContext context = new LibraryContext())
                {
                    var list = context.Users.Where(x => x.Active == true).Select(
                        u => new{
                            u.UserId,
                            u.FirstName,
                            u.LastName,
                            u.RoleId,
                            u.Email,
                            u.Role.Description
                        }).ToList();

                    List<UserView> users = new List<UserView>();

                    foreach (var item in list) {
                        users.Add(new UserView()
                        {
                            UserId = item.UserId,
                            Email = item.Email,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            RoleId = item.RoleId,
                            Role = item.Description
                        });
                    }

                    if (users.Count == 0)
                        return Content(HttpStatusCode.NotFound, new
                        {
                            Message = "No users have been found."
                        });
                    return Ok(users);
                }
            }catch(Exception e)
            {
                string Message = "Something went wrong.";
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
        public IHttpActionResult Register(User user)
        {
            try
            {
                if (user.RoleId == 0)
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        Message = "User doesn't has a role associated"
                    });
                using (LibraryContext context = new LibraryContext())
                {
                    var validation = Helpers.UserUtils.UserExists(context, user);

                    if (validation)
                        return Content(HttpStatusCode.Conflict, new
                        {
                            Message = "User already exists"
                        });

                    Helpers.PasswordUtil password = new Helpers.PasswordUtil();

                    string pass = password.GenerateRandomPassword();

                    if (!Helpers.RoleUtil.RoleExists(context, user.RoleId))
                        return Content(HttpStatusCode.Conflict, new
                        {
                            Message = "The role asociated with the user doesn't exist."
                        });

                    var usr = new User()
                    {
                        Active = true,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Password = password.PasswordEncrypt(pass),
                        RoleId = user.RoleId,
                        TempPassword = true
                    };

                    context.Users.Add(usr);

                    context.SaveChanges();

                    return Content(HttpStatusCode.OK, new
                    {
                        Message = "User created succesfully, \\n user: " + usr.Email
                            + "\\nTemporary Password: " + pass
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