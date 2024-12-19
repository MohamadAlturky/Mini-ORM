using Data.Contexts;
using Tests;

class Program
{
    private static readonly ProductDataService _productDataService = new ProductDataService();

    static async Task Main()
    {
        var products = await FilterContext
        .Filter<ProductView,
            ProductFilter,
            ProductFilterSpecification>(
                new ProductFilterSpecification() ,
                new ProductFilter()
                {
                    Start = 5,
                    // End = 8
                });

        
        foreach (var item in products)
        {
           System.Console.WriteLine("-------------------------"); 
           System.Console.WriteLine(); 
           System.Console.WriteLine("product"); 
           System.Console.WriteLine(item.Id); 
           System.Console.WriteLine(item.Name); 
           System.Console.WriteLine(item.Description); 
           System.Console.WriteLine(item.Category); 
           System.Console.WriteLine();
           System.Console.WriteLine("-------------------------"); 
        }
        while (true)
        {
            Console.WriteLine("Choose an action: 1) Create 2) Update 3) Delete 4) Get 5) Get All 6) Exit");
            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    await CreateProduct();
                    break;
                case "2":
                    await UpdateProduct();
                    break;
                case "3":
                    await DeleteProduct();
                    break;
                case "4":
                    await GetProduct();
                    break;
                case "5":
                    await GetAllProducts();
                    break;
                case "6":
                    return; // Exit the application
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static async Task CreateProduct()
    {
        Console.WriteLine("Enter Product Name:");
        string name = Console.ReadLine()!;

        Console.WriteLine("Enter Product Description:");
        string description = Console.ReadLine()!;

        Console.WriteLine("Enter Product Category (integer):");
        int category;
        while (!int.TryParse(Console.ReadLine(), out category))
        {
            Console.WriteLine("Invalid input. Please enter an integer for the category:");
        }

        var product = new Product
        {
            Name = name,
            Description = description,
            Category = category
        };

        var result = await _productDataService.CreateProduct(product);
        Console.WriteLine($"Product Created: {result}");
    }

    private static async Task UpdateProduct()
    {
        Console.WriteLine("Enter Product ID to update:");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid input. Please enter a valid Product ID:");
        }

        var product = await _productDataService.GetProduct(id);
        
        if (product is null)
        {
            Console.WriteLine("Product not found");
            return;
        }
        
        Console.WriteLine("Current product details:");
        DisplayProduct(product);

        Console.WriteLine("Enter new Product Name:");
        string name = Console.ReadLine()!;

        Console.WriteLine("Enter new Product Description:");
        string description = Console.ReadLine()!;

        Console.WriteLine("Enter new Product Category (integer):");
        int category;

        while (!int.TryParse(Console.ReadLine(), out category))
        {
            Console.WriteLine("Invalid input. Please enter an integer for the category:");
        }

        var updatedProduct = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Category = category
        };

        await _productDataService.UpdateProduct(updatedProduct);
        
        Console.WriteLine($"Product Updated: {updatedProduct.Id}");
    }

    private static async Task DeleteProduct()
    {
        Console.WriteLine("Enter Product ID to delete:");
        
        int id;
        
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid input. Please enter a valid Product ID:");
        }

        var product = await _productDataService.GetProduct(id);
        
        if (product is null)
        {
            Console.WriteLine("Product not found");
            return;
        }
        
        Console.WriteLine("Current product details:");
        DisplayProduct(product);

        await _productDataService.DeleteProduct(id);

        Console.WriteLine($"Product with ID {id} deleted successfully.");
    }

    private static async Task GetProduct()
    {
        Console.WriteLine("Enter Product ID to retrieve:");
        
        int id;
        
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Invalid input. Please enter a valid Product ID:");
        }

        var product = await _productDataService.GetProduct(id);
        
        if (product is null)
        {
            Console.WriteLine("Product not found");
            return;
        }
        
       DisplayProduct(product);
    }

    private static async Task GetAllProducts()
    {
       IEnumerable<Product> products = await _productDataService.GetAllProducts();
       
       if (products == null || !products.Any())
       {
           Console.WriteLine("No products found.");
           return;
       }
       
       foreach (var product in products)
       {
           DisplayProduct(product);
       }
   }

   private static void DisplayProduct(Product product)
   {
       Console.WriteLine($"Id: {product.Id}");
       Console.WriteLine($"Name: {product.Name}");
       Console.WriteLine($"Description: {product.Description}");
       Console.WriteLine($"Category: {product.Category}");
       Console.WriteLine("---------------------------");
   }
}
