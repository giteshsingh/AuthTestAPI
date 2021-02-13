using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace AuthTestAPI.Services
{
    public class JwtServices
    {
        private readonly string _secret;
        private readonly string _expDate;

        JwtServices(IConfiguration config)
        {
            _secret = config.GetSection("JWTConfig").GetSection("Secret").Value;
            _expDate = config.GetSection("JWTConfig").GetSection("ExpirationInMinutes").Value;
        }

        private string GenerateSecurityToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email,email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
