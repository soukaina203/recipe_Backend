using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
namespace Models
{
    public class Recipe
    {
        public int Id { get; set; }
              public string Title { get; set; }
              public string Description { get; set; }
              public string Ingredients { get; set; }
              public string Instructions { get; set; }
              public string Image { get; set; }
        

        public int IdUser { get; set; }
        public virtual User? User { get; set; }

        
        public int IdCategory { get; set; }
        public virtual Category? Category { get; set; }
    }
}