using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bookmark.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Session["User"] != null)
                {
                    Users user = (Users)filterContext.HttpContext.Session["User"];

                    ViewBag.lblFirstName = user.FirstName;
                    ViewBag.IsUserLoggedIn = true;
                    ViewBag.IsOwner = user.IsOwner;
                    RedirectToProfile(filterContext);
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
                    if (Request.Url.AbsoluteUri.Contains("bookmark/barcharts")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/barchartshorizon")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/bmadded")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/delete")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/domains")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/editbmfolder")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/import")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/linecharts")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/movebmfolder")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/mybookmarks")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/newbmfolder")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/piecharts")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/reports")
                        || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/scriptcode")
                        || Request.Url.AbsoluteUri.ToLower().Contains("account/changepassword")
                        || Request.Url.AbsoluteUri.ToLower().Contains("account/edituser")
                        || Request.Url.AbsoluteUri.ToLower().Contains("account/viewuser")
                          || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/add")
                          || Request.Url.AbsoluteUri.ToLower().Contains("bookmark/addbmrk")
                        )
                    {
                        //filterContext.Result = new RedirectResult("Account/Login");
                        filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary
                                                       {{"controller", "Account"}, {"action", "Login"}});
                        return;

                    }

                    if (Request.Url.AbsoluteUri.ToLower().Contains("bookmark/extbookmarks"))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary
                                                       {{"controller", "Account"}, {"action", "ExtLogin"}});
                        return;

                    }
                }
            }
            catch (Exception ex)
            {

                Session["User"] = null;
                Session["Facebook"] = null;
                Session["bookmark"] = null;
                Session.RemoveAll();

                if (Request.Cookies["BooqmarqsLogin"] != null)
                {
                    HttpCookie myCookie = Request.Cookies["BooqmarqsLogin"];
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    myCookie.Values["UserId"] = null;
                    myCookie.Values["FirstName"] = null;
                    myCookie.Values["LastName"] = null;
                    Response.Cookies.Set(myCookie);
                }


                filterContext.Result = new RedirectToRouteResult(
                                                   new RouteValueDictionary
                                                       {{"controller", "Home"}, {"action", "Index"}});
                return;
            }
        }

        private void RedirectToProfile(ActionExecutingContext filterContext)
        {
            if (Request.Url.AbsoluteUri.Contains("account/register")
                   || Request.Url.AbsoluteUri.ToLower().Contains("account/login")
                   || Request.Url.AbsoluteUri.ToLower().Contains("account/register"))
                {

                filterContext.Result = new RedirectToRouteResult(
                                              new RouteValueDictionary
                                                  {{"controller", "Account"}, {"action", "ViewUser"}});
            }
        }
    }
}