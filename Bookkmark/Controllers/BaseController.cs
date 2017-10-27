using System.Web.Mvc;

namespace Bookkmark.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] != null)
            {
                Users user = (Users)filterContext.HttpContext.Session["User"];

                ViewBag.lblFirstName = user.FirstName;
                ViewBag.UserEmail = user.Email;
                ViewBag.IsUserLoggedIn = false;
            }
        }
    }
}