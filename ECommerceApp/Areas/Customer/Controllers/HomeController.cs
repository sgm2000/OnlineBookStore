using System.Diagnostics;
using System.Security.Claims;
using App.DataAccess.Repository.IRepository;
using App.Models;
using App.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ECommerceApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, 
                    _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).Count());
            }
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
            return View(productList);
        }
        
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId
            };

            
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            
            ShoppingCart shoppingCartFromDB = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.ApplicationUserId == userId 
                                                && x.ProductId == shoppingCart.ProductId);

            if (shoppingCartFromDB != null)
            {
                shoppingCartFromDB.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(shoppingCartFromDB);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, 
                    _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == userId).Count());
            }
            
            TempData["success"] = "Cart updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
