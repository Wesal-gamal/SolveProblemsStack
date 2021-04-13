using Attendleave.Erp.Core.UnitOfWork;
using DataLayer.Tables;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Business
{
    public class solutionsBusiness : ISolutionsBusiness
    {
        private readonly IUnitOfWork<SolutionLikes> _unitofworkSolutionsLikes;
        private readonly IHttpContextAccessor _httpContextAccessor;

       
        public solutionsBusiness(IUnitOfWork<SolutionLikes> unitofworkSolutionsLikes,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _unitofworkSolutionsLikes = unitofworkSolutionsLikes;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.First(t => t.Type == "UserId").Value;
            return userId;
        }
        public async Task<bool> AddSolutionsLikes(int solutionId, bool like, bool disLike)
        {
            SolutionLikes solLikes = new SolutionLikes();
            solLikes.Solution_Id = solutionId;
            solLikes.User_Id = GetUserId();
            solLikes.Like = like;
            solLikes.Dislike = disLike;
            _unitofworkSolutionsLikes.Repository.Add(solLikes);
            var saved = await _unitofworkSolutionsLikes.SaveChanges() > 0;
            return saved;
           
        }
    }
}
