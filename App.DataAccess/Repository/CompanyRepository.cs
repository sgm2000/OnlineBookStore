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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDBContext _dbContext;

        public CompanyRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public void Update(Company Company)
        {
            var existingCompany = _dbContext.Companies.Find(Company.Id);
            if(existingCompany != null)
            {
                _dbContext.Entry(existingCompany).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            _dbContext.Companies.Update(Company);
        }
    }
}
