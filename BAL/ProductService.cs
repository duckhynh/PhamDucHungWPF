using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;

namespace BAL
{
    public class ProductService
    {
        private readonly ProductRepo _repo;

        public ProductService(ProductRepo repo)
        {
            _repo = repo;
        }

        public List<Product> GetAllProducts() => _repo.GetAll();

        public List<Product> Search(string keyword) => _repo.Search(keyword);

        public List<Product> FilterByCategory(int categoryId) => _repo.FilterByCategory(categoryId);

        public void AddProduct(Product product) => _repo.Add(product);

        public void DeleteProduct(int id) => _repo.Delete(id);

        public void UpdateProduct(Product product) => _repo.Update(product);

    }
}
