using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MyLibraryAPI.Context;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Helpers
{
    public class RoleUtil
    {
        public static bool RoleExists (in LibraryContext contexto, int roleId)
        {
            var item = contexto.Roles.SingleOrDefault(x => x.RoleId == roleId);
            if (item == null)
                return false;
            return true;
        }
    }
}