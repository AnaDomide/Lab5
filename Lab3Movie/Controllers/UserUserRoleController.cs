﻿using Lab3Movie.Services;
using Lab3Movie.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUserRolesController : ControllerBase
    {
        private IUserUserRoleService userUserRoleService;

        public UserUserRolesController(IUserUserRoleService userUserRoleService)
        {
            this.userUserRoleService = userUserRoleService;
        }


        /// <summary>
        /// Find an userUserRole by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /userUserRoles
        ///     [
        ///     {  
        ///        id: 3,
        ///        userId = 2,
        ///        UserRoleId = 3,
        ///        UserRole = "Regular",
        ///        StartTime = 2019-06-05,
        ///        EndTime = null
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>A list of userUserRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/UserUserRoles/5
        [HttpGet("{id}", Name = "GetUserUserRole")]
        public IActionResult Get(int id)
        {
            var found = userUserRoleService.GetHistoryRoleById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new UserUserRole
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Put /userUserRoles
        ///     {
        ///        userId = 1,
        ///        userRoleName = "UserManager"        
        ///     }
        /// </remarks>
        /// <param name="userUserRolePostModel">The input userUserRole to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] UserUserRolePostModel userUserRolePostModel)
        {
            userUserRoleService.Create(userUserRolePostModel);
        }
    }
}