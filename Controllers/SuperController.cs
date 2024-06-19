using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Models;
using System;
using Context;

namespace Controllers
{
    public class SuperController<T> : ControllerBase where T : class
    {

        protected readonly MyContext _context;

        public SuperController(MyContext context)
        {
            _context = context;
        }


     [HttpGet("")]
        public virtual async Task<IActionResult> GetAll()
        {
            var model = await _context.Set<T>().ToListAsync();
           
            return Ok(new {items=model});


        }



        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);
            if (model != null)
            {

                _context.Set<T>().Remove(model);
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
        [HttpPost]
        public virtual async Task<IActionResult> Post(T model)
        {
            await _context.Set<T>().AddAsync(model);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new{m="success"});
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var model = await _context.Set<T>().FindAsync(id);

            return Ok(model);
        }
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] T model)
        {
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { m = "success" });
        }





    }
}