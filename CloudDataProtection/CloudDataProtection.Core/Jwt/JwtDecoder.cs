﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using CloudDataProtection.Core.Controllers.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CloudDataProtection.Core.Jwt
{
    public class JwtDecoder : IJwtDecoder
    {
        private static readonly string AuthenticationScheme = "Bearer";
        
        public int? GetUserId(IHeaderDictionary headers)
        {
            int? userId = headers?
                .Where(header => header.Key == "Authorization")
                .Select(header => header.Value)
                .Select(value => value.ToString())
                .Select(GetToken)
                .Select(GetClaims)
                .Select(GetUserId)
                .FirstOrDefault();

            return userId;
        }

        public UserRole? GetUserRole(IHeaderDictionary headers)
        {
            UserRole? role = headers?
                .Where(header => header.Key == "Authorization")
                .Select(header => header.Value)
                .Select(value => value.ToString())
                .Select(GetToken)
                .Select(GetClaims)
                .Select(GetUserRole)
                .FirstOrDefault();

            return role;
        }

        private static string GetToken(string header)
        {
            if (!header.StartsWith(AuthenticationScheme))
            {
                return null;
            }

            return header.Replace(AuthenticationScheme, string.Empty).Trim();
        }

        private static IEnumerable<Claim> GetClaims(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken jsonToken = handler.ReadToken(token);
            JwtSecurityToken securityToken = jsonToken as JwtSecurityToken;

            return securityToken?.Claims;
        }

        private static int? GetUserId(IEnumerable<Claim> claims)
        {
            return claims
                .Where(c => c.Type == "unique_name")
                .Select(claim => claim.Value)
                .Select(value => int.TryParse(value, out int id) ? id : null as int?)
                .FirstOrDefault();
        }

        private static UserRole? GetUserRole(IEnumerable<Claim> claims)
        {
            return claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(claim => claim.Value)
                .Select(value => int.TryParse(value, out int role) ? (UserRole) role : null as UserRole?)
                .FirstOrDefault();
        }
    }
}