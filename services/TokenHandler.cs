using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Services
{
     public class TokenHandlerService
    {
        private readonly AppSettings _appSettings;
        public TokenHandlerService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string GenerateTokken(Claim[] claims)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            // var key = Encoding.ASCII.GetBytes("this is the secret phrase");
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            // var claims = new Claim[]
            //     {
            //             new Claim(ClaimTypes.Name, user.Id.ToString()),
            //             new Claim(ClaimTypes.Email, user.Email),
            //             new Claim(ClaimTypes.Role, role.Id.ToString())
            //     };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // Expires = DateTime.UtcNow.AddDays(7),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var createToken = tokenHandler.CreateToken(tokenDescriptor);

            // Console.WriteLine(createToken.ValidTo);
            // Console.WriteLine(createToken.ValidFrom);

            return tokenHandler.WriteToken(createToken);
        }
    }
}