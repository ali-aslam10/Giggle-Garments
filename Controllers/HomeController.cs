using Giggle_Garments_MVC.Models;
using Giggle_Garments_MVC.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Giggle_Garments_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        public IActionResult Index()
        {
            //Get cart count
            if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                string cartId=HttpContext.User.Identity.Name;
                CartRepository cartRepository = new CartRepository(_configuration.GetConnectionString("DefaultConnection"));
                int quantity = cartRepository.GetCartItemQuantity(cartId);
                if(quantity!=0)
                    HttpContext.Response.Cookies.Append("LoginQuantity", quantity.ToString());
            }
                return View();
        }
        public IActionResult Search(string searchitem)
        {
            productRepository productRepository = new productRepository(_configuration.GetConnectionString("DefaultConnection"));
            return View(productRepository.Search(searchitem));
        }
    }
}
