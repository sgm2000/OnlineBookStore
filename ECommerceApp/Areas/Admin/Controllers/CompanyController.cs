using App.DataAccess.Data;
using App.DataAccess.Repository.IRepository;
using App.Models;
using App.Models.ViewModels;
using App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IList<Company> CompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(CompanyList);
        }

        public IActionResult Upsert(int? id)
        {
                     
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if(companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update (companyObj);
                }
                //_dbContext.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Company created Successfully";
                return RedirectToAction("Index");
            }    
            else
            {
                return View(companyObj);
            }
            
        }

        #region Deprecated
        //public IActionResult Edit(int? Id) // this should match with asp-tag which is declared in Edit view
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }

        //    //Company? CompanyDb = _dbContext.Categories.FirstOrDefault( x => x.Company_Id == CompanyId);
        //    Company? CompanyDb = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == Id);
        //    if (CompanyDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool CompanyExists = _unitOfWork.Company.GetAll().Any(x => x.ISBN.ToLower() == obj.ISBN.ToLower() && x.Id != obj.Id);
        //        if (CompanyExists)
        //        {
        //            ModelState.AddModelError("ISBN", "Company Name already exists");
        //            return View();
        //        }

        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Company updated Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public IActionResult Delete(int? Id)
        //{
        //    if (Id == null)
        //        return NotFound();

        //    Company? CompanyDb = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == Id);

        //    if (CompanyDb == null)
        //        return NotFound();
        //    return View(CompanyDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(Company obj)
        //{
        //    if (obj == null)
        //        return NotFound();
        //    _unitOfWork.Company.Delete(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company deleted Successfully";
        //    return RedirectToAction("Index");
        //}

        #endregion



        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var Company = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            if(Company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Delete(Company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company deleted Successfully" });
        }
        #endregion
    }
}
