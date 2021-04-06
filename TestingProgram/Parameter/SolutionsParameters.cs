using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Parameter
{
    public class SolutionsParameters
    {
        public int Problem_Id { get; set; }
        public string Content { get; set; }
    }

    public class SolutionsParametersGet
    {
        public int Id { get; set; }
        public string User_Id { get; set; }
        public int Problem_Id { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

    }

    public class SolutionsParametersUpadte
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }

    }

}
