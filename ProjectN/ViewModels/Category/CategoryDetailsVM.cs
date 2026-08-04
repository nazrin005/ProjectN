using ProjectN.Models;

namespace ProjectN.ViewModels.Category
{
    public record CategoryDetailsVM
    {
        public Models.Category Category { get; set; }

        public List<Product> Products { get; set; }
    }
}
