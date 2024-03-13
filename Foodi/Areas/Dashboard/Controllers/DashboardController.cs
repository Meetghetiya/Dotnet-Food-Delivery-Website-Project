using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;
using NuGet.Protocol.Plugins;

namespace Foodi.Areas.Dashboard.Controllers
{
    [CheckUserAccess]
    [Area("Dashboard")]
    [Route("Dashboard/[controller]/[action]")]
    public class DashboardController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly Category_BAlBase _categorybalbase;
        private readonly Product_BalBase _productService;
        private readonly Cart_BalBase _cartbalbase;
        private readonly IToastNotification _toastNotification;

        public DashboardController(IConfiguration configuration, IToastNotification toastNotification)
        {
            this.configuration = configuration;
            this._productService = new Product_BalBase();
            this._categorybalbase = new Category_BAlBase();
            this._cartbalbase = new Cart_BalBase();
            _toastNotification = toastNotification;
        }
        public IActionResult About()
        {
            return View();
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

        [HttpPost]
        public IActionResult MenuAdd(int productId)
        {
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);
            Cart cart = new Cart();

            string str = this.configuration.GetConnectionString("MyConnectionString");
            using (SqlConnection connection = new SqlConnection(str))
            {
                connection.Open();
                string query = "SELECT * FROM Carts WHERE ProductId = @ProductId AND UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cart = new Cart
                            {
                                CartId = Convert.ToInt32(reader["CartId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]) + 1,
                                UserId = Convert.ToInt32(reader["UserId"]),
                            };
                            _cartbalbase.UpdateCart(cart);
                        }
                        else
                        {
                            cart = new Cart
                            {
                                ProductId = productId,
                                Quantity = 1,
                                UserId = userID,
                            };

                            _toastNotification.AddInfoToastMessage("Product Add successfully");
                            _cartbalbase.InsertCart(cart);
                        }
                    }
                }
            }
            return Json(new { success = true, message = "Product added to the cart successfully" });
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
