using Attendleave.Erp.Core.APIUtilities;
using Attendleave.Erp.Core.UnitOfWork;
using DataLayer.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingProgram.Business;
using TestingProgram.Parameter;

namespace TestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionsController : ApiControllerBase
    {
        private readonly IUnitOfWork<Solutions> _unitofworkSolutions;
        private readonly IRepositoryActionResult _repositoryActionResult;
        private readonly IUnitOfWork<SolutionLikes> _unitofworkSolutionsLikes;
        private readonly ISolutionsBusiness _ISolutionsBusiness;
        public SolutionsController(
             IUnitOfWork<Solutions> unitOfWorkSolutions,
             IActionResultResponseHandler actionResultResponseHandler,
             IHttpContextAccessor httpContextAccessor,
             ISolutionsBusiness ISolutionsBusiness,
             IRepositoryActionResult repositoryActionResult ,
             IUnitOfWork<SolutionLikes> unitofworkSolutionsLikes)
            : base(actionResultResponseHandler, httpContextAccessor)
        {
            _unitofworkSolutions = unitOfWorkSolutions;
            _repositoryActionResult = repositoryActionResult;
            _ISolutionsBusiness = ISolutionsBusiness;
            _unitofworkSolutionsLikes = unitofworkSolutionsLikes;
        }

        [HttpPost(nameof(AddSolutions))]
        public async Task<IRepositoryResult> AddSolutions([FromBody] SolutionsParameters SolutionsPrm)
        {
            try
            {

                var Solution = new Solutions();
                Solution.Problem_Id = SolutionsPrm.Problem_Id;
                Solution.User_Id = _ISolutionsBusiness.GetUserId();
                Solution.Content = SolutionsPrm.Content;
                Solution.Date = DateTime.Now;

                var added = _unitofworkSolutions.Repository.Add(Solution);
                if (added != null)
                {
                    var saved = await _unitofworkSolutions.SaveChanges() > 0;
                    if (saved)
                    {
                        var repositoryresult = _repositoryActionResult.GetRepositoryActionResult(Solution.Id, status: RepositoryActionStatus.Created, message: "Save Success");
                        var result = HttpHandeller.GetResult(repositoryresult);
                        return result;
                    }
                    else
                    {
                        var repositoryresult = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "faild");
                        var result = HttpHandeller.GetResult(repositoryresult);
                        return result;
                    }
                }
                else
                {
                    var repositoryresult = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "faild");
                    var result = HttpHandeller.GetResult(repositoryresult);
                    return result;
                }

            }
            catch (Exception e)
            {
                var repositoryresult = _repositoryActionResult.GetRepositoryActionResult(exception: e, status: RepositoryActionStatus.Error, message: ResponseActionMessages.Error);
                var result = HttpHandeller.GetResult(repositoryresult);
                return result;
            }            
        }


        [HttpGet("GetAllSolutions")]
        public async Task<IRepositoryResult> GetAllSolutions()
        {
            var comment = _unitofworkSolutions.Repository.FindQueryable(p => p.Id > 0);

            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(comment, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpGet("GetAllSolutionsForProblem/{ProblemId}")]
        public async Task<IRepositoryResult> GetAllSolutionsForProblem(int ProblemId)
        {
            var comment = _unitofworkSolutions.Repository.FindQueryable(p => p.Problem_Id == ProblemId);
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(comment, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpGet("GetSolutions/{CommentId}")]
        public async Task<IRepositoryResult> GetSolutions(int SolutionId)
        {
            var Solution = await _unitofworkSolutions.Repository.FirstOrDefault(q => q.Id == SolutionId);
            if (Solution == null)
            {
                var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NotFound, message: "Not Found");
                var res = HttpHandeller.GetResult(repositoryRes);
                return res;
            }
            var SolutionDto = new SolutionsParametersGet();
            SolutionDto.Id = Solution.Id;
            SolutionDto.Problem_Id = Solution.Problem_Id;
            SolutionDto.User_Id = Solution.User_Id;
            SolutionDto.Content = Solution.Content;
            SolutionDto .Date = Solution.Date.ToString("yyyy-MM-dd H:mm");

            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(SolutionDto, status: RepositoryActionStatus.Ok);
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpPost(nameof(UpdateSolutions))]
        public async Task<IRepositoryResult> UpdateSolutions([FromBody] SolutionsParametersUpadte Solution)
        {
            try
            {
                var Solutions = await _unitofworkSolutions.Repository.FirstOrDefault(q => q.Id == Solution.Id && q.User_Id == _ISolutionsBusiness.GetUserId() );
                if (Solutions != null)
                {

                    Solutions.Id = Solution.Id;
                    Solutions.Content = Solution.Content;
                    Solutions.Date = DateTime.Now;

                    _unitofworkSolutions.Repository.Update(Solutions);
                    var result = await _unitofworkSolutions.SaveChanges() > 0;
                    if (result)
                    {
                        var repo = _repositoryActionResult.GetRepositoryActionResult(Solutions.Id, status: RepositoryActionStatus.Updated, message: "Update Success");
                        var res = HttpHandeller.GetResult(repo);
                        return res;
                    }
                    else
                    {
                        var repo = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "Error");
                        var res = HttpHandeller.GetResult(repo);
                        return res;
                    }


                }
                else
                {
                    var repo = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NotFound, message: "Not found");
                    var res = HttpHandeller.GetResult(repo);
                    return res;
                }
            }
            catch (Exception e)
            {
                var repo = _repositoryActionResult.GetRepositoryActionResult(exception: e, status: RepositoryActionStatus.Error, message: "Error");
                var res = HttpHandeller.GetResult(repo);
                return res;
            }
        }


        [HttpPost(nameof(DeleteSolution))]
        public async Task<IRepositoryResult> DeleteSolution(int id)
        {
            _unitofworkSolutions.Repository.Remove(p => p.Id == id && p.User_Id == _ISolutionsBusiness.GetUserId());
            var deleted = await _unitofworkSolutions.SaveChanges() > 0;
            if (deleted)
            {
                var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Deleted, message: "Deleted successfully");
                var result = HttpHandeller.GetResult(repositoryRes);
                return result;
            }
            else
            {
                var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "Faild");
                var result = HttpHandeller.GetResult(repositoryRes);
                return result;
            }

        }

        [HttpPost (nameof (SolutionsLikes))]
        public async Task<IRepositoryResult> SolutionsLikes(int solutionId , bool like , bool disLike)
        {
            if (!like && !disLike)
            {
                var repositoryRes3 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NothingAdded, message: "Not added");
                var result3 = HttpHandeller.GetResult(repositoryRes3);
                return result3;
            }
                var solExist = await _unitofworkSolutionsLikes.Repository.FirstOrDefault(p => p.Solution_Id == solutionId && p.User_Id == _ISolutionsBusiness.GetUserId());
            if(solExist == null)
            {
                var added = await _ISolutionsBusiness.AddSolutionsLikes(solutionId, like, disLike);
                if (added)
                {
                    var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Created , message: "Saved successfully");
                    var result = HttpHandeller.GetResult(repositoryRes);
                    return result;
                }
            }
            else
            {
                _unitofworkSolutionsLikes.Repository.Remove(solExist);
                var added =  await _ISolutionsBusiness.AddSolutionsLikes(solutionId, like, disLike);
                if (added)
                {
                    var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "Faild");
                    var result = HttpHandeller.GetResult(repositoryRes);
                    return result;
                }
            }
            var repositoryRes2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NothingAdded, message: "Not added");
            var result2 = HttpHandeller.GetResult(repositoryRes2);
            return result2;
        }

     


        }
}
