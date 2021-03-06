using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Attendleave.Erp.Core.APIUtilities;
using Attendleave.Erp.Core.UnitOfWork;

using DataLayer.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestingProgram.Business;
using TestingProgram.Parameter;

namespace TestingProgram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        private readonly IUnitOfWork<ApplicationUser> _unitofworkUsers;
        private readonly IRepositoryActionResult _repositoryActionResult;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MyDBContext _contextDb;
        private readonly IConfiguration _config;
        private readonly ISolutionsBusiness _ISolutionsBusiness;

        public UsersController( 
            IUnitOfWork<ApplicationUser> unitofworkUsers, 
            IRepositoryActionResult repositoryActionResult,
            IActionResultResponseHandler ActionResultResponseHandler 
            , IHttpContextAccessor HttpContextAccessor,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            ISolutionsBusiness ISolutionsBusiness,
        MyDBContext contextDb)
             : base(ActionResultResponseHandler, HttpContextAccessor
                   )
        {
            _unitofworkUsers = unitofworkUsers;
            _signInManager = signInManager;
            _repositoryActionResult = repositoryActionResult;
            _userManager = userManager;
            _contextDb = contextDb;
            _config = config;
            _ISolutionsBusiness = ISolutionsBusiness;
        }

  

        [AllowAnonymous]
        [HttpPost(nameof(InsertAccounts))]
        public async Task<IRepositoryResult> InsertAccounts([FromForm] UsersParameters model)
        {
            try
            {
                var User = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(User, model.Password);

                if (result.Succeeded)
                {
                    var newUser = await _userManager.FindByNameAsync(model.UserName.Trim());
                    var repository = _repositoryActionResult.GetRepositoryActionResult(User.UserName, status: RepositoryActionStatus.Created, message: "Saved Successfuly" );
                    var resultt = HttpHandeller.GetResult(repository);
                    return resultt;

                }
                string mesg = "";


                foreach (var error in result.Errors)
                {
                    mesg += error.Description + " ";
                }
                var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.BadRequest, message: mesg);
                var result2 = HttpHandeller.GetResult(repositoryResult);
                return result2;

            }
            catch (Exception e)
            {
                var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(exception: e, message: ResponseActionMessages.Error, status: RepositoryActionStatus.Error);
                var result = HttpHandeller.GetResult(repositoryResult);
                return result;
            }
        }


        [AllowAnonymous]
        [HttpGet("GetUsers")]
        public async Task<IRepositoryResult> GetUsers()
        {
            try 
            {
                var useres = await _unitofworkUsers.Repository.GetAll();
                var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(useres, status: RepositoryActionStatus.Ok, message: "Found");
                var result = HttpHandeller.GetResult(repositoryResult);
                return result;
            }
            catch (Exception e)
            {
                var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(exception: e, message: ResponseActionMessages.Error, status: RepositoryActionStatus.Error);
                var result = HttpHandeller.GetResult(repositoryResult);
                return result;
            }
          
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IRepositoryResult> Login(LoginUser loginParameter)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginParameter.UserName);
                if (user == null)
                {
                    var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(message: "There is no user with this UserName.", status: RepositoryActionStatus.Error);
                    var result2 = HttpHandeller.GetResult(repositoryRes);
                    return result2;
                }
                else
                {
                  

                    var result = await _signInManager.CheckPasswordSignInAsync(user, loginParameter.Password, false);
              
                        if (result.Succeeded)
                        {
                            var refToken = Guid.NewGuid().ToString();
                            // get child roles 
                            var userInfo = new ApplicationUser();
                            string refreshToken = "";
                            var userLoginreturn = new loginDto();
                            var claims = new[] {
                            new Claim( JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim("UserId", user.Id),
                        };
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]));

                        var expiryInHours = DateTime.Now.AddHours(Convert.ToDouble(_config["Jwt:ExpiryInHours"]));
                            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                issuer: _config["Jwt:Site"],
                                audience: _config["Jwt:Site"],
                                expires: expiryInHours,
                               signingCredentials: credentials,
                                claims: claims);
                            userLoginreturn.UserId = user.Id;
                            userLoginreturn.RefreshToken = refreshToken;
                            userLoginreturn.TokenValidTo = token.ValidTo;
                            userLoginreturn.Token = new JwtSecurityTokenHandler().WriteToken(token);
                            // var userLoginReturn = _token.GenerateJsonWebToken(user, refToken);
                            var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(userLoginreturn, status: RepositoryActionStatus.Ok);
                            var resultt = HttpHandeller.GetResult(repositoryResult);
                            return resultt;
                        }
                    

                    return HttpHandeller.GetResult(_repositoryActionResult.GetRepositoryActionResult(message: "Wrong Username or Password.", status: RepositoryActionStatus.Error));
                }
            }
            catch (Exception ex)
            {
                return HttpHandeller.GetResult(_repositoryActionResult.GetRepositoryActionResult(exception: ex, message: "SomeThing went Wrong.", status: RepositoryActionStatus.Error));
            }
        }

        //[AllowAnonymous]
        //[HttpPost(nameof(updateUserData))]
        //public async Task<IRepositoryResult> updateUserData([FromBody] UsersParameters model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.UserName);
        //    if (user == null) { /**/ }
        //  //  if (model.smsCode != user.SmsCode) { /**/}

        //    // compute the new hash string
        //    var newPassword = _userManager.PasswordHasher.HashPassword(user, model .Password );
        //    user.PasswordHash = newPassword;
        //    var res = await _userManager.UpdateAsync(user);

        //    if (res.Succeeded) 
        //    {
        //        var repositoryResult = _repositoryActionResult.GetRepositoryActionResult( status: RepositoryActionStatus.Updated ,message: " update success");
        //        var resultt = HttpHandeller.GetResult(repositoryResult);
        //        return resultt;
        //    }
        //    else 
        //    {
        //        var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(status: RepositoryActionStatus.Error , message: "Faild");
        //        var resultt = HttpHandeller.GetResult(repositoryResult);
        //        return resultt;
        //    }
        //}
        [AllowAnonymous]
        [HttpPost(nameof(UpdateAccounts))]
        public async Task<IRepositoryResult> UpdateAccounts(ChangePasswordParameter parameter)
        {

            var User = await _userManager.FindByIdAsync(parameter.Id.Trim());

            if (User == null)
            {
                return HttpHandeller.GetResult(_repositoryActionResult.GetRepositoryActionResult(message: "There is no user with this Id: " + parameter.Id.Trim() + ".", status: RepositoryActionStatus.NotFound));
            }

            User.UserName = parameter.UserName.Trim();
            User.Email = parameter.Email.Trim();
            if (parameter != null)
                {
                    var result = await _userManager.ChangePasswordAsync(User, parameter.OldPassword, parameter.NewPassword);
                    if (result.Succeeded)
                    {
                        return HttpHandeller.GetResult(new RepositoryActionResult(result: true, status: RepositoryActionStatus.Ok, exception: null,  "Updated Successfully."));

                    }
                    return HttpHandeller.GetResult(new RepositoryActionResult(result: result, status: RepositoryActionStatus.Error, exception: null, message: result.Errors.First().Description));
                }
                return HttpHandeller.GetResult(new RepositoryActionResult(result: null, status: RepositoryActionStatus.NotFound, exception: null, message: "No User with Username: " + parameter.UserName));
            
         //   return HttpHandeller.GetResult(new RepositoryActionResult(result: null, status: RepositoryActionStatus.NotFound, exception: null, message: "No Company with Unique Name: " ));
        }

            
        [HttpPost(nameof(NewUpdateAccount))]
        public async Task<IRepositoryResult> NewUpdateAccount([FromBody] NewUpdateUserParameters NewUser)
        {
            // var newPasswordHash = PasswordHasher.HashPassword(password);

            //var data = Encoding.ASCII.GetBytes(password);
            //var md5 = new MD5CryptoServiceProvider();
            //var md5data = md5.ComputeHash(data);
            //var hashedPassword = ASCIIEncoding.GetString(md5data);

            //string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            //var DataOfUser = await _unitofworkUsers.Repository.FirstOrDefault(q => q.Id == ID);
            //if (DataOfUser != null)
            //{

            //    var passwordHasher = new PasswordHasher<ApplicationUser>();
            //    var user = new ApplicationUser();
            //    user.UserName = DataOfUser.UserName;
            //    user.Name = DataOfUser.Name;
            //    var hashedPassword = passwordHasher.HashPassword(user, password);
            //}

            var user = await _unitofworkUsers.Repository.FirstOrDefault(q => q.Id == NewUser.Id);
            if (user == null)
            {
                var repositoryResNotCorrect = _repositoryActionResult.GetRepositoryActionResult(message: "There is not found this user", status: RepositoryActionStatus.NotFound);
                var resultNotCorrect = HttpHandeller.GetResult(repositoryResNotCorrect);
                return resultNotCorrect;
            }
            user.UserName = NewUser.UserName;
            user.Name = NewUser.Name;
            user.Email = NewUser.Email;

            var result = await _userManager.ChangePasswordAsync(user, NewUser.CurrentPassword, NewUser.NewPassword);

            if (!result.Succeeded)
            {
                var repositoryResNotCorrect = _repositoryActionResult.GetRepositoryActionResult(message: "There is not Succeeded", status: RepositoryActionStatus.BadRequest);
                var resultNotCorrect = HttpHandeller.GetResult(repositoryResNotCorrect);
                return resultNotCorrect;
            }

            var repositoryRes = _repositoryActionResult.GetRepositoryActionResult(message: "Updated successfully", status: RepositoryActionStatus.Ok);
            var result2 = HttpHandeller.GetResult(repositoryRes);
            return result2;
        }


        
        [HttpGet("GetDataOfUser")]
        public async Task<IRepositoryResult> GetDataOfUser()
        {

            var useres =  _unitofworkUsers.Repository.FindQueryable(q => q.Id == _ISolutionsBusiness.GetUserId()).Select (i => new 
            {
                i.UserName,
                i.Email, 
                i.Id,
                i.Name
            });
            if (useres != null)
            {
                var repositoryResult = _repositoryActionResult.GetRepositoryActionResult(useres, status: RepositoryActionStatus.Ok, message: "Found");
                var result = HttpHandeller.GetResult(repositoryResult);
                return result;
            }
            var repositoryResultNotFound = _repositoryActionResult.GetRepositoryActionResult( status: RepositoryActionStatus.NotFound, message: "NotFound");
            var resultNotFound = HttpHandeller.GetResult(repositoryResultNotFound);
            return resultNotFound;

        }


    }
}