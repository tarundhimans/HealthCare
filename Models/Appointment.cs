using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; } 
        public Doctor Doctor { get; set; }     

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Personal details are required.")]
        public string PersonalDetails { get; set; }

        [Required(ErrorMessage = "Medical history is required.")]
        public string MedicalHistory { get; set; }

        [Required(ErrorMessage = "Insurance information is required.")]
        public string InsuranceInformation { get; set; }

        [Required(ErrorMessage = "Appointment date and time are required.")]
        public DateTime AppointmentDateTime { get; set; }
      
    }
}
