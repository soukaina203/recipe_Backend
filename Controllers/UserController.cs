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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : SuperController<User>
    {

      public UserController(MyContext context) : base(context)
        {
        }

        [HttpPost]
        public override async Task<IActionResult> Post( User model)
        {
            //  var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\users\\", model.Image);

            //     var provider = new FileExtensionContentTypeProvider();
            //     if (!provider.TryGetContentType(filepath, out var contenttype))
            //     {
            //         contenttype = "application/octet-stream";
            //     }
            await _context.Users.AddAsync(model);
                await _context.SaveChangesAsync();

                return Ok(new {model.Id });



                // var bytes = await System.IO.File.ReadAllBytesAsync(filepath);

                // return File(bytes, contenttype, Path.GetFileName(filepath));
          
        }






 
    }
    }
