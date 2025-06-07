using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.DataAccess.Data;
using App.Models;
using App.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDBContext _dbContext;
        public DBInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDBContext dBContext) { 
            
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dBContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }
            }
            catch (Exception e)
            {
                // Log error if needed
                throw;
            }

            // Check and create roles only after migrations are applied
            if (!await _roleManager.RoleExistsAsync(StaticDetails.Role_Customer))
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer));
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin));
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company));
                await _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee));

                var user = new ApplicationUser
                {
                    UserName = "adminTextMartOnline@gmail.com",
                    Email = "adminTextMartOnline@gmail.com",
                    Name = "Admin",
                    PhoneNumber = "1234567890",
                    StreetAddress = "test 123 CA",
                    State = "CA",
                    PostalCode = "12345",
                    City = "SanFrancisco",
                };

                var result = await _userManager.CreateAsync(user, "Admin123*");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin);
                }
            }
        }
    }
}
