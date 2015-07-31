using System;  //These 4 will be used in all of your C# model classes.
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
 


namespace Hussain___Website.Models
{
    public class Post
    {
        public Post() 
        {
            this.Comments = new HashSet<Comment>();
        }
        public int Id { get; set; }
        public System.DateTimeOffset CreationDate { get; set; }
        public Nullable<DateTimeOffset> UpdatedDate { get; set; }
        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        [AllowHtml]
        [Required]
        public string Body { get; set; }
        public string MediaURL { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }

    public class Comment
    {
        public int Id { get; set; }
        public int PostId {get; set;}
        public string AuthorId { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public Nullable<DateTimeOffset> Updated { get; set; }
        public virtual Post Post { get; set; }
        public virtual ApplicationUser Author {get; set;}
    }
}




