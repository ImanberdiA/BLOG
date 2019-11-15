using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public virtual AppUser Author { get; set; }

        public int? AuthorId { get; set; }
    }
}