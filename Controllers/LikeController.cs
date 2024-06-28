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
    public class LikeController : SuperController<Like>
    {
          public LikeController(MyContext context) : base(context)
        {
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetLikesOfaRecipe(int id)
        {
         var count = await _context.Likes.CountAsync(e => e.IdRecipe == id);
         var Likes = await _context.Likes.Where(e => e.IdRecipe == id).ToListAsync();

            return Ok(new{count=count , likes=Likes});
        }

           [HttpDelete("{userId}/{recipeId}")]
        public  async Task<IActionResult> DeleteLike(int userId,int recipeId )
        {
    var model = await _context.Likes
                            .FirstOrDefaultAsync(e => e.IdUser == userId && e.IdRecipe == recipeId);
            if (model != null)
            {

                _context.Likes.Remove(model);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest(new { message = ex.Message });
                }

                return Ok(true);
            }
            return Ok(false);


        }

    }
}