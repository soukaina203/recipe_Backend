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
    public class RecipeController : SuperController<Recipe>
    {
          public RecipeController(MyContext context) : base(context)
        {
        }

                [HttpGet("")]
        public override async Task<IActionResult> GetAll()
        {
            // Custom implementation for RecipeController
            var recipes = await _context.Recipes
                                        .Include(r => r.User) // Assuming a relationship with Ingredients
                                        .Include(r => r.Category) // Assuming a relationship with Ingredients
                                        .ToListAsync();
            return Ok(new { items = recipes });
        }

    [HttpGet("{id}")]
    public override async Task<IActionResult> GetById(int id)
        {
            // Custom implementation for RecipeController
   var model = await _context.Recipes
                              .Include(r => r.Category)
                              .FirstOrDefaultAsync(r => r.Id == id);
            return Ok(model);
        }



    }
}