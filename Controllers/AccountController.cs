using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using Models;
using Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Context;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : SuperController<User>
    {
        private readonly TokenHandlerService _tokkenHandler;
        private readonly Crypto _crypto;
        private readonly MyContext _context;

        public AccountController(MyContext context, TokenHandlerService tokkenHandler, Crypto crypto) : base(context)
        {
            _tokkenHandler = tokkenHandler;
            _crypto = crypto;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous] // Allow anonymous access to the Register endpoint
        public async Task<IActionResult> Register(User user)
        {
            var emailExiste = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);
            if (emailExiste != null)
            {
                return Ok(new { code = -1, message = "Email already exists" });
            }
            user.Password = _crypto.HashPassword(user.Password);
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                };
                var token = _tokkenHandler.GenerateTokken(claims);
                return Ok(new { code = 1, message = "Register Successful", Token = token });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Ok(new { code = -2, message = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous] // Allow anonymous access to the Login endpoint
        public async Task<IActionResult> Login(UserDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Ok(new { message = "Email | password required", code = -4 });
            }
            var user = await _context.Users.Where(x => x.Email == model.Email).AsNoTracking().FirstOrDefaultAsync();
            var newHash = _crypto.HashPassword(model.Password);

            if (user == null)
            {
                return Ok(new { message = "Email error", code = -3 });
            }
            if (newHash != user.Password)
            {
                return Ok(new { message = "Error Password", code = -1 , oldPwd=user , newOne = newHash });
            }
            var claims = new Claim[]
            {
                new(ClaimTypes.Name, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email)
            };

            var token = _tokkenHandler.GenerateTokken(claims);
            return Ok(new { user, Token = token, code = 1 });
        }
    }

    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
