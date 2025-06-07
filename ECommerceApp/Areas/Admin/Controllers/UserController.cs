using App.DataAccess.Data;
using App.DataAccess.Repository.IRepository;
using App.Models;
using App.Models.ViewModels;
using App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDBContext db, UserManager<IdentityUser> userManager) { 
            
            _db = db;
            _userManager = userManager;

        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;

            RoleManagementVM roleManagementVM = new RoleManagementVM()
            {
                ApplicationUser = _db.ApplicationUsers.Include(x => x.Company).FirstOrDefault(x => x.Id == userId),
                RoleList = _db.Roles.Select( x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyList = _db.Companies.Select( x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            roleManagementVM.ApplicationUser.Role = _db.Roles.FirstOrDefault(x => x.Id == RoleId).Name;
            return View(roleManagementVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            string RoleId = _db.UserRoles.FirstOrDefault(x => x.UserId == roleManagementVM.ApplicationUser.Id).RoleId;
            string oldRoleFromDB = _db.Roles.FirstOrDefault(x => x.Id == RoleId).Name;
            
            if(roleManagementVM.ApplicationUser.Role != oldRoleFromDB)
            {
                ApplicationUser applicationUser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == roleManagementVM.ApplicationUser.Id);
                if(roleManagementVM.ApplicationUser.Role == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if(oldRoleFromDB == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _db.SaveChanges();  
                _userManager.RemoveFromRoleAsync(applicationUser, oldRoleFromDB).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            return View(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> usersList = _db.ApplicationUsers.Include(x => x.Company).ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach(var user in usersList)
            {
                var roleID = userRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(x => x.Id == roleID).Name;
                if(user.Company == null)
                {
                    user.Company = new() { Name = "" }; 
                }
            }

            return Json(new { data = usersList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string? id)
        {
            var objFromDB = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);   
            if(objFromDB == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (objFromDB.LockoutEnd != null && objFromDB.LockoutEnd > DateTime.Now)
            {
                // user is locked , needs to be unlocked
                objFromDB.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDB.LockoutEnd = DateTime.Now.AddYears(100);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operarion Successfull" });
        }
    }
}
