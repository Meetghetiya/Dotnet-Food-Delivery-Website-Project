using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using NToastNotify;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Foodi.Areas.Admin.Controllers
{
    [CheckAdminAccess]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]

    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly User_BalBase _userbalbase;
        private readonly IToastNotification _toastNotification;

        public UserController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IToastNotification toastNotification)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            _userbalbase = new User_BalBase();
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult User(string searchString, bool? isAdmin)
        {

            List<User> Users = _userbalbase.GetAllUsers(searchString, isAdmin);
            return View(Users);
        }

        public ActionResult Edit(int id)
        {
            User users = _userbalbase.GetUserById(id);
            return View(users);
        }

        private bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("Email"));
        }

        [HttpPost]
        public ActionResult Edit(User user, IFormFile Image)
        {
            
                _toastNotification.AddInfoToastMessage("Information successfully Update");
                user.Image = SaveImageAndGetPath(Image);
                _userbalbase.UpdateUser(user);
                return RedirectToAction("User");
            
           

        }

        #region SaveImageAndGetPath
        private string SaveImageAndGetPath(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
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

        public ActionResult Delete(int userId)
        {
            _toastNotification.AddInfoToastMessage("Delete successfully");
            _userbalbase.DeleteUser(userId);
            return RedirectToAction("User");
        }

        [HttpPost]
        public IActionResult DeleteMultipleCarts(List<int> userIds)
        {
            if (userIds != null && userIds.Count > 0)
            {
                foreach (var userId in userIds)
                {
                    _userbalbase.DeleteUser(userId);
                }

                return RedirectToAction("User");
            }
            return RedirectToAction("User");
        }

        public IActionResult ExportUsersToExcel()
        {

            List<User> Users = _userbalbase.GetAllUsers();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                // Add headers
                worksheet.Cell(1, 1).Value = "User Name";
                worksheet.Cell(1, 2).Value = "Mobile";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Address";
                worksheet.Cell(1, 5).Value = "IsAdmin";
                worksheet.Cell(1, 6).Value = "Created Date";
                worksheet.Cell(1, 7).Value = "Modified Date";
                // Add data
                int row = 2;
                foreach (var user in Users)
                {
                    worksheet.Cell(row, 1).Value = user.Name;
                    worksheet.Cell(row, 2).Value = user.Mobile;
                    worksheet.Cell(row, 3).Value = user.Email;
                    worksheet.Cell(row, 4).Value = user.Address;
                    worksheet.Cell(row, 5).Value = user.IsAdmin;
                    worksheet.Cell(row, 6).Value = user.CreatedDate.ToString("yyyyMM-dd");
                    worksheet.Cell(row, 7).Value = user.ModifiedDate.ToString("yyyyMM-dd");

                    row++;
                }
                // Set content type and filename
                var contentType = "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet";
                var fileName = "UsersData.xlsx";
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}

