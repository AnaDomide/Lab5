using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Models
{
    public class MoviesDbSeeder
    {
        public static void Initialize(MoviesDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any movies.
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            context.Movies.AddRange(
                new Movie
                {
                    Title = "Movie1",
                    Description = "Description1",
                    Genre = Genre.action,
                    DurationInMinutes = 120,
                    YearOfRelease = 2017,
                    Director = "Director1",
                    Rating = 10,
                    Watched = Watched.yes

                },
                new Movie
                {
                    Title = "Movie2",
                    Description = "Description2",
                    Genre = Genre.comedy,
                    DurationInMinutes = 110,
                    YearOfRelease = 2018,
                    Director = "Director2",
                    Rating = 9,
                    Watched = Watched.no
                }
            );
            context.SaveChanges();
        }
    }
}