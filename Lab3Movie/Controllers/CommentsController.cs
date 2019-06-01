using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3Movie.Services;
using Lab3Movie.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab3Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
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
            return commentService.GetAll(page, filterString);
        }
    }
}
