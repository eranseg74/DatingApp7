using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppRole : IdentityRole<int> // For the Id property. Sets it as Integer rather than string
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}