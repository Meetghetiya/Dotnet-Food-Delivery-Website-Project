using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Foodi.Models;
using System.Data;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using NToastNotify;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Foodi.Areas.Auth.Controllers
{


    [Area("Auth")]
    [Route("Auth/[controller]/[action]")]
    public class AuthController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Auth_BalBase _Authbalbase;
        private readonly IToastNotification _toastNotification;
        private readonly Cart_BalBase _cartbalbase;
        private readonly User_BalBase _userbalbase;

        public AuthController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IToastNotification toastNotification)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            this._Authbalbase = new Auth_BalBase();
            _userbalbase = new User_BalBase();
            this._cartbalbase = new Cart_BalBase();
            _toastNotification = toastNotification;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, IFormFile Image)
        {



            user.Image = SaveImageAndGetPath(Image);

            if (_Authbalbase.AddUser(user))
            {
                _toastNotification.AddInfoToastMessage("OTP has been send to your Email");

                string otp = GenerateOTP();
                HttpContext.Session.SetString("OTP", otp);

                // Send OTP via email
                SendOTPByEmail(user.Email, otp);

                return RedirectToAction("VarifyOtp");
            }
            else
            {
                if (_Authbalbase.ValidateUserByEmail(user))
                {
                    _toastNotification.AddErrorToastMessage("Email is already in use");
                }
                else
                {

                }

            }

            return View();
        }


        [HttpPost]
        public ActionResult VerifyOTP(string digit1, string digit2, string digit3, string digit4, string digit5, string digit6)
        {
            if (digit1 == null || digit2 == null || digit3 == null || digit4 == null || digit5 == null || digit6 == null)
            {
                _toastNotification.AddErrorToastMessage("OTP is not given");
                return View("VarifyOtp");
            }
            string enteredOTP = digit1 + digit2 + digit3 + digit4 + digit5 + digit6;
            string storedOTP = HttpContext.Session.GetString("OTP");

            if (enteredOTP == storedOTP)
            {
                _toastNotification.AddInfoToastMessage("Registration successful! Welcome.");
                return RedirectToAction("Login");
            }

            _toastNotification.AddErrorToastMessage("Incorrect OTP");
            TempData["ErrorMessage"] = "Incorrect OTP. Please try again.";

            return View("VarifyOtp");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user.Password == null && user.Email == null)
            {
                _toastNotification.AddWarningToastMessage("Email and Password is not given");
            }
            else if (user.Password == null)
            {
                _toastNotification.AddWarningToastMessage("Password is not given");
            }
            else if (user.Email == null)
            {
                _toastNotification.AddWarningToastMessage("Email is not given");
            }
            else
            {
                User User = _Authbalbase.ValidateUser(user);

                if (User == null)
                {
                    _toastNotification.AddWarningToastMessage("User Not Available");
                }
                else
                {


                    var claims = new List<Claim>
                     {
                        new Claim(ClaimTypes.Email, User.Email),
                     };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                 new ClaimsPrincipal(claimsIdentity));


                    HttpContext.Session.SetString("Email", User.Email);
                    HttpContext.Session.SetString("Image", "null");
                    HttpContext.Session.SetString("UserId", User.UserID?.ToString());
                    HttpContext.Session.SetString("Name", User.Name);



                    if (User.IsAdmin == true)
                    {
                        _toastNotification.AddInfoToastMessage("Admin Login Successfully");
                        return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }
                    else
                    {
                        _toastNotification.AddInfoToastMessage("Login Successfully");
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SendOTPByEmail(string email, string otp)
        {

          
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("meetghetiya0398@gmail.com", "vwjaqkhqllnckiqz"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("meetghetiya0398@gmail.com"),
                    Subject = "OTP Verification",
                    Body = $"Your OTP for login is: {otp}",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);

            
           

            /*var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("meetghetiya0398@gmail.com", "vwjaqkhqllnckiqz"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("meetghetiya0398@gmail.com"),
                Subject = "OTP Verification",
                Body = $"Your OTP for login is: {otp}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            smtpClient.Send(mailMessage);*/
        }


        public IActionResult VarifyOtp()
        {

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _toastNotification.AddInfoToastMessage("Logout Successsfully");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        public User GetUserByEmail(string email)
        {
            string str = this.configuration.GetConnectionString("MyConnectionString");
            User user = null;
            using (SqlConnection connection = new SqlConnection(str))
            {
                connection.Open();
                string storedProcedure = "PR_User_ByEmail";
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = reader["UserID"] as int?,
                            Name = reader["Name"] as string,
                            UserName = reader["UserName"] as string,
                            Mobile = reader["Mobile"] as string,
                            Email = reader["Email"] as string,
                            Address = reader["Address"] as string,
                            PostCode = reader["PostCode"] as string,
                            Password = reader["Password"] as string,
                            Image = reader["ImageUrl"] as string,
                            IsAdmin = (bool)reader["IsAdmin"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            ModifiedDate = (DateTime)reader["ModifiedDate"]
                        };
                    }

                }
            }
            return user;
        }

        public IActionResult EmailForgot(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();
            }
            else
            {
                string otp = GenerateOTP();
                HttpContext.Session.SetString("OTP", otp);

                try
                {
                    SendOTPByEmail(email, otp);
                }
                catch
                {
                    _toastNotification.AddInfoToastMessage("Error sending OTP in email. Please try again later.");
                    return View();
                }
               
                _toastNotification.AddInfoToastMessage("Otp send to your email");
                return RedirectToAction("ForgotPasswordOtpVerify",new { email = email });
            }
        }

        public IActionResult ResetOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();
            }
            else
            {
                string otp = GenerateOTP();
                HttpContext.Session.SetString("OTP", otp);

                try
                {
                    SendOTPByEmail(email, otp);
                }
                catch
                {
                    _toastNotification.AddInfoToastMessage("Error sending OTP in email. Please try again later.");
                    return View();
                }

                _toastNotification.AddInfoToastMessage("Otp send to your email");
                return RedirectToAction("ForgotPasswordOtpVerify", new { email = email });
            }
        }

        public IActionResult ForgotPasswordOtpVerify(string email )
        {
            ViewBag.Email = email; 
            return View();
        }

        
       

        [HttpPost]
        public IActionResult ForgotPasswordOtpVerify(string digit1 = null , string digit2 = null, string digit3 = null, string digit4 = null, string digit5 = null, string digit6 = null,string email = null)
        {

            if (digit1 == null || digit2 == null || digit3 == null || digit4 == null || digit5 == null || digit6 == null)
            {
                _toastNotification.AddErrorToastMessage("OTP is not given");
                return View();
            }

            string enteredOTP = digit1 + digit2 + digit3 + digit4 + digit5 + digit6;
            string storedOTP = HttpContext.Session.GetString("OTP");

            if (enteredOTP == storedOTP)
            {
                TempData["email"] = email;
                return RedirectToAction("ChangePassword");
            }
            return View();
        }

        public IActionResult ChangePassword()
        {
            ViewBag.Email = TempData["email"];
            TempData.Remove("email");
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string password, string email)
        {
            if(password != "null")
            {

            User users  = _userbalbase.GetUserByEmail(email);

            User user = new User
            {
                UserID = users.UserID,
                Name = users.Name,
                UserName = users.UserName,
                Mobile = users.Mobile,
                Email = users.Email,
                Address = users.Address,
                PostCode = users.PostCode,
                Password = password,
                Image = users.Image,
                IsAdmin = users.IsAdmin,
                CreatedDate = users.CreatedDate,
                ModifiedDate = users.ModifiedDate
            };

            _userbalbase.UpdateUser(user);

              return RedirectToAction("Login");
            }

            ViewBag.Email = email;
            return View(email);
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
    }
}
