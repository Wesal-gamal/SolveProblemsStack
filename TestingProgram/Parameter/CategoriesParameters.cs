using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Parameter
{
    public class CategoriesParameters
    {
        public string Name { get; set; }
    }

    public class CategoriesParametersUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
