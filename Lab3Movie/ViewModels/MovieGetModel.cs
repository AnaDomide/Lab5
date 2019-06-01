using Lab3Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.ViewModels
{
    public class MovieGetModel
    {
        public string Title { get; set; }
        public int DurationInMinutes { get; set; }
        public string Director { get; set; }
        public int YearOfRelease { get; set; }
        public int Rating { get; set; }
        public DateTime DateAdded { get; set; }
        public int NumberOfComments { get; set; }

        public static MovieGetModel FromMovie(Movie movie)

        {

            return new MovieGetModel
            {
                Title = movie.Title,
                DurationInMinutes = movie.DurationInMinutes,
                Director = movie.Director,
                YearOfRelease = movie.YearOfRelease,
                Rating = movie.Rating,
                DateAdded = movie.DateAdded,
                NumberOfComments = movie.Comments.Count
            };
        }
    }
}