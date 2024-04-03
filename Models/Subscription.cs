namespace E_Healthcare.Models
{
    public class Subscription
    {
        public int Id { get; set; }       
        public int SubscriptionPlanId { get; set; } 
        public SubscriptionPlan Plan { get; set; }
        public int ApplicationUserId {  get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
