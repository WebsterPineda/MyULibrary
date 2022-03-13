using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Text;

using MyLibraryAPI.Models;

namespace MyLibraryAPI.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext() : base("name=LibraryDBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LibraryContext, Migrations.Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                Helpers.AuditCheckers.CheckAuditories(ChangeTracker);
                Helpers.AuditUserChecker.CheckAuditUser(ChangeTracker);

                return base.SaveChanges();
            }catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();

                foreach(var failure in e.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity validation failed - errors follow: \n" +
                    sb.ToString(), e);
            }
        }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AuditUser> AuditUsers { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<CheckOut> CheckOuts { get; set; }
        public virtual DbSet<Privilege> Privileges { get; set; }
    }
}