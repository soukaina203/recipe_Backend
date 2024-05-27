using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Models
{
    public class Like
    {
        public int Id { get; set; }

        public int IdUser { get; set; }
        public virtual User? User { get; set; }
        public int IdRecipe { get; set; }
        public virtual Recipe? Recipe { get; set; }
        
    }
}