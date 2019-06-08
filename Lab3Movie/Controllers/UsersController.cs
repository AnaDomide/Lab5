using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3Movie.Models;
using Lab3Movie.Services;
using Lab3Movie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab3Movie.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService userService;

        public UsersController(IUsersService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var user = userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "Username already exists." });
            }
            return Ok(user);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>A list of all users</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,UserManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserGetModel> GetAll()
        {
            return userService.GetAll();

        }


        /// <summary>
        /// Find an user by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /users
        ///     {  
        ///        id: 3,
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>The user with the given id</returns>

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,UserManager")]
        public IActionResult Get(int id)
        {
            var found = userService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new User
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /users
        ///     {
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <param name="userPostModel">The input user to be added</param>

        [Authorize(Roles = "Admin,UserManager")]
        [HttpPost]
        public void Post([FromBody] UserPostModel userPostModel)
        {
            userService.Create(userPostModel);
        }



        /// <summary>
        /// Modify user if exists in dbSet , or add if not exist
        /// </summary>
        /// <param name="id">Id user to update</param>
        /// <param name="userPostModel">Object userPostModel to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /users/id
        ///     {
        ///        firstName = "Domide",
        ///        lastName = "Ana",
        ///        userName = "user123",
        ///        email = "ana@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostModel userPostModel)
        {
           
            User currentLogedUser = userService.GetCurentUser(HttpContext);
            var regDate = currentLogedUser.DataRegistered;
            var currentDate = DateTime.Now;
            var minDate = currentDate.Subtract(regDate).Days / (365 / 12);

            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser == null)
                {
                    return NotFound();
                }

            }

            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser.UserRole == UserRole.Admin)
                {
                    return Forbid();
                }


            }

            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser.UserRole == UserRole.UserManager && minDate <= 6)

                    return Forbid();
            }

            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser.UserRole == UserRole.UserManager && minDate >= 6)
                {
                    var result1 = userService.Upsert(id, userPostModel);
                    return Ok(result1);
                }

            }

            var result = userService.Upsert(id, userPostModel);
            return Ok(result);


        }


        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           
            User currentLogedUser = userService.GetCurentUser(HttpContext);
            var regDate = currentLogedUser.DataRegistered;
            var currentDate = DateTime.Now;
            var minDate = currentDate.Subtract(regDate).Days / (365 / 12);

            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser.UserRole == UserRole.Admin)
                {
                    return Forbid();
                }

            }

            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser.UserRole == UserRole.UserManager && minDate <= 6)

                    return Forbid();
            }
            if (currentLogedUser.UserRole == UserRole.UserManager)
            {
                UserGetModel getUser = userService.GetById(id);
                if (getUser.UserRole == UserRole.UserManager && minDate >= 6)
                {
                    var result1 = userService.Delete(id);
                    return Ok(result1);
                }


            }

            var result = userService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }


            return Ok(result);
        }
    }
}

