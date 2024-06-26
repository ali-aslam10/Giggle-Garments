using Giggle_Garments_MVC.Models;
using Giggle_Garments_MVC.Models.Repositories;
using Giggle_Garments_MVC.Models.View_Model;
using Microsoft.AspNetCore.Mvc;

namespace Giggle_Garments_MVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;
        public CheckoutController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
       public IActionResult Checkoutform()
       {
            return View();
       }
        [HttpPost]
        public IActionResult Checkoutform(OrderInfo order)
        {
            //get cart detail
            dynamic cartItems;
            if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {

                String cartid = HttpContext.User.Identity.Name;
                CartRepository cartRepository = new CartRepository(_configuration.GetConnectionString("DefaultConnection"));
                cartItems=cartRepository.getCart(cartid);
                cartRepository.RemoveCart(cartid);
                Response.Cookies.Delete("LoginQuantity");

            }
            else
            { 
                 cartItems = HttpContext.Session.Get<List<CartItem>>("cartItems");
                 Response.Cookies.Delete("Quantity");
                 HttpContext.Session.Remove("cartItems");
            }
            int orderId = GetOrderId();
            CheckoutRepository checkoutRepository = new CheckoutRepository(_configuration.GetConnectionString("DefaultConnection"));
            order.OrderDate = DateTime.Now.ToString("dd-MM-yyyy");
            order.Id=orderId;
            checkoutRepository.add(order);
            
            List<OrderedProduct> products = new List<OrderedProduct>();
            foreach (var item in cartItems)
            {
                products.Add(new OrderedProduct { OrderId= orderId,
                ProductId=item.ProdId,Size=item.Size,Quantity=item.Quantity}); 
            }
            checkoutRepository.AddOrderedProduct(products);
            TempData["id"] = orderId;
            return RedirectToAction("receipt", "Checkout");
        }
        public IActionResult receipt()
        {
            CheckoutRepository checkoutRepository = new CheckoutRepository(_configuration.GetConnectionString("DefaultConnection"));
            int orderid = (int)TempData["id"];
            OrderInfo order=checkoutRepository.GetOrderInfo(orderid);
            List<CartItem> orderedProducts=checkoutRepository.GetOrderedProducts(orderid);
            CustomerProduct customerProduct = new CustomerProduct();
            customerProduct.order = order;
            customerProduct.orderedProducts = orderedProducts; 

            return View(customerProduct);
        }
        private int GetOrderId()
        {
            Thread.Sleep(1000);

            // Get the current timestamp in milliseconds since Unix epoch
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            // Mask the timestamp to get only the lower 32 bits
            int seed = (int)(timestamp & 0xFFFFFFFF);

            // Initialize the random number generator with the truncated timestamp as the seed
            Random random = new Random(seed);

            // Generate a random 5-digit ID
            return random.Next(10000, 100000);
        }

    }
}
