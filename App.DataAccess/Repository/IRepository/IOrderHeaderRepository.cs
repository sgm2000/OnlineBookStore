using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);

        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);

        //create another method here for updating paymentID through Razorpay
    }
}
