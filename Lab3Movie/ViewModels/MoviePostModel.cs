using Lab3Movie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.ViewModels
{
    public class MoviePostModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public int YearOfRelease { get; set; }
        public string Director { get; set; }
        [Range(1, 10)]
        public int Rating { get; set; }
        public string Watched { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Comment> Comments { get; set; }

        public static Movie ToMovie(MoviePostModel movie)
        {
            Models.Genre genre = Models.Genre.action;

            if (movie.Genre == "comedy")
            {
                genre = Models.Genre.comedy;
            }
            else if (movie.Genre == "horror")
            {
                genre = Models.Genre.horror;
            }
            else
            {
                genre = Models.Genre.thriller;
            }

            Models.Watched watched = Models.Watched.yes;

            if (movie.Watched == "no")
            {
                watched = Models.Watched.no;
            }

            return new Movie
            {
                Title = movie.Title,
                Description = movie.Description,
                Genre = genre,
                DurationInMinutes = movie.DurationInMinutes,
                YearOfRelease = movie.YearOfRelease,
                Director = movie.Director,
                Rating = movie.Rating,
                Watched = watched,
                DateAdded = movie.DateAdded,
                Comments = movie.Comments
            };

        }
    }
}