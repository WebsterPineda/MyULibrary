namespace MyLibraryAPI.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.LibraryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context.LibraryContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(Seeders.RoleSeeder.listSeeder());
                context.SaveChanges();
            }

            Models.Role librarianRole = context.Roles.FirstOrDefault(x => x.Description == "Librarian");
            Models.Role studentRole = context.Roles.FirstOrDefault(x => x.Description == "Student");

            if (librarianRole != null && studentRole != null)
            {
                if (!context.Users.Any())
                {
                    context.Users.AddRange(Seeders.UserSeeder.usersSeedList(studentRole, librarianRole));

                    context.SaveChanges();
                }

                Models.User user = context.Users.FirstOrDefault(x => x.RoleId == librarianRole.RoleId);

                IList<Models.Book> bookList = context.Books.ToList();
                if (bookList.Count == 0)
                    bookList = Seeders.BookSeeder.booksSeederList(user.UserId);

                if (!context.Books.Any())
                {
                    context.Books.AddRange(bookList);

                    context.SaveChanges();
                }

                if (!context.Stocks.Any())
                {
                    context.Stocks.AddRange(Seeders.StockSeeder.stocksSeederList(bookList));

                    context.SaveChanges();
                }

                if (!context.Privileges.Any())
                {
                    context.Privileges.AddRange(Seeders.PrivilegeSeeder.GeneratePrivileges(librarianRole.RoleId, studentRole.RoleId));

                    context.SaveChanges();
                }
            }

            base.Seed(context);
        }
    }
}
