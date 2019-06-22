using Lab3Movie.Models;
using Lab3Movie.Services;
using Lab3Movie.Validators;
using Lab3Movie.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiTests
{
    class UserUserRoleServiceTests
    {

        [Test]
        public void GetByIdShouldReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRole))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userUserRolesService = new UserUserRoleService(null, context);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    DataRegistered = DateTime.Now,
                    UserUserRole = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Rol testare",
                    Description = "Creat pentru testare"
                };
                context.UserRole.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRole.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                var userUserRoleGetModels = userUserRolesService.GetHistoryRoleById(1);
                Assert.IsNotNull(userUserRoleGetModels.FirstOrDefaultAsync(uur => uur.EndTime == null));
            }
        }



        [Test]
        public void CreateShouldAddTheUserUserRole()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddTheUserUserRole))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var validator = new UserRoleValidator();
                var userUserRolesService = new UserUserRoleService(validator, context);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    DataRegistered = DateTime.Now,
                    UserUserRole = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                UserRole addUserRoleAdmin = new UserRole
                {
                    Name = "AdminDeTest",
                    Description = "Creat pentru testare"
                };
                context.UserRole.Add(addUserRoleRegular);
                context.UserRole.Add(addUserRoleAdmin);
                context.SaveChanges();

                context.UserUserRole.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRoleRegular,
                    StartTime = DateTime.Parse("2019-06-13T00:00:00"),
                    EndTime = null
                });
                context.SaveChanges();

                //sectiunea de schimbare valori invalidata de catre UserRoleValidator
                var uurpm = new UserUserRolePostModel
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "Admin"
                };
                var result1 = userUserRolesService.Create(uurpm);
                Assert.IsNotNull(result1);   //User role nu exista in baza de date dupa validare, ==> exista erori la validare

                //sectiunea de schimbare valori validata de catre UserRoleValidator
                var uurpm1 = new UserUserRolePostModel
                {
                    UserId = userToAdd.Id,
                    UserRoleName = "AdminDeTest"
                };
                var result2 = userUserRolesService.Create(uurpm1);
                Assert.IsNull(result2);   //User role exista si se face upsert
            }
        }


        [Test]
        public void GetUserRoleNameByIdShouldReturnUserRoleName()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetUserRoleNameByIdShouldReturnUserRoleName))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userUserRolesService = new UserUserRoleService(null, context);

                User userToAdd = new User
                {
                    Email = "user@yahoo.com",
                    LastName = "Ion",
                    FirstName = "POpescu",
                    Password = "secret",
                    DataRegistered = DateTime.Now,
                    UserUserRole = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRole.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRole.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Parse("2019-06-13T00:00:00"),
                    EndTime = null
                });
                context.SaveChanges();

                string userRoleName = userUserRolesService.GetUserRoleNameById(1);
                Assert.AreEqual("Regular", userRoleName);
            }
        }


    }
}