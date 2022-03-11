using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Seeders
{
    public class UserSeeder
    {
        public static IList<Models.User> usersSeedList(Models.Role studentRole, Models.Role librariaRole)
        {
            Helpers.PasswordUtil pswdUtil = new Helpers.PasswordUtil();
            IList<Models.User> users = new List<Models.User>();
            users.Add(new Models.User()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                RoleId = librariaRole.RoleId,
                Password = pswdUtil.PasswordEncrypt("123456"),
                Active = true,
                TempPassword = false
            });
            users.Add(new Models.User()
            {
                FirstName = "Jane",
                LastName = "Blow",
                Email = "jane.blow@hotmail.com",
                Password = pswdUtil.PasswordEncrypt("123"),
                RoleId = studentRole.RoleId,
                Active = true,
                TempPassword = false
            });
            return users;
        }
    }
}