using Microsoft.AspNetCore.Identity;

namespace client.Models
{
    public class UserApp : IdentityUser
    {
        public string? Names { get; set; }
    }
}