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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDBContext _dbContext;

        public ShoppingCartRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public void Update(ShoppingCart shoppingCart)
        {
            var existingCategory = _dbContext.Categories.Find(shoppingCart.Id);
            if(existingCategory != null)
            {
                _dbContext.Entry(existingCategory).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            _dbContext.ShoppingCarts.Update(shoppingCart);
        }
       
    }
}
