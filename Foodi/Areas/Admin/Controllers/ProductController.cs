using ClosedXML.Excel;
using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using NToastNotify;
using System.Data;
using System.Data.SqlClient;

namespace Foodi.Areas.Admin.Controllers
{
    [CheckAdminAccess]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ProductController : Controller
    {


        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Category_BAlBase _categorybalbase;
        private readonly Product_BalBase _productService;
        private readonly IToastNotification _toastNotification;



        public ProductController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IToastNotification toastNotification)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            this._productService = new Product_BalBase();
            _toastNotification = toastNotification;
        }




        public ActionResult Product(string searchName , bool? isActive)
        {
            List<Product> products = _productService.GetAllProducts(searchName , isActive);
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            List<Category> categories = GetAllCategories();
            ViewBag.Category = categories ?? new List<Category>();
            return View("AddEditProduct");
        }



        [HttpPost]
        public ActionResult AddProduct(Product product, IFormFile ImageUrl)
        {
            if(ImageUrl ==null)
            {
                List<Category> categories = GetAllCategories();
                ViewBag.Category = categories ?? new List<Category>();
                _toastNotification.AddInfoToastMessage("Image Is not Given");
                return View("AddEditProduct", product);
            }
            _toastNotification.AddInfoToastMessage("Product Add successfully");
            product.ImageUrl = SaveImageAndGetPath(ImageUrl);
            _productService.AddProduct(product);
            return RedirectToAction("Product");
        }


        public IActionResult EditProduct(int productId)
        {
            List<Category> categories = GetAllCategories();
            ViewBag.Category = categories ?? new List<Category>();

            Product products = _productService.GetProductById(productId);
            return View("AddEditProduct", products);
        }
      
        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            var connectionString = this.configuration.GetConnectionString("MyConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Category_IdName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["Name"].ToString()
                            };
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

        [HttpPost]
        public ActionResult EditProduct(Product product, IFormFile ImageUrl)
        {
            _toastNotification.AddInfoToastMessage("Information successfully Update");
            product.ImageUrl = SaveImageAndGetPath(ImageUrl);
            _productService.UpdateProduct(product);
            return RedirectToAction("Product");
        }

       
        public ActionResult DeleteProduct(int ProductId)
        {
            _toastNotification.AddInfoToastMessage("Delete successfully");
            _productService.DeleteProduct(ProductId);
            return RedirectToAction("Product");
        }


        [HttpPost]
        public IActionResult DeleteMultipleCarts(List<int> productIds)
        {
            if (productIds != null && productIds.Count > 0)
            {
                foreach (var productId in productIds)
                {
                    _productService.DeleteProduct(productId);
                }

                return RedirectToAction("Product");
            }

            return RedirectToAction("Product");
        }


        public IActionResult ExportProductsToExcel()
        {

            List<Product> products = _productService.GetAllProducts();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Students");
                // Add headers
                worksheet.Cell(1, 1).Value = "Product Name";
                worksheet.Cell(1, 2).Value = "Price";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "CategoryName";
                worksheet.Cell(1, 5).Value = "IsActive";
                worksheet.Cell(1, 6).Value = "Created Date";
                worksheet.Cell(1, 7).Value = "Modified Date";
                // Add data
                int row = 2;
                foreach (var product in products)
                {
                    worksheet.Cell(row, 1).Value = product.Name;
                    worksheet.Cell(row, 2).Value = product.Price;
                    worksheet.Cell(row, 3).Value = product.Description;
                    worksheet.Cell(row, 4).Value = product.CategoryName;
                    worksheet.Cell(row, 5).Value = product.IsActive;
                    worksheet.Cell(row, 6).Value = product.CreatedDate.ToString("yyyyMM-dd");
                    worksheet.Cell(row, 7).Value = product.ModifiedDate.ToString("yyyyMM-dd");
                   
                    row++;
                }
                // Set content type and filename
                var contentType = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
                var fileName = "ProductData.xlsx";
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }

        #region SaveImageAndGetPath
        private string SaveImageAndGetPath(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                /*var fileName = Path.GetFileName(file.FileName);*/
                var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "Products");
                string uniqfilename = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadPath, uniqfilename);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var relativePath = filePath.Replace(webHostEnvironment.WebRootPath, string.Empty)
                                  .TrimStart(Path.DirectorySeparatorChar);
                return relativePath;
            }
            return null;
        }
        #endregion
    }
}
