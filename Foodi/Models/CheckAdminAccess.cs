using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Foodi.BAL;

namespace Foodi.Models
{
    public class CheckAdminAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var rd = filterContext.RouteData;
            string currentAction = rd.Values["action"].ToString();
            string currentController = rd.Values["controller"].ToString();
            //string currentArea = rd.DataTokens["area"].ToString();

            

            if (filterContext.HttpContext.Session.GetString("UserId") == null)
            {
                
                filterContext.Result = new RedirectToActionResult("Login", "Auth", new { Area = "Auth" });
            }else
            {
                string userId = filterContext.HttpContext.Session.GetString("UserId");
                int userID = int.Parse(userId);

                User_BalBase _userbalbase = new User_BalBase();
                User User = _userbalbase.GetUserById(userID);

                if (User.IsAdmin != true)
                {
                    filterContext.Result = new RedirectToActionResult("Login", "Auth", new { Area = "Auth" });
                }
            }

            /*  if (filterContext.HttpContext.Session.GetString("UserId") == null)
              {
                  filterContext.Result = new RedirectToActionResult("Login", "Auth", new { Area = "Auth" });
              }*/
        }
    }
}





