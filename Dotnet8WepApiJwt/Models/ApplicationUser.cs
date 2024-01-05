using Dotnet8WepApiJwt.Enums;
using Microsoft.AspNetCore.Identity;

namespace Dotnet8WepApiJwt.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}
