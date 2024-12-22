using QueryBuilding.Models;

namespace QueryBuilding.Contexts;

public static class DataTableContext<T>
{
    public static Set<T> Query()
    {
        return new Set<T>();
    }
}