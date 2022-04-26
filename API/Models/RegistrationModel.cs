using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class RegistrationModel
    {
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string mobile { get; set; }
        [Required]
        public string password { get; set; }
        [Compare("password",ErrorMessage ="Password not matched")]
        public string confirmpassword { get; set; }
    }
}
