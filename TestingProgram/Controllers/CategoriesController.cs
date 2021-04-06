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
using TestingProgram.Parameter;

namespace TestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ApiControllerBase
    {
        private readonly IUnitOfWork<Categories> _unitofworkcategories;
        private readonly IRepositoryActionResult _repositoryActionResult;

        public CategoriesController(
             IUnitOfWork<Categories> unitOfWorkCategories,
             IActionResultResponseHandler actionResultResponseHandler,
             IHttpContextAccessor httpContextAccessor,
             IRepositoryActionResult repositoryActionResult)
            : base(actionResultResponseHandler, httpContextAccessor)
        {
            _unitofworkcategories = unitOfWorkCategories;
            _repositoryActionResult = repositoryActionResult;

        }

    
        [HttpPost(nameof(AddCategories))]
        public async Task<IRepositoryResult> AddCategories([FromBody] CategoriesParameters Category)
        {
            try
            {

                var Cat = new Categories();
                Cat.Name = Category.Name;
                var putCategory = _unitofworkcategories.Repository.Add(Cat);
                if (putCategory != null)
                {
                    var result = await _unitofworkcategories.SaveChanges() > 0;
                    if (result)
                    {
                        var repo2 = _repositoryActionResult.GetRepositoryActionResult(Cat.Id, status: RepositoryActionStatus.Created, message: "Save Success");
                        var res2 = HttpHandeller.GetResult(repo2);
                        return res2;
                    }

                }               
                var repo = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error, message: "Error");
                var res = HttpHandeller.GetResult(repo);
                return res;
                
            }
            catch (Exception e)
            {
                var repo = _repositoryActionResult.GetRepositoryActionResult(exception: e, status: RepositoryActionStatus.Error, message: ResponseActionMessages.Error);
                var res = HttpHandeller.GetResult(repo);
                return res;
            }
        }
     
        [HttpGet("GetAllCategory")]
        public async Task<IRepositoryResult> GetAllCategory()
        {
            var Categor = await _unitofworkcategories.Repository.GetAll();
            var CatList = Categor.ToList();
            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(CatList, status: RepositoryActionStatus.Ok, message: "Found");
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }
    
        [HttpPost(nameof(UpdateCategories))]
        public async Task<IRepositoryResult> UpdateCategories([FromBody] CategoriesParametersUpdate Category)
        {
            try
            {
                var Cat =await _unitofworkcategories.Repository.FirstOrDefault(q=>q.Id == Category.Id);
                if (Cat != null)
                {
                    Cat.Name = Category.Name;
                    _unitofworkcategories.Repository.Update(Cat);
                    var result = await _unitofworkcategories.SaveChanges() > 0;
                    if (result)
                    {
                        var repo = _repositoryActionResult.GetRepositoryActionResult(Cat.Id, status: RepositoryActionStatus.Updated, message: "Update Success");
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
                    var repo = _repositoryActionResult.GetRepositoryActionResult( status: RepositoryActionStatus.NotFound, message: "Not found");
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


        [HttpGet("GetCategory/{CategoryId}")]
        public async Task<IRepositoryResult> GetCategory(int CategoryId)
        {
            var Category = await _unitofworkcategories.Repository.FirstOrDefault(q => q.Id == CategoryId);
            if (Category == null)
            {
                var repositoryResult2 = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.NotFound, message: "Not Found");
                var result2 = HttpHandeller.GetResult(repositoryResult2);
                return result2;
            }
            var CatDto = new CategoriesParametersUpdate();
            CatDto.Id = Category.Id;
            CatDto.Name = Category.Name;

            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(CatDto, status: RepositoryActionStatus.Ok);
            var result = HttpHandeller.GetResult(repositoryResult);
            return result;
        }

    }
}
