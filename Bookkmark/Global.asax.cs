using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Bookkmark
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 45;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.ToString().ToLower().Contains("/que/ans?id"))
            {
                if (Request.Url.ToString().Contains("localhost"))
                    HttpContext.Current.Response.Redirect("/Bookkmark/Home/NotFound");
                else
                    HttpContext.Current.Response.Redirect("/Home/NotFound");
            }
            else if ((Request.Url.ToString().ToLower().Equals("http://bookmaq.com/que/ans/48408/")) ||
               (Request.Url.ToString().ToLower().Equals("http://bookmaq.com/que/ans/home/rewards")) ||
               (Request.Url.ToString().ToLower().Equals("http://bookmaq.com/que/ans/48360/")) ||
               (Request.Url.ToString().ToLower().Equals("http://bookmaq.com/articles/details/20073/")) ||
               (Request.Url.ToString().ToLower().Equals("http://bookmaq.com/que/ans/38198/")) ||
               (Request.Url.ToString().ToLower().Contains("bookmaq.com/soln.aspx")) ||
               (Request.Url.ToString().ToLower().Contains("bookmaq.com/questions/soln")) ||
               (Request.Url.ToString().ToLower().Contains("bookmaq.com/articles/details/home/rewards")) ||
               (Request.Url.ToString().ToLower().Contains("bookmaq.com/que/ans/home/rewards")))
            {
                if (Request.Url.ToString().Contains("localhost"))
                    HttpContext.Current.Response.Redirect("/Bookkmark/Home/NotFound");
                else
                    HttpContext.Current.Response.Redirect("/Home/NotFound");
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception ex = new Exception();
                ex = Server.GetLastError();

                if (ex.Message.ToLower().Contains("the controller for path")
                    && ex.Message.ToLower().Contains("was not found or does not implement icontroller"))
                {
                    if (Request.Url.ToString().Contains("localhost"))
                        HttpContext.Current.Response.Redirect("/Bookkmark/Home/NotFound");
                    else
                        HttpContext.Current.Response.Redirect("/Home/NotFound");
                }
                else if (ex.Message.ToLower().Contains("a public action method")
                    && ex.Message.ToLower().Contains("was not found on controller"))
                {
                    if (Request.Url.ToString().Contains("localhost"))
                        HttpContext.Current.Response.Redirect("/Bookkmark/Home/NotFound");
                    else
                        HttpContext.Current.Response.Redirect("/Home/NotFound");
                }
                else if (ex.Message.ToLower().Contains("this is an invalid webresource request")
                    || ex.Message.ToLower().Contains("this is an invalid script resource request"))
                {
                    if (Request.Url.ToString().Contains("localhost"))
                        HttpContext.Current.Response.Redirect("/Bookkmark/Home/NotFound");
                    else
                        HttpContext.Current.Response.Redirect("/Home/NotFound");
                }
                else if (ex.Message.ToLower().Contains("a potentially dangerous request"))
                {
                    //..ignore
                }
                else
                {

                    // Code that runs when an unhandled error occurs
                    Mail mail = new Mail();
                    string strBody = "";

                    if (HttpContext.Current != null)
                    {
                        var varURL = HttpContext.Current.Request.Url;
                        strBody += "URL -- " + varURL + "<br /><br />";
                        try
                        {
                            string strReferer = Request.UrlReferrer.ToString();
                            strBody += "Previous URL -- " + strReferer + "<br /><br />";
                        }
                        catch
                        {

                        }
                    }

                    if (!string.IsNullOrEmpty(ex.Message))
                        strBody += "Message -- " + ex.Message + "<br /><br />";
                    if (!string.IsNullOrEmpty(ex.Source))
                        strBody += "Source -- " + ex.Source + "<br /><br />";
                    if (ex.TargetSite != null)
                        strBody += "TargetSite -- " + ex.TargetSite + "<br /><br />";
                    if (ex.Data != null)
                        strBody += "Data -- " + ex.Data + "<br /><br />";
                    if (ex.InnerException != null)
                        strBody += "InnerException -- " + ex.InnerException + "<br /><br />";
                    if (!string.IsNullOrEmpty(ex.Source))
                        strBody += "Source -- " + ex.Source + "<br /><br />";

                    try
                    {
                        strBody += "IP - " + Utilities.GetUserIP() + "<br /><br />";
                    }
                    catch
                    {

                    }

                    try
                    {
                        if (ex.GetType().ToString() != null)
                            strBody += "Type -- " + ex.GetType().ToString() + "<br />";
                    }
                    catch
                    {

                    }

                    if (ex.StackTrace != null)
                        strBody += " Stack Trace -- " + ex.StackTrace + " <br /><br />";

                    mail.Body = strBody;
                    mail.FromAdd = "admin@bookmaq.com";
                    mail.Subject = "Error";
                    mail.ToAdd = "admin@bookmaq.com";
                    mail.IsBodyHtml = true;
                  //  mail.SendMail();

                    //if (Request.Url.ToString().Contains("localhost"))
                    //    HttpContext.Current.Response.Redirect("/Bookkmark/Home/Error");
                    //else
                    //    HttpContext.Current.Response.Redirect("/Home/Error");
                }
            }
            catch
            {
                HttpContext.Current.Response.Redirect("/Home/Error");
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
