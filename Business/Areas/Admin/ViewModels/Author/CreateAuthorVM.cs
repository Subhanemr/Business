using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class CreateAuthorVM
    {
        [Required(ErrorMessage = "Is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Is required")]
        public IFormFile Photo { get; set; }
    }
}
