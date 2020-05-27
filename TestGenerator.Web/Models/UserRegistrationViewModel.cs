using System.ComponentModel.DataAnnotations;

namespace TestGenerator.Web.Models
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Une adresse e-mail est requise")]
        [EmailAddress]
        [Display(Name="Adresse e-mail")]
        public string Email { get; set; }

        [Compare("Email", ErrorMessage = "Les champs ne correspondent pas.")]
        [Display(Name="Confirmation de l'adresse e-mail")]
        public string EmailConfirm { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        [Display(Name="Nom")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        [Display(Name="Prénom")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Ce champs est requis.")]
        [DataType(DataType.Password)]
        [Display(Name="Mot de passe")]
        public string Password { get; set; }
 
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les champs ne correspondent pas.")]
        [Display(Name="Confirmation de mot de passe")]
        public string ConfirmPassword { get; set; }
    }
}
