using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
              public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Image { get; set; }
          public string Email { get; set; }
        public string Password { get; set; }
        public int IsAdmin { get; set; }


    }
}