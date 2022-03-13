using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyLibraryAPI.Models;

namespace MyLibraryAPI.Auth
{
    public class TokenGenerator
    {
        public static Token Generate(int userId, int roleId)
        {
            Helpers.TokenUtil.GetTokenConfig(out string audience, out string issuer, out string key);

            SymmetricSecurityKey secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials signCredentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            Guid guid = Guid.NewGuid();
            List<Claim> lClaims = new List<Claim>();
            lClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, guid.ToString()));
            lClaims.Add(new Claim("valid", "1"));
            lClaims.Add(new Claim("Usr", userId.ToString()));
            lClaims.Add(new Claim("Type", roleId.ToString()));

            ClaimsIdentity claims = new ClaimsIdentity(lClaims);

            DateTime generateTime = DateTime.UtcNow;
            DateTime expires = generateTime.AddDays(1).AddSeconds(-1);

            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.CreateJwtSecurityToken(
                audience: audience,
                issuer: issuer,
                subject: claims,
                notBefore: generateTime,
                expires: expires,
                signingCredentials: signCredentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new Token()
            {
                token = jwtToken
            };
        }
    }
}