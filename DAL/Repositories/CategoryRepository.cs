using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly FuminiTikiSystemContext _context;

        public CategoryRepository()
        {
            _context = new FuminiTikiSystemContext();
        }

        public List<Category> GetAll() => _context.Categories.Include(c => c.Products).ToList();

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int categoryId)
        {
            
            
                var category = _context.Categories.Find(categoryId);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }
            
            
        }
            
        
    }

}
