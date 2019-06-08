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
using Microsoft.EntityFrameworkCore;

namespace Lab3Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMovieService movieService;
        private IUsersService usersService;

        public MoviesController(IMovieService movieService, IUsersService usersService)
        {
            this.movieService = movieService;
            this.usersService = usersService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public PaginatedList<MovieGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to,[FromQuery] int page = 1)
        {
            // TODO: make pagination work with /api/flowers/page/<page number>
            page = Math.Max(page, 1);
            return movieService.GetAll(page,from, to);
            //IQueryable<Movie> result = context.Movies.Include(c => c.Comments).OrderByDescending(m => m.YearOfRelease);
            //if (from == null && to == null)
            //{
            //    return result;
            //}
            //if (from != null)
            //{
            //    result = result.Where(m => m.DateAdded >= from);
            //}
            //if (to != null)
            //{
            //    result = result.Where(m => m.DateAdded <= to);
            //}
            //return result;
        }


       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Movies/5
        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var found = movieService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }


        ///<remarks>
        /// {
        /// "title": "Movie9",
        /// "description": "Description9",
        /// "genre": 1,
        /// "durationInMinutes": 110,
        /// "yearOfRelease": 2016,
        /// "director": "Director9",
        /// "rating": 8,
        /// "watched": 0,
        /// "dateAdded": "2019-06-06T00:00:00",
        ///  "comments": [
        ///        {
        ///          "text": "another comment",
        ///          "important": true
        ///       }  
        ///    ]
        ///  }
        ///</remarks>
        /// <summary>
        /// Add movie to database.
        /// </summary>
        /// <param name="movie">Movie to add.</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // POST: api/Movies
        [Authorize(Roles = "Admin,Regular")]
        [HttpPost]

        public void Post([FromBody] MoviePostModel movie)
        {
            //if (!ModelState.IsValid)
            //{

            //}
            //context.Movies.Add(movie);
            //context.SaveChanges();
            // NEXT TIME: folosirea permisiunilor.
            User addedBy = usersService.GetCurentUser(HttpContext);
            //if (addedBy.UserRole == UserRole.UserManager)
            //{
            //    return Forbid();
            //}
            movieService.Create(movie, addedBy);
           // movieService.Create(movie);
        }

        /// <summary>
        /// Update movie from database.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="movie"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // PUT: api/Movies/5
        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Movie movie)
        {
            //var existing = context.Movies.AsNoTracking().FirstOrDefault(m => m.Id == id);
            //if (existing == null)
            //{
            //    context.Movies.Add(movie);
            //    context.SaveChanges();
            //    return Ok(movie);
            //}
            //movie.Id = id;
            //context.Movies.Update(movie);
            //context.SaveChanges();
            //return Ok(movie);
            var result = movieService.Upsert(id, movie);
            return Ok(result);
        }


        /// <summary>
        /// Delete movie from database.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "Admin,Regular")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var existing = context.Movies.FirstOrDefault(movie => movie.Id == id);
            //if (existing == null)
            //{
            //    return NotFound();
            //}
            //context.Movies.Remove(existing);
            //context.SaveChanges();
            //return Ok();
            var result = movieService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
