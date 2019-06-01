using Lab3Movie.Models;
using Lab3Movie.Services;
using Lab3Movie.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class UsersServiceTests
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadhjcghduihdfhdifd8ih"
            });
        }


        [Test]
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new Lab3Movie.ViewModels.RegisterPostModel
                    {
                    Email = "a@a.b",
                    FirstName = "fdsfsdfs",
                    LastName = "fdsfs",
                    Password = "1234567",
                    Username = "test_username"
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.Username);
            }
         }

        [Test]
        public void AuthenticateShouldLoginAUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLoginAUser))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new Lab3Movie.ViewModels.RegisterPostModel

                {
                    FirstName = "ana",
                    LastName = "domide",
                    Username = "ana",
                    Email = "ana@gmail.com",
                    Password = "1234567"
                };
                var result = usersService.Register(added);
                var authenticated = new Lab3Movie.ViewModels.LoginPostModel
                {
                    Username = "ana",
                    Password = "1234567"
                };
                var authresult = usersService.Authenticate(added.Username, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.Username);
            }
        }
        [Test]
        public void GetAllShouldReturnAllUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllUser))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added1 = new Lab3Movie.ViewModels.RegisterPostModel

                {
                    FirstName = "ana1",
                    LastName = "domide1",
                    Username = "ana1",
                    Email = "ana1@gmail.com",
                    Password = "1234567"
                };
                var added2 = new Lab3Movie.ViewModels.RegisterPostModel

                {
                    FirstName = "ana2",
                    LastName = "domide2",
                    Username = "ana2",
                    Email = "ana2@gmail.com",
                    Password = "1234567"
                };

                usersService.Register(added1);
                usersService.Register(added2);

                int numberOfElements = usersService.GetAll().Count();

                Assert.NotZero(numberOfElements);
                Assert.AreEqual(2, numberOfElements);
               
            }
        }
    }
}