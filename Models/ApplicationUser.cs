using Microsoft.AspNetCore.Identity;

namespace Appointement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Appointment>? Appointments { get; set; }
    }
}
