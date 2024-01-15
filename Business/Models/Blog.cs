namespace Business.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string SubTitle { get; set; } = null!;
        public string Img { get; set; } = null!;
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

    }
}
