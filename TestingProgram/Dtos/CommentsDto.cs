using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Dtos
{
    public class CommentsDto
    {
        public int id { get; set; }
        public int Problem_Id { get; set; }
        public string User_Id { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }


    }
    public class CommentsUpdate
    {
        public int id { get; set; }
        public int Problem_Id { get; set; }
        public string Content { get; set; }
        public string  Date { get; set; }

    }
}
