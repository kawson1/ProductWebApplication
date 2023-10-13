using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Models;

namespace ProductWebAPI.Services
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }
    }
    public class ProductRepository : IProductRepository
    {
        ProductDBContext dbContext;
        public ProductRepository(ProductDBContext context)
        {
            dbContext = context;
        }

        public async Task<Product> Find(int id)
        {
            return await dbContext.Set<Product>().FindAsync(id);
        }

        public async Task<Product> Update(int id, Product product)
        {
            try
            {
                var p = await Find(id);
                p.Value = product.Value;
                p.Name = product.Name;
                p.Description = product.Description;
                dbContext.Entry(p).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return product;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    throw new NotFoundException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Delete(int id)
        {
            // 1 approach: 
            var p = await dbContext.Products.FindAsync(id);
            if (p == null)
                return;
            dbContext.Products.Remove(p);
            await dbContext.SaveChangesAsync();
            return;

            // 2 approach:
            //      - remove immediately 
            //      - catch exception in exception filter (in controller) 
        }

        public async Task<Product> AddProduct(Product product)
        {
            if (dbContext.Products != null)
            {
                var _product = dbContext.Products.Add(product).Entity;
                await dbContext.SaveChangesAsync();
                return _product;
            }
            else
                throw new Exception("Failed during adding product");
        }

        public async Task<List<Product>> GetProducts()
        {
            if (dbContext.Products != null)
                return await dbContext.Products.ToListAsync();
            else
                return new List<Product>();
        }

        private bool ProductExists(int id)
        {
            return (dbContext.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
