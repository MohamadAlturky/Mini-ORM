using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;
using QueryBuilding.Contexts;

using var connection = new SqlConnection("Server=DESKTOP-OO326C9\\SQLEXPRESS01;Database=Test;User id=sa; Password=A@123456789; MultipleActiveResultSets=true;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Trusted_Connection=false");

var result = await Set<Product>
        .Query()
        // .Where(e => e.Id == 1)
        .OrderByDescending(e => e.Id)
        .OrderByDescending(e => e.Name)
        .ToList(connection);

foreach (var item in result)
{
    System.Console.WriteLine($"");
    System.Console.WriteLine($"{nameof(item.Name)} = {item.Name}");
    System.Console.WriteLine($"{nameof(item.Description)} = {item.Description}");
    System.Console.WriteLine($"{nameof(item.Id)} = {item.Id}");
    System.Console.WriteLine($"{nameof(item.Category)} = {item.Category}");
    System.Console.WriteLine($"");

}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Category { get; set; }
}
