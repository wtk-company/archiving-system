using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "UserNameRequired")]
        [Display(Name = "UserName", ResourceType = typeof(main_lang))]
        public string UserName { get; set; }




        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(main_lang))]
        public string Password { get; set; }




        [Display(Name = "RememberMe", ResourceType = typeof(main_lang))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(main_lang))]
        public string Email { get; set; }

        

        [Display(Name = "DepartmentId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "DepartmentRequired")]
        public int DepartmentID { get; set; }




        [Display(Name = "JobTitleId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "JobTitleRequired")]
        public int JobTitleId { get; set; }


        
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "FullNameRequired")]
        [Display(Name = "FullName", ResourceType = typeof(main_lang))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        public string FullName { get; set; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "UserNameRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        [Display(Name = "UserName", ResourceType = typeof(main_lang))]
        public string UserName { get; set; }


        
        [Display(Name = "Gender", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "GenderRequired")]
        public string Gender { set; get; }



        [Display(Name = "Role", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "RoleRequired")]
        public string Role { set; get; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(main_lang))]
        public string Password { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(main_lang))]
        [Compare("ConfirmPassword", ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "ConfirmPasswordCompare")]
        public string ConfirmPassword { get; set; }
    }

    public class EditProfileViewModel
    {
        public string Id { set; get; }



        [EmailAddress(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(main_lang))]
        public string Email { get; set; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "FullNameRequired")]
        [Display(Name = "FullName", ResourceType = typeof(main_lang))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        public string FullName { get; set; }




        [Display(Name = "Gender", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "GenderRequired")]
        public string Gender { set; get; }




        [Display(Name = "Role", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "RoleRequired")]
        public string Role { set; get; }





        [Display(Name = "DepartmentId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "DepartmentRequired")]
        public int DepartmentID { get; set; }


        
        [Display(Name = "JobTitleId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "JobTitleRequired")]
        public int JobTitleId { get; set; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "UserNameRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        [Display(Name = "UserName", ResourceType = typeof(main_lang))]
        public string UserName { get; set; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordLength")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(main_lang))]
        public string Password { get; set; }



        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(main_lang))]
        [Compare("ConfirmPassword", ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "ConfirmPasswordCompare")]
        public string ConfirmPassword { get; set; }
    }


    public class EditUserNameAndPassword
    {
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "OldUserNameRequired")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        [Display(Name = "OldUserName", ResourceType = typeof(main_lang))]
        public string OldUserName { set; get; }



        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NewUserNameRequired")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NameLength")]
        [Display(Name = "NewUserName", ResourceType = typeof(main_lang))]
        public string NewUserName { set; get; }




        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "OldPasswordRequired")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordLength")]
        [Display(Name = "OldPassword", ResourceType = typeof(main_lang))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }




        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "NewPasswordRequired")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "PasswordLength")]
        [Display(Name = "NewPassword", ResourceType = typeof(main_lang))]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(main_lang))]
        [Compare("ConfirmPassword", ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "ConfirmPasswordCompare")]
        public string ConfirmPassword { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
