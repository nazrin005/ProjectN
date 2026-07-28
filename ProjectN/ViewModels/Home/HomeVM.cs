using ProjectN.Models;

namespace ProjectN.ViewModels.Home
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Product> BestSellers { get; set; }
        public List<ProjectN.Models.Category> Categories { get; set; }
        public List<Blog> Blogs { get; set; }
        public Campaign? Campaign { get; set; }
        public HeroBanner? HeroBanner { get; set; }
    }
}
