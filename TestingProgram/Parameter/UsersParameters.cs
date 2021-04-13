using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class ChangePasswordParameter
    {
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(200)]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

    }
    public class NewUpdateUserParameters
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
