using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodi.Areas.Dashboard.Controllers
{
    [CheckUserAccess]
    [Area("Dashboard")]
    [Route("Dashboard/[controller]/[action]")]
    public class MenuController : Controller
    {


        private readonly IConfiguration configuration;
        private readonly Category_BAlBase _categorybalbase;
        private readonly Product_BalBase _productService;



        public MenuController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this._productService = new Product_BalBase();
            this._categorybalbase = new Category_BAlBase();
        }
        public IActionResult Menu()
        {
            List<Category> categories = _categorybalbase.GetAllCategories();
            List<Product> products = _productService.GetAllProducts();

            MenuViewModel viewModel = new MenuViewModel
            {
                Categories = categories,
                Products = products
            };

            return View(viewModel);
        }

        public IActionResult MenuAdd(int ProductId)
        {
            List<Category> categories = _categorybalbase.GetAllCategories();
            List<Product> products = _productService.GetAllProducts();

            MenuViewModel viewModel = new MenuViewModel
            {
                Categories = categories,
                Products = products
            };

            return View(viewModel);
        }
    }
}
