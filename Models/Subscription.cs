using System.ComponentModel.DataAnnotations;

namespace E_Healthcare.Models
{
    public class Subscription
    {
        public int Id { get; set; }       
        public int SubscriptionPlanId { get; set; } 
        public SubscriptionPlan Plan { get; set; }
        public int ApplicationUserId {  get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date for the start date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date for the end date")]
        public DateTime EndDate { get; set; }

    }
}
