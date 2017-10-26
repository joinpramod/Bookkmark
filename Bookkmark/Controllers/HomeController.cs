using Bookkmark.AppCode;
using Bookkmark.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bookkmark.Controllers
{
    public class HomeController : BaseController
    {
        private Users user = new Users();

        public ActionResult Index()
        {
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            // ViewBag.Message = "Your app description page.";

            return View();
        }

        [ReCaptcha]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Contact(string txtEMail, string txtSuggestion)
        {
            //if (ModelState.IsValid)
            //{
                if (!string.IsNullOrEmpty(txtSuggestion))
                {
                    if (string.IsNullOrEmpty(txtEMail))
                    {
                        if (Session["User"] != null)
                        {
                            user = (Users)Session["User"];
                            txtEMail = user.Email;
                        }
                    }
                    //else
                    //{
                    //    return View();
                    //}
                    Mail mail = new Mail();

                    string strBody = txtSuggestion + " from " + txtEMail;

                    try
                    {
                        strBody += "<br /><br /> IP - " + Utilities.GetUserIP() + "<br /><br />";
                    }
                    catch
                    {

                    }

                    mail.Body = strBody;

                    mail.IsBodyHtml = true;
                    //mail.Body = txtSuggestion + " from " + txtEMail;
                    //if (Session["User"] != null)
                    mail.FromAdd = "admin@codeanalyze.com";
                    // else
                    // mail.FromAdd = txtEMail;
                    mail.Subject = "Suggestion";
                    mail.ToAdd = "admin@codeanalyze.com";

                    mail.SendMail();
                    ViewBag.Ack = "Thanks for your time in reaching out to us, we will get back to you soon if needed.";

                    return View();
                }

                if (Session["User"] != null)
                {
                    user = (Users)Session["User"];
                    ViewBag.UserEMail = user.Email;
                }
            //}

            //else
            //{
            //    if (!string.IsNullOrEmpty(txtSuggestion))
            //    {
            //        ViewBag.Ack = "Invalid ReCaptcha, Please try again";
            //    }
            //}
            return View();
        }

        public ActionResult Rewards()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Policy()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Disclaimer()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFound()
        {        
            //Mail mail = new Mail();
            //string strBody = "";

            //try
            //{
            //    var varURL = Request.Url;
            //    strBody += "URL -- " + varURL + "<br /><br />";
            //    string strReferer = Request.UrlReferrer.ToString();
            //    strBody += "Previous URL -- " + strReferer + "<br /><br />";
            //}
            //catch
            //{

            //}
              

            //try
            //{
            //    strBody += "IP - " + Utilities.GetUserIP() + "<br /><br />";
            //}
            //catch
            //{

            //}

            //mail.Body = strBody;
            //mail.FromAdd = "admin@codeanalyze.com";
            //mail.Subject = "Not Found";
            //mail.ToAdd = "admin@codeanalyze.com";
            //mail.IsBodyHtml = true;
            //mail.SendMail();         
        
            Response.StatusCode = 404;
            Response.StatusDescription = "Page not found";
            return View();
        }

    }
}
