using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Specialties { get; set; }
        [Required]
        public string Qualifications { get; set; }
        [Required]
        public decimal ConsultationFee { get; set; }
    }
}
