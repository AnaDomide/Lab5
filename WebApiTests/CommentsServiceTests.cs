using Lab3Movie.Models;
using Lab3Movie.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    public class CommentsServiceTests
    {
        [Test]
        public void GetAllShouldReturnCorrectNumberOfPages()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPages))
              .Options;

            using (var context = new MoviesDbContext(options))
            {

                var commentService = new CommentService(context);
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

                var allComments = commentService.GetAll(1, string.Empty);
                Assert.AreEqual(allComments.NumberOfPages, 1);
            }
        }

        [Test]
        public void GetByIdTest()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdTest))
              .Options;

            using (var context = new MoviesDbContext(options))
            {

                var commentService = new CommentService(context);
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

                var addedComment = commentService.Create(new Lab3Movie.ViewModels.CommentPostModel
                {
                    Important = true,
                    Text = "asd",
                }, addedMovie.Id);

                var comment = commentService.GetById(addedComment.Id);
                Assert.NotNull(comment);
            }
        }

        [Test]
        public void DeleteTest()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteTest))
              .Options;

            using (var context = new MoviesDbContext(options))
            {

                var commentService = new CommentService(context);
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


                var addedComment = commentService.Create(new Lab3Movie.ViewModels.CommentPostModel
                {
                    Important = true,
                    Text = "fdlkflsdkm",
                }, addedMovie.Id);

                var comment = commentService.Delete(addedComment.Id);
                var commentNull = commentService.Delete(17);
                Assert.IsNull(commentNull);
                Assert.NotNull(comment);
            }
        }


    }
}

