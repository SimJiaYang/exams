using Q1.Model;

using System.Collections.Generic;
using System.Linq;

namespace Q1.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new List<Product>();

        public Product GetProduct(long id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products.AsReadOnly();
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }
    }
}