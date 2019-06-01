using Lab3Movie.Models;
using Lab3Movie.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Services
{
    public interface IMovieService
    {
        PaginatedList<MovieGetModel> GetAll(int page, DateTime? from = null, DateTime? to = null);
        Movie GetById(int id);
        Movie Create(MoviePostModel movie,User addedBy);
        Movie Upsert(int id, Movie movie);
        Movie Delete(int id);
    }

    public class MovieService : IMovieService
    {
        //aici tb sa declar un DB context
        private MoviesDbContext context;
        //tb constructor
        public MovieService(MoviesDbContext context)
        {
            this.context = context;
        }

        //acum mutam logica din Controller pe Service. 
        //Nu il eliminam dar Controller-ul va apela Service si nu va mai apela UI-ul Service-ul

        public Movie Create(MoviePostModel movie,User addedBy)
        {
            Movie toAdd = MoviePostModel.ToMovie(movie);
            toAdd.Owner = addedBy;
            context.Movies.Add(toAdd);
            context.SaveChanges();
            return toAdd;
        }

        public Movie Delete(int id)
        {
            var existing = context.Movies.Include(x => x.Comments).FirstOrDefault(movie => movie.Id == id);
            //var existing = context.Movies.FirstOrDefault(movie => movie.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Movies.Remove(existing);
            context.SaveChanges();

            return existing;
        }
        public PaginatedList<MovieGetModel> GetAll(int page,DateTime? from = null, DateTime? to = null)
        {
            //IQueryable<Movie> result = context.Movies.Include(f => f.Comments);
            IQueryable<Movie> result = context
                .Movies
                .OrderBy(f => f.Id)
                .Include(c => c.Comments)
                .OrderByDescending(m => m.YearOfRelease);
            PaginatedList<MovieGetModel> paginatedResult = new PaginatedList<MovieGetModel>();
            paginatedResult.CurrentPage = page;

            if (from != null)
            {
                result = result.Where(f => f.DateAdded >= from);
            }
            if (to != null)
            {
                result = result.Where(f => f.DateAdded <= to);
            }

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<MovieGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<MovieGetModel>.EntriesPerPage)
                .Take(PaginatedList<MovieGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(m => MovieGetModel.FromMovie(m)).ToList();

            return paginatedResult;
        }

        public Movie GetById(int id)
        {
            // sau context.Movies.Find()
            return context.Movies
                .Include(f => f.Comments)
                .FirstOrDefault(f => f.Id == id);
        }


        public Movie Upsert(int id, Movie movie)
        {
            var existing = context.Movies.AsNoTracking().FirstOrDefault(f => f.Id == id);
            if (existing == null)
            {
                context.Movies.Add(movie);
                context.SaveChanges();
                return movie;
            }
            movie.Id = id;
            context.Movies.Update(movie);
            context.SaveChanges();
            return movie;
        }


    }

}

