namespace Core.Models;

public class PaginatedResult<T>
{
    public T[] Items { get; set; } = null!;
    public int TotalCount { get; set; }
}
