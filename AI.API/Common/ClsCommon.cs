using AI.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace AI.API.Common
{
    public class ClsCommon
    {
        private static string securityKey = ConfigurationManager.AppSettings["JwtKey"].ToString();
        private static string issuer = ConfigurationManager.AppSettings["JwtIssuer"].ToString();
        private static string audience = ConfigurationManager.AppSettings["JwtAudience"].ToString();
        public ClsCommon()
        {

        }
        public static string GenerateJwtToken(UserIdentityData objUserData)
        {
            try
            {
                var expiryDate = ConfigurationManager.AppSettings["JwtExpireDays"].ToString();
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(securityKey);
                var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                         new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, objUserData.EmpName),
                         new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, objUserData.EmpEmail),
                         new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, objUserData.SkillId.ToString()),
                         new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, objUserData.EmpID),
            }),
                    Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(expiryDate)),
                    Audience = audience,
                    Issuer = issuer,
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool ValidateJwtToken(string authToken, string empId, string skillId)
        {
            try
            {
                var token = authToken.Split(' ');
                authToken = token[1];
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);

                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    throw new Exception("Token expired");
                }
                var principalempId = string.Empty;
                if (principal.FindFirst(ClaimTypes.NameIdentifier).Value != null)
                {
                    principalempId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                var principalSkillId = string.Empty;
                if (principal.FindFirst(ClaimTypes.Role).Value != null)
                {
                    principalSkillId = principal.FindFirst(ClaimTypes.Role).Value;
                }
                if (!string.IsNullOrEmpty(principalempId)&& !string.IsNullOrEmpty(principalSkillId)
                    && string.Compare(principalempId, empId, false) == 0 && string.Compare(principalSkillId, skillId, false) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(securityKey);
                return new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static CheckAuthentication ValidateJwtToken(string authToken)
        {
            CheckAuthentication objCheckAuthentication = new CheckAuthentication();
            try
            {
                var token = authToken.Split(' ');
                authToken = token[1];
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);

                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    throw new Exception("Token expired");
                }
                var principalempId = string.Empty;
                if (principal.FindFirst(ClaimTypes.NameIdentifier).Value != null)
                {
                    principalempId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                var principalSkillId = string.Empty;
                if (principal.FindFirst(ClaimTypes.Role).Value != null)
                {
                    principalSkillId = principal.FindFirst(ClaimTypes.Role).Value;
                }
                objCheckAuthentication.CheckToken = true;
                objCheckAuthentication.EmpId = Convert.ToInt32(principalempId);
                objCheckAuthentication.ProfileId = Convert.ToInt32(principalSkillId);
            }

            catch (Exception)
            {
                objCheckAuthentication.CheckToken = false;
            }
            return objCheckAuthentication;
        }
        public static string CheckRequiredField(Entity entity)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(entity.EntityName))
            {
                message= "'EntityName' parameter is required !";
            }
            else if (entity.DurationFrom == null)
            {
                message= "'DurationFrom' parameter is required !";
            }
            else if(entity.DurationTo == null)
            {
                message= "'DurationTo' parameter is required !";
            }
            return message;
        }
    }
}