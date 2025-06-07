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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IList<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Category_Id.ToString(),
            });
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };            
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if(!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using ( var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + fileName;
                }
                if(obj.Product.Id == 0)
                {
                    bool productExists = _unitOfWork.Product.GetAll().Any(x => x.ISBN.ToLower() == obj.Product.ISBN.ToLower());
                    if (productExists)
                    {
                        ModelState.AddModelError("Name", "Product Name already exists");
                        return View();
                    }
                    //_dbContext.Add(obj);
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }

                //_dbContext.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Product created Successfully";
                return RedirectToAction("Index");
            }    
            else
            {
                obj.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Category_Id.ToString(),
                });
                return View(obj);
            }
            
        }

        #region Deprecated
        //public IActionResult Edit(int? Id) // this should match with asp-tag which is declared in Edit view
        //{
        //    if (Id == null || Id == 0)
        //    {
        //        return NotFound();
        //    }

        //    //Product? ProductDb = _dbContext.Categories.FirstOrDefault( x => x.Product_Id == ProductId);
        //    Product? ProductDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == Id);
        //    if (ProductDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ProductDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool productExists = _unitOfWork.Product.GetAll().Any(x => x.ISBN.ToLower() == obj.ISBN.ToLower() && x.Id != obj.Id);
        //        if (productExists)
        //        {
        //            ModelState.AddModelError("ISBN", "Product Name already exists");
        //            return View();
        //        }

        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product updated Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public IActionResult Delete(int? Id)
        //{
        //    if (Id == null)
        //        return NotFound();

        //    Product? ProductDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == Id);

        //    if (ProductDb == null)
        //        return NotFound();
        //    return View(ProductDb);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(Product obj)
        //{
        //    if (obj == null)
        //        return NotFound();
        //    _unitOfWork.Product.Delete(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product deleted Successfully";
        //    return RedirectToAction("Index");
        //}

        #endregion



        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if(product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Delete(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted Successfully" });
        }
        #endregion
    }
}
