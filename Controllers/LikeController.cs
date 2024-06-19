using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Context;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LikeController : SuperController<Like>
    {
          public LikeController(MyContext context) : base(context)
        {
        }
    }
}