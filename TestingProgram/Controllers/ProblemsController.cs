using Attendleave.Erp.Core.APIUtilities;
using Attendleave.Erp.Core.UnitOfWork;
using DataLayer.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingProgram.Business;
using TestingProgram.Dtos;
using TestingProgram.Parameter;

namespace TestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemsController : ApiControllerBase
    {

        private readonly IUnitOfWork<Problems> _unitofworkProblems;
        private readonly IUnitOfWork<Solutions> _unitofworkSolutions;
        private readonly IUnitOfWork<CommentLikes> _unitofworkCommentLikes;
        private readonly IUnitOfWork<SolutionLikes> _unitofworkSolutionLikes;
        private readonly IUnitOfWork<Comments> _unitofworkComments;
        private readonly IRepositoryActionResult _repositoryActionResult;
        private readonly ISolutionsBusiness _ISolutionsBusiness;

        public ProblemsController(
             IUnitOfWork<Problems> unitOfWorkProblems,
             IActionResultResponseHandler actionResultResponseHandler,
             IHttpContextAccessor httpContextAccessor,
             IUnitOfWork<Solutions> unitofworkSolutions,
              IUnitOfWork<CommentLikes> unitofworkCommentLikes,
             IUnitOfWork<Comments> unitofworkComments,
             IRepositoryActionResult repositoryActionResult ,
             IUnitOfWork<SolutionLikes> unitofworkSolutionLikes,
             ISolutionsBusiness ISolutionsBusiness)
            : base(actionResultResponseHandler, httpContextAccessor)
        {
            _unitofworkProblems = unitOfWorkProblems;
            _unitofworkCommentLikes = unitofworkCommentLikes;
            _repositoryActionResult = repositoryActionResult;
            _unitofworkSolutions = unitofworkSolutions;
            _unitofworkComments = unitofworkComments;
            _unitofworkSolutionLikes = unitofworkSolutionLikes;
            _ISolutionsBusiness = ISolutionsBusiness;
        }

        [HttpPost(nameof(AddProblem))]
        public async Task<IRepositoryResult> AddProblem([FromBody] ProblemsParameters Problem)
        {
            try
            {
                var Prob = new Problems();
                Prob.Cat_Id = Problem.Cat_Id;
                Prob.User_Id = _ISolutionsBusiness.GetUserId();
                Prob.Title = Problem.Title;
                Prob.Description = Problem.Description;
                Prob.Solved = Problem.Solved;
                Prob.Date = DateTime.Now;
                var putProblem = _unitofworkProblems.Repository.Add(Prob);
                if (putProblem != null)
                {
                    var result = await _unitofworkProblems.SaveChanges() > 0;

                    if (result)
                    {
                        var repo = _repositoryActionResult.GetRepositoryActionResult(Prob.Id, status: RepositoryActionStatus.Created, message: "Save Success");
                        var res = HttpHandeller.GetResult(repo);
                        return res;
                    }

                }
                var repo2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "Error");
                var res2 = HttpHandeller.GetResult(repo2);
                return res2;
            }
            catch (Exception e)
            {
                var repo = _repositoryActionResult.GetRepositoryActionResult(exception: e, status: RepositoryActionStatus.Error, message: ResponseActionMessages.Error);
                var res = HttpHandeller.GetResult(repo);
                return res;
            }
        }



        [HttpGet("GetProblem/{ProblemId}")]
        public async Task<IRepositoryResult> GetProblem(int ProblemId)
        {
            var Problem = await _unitofworkProblems.Repository.FirstOrDefault(q => q.Id == ProblemId);
            if (Problem == null)
            {
                var repositoryResult2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NotFound, message: "Not Found");
                var result2 = HttpHandeller.GetResult(repositoryResult2);
                return result2;
            }
            var ProbDto = new ProblemsParametersGet();
            ProbDto.Cat_Id = Problem.Cat_Id;
            ProbDto.User_Id = Problem.User_Id;
            ProbDto.Title = Problem.Title;
            ProbDto.Description = Problem.Description;
            ProbDto.Solved = Problem.Solved;
            ProbDto.Date = Problem.Date.ToString("yyyy-mm-dd hh:mm:ss");
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(ProbDto, status: RepositoryActionStatus.Ok);
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpGet("GetAllProblems")]
        public async Task<IRepositoryResult> GetAllProblems()
        {
            //List<ProblemsParametersGetAll> LisDto = new List<ProblemsParametersGetAll>();
            var Prob = _unitofworkProblems.Repository.FindQueryable(q => q.Id > 0);
            //foreach (var item in Prob)
            //{
            //    ProblemsParametersGetAll ProbDto = new ProblemsParametersGetAll();
            //    ProbDto.Id = item.Id;
            //    ProbDto.Cat_Id = item.Cat_Id;
            //    ProbDto.User_Id = item.User_Id;
            //    ProbDto.Title = item.Title;
            //    ProbDto.Description = item.Description;
            //    ProbDto.Solved = item.Solved;
            //    LisDto.Add(ProbDto);
            //}
            var ProbList = Prob.ToList();
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(ProbList, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }

        [HttpGet("GetAllProblemsForUser")]
        public async Task<IRepositoryResult> GetAllProblemsForUser()
        {
            //List<ProblemsParametersGetAll> LisDto = new List<ProblemsParametersGetAll>();
            var Prob = _unitofworkProblems.Repository.FindQueryable(q => q.User_Id == _ISolutionsBusiness.GetUserId());
            //foreach (var item in Prob)
            //{
            //    ProblemsParametersGetAll ProbDto = new ProblemsParametersGetAll();
            //    ProbDto.Id = item.Id;
            //    ProbDto.Cat_Id = item.Cat_Id;
            //    ProbDto.User_Id = item.User_Id;
            //    ProbDto.Title = item.Title;
            //    ProbDto.Description = item.Description;
            //    ProbDto.Solved = item.Solved;
            //    LisDto.Add(ProbDto);
            //}
            var ProbList = Prob.ToList();
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(ProbList, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }

        [HttpPost(nameof(UpdateProblems))]
        public async Task<IRepositoryResult> UpdateProblems([FromBody] ProblemsParametersGetAll Problem)
        {
            try
            {
                var Prob = await _unitofworkProblems.Repository.FirstOrDefault(q => q.Id == Problem.Id && q.User_Id == _ISolutionsBusiness.GetUserId());
                if (Prob != null)
                {
                    Prob.Cat_Id = Problem.Cat_Id;
                    Prob.Title = Problem.Title;
                    Prob.Description = Problem.Description;
                    Prob.Solved = Problem.Solved;
                    Prob.Date = DateTime.Now;
                    _unitofworkProblems.Repository.Update(Prob);
                    var result = await _unitofworkProblems.SaveChanges() > 0;
                    if (result)
                    {
                        var repo = _repositoryActionResult.GetRepositoryActionResult(Prob.Id, status: RepositoryActionStatus.Updated, message: "Update Success");
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


        [HttpGet("RemoveProblem/{ProblemId}")]
        public async Task<IRepositoryResult> RemoveProblem(int ProblemId)
        {
            _unitofworkProblems.Repository.Remove(q => q.Id == ProblemId && q.User_Id == _ISolutionsBusiness.GetUserId());
            var result = await _unitofworkProblems.SaveChanges() > 0;

            if (result)
            {
                var repo = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Deleted, message: "Delete Success");
                var res = HttpHandeller.GetResult(repo);
                return res;
            }

            var repositoryResult2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "Error");
            var result2 = HttpHandeller.GetResult(repositoryResult2);
            return result2;



        }


        [HttpGet("GetListProblem")]
        public async Task<IRepositoryResult> GetListProblem()
        {
            var Problem = _unitofworkProblems.Repository.FindQueryable(q => q.Id > 0);
            if (Problem == null)
            {
                var repositoryResult2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NotFound, message: "Not Found");
                var result2 = HttpHandeller.GetResult(repositoryResult2);
                return result2;
            }
            List<ProblemsListsDto> ProblemsObjLists = new List<ProblemsListsDto>();
            foreach (var itemProblem in Problem)
            {
                var ProblemsObj = new ProblemsListsDto();
                ProblemsObj.Id = itemProblem.Id;
                ProblemsObj.Title = itemProblem.Title;
                ProblemsObj.Description = itemProblem.Description;
                ProblemsObj.Date = itemProblem.Date.ToString("yyyy-mm-dd hh:mm:ss");
                var Solutions = _unitofworkSolutions.Repository.FindQueryable(q => q.Problem_Id == itemProblem.Id);
                foreach (var itemSolution in Solutions)
                {
                    var SolutionsObj = new SolutionsListsDto();
                    SolutionsObj.Id = itemSolution.Id;
                    SolutionsObj.Content = itemSolution.Content;
                    SolutionsObj.LikesCount = _unitofworkSolutionLikes.Repository.FindQueryable(q => q.Solution_Id  == itemSolution.Id && q.Like == true).ToList().Count();
                    SolutionsObj.DisLikeCount = _unitofworkSolutionLikes.Repository.FindQueryable(q => q.Solution_Id == itemSolution.Id && q.Dislike == true).ToList().Count();
                    SolutionsObj.Date =itemSolution.Date.ToString("yyyy-MM-dd H:mm");
                    ProblemsObj.Solutions.Add(SolutionsObj);
                    var Comments = _unitofworkComments.Repository.FindQueryable(q => q.Solution_Id == itemSolution.Id);

                    foreach (var itemComments in Comments)
                    {
                        var CommentsObj = new CommentsListsDto();
                        CommentsObj.Id = itemComments.Id;
                        CommentsObj.Content = itemComments.Content;
                        CommentsObj.LikesCount = _unitofworkCommentLikes.Repository.FindQueryable(q => q.Comment_Id == itemComments.Id&&q.Like==true).ToList().Count();
                        CommentsObj.DisLikeCount = _unitofworkCommentLikes.Repository.FindQueryable(q => q.Comment_Id == itemComments.Id && q.Dislike == true).ToList().Count();
                        CommentsObj .Date = itemComments.Date.ToString("yyyy-MM-dd H:mm");
                        SolutionsObj.comments.Add(CommentsObj);
                      
                    }
                }
                ProblemsObjLists.Add(ProblemsObj);
            }
         

            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(ProblemsObjLists, status: RepositoryActionStatus.Ok);
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


  
    }
    }
