using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MyLibraryAPI.Helpers
{
    public class AuditCheckers
    {
        public static void CheckAuditories(DbChangeTracker tracker)
        {
            var auditEntries = tracker.Entries()
                .Where(x => x.Entity is Models.AuditModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in auditEntries)
            {
                var entity = (Models.AuditModel)entry.Entity;
                if (entry.State == EntityState.Added)
                    entity.CreatedAt = DateTime.Now;
                if (entry.State == EntityState.Modified)
                    entity.UpdatedAt = DateTime.Now;
            }
        }
    }
}