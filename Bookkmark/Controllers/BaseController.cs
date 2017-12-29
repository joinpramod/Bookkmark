using System.Web.Mvc;
using System.Web.Routing;

namespace Bookmark.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] != null)
            {
                Users user = (Users)filterContext.HttpContext.Session["User"];

                ViewBag.lblFirstName = user.FirstName;
                ViewBag.IsUserLoggedIn = true;
            }
            else if (Request.Cookies["BooqmarqsLogin"] != null && Request.Cookies["BooqmarqsLogin"].HasKeys)
            {
                string uname = Request.Cookies["BooqmarqsLogin"].Values["EMail"];
                Users user1 = new Users();
                user1 = user1.GetUser(uname);
                Session["User"] = user1;
                ViewBag.lblFirstName = user1.FirstName;
            }
            else
            {
                if (Request.Url.AbsoluteUri.Contains("Bookmark/Reports")
                    || Request.Url.AbsoluteUri.Contains("Bookmark/ScriptCode")
                    || Request.Url.AbsoluteUri.Contains("Bookmark/Domains")
                    || Request.Url.AbsoluteUri.Contains("Bookmark/MyBookmarks")
                    || Request.Url.AbsoluteUri.Contains("Bookmark/Import")
                    || Request.Url.AbsoluteUri.Contains("Account/ViewUser")
                    || Request.Url.AbsoluteUri.Contains("Account/EditUser")
                    )
                {
                    //filterContext.Result = new RedirectResult("Account/Login");
                    filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary
                                                   {{"controller", "Account"}, {"action", "Login"}});
                    return;
                    
                }

                //Users user1 = new Users();
                //user1.UserId = 1;
                //user1.FirstName = "FirstName";
                //user1.LastName = "LastName";
                //user1.Email = "test@booqmarqs.com";
                //Session["User"] = user1;

            }
        }
    }
}