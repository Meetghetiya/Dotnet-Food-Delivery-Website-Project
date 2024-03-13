using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Foodi.BAL;

namespace Foodi.Models
{
    public class CheckUserAccess : ActionFilterAttribute, IAuthorizationFilter
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
            }
           

            /*  if (filterContext.HttpContext.Session.GetString("UserId") == null)
              {
                  filterContext.Result = new RedirectToActionResult("Login", "Auth", new { Area = "Auth" });
              }*/
        } 
    
    
    }
}
