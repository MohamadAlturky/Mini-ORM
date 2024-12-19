using Core.Models;
public class Product : ITable<int>
{

    public int Id { get; set; }

    public string TableName { get; set; } = nameof(Product);
    
    public List<string> ColumnsWithOutId { get; set; } = new List<string>
    {
        nameof(Name),
        nameof(Description),
        nameof(Category)
    };

    public string Name { get; set; }
    public string Description { get; set; }
    public int Category { get; set; }
}