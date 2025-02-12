using System.ComponentModel.DataAnnotations;

namespace CustomUserManmgment.ViewModels
{
    public class UpdateUserDataByAdminViewModel
    {

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

      
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public List<string>? Roles { get; set; }


    }
}
