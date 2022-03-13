using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Seeders
{
    public class PrivilegeSeeder
    {
        public static IList<Models.Privilege> GeneratePrivileges(int LibrarianId, int StudentId)
        {
            IList<Models.Privilege> list = new List<Models.Privilege>();

            list.Add(new Models.Privilege()
            {
                Action = "GetById",
                Controller = "User",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Create",
                Controller = "User",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Update",
                Controller = "User",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Get",
                Controller = "CheckOut",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Update",
                Controller = "CheckOut",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Get",
                Controller = "Book",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "GetById",
                Controller = "Book",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Create",
                Controller = "Book",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Update",
                Controller = "Book",
                RoleId = LibrarianId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Get",
                Controller = "Book",
                RoleId = StudentId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "GetById",
                Controller = "Book",
                RoleId = StudentId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Get",
                Controller = "CheckOut",
                RoleId = StudentId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Create",
                Controller = "CheckOut",
                RoleId = StudentId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "GetById",
                Controller = "User",
                RoleId = StudentId,
                Granted = true
            });
            list.Add(new Models.Privilege()
            {
                Action = "Update",
                Controller = "User",
                RoleId = StudentId,
                Granted = true
            });

            return list;
        }
    }
}