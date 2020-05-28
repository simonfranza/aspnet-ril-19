using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TestGenerator.Model.Entities
{
    public class User : IdentityUser
    {

        [Required]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Display(Name = "First name")]
        public string Firstname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MaxLength(255)]
        [Display(Name = "Last name")]
        public string Lastname { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => Firstname+ " " + Lastname;
    }
}
