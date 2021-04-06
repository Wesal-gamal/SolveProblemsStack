using DataLayer.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Solutions")]
        public int Solution_Id { get; set; }
        public virtual Solutions Solutions { get; set; }

        [ForeignKey("user")]
        public string User_Id { get; set; }
        public virtual ApplicationUser user { get; set; }

        public string Content { get; set; }
        public virtual ICollection<CommentLikes> CommentLikes { get; set; }

        public DateTime Date { get; set; }

    }
}
