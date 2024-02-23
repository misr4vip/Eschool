using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ESchool.Models.ApplicationUser;

namespace ESchool.Models
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
        [Required]
        [Display(Name = "اسم المستخدم")]
        
        public string IdentityId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [Display(Name = "تذكرني")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        [Required, MinLength(10, ErrorMessage = "رقم الهوية اقل من 10 ارقام"), MaxLength(10, ErrorMessage = "رقم الهوية اكثر من 10 ارقام"), DisplayName("رقم الهوية")]
        public string IdentityId { get; set; }
        [Required, Display(Name = "الاسم")]
        public string Name { get; set; }
        [Display(Name = "الاسم باللغة الإنجليزية")]
        public string EnglishName { get; set; }
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }
        [Required, Display(Name = "تاريخ الميلاد ")]
        public string Dob { get; set; }
        [Required, Display(Name = "الجنسية")]
        public Nathionality nathionality { get; set; }

        [Display(Name = "جوال 1 ")]
        public string Mobile1 { get; set; }
        [Display(Name = "جوال 2 ")]
        public string Mobile2 { get; set; }
        [Display(Name = "هاتف المنزل ")]
        public string tel { get; set; }
        //public virtual Nationality nationality { get; set; }
        [Display(Name = "الحالة ")]
        public MemberStatus status { get; set; }
        [Display(Name = "نوع الحساب ")]
        public MemberType type { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تاكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
