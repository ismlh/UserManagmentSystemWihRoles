using System.ComponentModel.DataAnnotations;

namespace CustomUserManmgment.ViewModels
{
    public class RigsterViewModel
    {

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password {  get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassworded { get; set; }
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public byte[]? profilePicture { get; set; }

        public List<string>? Roles { get; set; }
    }
}
