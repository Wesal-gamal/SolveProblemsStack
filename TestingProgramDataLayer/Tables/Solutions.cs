using DataLayer.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class Solutions
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Problem")]
        public int Problem_Id { get; set; }
        public virtual Problems Problem { get; set; }

        [ForeignKey("user")]
        public string User_Id { get; set; }
        public virtual ApplicationUser user { get; set; }

        public string Content { get; set; }
        public virtual ICollection<CommentLikes> CommentLikes { get; set; }

        public virtual ICollection<SolutionLikes> SolutionLikes { get; set; }
        public DateTime Date { get; set; }

    }
}
