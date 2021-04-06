using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Business
{
   public  interface ISolutionsBusiness
    {
        Task<bool> AddSolutionsLikes(int solutionId, bool like, bool disLike);
        string GetUserId();
    }
}
