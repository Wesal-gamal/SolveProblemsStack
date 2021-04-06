using DataLayer.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{

    public class CommentLikes
    {
     
        public int Comment_Id { get; set; }
        [ForeignKey("Comment_Id")]
        public virtual Comments Comment { get; set; }

        public bool Like { get; set; }

        public bool Dislike { get; set; }

        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser user { get; set; }



    }
}
