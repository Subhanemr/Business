using a =Business.Models;
using System.ComponentModel.DataAnnotations;

namespace Business.Areas.Admin.ViewModels
{
    public class CreateBlogVM
    {
        [Required(ErrorMessage = "Is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Is required")]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "Is required")]
        public int AuthorId { get; set; }
        public ICollection<a.Author>? Authors { get; set; }
        
        [Required(ErrorMessage = "Is required")]
        public IFormFile Photo { get; set; }
    }
}
