
using Microsoft.EntityFrameworkCore;

namespace ProductWebAPI.Models
{
    public class ProductDBContext : DbContext
    {
        public DbSet<Product> Products { get; set;}

        public ProductDBContext(DbContextOptions<ProductDBContext> options)
            : base(options)
        {
        }

        public static void Initialize(ProductDBContext context)
        {
            if (context.Products.Any())
                return;

            var products = new List<Product>()
            {
                new Product
                {
                    //Id = 1,
                    Name = "product1",
                    Description = "p1 description",
                    Value = 9.99f
                },
                new Product
                {
                   //Id = 2,
                    Name = "product2",
                    Description = "dgfhdfgh dfghdfg hdfgh",
                    Value = 1.23f
                },
                new Product
                {
                    //Id = 3,
                    Name = "product3",
                    Description = "d fghdfgh dfghdfghdfgh",
                    Value = 12.50f
                },
                new Product
                {
                    //Id = 4,
                    Name = "product4",
                    Description = " dfgh dfgh fdgh fdg hdfghd fdf ghfdghdfgh dfgh dfg hdfg hdfgh",
                    Value = 1.00f
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
