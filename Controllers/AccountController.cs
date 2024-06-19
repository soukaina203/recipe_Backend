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
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AccountController : SuperController<User>
    {
        private readonly TokenHandler _tokkenHandler;
        private readonly Crypto _crypto;
        private readonly MyContext _context;

           public AccountController(MyContext context
               , TokenHandler tokkenHandler
            , Crypto crypto
           ) : base(context)
        {
              _tokkenHandler = tokkenHandler;
            _crypto = crypto;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
{
            var emailExiste = await _context.Users.FirstOrDefaultAsync(e => e.Email == user.Email);
            if (emailExiste != null)
            {
                return Ok(new { code = -1, message = "Email deja exister" });
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
                return Ok(new { code = 1, message = "Register Successful" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Ok(new { code = -2, message = ex.Message });

            }        
}
 [HttpPost]
        public async Task<IActionResult> Login(UserDTO model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return Ok(new { message = "Email | password required", code = -4 });
            }
            var User = await _context.Users.Where(x => x.Email == model.Email).AsNoTracking().FirstOrDefaultAsync();
            var newHash = _crypto.HashPassword(model.Password);
            if (User == null)
            {
                return Ok(new { message = "Email error ", code = -3 });
            }
            if (newHash != User.Password)
            {
                return Ok(new { message = "Error Password", code = -1 });
            }
            var claims = new Claim[]
                           {
                        new(ClaimTypes.Name, User.Id.ToString()),
                        new(ClaimTypes.Email, User.Email)};

            var token = _tokkenHandler.GenerateTokken(claims);
            return Ok(new { User, Token = token, Code = 1 });
        }

    }    public class UserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }



    }
