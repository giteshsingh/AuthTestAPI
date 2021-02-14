using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AuthTestAPI.Services
{
    public class JwtServices
    {
        private readonly string _secret;
        private readonly int _expDate;
        private readonly string _issuer;
        private IConfiguration configuration;
        public JwtServices(IConfiguration config)
        {
            configuration = config;
            _secret = config.GetValue<string>("JwtConfig:secret");
            _expDate = config.GetValue<int>("JwtConfig:expirationInMinutes");
            _issuer = config.GetValue<string>("JwtConfig:Issuer");
        }

        public string GenerateSecurityToken(string email)
        {
            //string signingKey = config.GetValue<string>("Jwt:Key");
            //string issuer = configuration.GetValue<string>("Jwt:Issuer");
            //int hours = configuration.GetValue<int>("Jwt:HoursValid");
            System.DateTime expireDateTime = System.DateTime.UtcNow.AddMinutes(_expDate);

            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(_secret);
            SymmetricSecurityKey secKey = new SymmetricSecurityKey(signingKeyBytes);
            SigningCredentials creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
                                     {
                                          new Claim(ClaimTypes.Email, email)
                                     };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims: authClaims,
                expires: System.DateTime.UtcNow.AddMinutes(_expDate),
                signingCredentials: creds
            );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string writtenToken = handler.WriteToken(token);

            return writtenToken;
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.Email,email)
            //    }),
            //    Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),                
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
        }
    }
}
