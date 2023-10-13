using Microsoft.AspNetCore.Identity;

namespace server.Models
{
    public class UserApp : IdentityUser
    {
        public string? Names { get; set; }
    }
}