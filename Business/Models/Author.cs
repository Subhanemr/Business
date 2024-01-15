namespace Business.Models
{
    public class Author : BaseNameEntity
    {
        public string Surname { get; set; } = null!;
        public string Img { get; set; } = null!;

        public ICollection<Blog>? Blogs { get; set; }

    }
}
