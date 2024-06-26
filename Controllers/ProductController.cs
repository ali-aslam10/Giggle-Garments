using Giggle_Garments_MVC.Models;
using Giggle_Garments_MVC.Models.Repositories;
using Giggle_Garments_MVC.Models.View_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Giggle_Garments_MVC.Controllers
{
    [Authorize(Policy = "onlyadmin")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly string constr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GiggleGarmentsDB;Integrated Security=True;";

        public ProductController(ILogger<ProductController> logger, IWebHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            _configuration = configuration;
        }
        public IActionResult ViewProducts()
        {
            productRepository productRepository = new productRepository(constr);
            StockRepository stockRepository = new StockRepository(constr);
            ProductStock productStock = new ProductStock();
            productStock.Stocks = stockRepository.GetAll();
            productStock.Products = productRepository.GetAll();

            return View(productStock);

        }
    public ViewResult Index()
        {
            productRepository productRepository = new productRepository(constr);

            return View(productRepository.GetAll());
        }
        public ViewResult viewStock()
        {
            StockRepository stockRepository = new StockRepository(constr);
            return View(stockRepository.GetAll());
        }
        [HttpGet]
        public ViewResult addProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addProduct(Product p, IFormFile myfile)
        {
            if(ModelState.IsValid)
            {
                string wwwrootpath = _env.WebRootPath;
               
                string folder;
                if (p.CategoryId == 1)
                    folder = "Boys images";
                else if (p.CategoryId == 2)
                    folder = "Girls images";
                else 
                    folder = "Newborn images";
                

                string path = Path.Combine(wwwrootpath, folder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (myfile.Length > 0)
                {
                    string uniquename = Guid.NewGuid().ToString() + "_" + myfile.FileName;
                    string filepath = Path.Combine(path, uniquename);
                    using (var FileStream = new FileStream(filepath, FileMode.Create))
                    {
                        myfile.CopyTo(FileStream);
                    }
                    p.ImagePath = filepath;
                }

                productRepository productRepository = new productRepository(constr);
                productRepository.Add(p);

                return View("addStock");
            }
            else
            {
                return View();
            }
            
        }
        [HttpGet]
        public ViewResult UpdateProduct(int id)
        {
            productRepository productRepository = new productRepository(constr);
            Product p=productRepository.GetById(id);
            return View("UpdateProduct", p);
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product p)
        {
            if (ModelState.IsValid)
            {
                productRepository productRepository = new productRepository(constr);
                productRepository.Update(p);

                return RedirectToAction("Index", "Product");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ViewResult addStock()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addStock(Stock s)
        {
           
            StockRepository stockRepository = new StockRepository(constr);
            stockRepository.Add(s);

            return RedirectToAction("Index", "Product");
        }
        [HttpGet]
        public ViewResult UpdateStock(int id)
        {
            StockRepository stockRepository = new StockRepository(constr);
            Stock s = stockRepository.GetbyId(id);
            return View("UpdateStock", s);
        }
        [HttpPost]
        public IActionResult UpdateStock(Stock s)
        {
            StockRepository stockRepository = new StockRepository(constr);
            stockRepository.Update(s);
            return RedirectToAction("viewStock", "Product");

        }
        public IActionResult removeProdct(int id)
        {
            productRepository productRepository = new productRepository(constr);
            Product p = productRepository.GetById(id);

            //Delete image
            string imagePath = p.ImagePath;
            if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            productRepository.Delete(id);

            return RedirectToAction("Index", "Product");
        }
        public ViewResult editProduct()
        {
            return View();
        }
    }
}
