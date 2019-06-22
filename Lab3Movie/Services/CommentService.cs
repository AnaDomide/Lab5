using Lab3Movie.Models;
using Lab3Movie.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Services
{
    public interface ICommentService
    {
        PaginatedList<CommentGetModel> GetAll(int page, string filter);
        Comment GetById(int id);
        Comment Create(CommentPostModel expense, int id);
        Comment Delete(int id);
    }
    public class CommentService : ICommentService
    {
        private MoviesDbContext context;
        public CommentService(MoviesDbContext context)
        {
            this.context = context;
        }

        public PaginatedList<CommentGetModel> GetAll(int page, string filter)
        {
            IQueryable<Comment> result = context
                .Comments
                .Where(c => string.IsNullOrEmpty(filter) || c.Text.Contains(filter))
                .OrderBy(c => c.Id)
                .Include(c => c.Movie);
            var paginatedResult = new PaginatedList<CommentGetModel>
            {
                CurrentPage = page,

                NumberOfPages = (result.Count() - 1) / PaginatedList<CommentGetModel>.EntriesPerPage + 1
            };
            result = result
                .Skip((page - 1) * PaginatedList<CommentGetModel>.EntriesPerPage)
                .Take(PaginatedList<CommentGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(c => CommentGetModel.FromComment(c)).ToList();

            return paginatedResult;
        }
        public Comment Create(CommentPostModel comment, int id)
        {
            Comment toAdd = CommentPostModel.ToComment(comment);
            Movie movie = context.Movies.FirstOrDefault(e => e.Id == id);
            movie.Comments.Add(toAdd);
            context.SaveChanges();
            return toAdd;


        }
        public Comment GetById(int id)
        {
            return context.Comments
                .FirstOrDefault(c => c.Id == id);
        }

        //public Comment Upsert(int id, Comment expense)
        //{
        //    var existing = context.Comment.AsNoTracking().FirstOrDefault(c => c.Id == id);
        //    if (existing == null)
        //    {
        //        context.Comment.Add(expense);
        //        context.SaveChanges();
        //        return expense;
        //    }
        //    expense.Id = id;
        //    context.Comment.Update(expense);
        //    context.SaveChanges();
        //    return expense;
        //}


        public Comment Delete(int id)
        {
            var existing = context.Comments.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comments.Remove(existing);
            context.SaveChanges();
            return existing;
        }

    }
}