using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class CreateSettingsVM
    {
        [Required(ErrorMessage = "Is required")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string Value { get; set; }
    }
}
