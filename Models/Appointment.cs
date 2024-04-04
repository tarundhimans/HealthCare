using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Personal details are required.")]
        public string PersonalDetails { get; set; }

        [Required(ErrorMessage = "Medical history is required.")]
        public string MedicalHistory { get; set; }

        [Required(ErrorMessage = "Insurance information is required.")]
        public string InsuranceInformation { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment time is required.")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentTime { get; set; }


    }
}
