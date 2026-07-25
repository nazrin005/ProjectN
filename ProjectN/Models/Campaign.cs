using ProjectN.Models.Base;

namespace ProjectN.Models
{
    public class Campaign :BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }

        public string ButtonText { get; set; }
        public string ButtonUrl { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
