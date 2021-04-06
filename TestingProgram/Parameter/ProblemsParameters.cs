using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Parameter
{
    public class ProblemsParameters
    {
        public int Cat_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Solved { get; set; }
    }
        public class ProblemsParametersGet
    {
        public int Cat_Id { get; set; }
        public string Title { get; set; }
        public string User_Id { get; set; }
        public string Description { get; set; }
        public int? Solved { get; set; }
    }
    public class ProblemsParametersGetAll
    {
        public int Id { get; set; }
        public int Cat_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Solved { get; set; }
    }

}
