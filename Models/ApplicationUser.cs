using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CustomUserManmgment.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName {  get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        public string FullName { get; set; }

        public byte[]? profilePicture { get; set; }
    }
}
