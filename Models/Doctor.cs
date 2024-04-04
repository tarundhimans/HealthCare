using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialties are required")]
        public string Specialties { get; set; }

        [Required(ErrorMessage = "Qualifications are required")]
        public string Qualifications { get; set; }

        [Required(ErrorMessage = "Consultation fee is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Consultation fee must be a positive value")]
        public decimal ConsultationFee { get; set; }

    }
}
