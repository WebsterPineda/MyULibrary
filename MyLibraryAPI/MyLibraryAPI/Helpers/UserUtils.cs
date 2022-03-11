using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Helpers
{
    public class UserUtils
    {
        public static bool UserExists(in LibraryContext context, User usr)
        {
            var user = context.Users.SingleOrDefault(u => u.Email == usr.Email);
            if (user == null)
                return false;
            return true;
        }
    }
}