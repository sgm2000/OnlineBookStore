﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category{ get; }
        IProductRepository Product{ get; }
        ICompanyRepository Company{ get; }
        IShoppingCartRepository ShoppingCart{ get; }
        IApplicationUserRepository ApplicationUser{ get; }
        IOrderDetailRepository OrderDetail{ get; }
        IOrderHeaderRepository OrderHeader{ get; }
        IProductImageRepository productImage{ get; }
        void Save();

    }
}
