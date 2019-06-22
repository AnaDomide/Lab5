using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Models
{
  
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
       // public List<Movie> Movies { get; set; }
       
        public DateTime DataRegistered { get; set; }
        public bool isRemoved { get; set; }
        public IEnumerable<UserUserRole> UserUserRole { get; set; }
    }
}
