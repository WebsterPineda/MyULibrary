using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

using MyLibraryAPI.Context;
using MyLibraryAPI.Helpers;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                using(LibraryContext context = new LibraryContext())
                {
                    int usrId = int.Parse(TokenUtil.ClaimValue("Usr", RequestContext.Principal));

                    if (id != usrId)
                        return Content(HttpStatusCode.Forbidden, new { Message = "User is not authorized to retrieve that data." });

                    var usr = context.Users.Select(u => new
                        {
                            u.UserId,
                            u.FirstName,
                            u.LastName,
                            u.Email,
                            u.Active
                        }).SingleOrDefault(u => u.UserId == id && u.Active == true);

                    if (usr == null)
                        return Content(HttpStatusCode.NotFound, new { Message = "User was not found or is inactive." });

                    return Ok(usr);
                }
            }catch(Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }

        [HttpPost]
        public IHttpActionResult Create(User user)
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
                    var validation = UserUtils.UserExists(context, user);

                    if (validation)
                        return Content(HttpStatusCode.Conflict, new
                        {
                            Message = "User already exists"
                        });

                    PasswordUtil password = new PasswordUtil();

                    string pass = password.GenerateRandomPassword();

                    if (!RoleUtil.RoleExists(context, user.RoleId))
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
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }

        [HttpPut]
        public IHttpActionResult Update(User user)
        {
            try
            {
                int usrId = int.Parse(TokenUtil.ClaimValue("Usr", RequestContext.Principal));
                int roleId = int.Parse(TokenUtil.ClaimValue("Type", RequestContext.Principal));

                using (LibraryContext context = new LibraryContext())
                {
                    var usr = context.Users.SingleOrDefault(x => x.UserId == user.UserId);
                    var role = context.Roles.SingleOrDefault(r => r.RoleId == roleId);
                    if (role == null)
                        return Content(HttpStatusCode.BadRequest, new { Message = "Theres problem with user type" });

                    if (user.UserId != usrId && role.Description.ToLower().Equals("student")
                        || (user.UserId == usrId && !usr.Active) || (roleId != usr.RoleId))
                    {
                        return Content(HttpStatusCode.Forbidden, "You don't have required permissions to perform that action.");
                    }

                    if (user.FirstName != null)
                        usr.FirstName = user.FirstName;
                    if (user.LastName != null)
                        usr.LastName = user.LastName;

                    context.SaveChanges();

                    return Content(HttpStatusCode.OK, new { Message = "User has been updated!" });
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ErrorsUtil.GetErrorMessage(e));
            }
        }
    }
}