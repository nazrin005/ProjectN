using ProjectN.Models;

namespace ProjectN.ViewModels.Category
{
    public class CategoryDetailsVM
    {
        public Models.Category Category { get; set; }

        public List<Product> Products { get; set; }
    }
}
