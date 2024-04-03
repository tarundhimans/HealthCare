using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration in months is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 month")]
        public int DurationMonths { get; set; }
    }
}
