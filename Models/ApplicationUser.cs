using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Healthcare.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The City field is required.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "The State field is required.")]
        [Display(Name = "State")]
        public string State { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}