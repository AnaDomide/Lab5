using Lab3Movie.Models;
using Lab3Movie.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    class MoviesServiceTests
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new MoviesDbContext(options))
            {


                var movieService = new MovieService(context);
                var addedTask = movieService.Create(new Lab3Movie.ViewModels.MoviePostModel
                {
                    Title = "movie 1",
                    Description = "agfas",
                    Genre = "comedy",
                    DurationInMinutes = 100,
                    YearOfRelease = 2019,
                    Director = "director1",
                    Rating = 10,
                    Watched = "yes",
                    DateAdded = new DateTime(),
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {

                            Text = "text",
                            Important = true,
                            Owner = null
                        }
                    },

                }, null);

                var allMovies = movieService.GetAll(1);
                Assert.NotNull(allMovies);
            }
        }

        [Test]
        public void CreateMovieAndGetByIdTest()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateMovieAndGetByIdTest))
              .Options;

            using (var context = new MoviesDbContext(options))
            {


                var movieService = new MovieService(context);
                var addedMovie = movieService.Create(new Lab3Movie.ViewModels.MoviePostModel
                {
                    Title = "movie 1",
                    Description = "agfas",
                    Genre = "comedy",
                    DurationInMinutes = 100,
                    YearOfRelease = 2019,
                    Director = "director1",
                    Rating = 10,
                    Watched = "yes",
                    DateAdded = new DateTime(),
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {

                            Text = "text",
                            Important = true,
                            Owner = null
                        }
                    },

                }, null);

                var movieCreated = movieService.GetById(addedMovie.Id);
                Assert.NotNull(movieCreated);
                Assert.AreEqual(addedMovie, movieCreated);
            }
        }

        [Test]
        public void DeleteMovieTest()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteMovieTest))
              .Options;

            using (var context = new MoviesDbContext(options))
            {


                var movieService = new MovieService(context);
                var addedMovie = movieService.Create(new Lab3Movie.ViewModels.MoviePostModel
                {
                    Title = "movie 1",
                    Description = "agfas",
                    Genre = "comedy",
                    DurationInMinutes = 100,
                    YearOfRelease = 2019,
                    Director = "director1",
                    Rating = 10,
                    Watched = "yes",
                    DateAdded = new DateTime(),
                    Comments = new List<Comment>()
                    {
                        new Comment
                        {

                            Text = "text",
                            Important = true,
                            Owner = null
                        }
                    },

                }, null);

                var movieDeleted = movieService.Delete(addedMovie.Id);
                Assert.IsNotNull(movieDeleted);
            }
        }
    }
}