using Giggle_Garments_MVC.Models;
using Giggle_Garments_MVC.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Giggle_Garments_MVC.Controllers
{
    public class OrderController : Controller
    {
        public string cartItemCountKey { get; set; }
        private readonly IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        public ViewResult viewCart()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                
                    String cartid = HttpContext.User.Identity.Name;
                    CartRepository cartRepository = new CartRepository(_configuration.GetConnectionString("DefaultConnection"));
                    return View(cartRepository.getCart(cartid));
                
            }
            else
            {

                var cartItems = HttpContext.Session.Get<List<CartItem>>("cartItems");
                

                if (cartItems != null)
                {
                    return View(cartItems);
                }
            }

            //if cartitems null in 2nd case just send empty list 

            return View(new List<CartItem>());
        }
        
        public IActionResult addtoCart(CartItem cartitem)
        {
            cartitem.Id =Guid.NewGuid().ToString();

            if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                cartitem.CartId = HttpContext.User.Identity.Name;
                CartRepository cartRepository = new CartRepository(_configuration.GetConnectionString("DefaultConnection"));

                if (!cartRepository.Isupdate(cartitem.CartId, cartitem.ProdId, cartitem.Quantity, cartitem.Size))
                {
                    cartRepository.add(cartitem);
                }
                cartItemCountKey = "LoginQuantity";
                
               
                if (HttpContext.Request.Cookies.ContainsKey(cartItemCountKey))
                {
                    //Update Quantity

                    int cartItemCount = Convert.ToInt32(HttpContext.Request.Cookies[cartItemCountKey]) + cartitem.Quantity;
                    HttpContext.Response.Cookies.Append(cartItemCountKey, cartItemCount.ToString());
                }
                else
                {
                    int quantity = cartRepository.GetCartItemQuantity(cartitem.CartId);
                    HttpContext.Response.Cookies.Append(cartItemCountKey, quantity.ToString());

                }

            }
            else
            {
                cartItemCountKey = "Quantity";
                var cartItems = HttpContext.Session.Get<List<CartItem>>("cartItems");
                //if come first time

                if (cartItems == null)
                {
                    List<CartItem> items = new List<CartItem>();
                    items.Add(cartitem);
                    HttpContext.Session.Set<List<CartItem>>("cartItems", items);
                    HttpContext.Response.Cookies.Append(cartItemCountKey, cartitem.Quantity.ToString());
                }
                else
                {


                    //if item already in cart just increase quantity

                    var item = cartItems.SingleOrDefault(x => x.ProdId == cartitem.ProdId && x.Size == cartitem.Size);
                    if (item != null)
                    {
                        item.Quantity += cartitem.Quantity;

                    }
                    else
                    {
                        cartItems.Add(cartitem);
                    }
                    HttpContext.Session.Set<List<CartItem>>("cartItems", cartItems);

                    //Update Quantity

                    int cartItemCount = Convert.ToInt32(HttpContext.Request.Cookies[cartItemCountKey]) + cartitem.Quantity;
                    HttpContext.Response.Cookies.Append(cartItemCountKey, cartItemCount.ToString());

                }
            }

            

           

            return RedirectToAction("specific", "Category",new { id = cartitem.ProdId });
        }
        public IActionResult removefromcart(string Id)
        {

            if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                CartRepository cartRepository = new CartRepository(_configuration.GetConnectionString("DefaultConnection"));
                cartItemCountKey = "LoginQuantity";
                //remove product from cart
                int quantity = cartRepository.Getquantity(Id);
                cartRepository.RemovefromCart(Id);
                if (HttpContext.Request.Cookies.ContainsKey(cartItemCountKey))
                {
                    int count = Convert.ToInt32(HttpContext.Request.Cookies[cartItemCountKey]) - quantity;
                    //if cart become empty
                    if (count == 0)
                    {
                        // delete cookie
                        HttpContext.Response.Cookies.Delete(cartItemCountKey);
                    }
                    else
                    {
                        //Update Quantity
                        HttpContext.Response.Cookies.Append(cartItemCountKey, count.ToString());
                    }
                }


            }
            else
            {
                cartItemCountKey = "Quantity";

                var cartItems = HttpContext.Session.Get<List<CartItem>>("cartItems");
                var item = cartItems.SingleOrDefault(x => x.Id == Id);

                if (item != null)
                {
                    cartItems.Remove(item);
                    int cartItemCount = Convert.ToInt32(HttpContext.Request.Cookies[cartItemCountKey]) - item.Quantity;

                    //if cart become empty
                    if (cartItemCount == 0)
                    {
                        HttpContext.Response.Cookies.Delete(cartItemCountKey);
                        
                    }
                    else
                    {
                        //update quantity

                        HttpContext.Response.Cookies.Append(cartItemCountKey, cartItemCount.ToString());
                    }
                }
                HttpContext.Session.Set<List<CartItem>>("cartItems", cartItems);
            }




            return RedirectToAction("viewCart", "Order");
        }
        
       

    }
}
