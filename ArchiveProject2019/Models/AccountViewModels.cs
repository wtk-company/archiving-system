using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "يجب إدخال اسم المستخدم")]

        [Display(Name = "اسم المستخدم")]
       
        public string UserName { get; set; }

        [Required(ErrorMessage = "يجب إدخال كلمة المرور ")]

        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
       // [Required(ErrorMessage ="يجب إدخال البريد الالكتروني")]
        [EmailAddress(ErrorMessage ="يجب أن يكون بريد إلكتروني")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }




        
        [Display(Name = "القسم")]
        [Required(ErrorMessage ="يجب إدخال القسم")]
        public int DepartmentID { get; set; }




        [Display(Name = "المسمى الوظيفي")]
        [Required(ErrorMessage = "يجب إدخال المسمى الوظيفي")]
        public int JobTitleId { get; set; }





        [Required(ErrorMessage = "يجب إدخال الاسم الثلاثي ")]
        [Display(Name = "الاسم الثلاثي")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول الاسم الثلاثي  يجب أن يكون بين 3 و 50 محرف")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "يجب إدخال اسم المستخدم ")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول  اسم المستخدم يجب أن يكون بين 3 و 50 محرف")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }




        [Display(Name ="الجنس")]
        [Required(ErrorMessage ="يجب إختيار الجنس")]
        public string Gender { set; get; }


        [Display(Name ="الدور")]
        [Required(ErrorMessage ="يجب إختيار الدور")]
        public string Role { set; get; }



        [Required(ErrorMessage ="يجب إدخال كلمة السر")]
        [StringLength(100, ErrorMessage = "يجب أن تكون طول كلمة السر أكبر من 5", MinimumLength =6 )]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة السر")]
        [Compare("Password", ErrorMessage = "كلمة السر وتأكيدها غير متوافقتين، يرجى إعادةالأدخال ")]
        public string ConfirmPassword { get; set; }
    }

    public class EditProfileViewModel
    {
        public string Id { set; get; }
        //[Required(ErrorMessage = "يجب إدخال البريد الالكتروني")]
        [EmailAddress(ErrorMessage = "يجب أن يكون بريد إلكتروني")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "يجب إدخال الاسم الثلاثي ")]
        [Display(Name = "الاسم الثلاثي")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول الاسم الثلاثي  يجب أن يكون بين 3 و 50 محرف")]

        public string FullName { get; set; }

        [Display(Name = "الجنس")]
        [Required(ErrorMessage = "يجب إختيار الجنس")]
        public string Gender { set; get; }


        [Display(Name = "الدور")]
        [Required(ErrorMessage = "يجب إختيار الدور")]
        public string Role { set; get; }





        [Display(Name = "القسم")]
        [Required(ErrorMessage = "يجب إدخال القسم")]
        public int DepartmentID { get; set; }




        [Display(Name = "المسمى الوظيفي")]
        [Required(ErrorMessage = "يجب إدخال المسمى الوظيفي")]
        public int JobTitleId { get; set; }


        [Required(ErrorMessage = "يجب إدخال اسم المستخدم ")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول  اسم المستخدم يجب أن يكون بين 3 و 50 محرف")]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }




        [Required(ErrorMessage = "يجب إدخال كلمة السر")]
        [StringLength(100, ErrorMessage = "يجب أن تكون طول كلمة السر أكبر من 5", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }





        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة السر")]
        [Compare("Password", ErrorMessage = "كلمة السر وتأكيدها غير متوافقتين، يرجى إعادةالأدخال ")]
        public string ConfirmPassword { get; set; }
    }


    public class EditUserNameAndPassword {

        [Display(Name = "اسم المستخدم الحالي")]
        [Required(ErrorMessage = "يجب الإدخال")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول  اسم المستخدم يجب أن يكون بين 3 و 50 محرف")]

        public string OldUserName { set; get; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "طول  اسم المستخدم يجب أن يكون بين 3 و 50 محرف")]

        [Display(Name = "اسم المستخدم الجديد")]
        [Required(ErrorMessage = "يجب إدخال الاسم")]
        public string NewUserName { set; get; }




        [Required(ErrorMessage = "يجب إدخال كلمة السر")]
        [StringLength(100, ErrorMessage = "يجب أن تكون طول كلمة السر أكبر من 5", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر الحالية")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "يجب إدخال كلمة السر")]
        [StringLength(100, ErrorMessage = "يجب أن تكون طول كلمة السر أكبر من 5", MinimumLength = 6)]

        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر الجديدة")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيدكلمةالسر")]
        [Compare("NewPassword", ErrorMessage = "كلمة السر وتأكيدها غير متوافقتين، يرجى إعادةالأدخال ")]
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
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
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
