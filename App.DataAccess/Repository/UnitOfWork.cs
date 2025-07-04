﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DataAccess.Data;
//using App.DataAccess.Migrations;
using App.DataAccess.Repository.IRepository;

namespace App.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _db;

        public ICategoryRepository Category{ get; private set; }
        public IProductRepository Product{ get; private set; }
        public ICompanyRepository Company{ get; private set; }
        public IShoppingCartRepository ShoppingCart{ get; private set; }
        public IApplicationUserRepository ApplicationUser{ get; private set; }
        public IOrderHeaderRepository OrderHeader{ get; private set; }
        public IOrderDetailRepository OrderDetail{ get; private set; }
        public IProductImageRepository productImage{ get; private set; }
        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            productImage = new ProductImageRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
