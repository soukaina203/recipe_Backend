using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Context;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class commentaireController : SuperController<Comment>
    {
          public commentaireController(MyContext context) : base(context)
        {
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> getCommentOfaRecipe(int id)
        {
         var count = await _context.Comments.CountAsync(e => e.IdRecipe == id);
         var comments = await _context.Comments.Include(u=>u.User).Where(e => e.IdRecipe == id).ToListAsync();

            return Ok(new{count=count , comments=comments});
        }    }
}