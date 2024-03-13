using Foodi.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Foodi.BAL;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Hosting.Internal;
using NToastNotify;
using Foodi.Models;
using System.Data;
using ClosedXML.Excel;
using Foodi.DAL;

namespace Foodi.Areas.Admin.Controllers
{
    [CheckAdminAccess]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AdminController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Category_BAlBase _categorybalbase;
        private readonly Count_DalBase _countdalbase;
        private readonly User_BalBase _userbalbase;
        private readonly IToastNotification _toastNotification;

        public AdminController(IWebHostEnvironment webHostEnvironment, IToastNotification toastNotification, IConfiguration configuration)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            this._categorybalbase = new Category_BAlBase();
            this._countdalbase = new Count_DalBase();
            _userbalbase = new User_BalBase();
            _toastNotification = toastNotification;
        }


        public ActionResult Index()
        {
            var counts  = _countdalbase.Count();
            ViewBag.ProductCount = counts.Item1; 
            ViewBag.CategoryCount = counts.Item2; 
            ViewBag.UserCount = counts.Item3;
            ViewBag.orderPending = counts.Item4;
            ViewBag.orderDeliver = counts.Item5;

            string UserId = HttpContext.Session.GetString("UserId");
            int UserID = int.Parse(UserId);
            User users = _userbalbase.GetUserById(UserID);

            return View(users);
        }

        public IActionResult Category(string search)
        {
          
            List<Category> categories = _categorybalbase.GetAllCategories(search);
            return View(categories);
        }



        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category, IFormFile image)
        {
            _toastNotification.AddInfoToastMessage("Product Add successfully");
            category.Image = SaveImageAndGetPath(image);
            _categorybalbase.AddCategory(category);
            return RedirectToAction("Category");
        }

        public ActionResult EditCategory(int categoryId)
        {
            Category category = _categorybalbase.GetCategoryById(categoryId);
            return View("AddCategory", category);
        }

        [HttpPost]
        public ActionResult EditCategory(Category category, IFormFile image)
        {
            if (image != null)
            {
                category.Image = SaveImageAndGetPath(image);
            }

            _toastNotification.AddInfoToastMessage("Information successfully Update");
            _categorybalbase.UpdateCategory(category);
            return RedirectToAction("Category");
        }



        public ActionResult DeleteCategory(int categoryId)
        {
            _toastNotification.AddInfoToastMessage("Delete successfully");
            _categorybalbase.DeleteCategory(categoryId);
            return RedirectToAction("Category");
        }

        [HttpPost]
        public IActionResult DeleteMultipleCarts(List<int> categoryIds)
        {
            if (categoryIds != null && categoryIds.Count > 0)
            {
                foreach (var categoryId in categoryIds)
                {
                    _categorybalbase.DeleteCategory(categoryId);
                }

                return RedirectToAction("Category");
            }

            return RedirectToAction("Category");
        }

        public IActionResult ExportCateoriesToExcel()
        {
     
            List<Category> categories = _categorybalbase.GetAllCategories();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Students");
                // Add headers
                worksheet.Cell(1, 1).Value = "Category Name";
                worksheet.Cell(1, 2).Value = "IsActive";
                worksheet.Cell(1, 3).Value = "Created Date";
                worksheet.Cell(1, 4).Value = "Modified Date";
                // Add data
                int row = 2;
                foreach (var category in categories)
                {
                    worksheet.Cell(row, 1).Value = category.CategoryName;
                    worksheet.Cell(row, 2).Value = category.IsActive;
                    worksheet.Cell(row, 3).Value = category.CreatedDate.ToString("yyyyMM-dd");
                    worksheet.Cell(row, 4).Value = category.ModifiedDate.ToString("yyyyMM-dd");
                    row++;
                }
                // Set content type and filename
                var contentType = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
                var fileName = "CategoriesData.xlsx";
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
                var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
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

