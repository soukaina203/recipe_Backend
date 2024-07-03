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

[HttpGet("{userId}")]
public async Task<IActionResult> GetLikedRecipes(int userId)
{
  

    var likes = await _context.Likes.Where(e => e.IdUser == userId).ToListAsync();

    if (likes == null || !likes.Any())
    {
        return NotFound("No likes found for the specified user.");
    }

    var recipeIds = likes.Select(e => e.IdRecipe).ToList();

    if (recipeIds == null || !recipeIds.Any())
    {
        return NotFound("No recipe IDs found for the likes.");
    }

    var recipes = await _context.Recipes.Where(r => recipeIds.Contains(r.Id)).ToListAsync();

    if (recipes == null || !recipes.Any())
    {
        return NotFound("No recipes found for the specified recipe IDs.");
    }

    return Ok(recipes);
}


[HttpGet("")]
public async Task<IActionResult> GetTopRecipe()
{
    // Get the top 3 recipe Ids based on the count of likes
    var topRecipeIds = await _context.Likes
        .GroupBy(l => l.IdRecipe)
        .Select(g => new 
        {
            IdRecipe = g.Key,
            Count = g.Count()
        })
        .OrderByDescending(g => g.Count)
        .Take(3)
        .Select(g => g.IdRecipe)
        .ToListAsync();

    // Fetch the recipe details for these top 3 recipes
    var topRecipes = await _context.Recipes
        .Where(r => topRecipeIds.Contains(r.Id))
        .ToListAsync();

    return Ok(topRecipes);
}


    }
}