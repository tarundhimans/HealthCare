using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; } 
        public Doctor Doctor { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string PersonalDetails { get; set; }
        [Required]
        public string MedicalHistory { get; set; }
        [Required]
        public string InsuranceInformation { get; set; }
        [Required]
        public DateTime AppointmentDateTime { get; set; }
    }
}
