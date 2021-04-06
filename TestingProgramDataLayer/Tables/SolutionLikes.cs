using DataLayer.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class SolutionLikes
    {
        public int Solution_Id { get; set; }
        [ForeignKey("Solution_Id")]
        public virtual Solutions Solution { get; set; }

        public bool Like { get; set; }

        public bool Dislike { get; set; }

        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public virtual ApplicationUser user { get; set; }
    }
}
