using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class UpdateSettingsVM
    {
        [Required(ErrorMessage = "Is required")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string Value { get; set; }
    }
}
