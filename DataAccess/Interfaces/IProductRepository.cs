// Vị trí: ConvenienceStoreManager/DataAccess/Interfaces/IProductRepository.cs

using System.Collections.Generic;
using System.Threading.Tasks;
using ConvenienceStoreManager.Entities;

namespace ConvenienceStoreManager.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        // Phương thức đồng bộ (giữ lại để tương thích với code cũ)
        List<Product> GetAllProducts();
        Product GetProductById(int productId);
        Product GetProductByCode(string productCode);
        List<Product> SearchProducts(string keyword);
        int AddProduct(Product product);
        bool UpdateProduct(Product product);
        bool DeleteProduct(int productId);
        bool UpdateStock(int productId, int quantityChange);

        // Phương thức bất đồng bộ mới
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> GetProductByCodeAsync(string productCode);
        Task<List<Product>> SearchProductsAsync(string keyword);
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        Task<int> AddProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantityChange);

    }
}