using Lab3Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.ViewModels
{
    public class CommentGetModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public int? MovieId { get; set; }

        public static CommentGetModel FromComment(Comment c)
        {
            return new CommentGetModel
            {
                Id = c.Id,
                MovieId = c.Movie?.Id,
                Important = c.Important,
                Text = c.Text
            };
        }
    }
}
