namespace Business.Models
{
    public class Settings : BaseEntity
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

    }
}
