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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDBContext _dbContext;

        public OrderDetailRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public void Update(OrderDetail orderDetail)
        {
            var existingOrderDetail = _dbContext.OrderDetails.Find(orderDetail.Id);
            if(existingOrderDetail != null)
            {
                _dbContext.Entry(existingOrderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            _dbContext.OrderDetails.Update(orderDetail);
        }
    }
}
