using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class UpdateAuthorVM
    {
        [Required(ErrorMessage = "Is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string Surname { get; set; }

        public string? Img { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
