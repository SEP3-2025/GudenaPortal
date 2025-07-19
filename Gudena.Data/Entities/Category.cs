namespace Gudena.Data.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CategoryType { get; set; }

    public int ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }
    public List<Category> Children { get; set; }
    public ICollection<Product> Products { get; set; }
}