using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attendleave.Erp.Core.APIUtilities;
using Attendleave.Erp.Core.UnitOfWork;
using DataLayer.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestingProgram.Business;
using TestingProgram.Dtos;
using TestingProgram.Parameter;

namespace TestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ApiControllerBase
    {

        private readonly IUnitOfWork<Comments> _unitofworkComments;
        private readonly IUnitOfWork<CommentLikes> _unitofworkCommentLikes;
        private readonly IRepositoryActionResult _repositoryActionResult;
        private readonly ISolutionsBusiness _ISolutionsBusiness;

        public CommentsController(
             IUnitOfWork<Comments> unitOfWorkComments,
             IActionResultResponseHandler actionResultResponseHandler,
             IUnitOfWork<CommentLikes> unitofworkCommentLikes,
             IHttpContextAccessor httpContextAccessor,
             IRepositoryActionResult repositoryActionResult,
             ISolutionsBusiness ISolutionsBusiness)
            : base(actionResultResponseHandler, httpContextAccessor)
        {
            _unitofworkComments = unitOfWorkComments;
            _repositoryActionResult = repositoryActionResult;
            _unitofworkCommentLikes = unitofworkCommentLikes;
            _ISolutionsBusiness = ISolutionsBusiness;
        }

        [HttpPost(nameof(AddComments))]
        public async Task<IRepositoryResult> AddComments([FromBody] CommentsParameters CommentsPrm)
        {
            try
            {
               
                var comment = new Comments();
                comment.Solution_Id = CommentsPrm.Solution_Id;
                comment.User_Id = _ISolutionsBusiness.GetUserId();
                comment.Content = CommentsPrm.Content;
               var added =   _unitofworkComments.Repository.Add(comment);
             if(added != null )
              { 
                var saved = await _unitofworkComments.SaveChanges() > 0;
                if (saved)
                {
                    var repositoryresult = _repositoryActionResult.GetRepositoryActionResult(comment.Id, status: RepositoryActionStatus.Created, message: "Save Success");
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
            //gklfjlkg
        }


        [HttpGet("GetAllComments")]
        public async Task<IRepositoryResult> GetAllComments()
        {
            var comment = _unitofworkComments.Repository.FindQueryable(p => p.Id > 0);
           
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(comment, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpGet("GetAllCommentsForSolution/{SolutionId}")]
        public async Task<IRepositoryResult> GetAllCommentsForSolution(int SolutionId)
        {
            var comment = _unitofworkComments.Repository.FindQueryable(p => p.Solution_Id== SolutionId);
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(comment, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpGet("GetComments/{CommentId}")]
        public async Task<IRepositoryResult> GetComments(int CommentId)
        {
            var Comment = await _unitofworkComments.Repository.FirstOrDefault(q => q.Id == CommentId);
            if (Comment == null)
            {
                var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NotFound, message: "Not Found");
                var res = HttpHandeller.GetResult(repositoryRes);
                return res;
            }
            var commentDto = new CommentsDto();
            commentDto.id  = Comment.Id;
            commentDto.Problem_Id = Comment.Solution_Id;
            commentDto.User_Id = Comment.User_Id;
            commentDto.Content  = Comment.Content;


            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(commentDto, status: RepositoryActionStatus.Ok);
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }


        [HttpPost(nameof(UpdateComments))]
        public async Task<IRepositoryResult> UpdateComments([FromBody] CommentsUpdate comment)
        {
            try
            {
                var comments = await _unitofworkComments.Repository.FirstOrDefault(q => q.Id == comment.id && q.User_Id == _ISolutionsBusiness.GetUserId());
                if (comments != null)
                {

                    comments.Id = comment.id;
                    comments.Solution_Id = comment.Problem_Id;
                    comments.Content = comment.Content;

                    _unitofworkComments.Repository.Update(comments);
                    var result = await _unitofworkComments.SaveChanges() > 0;
                    if (result)
                    {
                        var repo = _repositoryActionResult.GetRepositoryActionResult(comments.Id, status: RepositoryActionStatus.Updated, message: "Update Success");
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
       

        [HttpPost(nameof(DeleteComment))]
        public async Task<IRepositoryResult> DeleteComment(int id)
        {
            _unitofworkComments.Repository.Remove(p => p.Id == id && p.User_Id == _ISolutionsBusiness.GetUserId());
            var deleted = await _unitofworkComments.SaveChanges() > 0;
            if(deleted)
            {
                var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Deleted, message: "Deleted successfully");
                var result = HttpHandeller.GetResult(repositoryRes);
                return result;
            }
            else
            {
                var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error , message: "Faild");
                var result = HttpHandeller.GetResult(repositoryRes);
                return result;
            }
           
        }



        [HttpPost(nameof(CommentsLikes))]
        public async Task<IRepositoryResult> CommentsLikes(int CommentId, bool Like, bool Dislike)
        {
            if (!Like  && !Dislike)
            {
                var repositoryNotAdded2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NothingAdded);
                var resultNotAdded2 = HttpHandeller.GetResult(repositoryNotAdded2);
                return resultNotAdded2;
            }
            var GetLike =await _unitofworkCommentLikes.Repository.FirstOrDefault(q => q.Comment_Id == CommentId && q.User_Id == _ISolutionsBusiness.GetUserId());
            if (GetLike == null)
            {
                var comLik = new CommentLikes();
                comLik.User_Id = _ISolutionsBusiness.GetUserId();
                comLik.Comment_Id = CommentId;
                comLik.Like = Like;
                comLik.Dislike = Dislike;
                var add = _unitofworkCommentLikes.Repository.Add(comLik);
                if (add != null)
                {
                   await _unitofworkCommentLikes.SaveChanges();
                    var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Created, message: "Created successfully");
                    var result = HttpHandeller.GetResult(repositoryRes);
                  return result;
                }
  
            }
            else
            {
                _unitofworkCommentLikes.Repository.Remove(GetLike);

                var comLik = new CommentLikes();
                comLik.User_Id = _ISolutionsBusiness.GetUserId();
                comLik.Comment_Id = CommentId;
                comLik.Like = Like;
                comLik.Dislike = Dislike;
                var add = _unitofworkCommentLikes.Repository.Add(comLik);
                if (add != null)
                {
                    await _unitofworkCommentLikes.SaveChanges();
                    var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Created, message: "Created successfully");
                    var result = HttpHandeller.GetResult(repositoryRes);
                    return result;
                }

            }

            var repositoryNotAdded = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NothingAdded);
            var resultNotAdded = HttpHandeller.GetResult(repositoryNotAdded);
            return resultNotAdded;

        }

    }
}