using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class CategoryService
    {
        private readonly CategoryRepository _repo;

        public CategoryService()
        {
            _repo = new CategoryRepository();
        }

        public List<Category> GetAll() => _repo.GetAll();

        public void Add(Category category) => _repo.Add(category);

        public void Update(Category category) => _repo.Update(category);

        public void Delete(int id) => _repo.Delete(id);
    }


}
