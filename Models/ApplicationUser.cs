using Microsoft.AspNetCore.Identity;

namespace TaskManagerApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public int Age { get; set; }
    }
}
