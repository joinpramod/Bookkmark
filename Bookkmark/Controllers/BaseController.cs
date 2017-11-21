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
                ViewBag.IsUserLoggedIn = true;
            }
            else if (Request.Cookies["BookmaqLogin"] != null && Request.Cookies["BookmaqLogin"].HasKeys)
            {
                string uname = Request.Cookies["BookmaqLogin"].Values["UserId"].ToString();
                Users user1 = new Users();
                //user1.UserId = 1;
                user1.FirstName = "FirstName";
                user1.LastName = "LastName";
                user1.Email = uname;
                Session["User"] = user1;
                Session["user.Email"] = user1.Email;
                ViewBag.UserEmail = user1.Email;
                ViewBag.lblFirstName = user1.FirstName;

            }
        }
    }
}