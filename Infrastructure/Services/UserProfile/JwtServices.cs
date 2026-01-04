using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using Application.Interfaces;

namespace Spark.Infrastructure.Services.UserProfile
{
    public sealed class JwtService(IConfiguration cfg) : IJwtService
    {
        public (string token, DateTimeOffset expiresAt) CreateAccessToken(Guid userId, string email)
        {
            var iss = cfg["Jwt:Issuer"]!;
            var aud = cfg["Jwt:Audience"]!;
            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
            var ttlMin = int.TryParse(cfg["Jwt:AccessMinutes"], out var m) ? m : 15;

            var now = DateTimeOffset.UtcNow;
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString())
        };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = now.AddMinutes(ttlMin);

            var jwt = new JwtSecurityToken(issuer: iss, audience: aud, claims: claims,
                notBefore: now.UtcDateTime, expires: expires.UtcDateTime, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }
    }
}
