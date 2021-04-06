using DataLayer.Tables;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Identity
{
   public class ApplicationUser: IdentityUser
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CommentLikes> CommentLikes { get; set; }
    }
}
