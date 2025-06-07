using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DataAccess.Data;
using App.DataAccess.Repository.IRepository;
using App.Models;

namespace App.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDBContext _dbContext;

        public CategoryRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public void Update(Category category)
        {
            var existingCategory = _dbContext.Categories.Find(category.Category_Id);
            if(existingCategory != null)
            {
                _dbContext.Entry(existingCategory).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            _dbContext.Categories.Update(category);
        }
    }
}
