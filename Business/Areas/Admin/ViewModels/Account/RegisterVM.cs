using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Compare with Password")]
        public string ConfirmPassword { get; set; }
    }
}
