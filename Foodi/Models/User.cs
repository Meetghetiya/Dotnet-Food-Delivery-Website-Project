using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Foodi.Models
{
    public class User
    {
        public int? UserID { get; set; }

        [DisplayName("Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The UserName field is required.")]
        [DisplayName("UserName")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "The Mobile Number field is required.")]
        [DisplayName("Mobile Number")]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Please Enter valid Email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "The Address field is required.")]
        [DisplayName("Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "The PostCode field is required.")]
        [DisplayName("PostCode")]
        public string? PostCode { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid password")]
        public string? Password { get; set; }

        [DisplayName("Image")]
        public string? Image { get; set; }

        public bool IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

 


  
}
