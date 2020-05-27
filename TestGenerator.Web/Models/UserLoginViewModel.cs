using System.ComponentModel.DataAnnotations;

namespace TestGenerator.Web.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Ce champs est requis")]
        [EmailAddress]
        [Display(Name="Adresse e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ce champs est requis.")]
        [DataType(DataType.Password)]
        [Display(Name="Mot de passe")]
        public string Password { get; set; }

        
        [Display(Name="Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
}