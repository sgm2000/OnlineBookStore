using App.DataAccess.Data;
using App.DataAccess.Repository.IRepository;
using App.Models;
using App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ECommerceApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IList<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                bool categoryExists = _unitOfWork.Category.GetAll().Any(x => x.Name.ToLower() == obj.Name.ToLower());
                if (categoryExists)
                {
                    ModelState.AddModelError("Name", "Category Name already exists");
                    return View();
                }

                //_dbContext.Add(obj);
                _unitOfWork.Category.Add(obj);

                //_dbContext.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Category created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? categoryId) // this should match with asp-tag which is declared in Edit view
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }

            //Category? categoryDb = _dbContext.Categories.FirstOrDefault( x => x.Category_Id == categoryId);
            Category? categoryDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_Id == categoryId);
            if (categoryDb == null)
            {
                return NotFound();
            }
            return View(categoryDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                bool categoryExists = _unitOfWork.Category.GetAll().Any(x => x.Name.ToLower() == obj.Name.ToLower() && x.Category_Id != obj.Category_Id);
                if (categoryExists)
                {
                    ModelState.AddModelError("Name", "Category Name already exists");
                    return View();
                }

                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? categoryId)
        {
            if (categoryId == null)
                return NotFound();

            Category? categoryDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_Id == categoryId);

            if (categoryDb == null)
                return NotFound();
            return View(categoryDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(Category obj)
        {
            if (obj == null)
                return NotFound();
            _unitOfWork.Category.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
