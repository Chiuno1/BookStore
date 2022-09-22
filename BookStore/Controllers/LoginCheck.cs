using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class LoginCheck : ActionFilterAttribute
    {
        void LoginState(HttpContext context)
        {
            if (context.Session["username"] == null)
            {
                context.Response.Redirect("/Home/Index");
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            LoginState(context);
        }
    }
}