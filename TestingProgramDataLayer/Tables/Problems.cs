using DataLayer.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Tables
{
    public class Problems
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("category")]
        public int Cat_Id { get; set; }
        public virtual Categories  category { get; set; }

        [ForeignKey("user")]
        public string User_Id { get; set; }       
        public virtual ApplicationUser user{ get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public int? Solved { get; set; }
        public DateTime Date { get; set; }


    }
}
