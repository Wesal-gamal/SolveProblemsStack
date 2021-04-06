using DataLayer.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Identity
{
   public partial class MyDBContext
    {
        public virtual DbSet<Categories> Category { get; set; }
        public virtual DbSet<Problems> Problem { get; set; }
        public virtual DbSet<Comments> Comment { get; set; }
        public virtual DbSet<CommentLikes> CommentLike { get; set; }
        public virtual DbSet<SolutionLikes> SolutionLike { get; set; }
        public virtual DbSet<Solutions> Solutions { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        
    }
}
