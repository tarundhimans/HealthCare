using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        [Range(0, 5, ErrorMessage = "Rating value must be between 0 and 5.")]
        public int RatingValue { get; set; }
        public string Comment { get; set; }
    }
}
