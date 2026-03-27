using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace SOS100GroupProjectMVC.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            
            // Släpp alltid igenom Login-sidan, annars kan man aldrig logga in
            if (string.Equals(controllerName, "Login", StringComparison.OrdinalIgnoreCase))
            {
                base.OnActionExecuting(context);
                return;
            }

            // Kolla om användaren har en giltig userId-cookie (är inloggad)
            var userIdCookie = context.HttpContext.Request.Cookies["userId"];
            if (string.IsNullOrWhiteSpace(userIdCookie))
            {
                // Om man inte är inloggad, kasta tillbaka personen till Login-sidan bevisligen
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
