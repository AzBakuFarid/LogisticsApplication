using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LogisticsApp.Models
{

    public class CountriesInManagmentViewModel :GeneralContentViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public string Username { get; set; }
        public string UserSurname { get; set; }
    }
    public class EditPersonalDataViewModel : GeneralContentViewModel
    {

        [EmailAddress]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Ad *")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Soyad *")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Dogum tarixi *")]
        public string Birthday { get; set; }

        [Required]
        [Display(Name = "Unvan *")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Sexsiyyet vesiqesinin seriya ve nomresi *")]
        public string IDCardNumber { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 7)]
        [Display(Name = "FIN *")]
        public string FINNumber { get; set; }

        [Required]
        [Display(Name = "Telefon *")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Kohne parol *")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni parol *")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni parolu tekrar daxil edin *")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class GeneralContentViewModel {
        public int CustomerID { get; set; }
        public double Balance { get; set; }
        public int MessageCounter { get; set; }
        public int InqueryCounter { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel 
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

   

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}