using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Services;
using Microsoft.Extensions.Options;

namespace Services
{
    public class Crypto
    {
        private AppSettings _appSettings;
        public Crypto(IOptions<AppSettings> appSettings) {
             _appSettings = appSettings.Value;
        }
        public string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(_appSettings.Secret),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 9999,
                numBytesRequested: 256 / 8));
        }
        
    }
}