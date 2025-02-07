using Microsoft.IdentityModel.Tokens;
using FixProUsApi.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FixProUsApi
{
    public class TokenService  
    {
        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
        public static string GenerateToken(string ConnectionStringX,string PathFolderUrlx , string Id, string username,string OwnerDomainWebSiteX,string AccountCompanyNameX,string ActiveCustomerPhoneX)
        {
            if (PathFolderUrlx == null)
            {
                PathFolderUrlx = "";
            }
            if (OwnerDomainWebSiteX == null)
            {
                OwnerDomainWebSiteX = "";
            }
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim("ConnectionString", ConnectionStringX),
                new Claim("PathFolderUrl", PathFolderUrlx),
                new Claim("username", username),
                new Claim("Id", Id),
                new Claim("OwnerDomainWebSite", OwnerDomainWebSiteX),
                new Claim("AccountCompanyName", AccountCompanyNameX),
                 new Claim("ActiveCustomerPhone", ActiveCustomerPhoneX)
                }),


                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(securityKey,
         SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                if (expires > DateTime.UtcNow)
                    return true;
            }
            return false;
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    LifetimeValidator = CustomLifetimeValidator,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);


                if (principal == null)
                    return null;
                ClaimsIdentity identity = null;
                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (NullReferenceException)
                {
                    return null;
                }
                var usernameClaim = identity.Claims.FirstOrDefault().Value;
                var PathFolderUploadClaim = identity.Claims.ToList();
                PropertyBaseDTO.ConnectSt = usernameClaim;
                if (!string.IsNullOrEmpty(PathFolderUploadClaim[1].Value))
                {
                    PropertyBaseDTO.PathFolderUpload = PathFolderUploadClaim[1].Value;
                
                }
                if (!string.IsNullOrEmpty(PathFolderUploadClaim[4].Value))
                {
                    PropertyBaseDTO.DomainUrl = PathFolderUploadClaim[4].Value;
                } 
                PropertyBaseDTO.AccountName = PathFolderUploadClaim[5].Value;
                PropertyBaseDTO.CustomerPhone = PathFolderUploadClaim[6].Value;
                PropertyBaseDTO.RefreshVariable();
                //string username = usernameClaim.Value;

                return principal;
            }
            catch (Exception x)
            {
                return null;
            }
        }

        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }
    }
}