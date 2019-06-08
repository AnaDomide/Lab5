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
        PaginatedList<CommentGetModel> GetAll(int page, string filterString);
        Comment Create(CommentPostModel task, User addedBy);

        Comment Upsert(int id, Comment comment);

        Comment Delete(int id);

        Comment GetById(int id);
    }
    public class CommentService : ICommentService
    {
        private MoviesDbContext context;
        public CommentService(MoviesDbContext context)
        {
            this.context = context;
        }

        public PaginatedList<CommentGetModel> GetAll(int page, string filterString)
        {
            IQueryable<Comment> result = context
                .Comments
                .Where(c => string.IsNullOrEmpty(filterString) || c.Text.Contains(filterString))
                .OrderBy(c => c.Id)
                .Include(c => c.Movie);
            var paginatedResult = new PaginatedList<CommentGetModel>();
            paginatedResult.CurrentPage = page;

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<CommentGetModel>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<CommentGetModel>.EntriesPerPage)
                .Take(PaginatedList<CommentGetModel>.EntriesPerPage);
            paginatedResult.Entries = result.Select(c => CommentGetModel.FromComment(c)).ToList();

            return paginatedResult;
        }

        public Comment Create(CommentPostModel comment, User addedBy)
        {
            Comment commentAdd = CommentPostModel.ToComment(comment);
            commentAdd.Owner = addedBy;
            context.Comments.Add(commentAdd);
            context.SaveChanges();
            return commentAdd;
        }

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

        public Comment GetById(int id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment Upsert(int id, Comment comment)
        {
            var existing = context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (existing == null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return comment;

            }

            comment.Id = id;
            context.Comments.Update(comment);
            context.SaveChanges();
            return comment;
        }
    }

}

