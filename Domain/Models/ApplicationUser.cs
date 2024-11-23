
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        
        public string? VerificationCode { get; set; }
        
        //ONE-TO-MANY RELATIONSHIP
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
