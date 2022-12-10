using ProjectTracker.Core.Constants;
using ProjectTracker.Infrastructure.DataConstants;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ProjectTracker.Core.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(EmployeeConstants.UserNameMaxLength,
            MinimumLength = EmployeeConstants.UserNameMinLength)]
        [RegularExpression(ValidationRegex.PropertyRegex,
            ErrorMessage = "Contains unallowed characters")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(EmployeeConstants.FirstNameMaxLength,
            MinimumLength = EmployeeConstants.FirstNameMinLength)]
        [RegularExpression(ValidationRegex.PropertyRegex,
            ErrorMessage = "Contains unallowed characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(EmployeeConstants.LastNameMaxLength,
            MinimumLength = EmployeeConstants.LastNameMinLength)]
        [RegularExpression(ValidationRegex.PropertyRegex,
            ErrorMessage = "Contains unallowed characters")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(EmployeeConstants.EmailMaxLength,
            MinimumLength = EmployeeConstants.EmailMinLength)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(EmployeeConstants.PasswordMaxLength,
            MinimumLength = EmployeeConstants.PasswordMinLength)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}
