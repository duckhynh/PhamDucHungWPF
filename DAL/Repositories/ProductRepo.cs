using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepo
    {
        private readonly FuminiTikiSystemContext _context;

        public ProductRepo(FuminiTikiSystemContext context)
        {
            _context = context;
        }

        public List<Product> GetAll() => _context.Products.Include(p => p.Category).ToList();

        public List<Product> Search(string keyword) =>
            _context.Products
                    .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                    .Include(p => p.Category)
                    .ToList();

        public List<Product> FilterByCategory(int categoryId) =>
            _context.Products
                    .Where(p => p.CategoryId == categoryId)
                    .Include(p => p.Category)
                    .ToList();

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
