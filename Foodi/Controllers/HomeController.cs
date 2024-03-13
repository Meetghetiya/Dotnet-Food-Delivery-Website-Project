using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;

namespace Foodi.Controllers
{
    [CheckUserAccess]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

      

        private readonly IConfiguration configuration;
        private readonly Category_BAlBase _categorybalbase;
        private readonly Product_BalBase _productService;
        private readonly IToastNotification _toastNotification;

        public HomeController(ILogger<HomeController> logger,IConfiguration configuration, IToastNotification toastNotification)
        {
            _logger = logger;
            this.configuration = configuration;
            this._productService = new Product_BalBase();
            this._categorybalbase = new Category_BAlBase();
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
           /* string email = HttpContext.Session.GetString("Email");
            ViewBag.Email = email;*/

            List<Category> categories = _categorybalbase.GetAllCategories();
            List<Product> products = _productService.GetAllProducts();

            MenuViewModel viewModel = new MenuViewModel
            {
                Categories = categories,
                Products = products
            };

            return View(viewModel);
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
