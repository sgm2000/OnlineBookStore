using App.DataAccess.Data;
using App.DataAccess.Repository;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) {

            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public ActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {

            RoleManagementVM roleManagementVM = new RoleManagementVM()
            {
                ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userId, includeProperties: "Company"),
                RoleList = _roleManager.Roles.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            roleManagementVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser
                .GetFirstOrDefault(x => x.Id == userId)).GetAwaiter().GetResult().FirstOrDefault();
            return View(roleManagementVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            string oldRoleFromDB = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser
                .GetFirstOrDefault(x => x.Id == roleManagementVM.ApplicationUser.Id)).GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == roleManagementVM.ApplicationUser.Id);

            if (roleManagementVM.ApplicationUser.Role != oldRoleFromDB)
            {
                if(roleManagementVM.ApplicationUser.Role == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                if(oldRoleFromDB == StaticDetails.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();  
                _userManager.RemoveFromRoleAsync(applicationUser, oldRoleFromDB).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            else
            {
                if(oldRoleFromDB == StaticDetails.Role_Company && applicationUser.CompanyId != roleManagementVM.ApplicationUser.CompanyId)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> usersList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();
            foreach (var user in usersList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null)
                {
                    user.Company = new() { Name = "" };
                }
            }

            return Json(new { data = usersList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string? id)
        {
            var objFromDB = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id, tracked: true);
            if (objFromDB == null)
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
            _unitOfWork.ApplicationUser.Update(objFromDB);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operarion Successfull" });
        }
    }
}
