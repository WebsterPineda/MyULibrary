using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Seeders
{
    public class RoleSeeder
    {
        public static IList<Models.Role> listSeeder()
        {
            IList<Models.Role> roles = new List<Models.Role>();
            roles.Add(new Models.Role() { Description = "Librarian" });
            roles.Add(new Models.Role() { Description = "Student" });
            return roles;
        }
    }
}