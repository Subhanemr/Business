using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Is required")]
        public string UserNameOrEmail { get; set; }


        [Required(ErrorMessage = "Is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
