//using Bookmark.AppCode;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using System.Web.Mvc;

namespace Bookmark.Controllers
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

        //[ReCaptcha]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Contact(string txtEMail, string txtMessage, string txtSubject, string txtNumber, string txtName)
        {


            if (!string.IsNullOrEmpty(txtMessage))
            {
                var response = Request["g-recaptcha-response"];
                string secretKey = ConfigurationManager.AppSettings["ReCaptcha.PrivateKey"];
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");
                //ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";

                if (status)
                {

                    if (string.IsNullOrEmpty(txtEMail))
                    {
                        if (Session["User"] != null)
                        {
                            user = (Users)Session["User"];
                            txtEMail = user.Email;
                        }
                    }
                    Mail mail = new Mail();

                    string strBody = txtMessage + " from " + txtEMail + "<br /><br /> Number - " + txtNumber;

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
                    mail.FromAdd = "admin@booqmarqs.com";
                    // else
                    // mail.FromAdd = txtEMail;
                    mail.Subject = "Suggestion - " + txtSubject;
                    mail.ToAdd = "admin@booqmarqs.com";

                    mail.SendMail();
                    ViewBag.Ack = "Thanks for your time in reaching out to us, we will get back to you soon if needed.";

                    return View();
                }
                else
                {
                    ViewBag.Ack = "reCaptcha validation failed";
                    return View();
                }
            }

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

        public ActionResult API()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FAQ()
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
            //mail.FromAdd = "admin@booqmarqs.com";
            //mail.Subject = "Not Found";
            //mail.ToAdd = "admin@booqmarqs.com";
            //mail.IsBodyHtml = true;
            //mail.SendMail();         
        
            Response.StatusCode = 404;
            Response.StatusDescription = "Page not found";
            return View();
        }

    }
}
