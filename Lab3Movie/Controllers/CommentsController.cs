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
    public class CommentsController : ControllerBase
    {
        private ICommentService commentsService;
        private IUsersService usersService;

        public CommentsController(ICommentService commentsService, IUsersService usersService)
        {
            this.commentsService = commentsService;
            this.usersService = usersService;
        }

        /// <summary>
        /// Get all comments.
        /// </summary>
        /// <param name="filterString">Optional, filter by text</param>
        /// <param name="page">Page</param>
        /// <returns>List of comments</returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // GET: api/Comments
        [HttpGet]
        public PaginatedList<CommentGetModel> Get([FromQuery]string filterString, [FromQuery]int page = 1)
        {
            page = Math.Max(page, 1);
            return commentsService.GetAll(page, filterString);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Get(int id)
        {
            var found = commentsService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Regular")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public void Post([FromBody] CommentPostModel comment)
        {
            User addedBy = usersService.GetCurentUser(HttpContext);
            commentsService.Create(comment, addedBy);
        }

        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            var result = commentsService.Upsert(id, comment);
            return Ok(result);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Delete(int id)
        {
            var result = commentsService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


    }
}
