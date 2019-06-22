using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<UserUserRole> UserUserRole { get; set; }
        public User Owner { get; set; }
    }
}
