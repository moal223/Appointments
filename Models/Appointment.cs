namespace Appointement.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Details { get; set; }
        public DateTime? ExpiDate { get; set; }
        public bool ReminderSent { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
