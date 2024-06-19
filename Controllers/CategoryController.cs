using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Context;
using Controllers;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : SuperController<Category>
    {
          public CategoryController(MyContext context) : base(context)
        {
        }
    }
}