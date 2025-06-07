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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDBContext _dbContext;

        public ProductRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public void Update(Product product)
        {
            var existingProduct = _dbContext.Products.Find(product.Id);
            if(existingProduct != null)
            {
                //Explicitly updating each attribute in database or manual mapping
                _dbContext.Entry(existingProduct).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                existingProduct.Title = product.Title;
                existingProduct.Author = product.Author;
                existingProduct.ISBN = product.ISBN;
                existingProduct.Price = product.Price;
                existingProduct.Price50 = product.Price50;
                existingProduct.ListPrice = product.ListPrice;
                existingProduct.Price100 = product.Price100;
                existingProduct.Description = product.Description;
                existingProduct.Category_Id = product.Category_Id;
                if (product.ImageUrl != null)
                {
                    existingProduct.ImageUrl = product.ImageUrl;
                }
                _dbContext.Products.Update(existingProduct);
            }
            
            //_dbContext.Products.Update(product);  // Auto update from entity framework
        }
    }
}
