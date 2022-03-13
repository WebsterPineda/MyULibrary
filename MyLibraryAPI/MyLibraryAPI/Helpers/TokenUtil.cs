using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace MyLibraryAPI.Helpers
{
    public class TokenUtil
    {
        public static void GetTokenConfig(out string audience, out string issuer, out string key)
        {
            string secret = ConfigurationManager.AppSettings["JWT_SEC_KEY"] ?? "AZXCVMALKDFPQWOE";
            key = secret + "_L1b@p1_2022";
            issuer = ConfigurationManager.AppSettings["JWT_ISSUER"] ?? "http://localhost:56534";
            audience = ConfigurationManager.AppSettings["JWT_AUDIENCE"] ?? "http://localhost:56534";
        }

        public static bool LifeTimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        public static bool AlgorithmValidator(string algorithm, SecurityKey securityKey, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (algorithm == SecurityAlgorithms.HmacSha256)
                return true;
            return false;
        }

        public static string ClaimValue(string claimName, IPrincipal principal)
        {
            string value = "";
            ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IList<Claim> claims = identity.Claims.ToList();
                value = claims.FirstOrDefault(x => x.Type == claimName)?.Value;
            }
            return value;
        }
    }
}