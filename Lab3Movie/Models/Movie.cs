using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Models
{
    public enum Genre
    {
        action, comedy, horror, thriller
    }

    public enum Watched
    {
        yes, no
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [EnumDataType(typeof(Genre))]
        public Genre Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public int YearOfRelease { get; set; }
        public string Director { get; set; }
        [Range(1, 10)]
        public int Rating { get; set; }
        [EnumDataType(typeof(Watched))]
        public Watched Watched { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Comment> Comments { get; set; }
        public User Owner { get; set; }
    }
}
