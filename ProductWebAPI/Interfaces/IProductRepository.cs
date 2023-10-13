using System.Collections.Generic;
using System.Threading.Tasks;
using ProductWebAPI.Models;

namespace ProductWebAPI.Services
{
    public interface IProductRepository
    {
        Task<Product> Find(int id);
        Task<Product> Update(int id, Product product);
        Task<Product> AddProduct(Product product);
        Task<List<Product>> GetProducts();
        Task Delete(int id);
    }
}