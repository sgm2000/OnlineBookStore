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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDBContext _dbContext;

        public OrderHeaderRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public void Update(OrderHeader orderHeader)
        {
            //var existingOrderHeader = _dbContext.OrderHeaders.Find(orderHeader.Id);
            //if(existingOrderHeader != null)
            //{
            //    _dbContext.Entry(existingOrderHeader).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            //}
            _dbContext.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _dbContext.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        //create another method here for updating paymentID through Razorpay
    }
}
