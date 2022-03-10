using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Helpers
{
    public class AuditUserChecker
    {
        public static void CheckAuditUser(DbChangeTracker tracker)
        {
            var entries = tracker.Entries()
                .Where(x => x.Entity is Models.AuditUser &&
                (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (Models.AuditUser)entry.Entity;
                if (entry.State == EntityState.Added)
                    entity.Action = "CREATED";
                if (entry.State == EntityState.Modified)
                    entity.Action = "UPDATED";
                if (entry.State == EntityState.Deleted)
                    entity.Action = "DELETED";
            }
        }
    }
}