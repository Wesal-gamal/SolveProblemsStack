using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProgram.Parameter
{
    public class UsersParameters
    {
        public string Name { get; set; }
        public  string UserName { get; set; }
        public  string Password { get; set; }
        public  string Email { get; set; }

        public  string Image { get; set; }
    }
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class loginDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenValidTo { get; set; }
        public int EmployeeId { get; set; }
        public string UserId { get; set; }
        public string Url { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string ImagePath { get; set; }
        public string RedirectToUrl { get; set; }
    }
    public class UpdateUserParameters
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
