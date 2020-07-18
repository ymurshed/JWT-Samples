using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(User user)
        {
            return $"{Constants.Bearer} {new JwtSecurityTokenHandler().WriteToken(BuildToken(user))}";
        }

        public TokenValidationParameters GeTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration[Constants.JwtIssuer],
                ClockSkew = TimeSpan.FromMinutes(Convert.ToDouble(_configuration[Constants.JwtExpireTimeInMinutes])),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.JwtSecretKey]))
            };
        }

        #region Private members
        private JwtSecurityToken BuildToken(User user)
        {
            var claims = GetClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[Constants.JwtSecretKey]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _configuration[Constants.JwtIssuer],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration[Constants.JwtExpireTimeInMinutes])),
                signingCredentials: credentials);
        }

        private static IEnumerable<Claim> GetClaims(User user)
        {
            var isAdmin = user.Role.Equals(Constants.Admin, StringComparison.CurrentCultureIgnoreCase);
            return new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(Constants.AdminClaimType, isAdmin.ToString())
            };
        }
        #endregion
    }
}