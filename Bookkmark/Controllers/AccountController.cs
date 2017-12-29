using System.Web.Mvc;
using System.Data;
using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Bookmark.Models;
using System.Security.Claims;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Bookmark.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private Users user = new Users();

        public ActionResult Login()
        {
                return View();
        }

        public ActionResult QuickLogin()
        {
                return View();
        }

        public ActionResult ProcessLogin(string txtEMailId, string txtPassword)
        {
            if (Request.Form["hfUserEMail"] != null)
            {
                txtEMailId = Request.Form["hfUserEMail"];
                ConnManager con = new ConnManager();
                DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + txtEMailId + "'");
                if (dtUserActivation.Rows.Count > 0)
                {
                    SendActivationEMail(txtEMailId, dtUserActivation.Rows[0]["ActivationCode"].ToString());
                    ViewBag.Activation = "Activation Code Sent. Please check your inbox and click on the activation link.";
                    ViewBag.UserActEMail = txtEMailId;
                }
                return View("../Account/Login");
            }
            else
            {
                return CheckUserLogin(txtEMailId, txtPassword);
            }
        } 

        private ActionResult CheckUserLogin(string txtEMailId, string txtPassword)
        {
            ConnManager connManager = new ConnManager();
            DataTable DtUserList = new DataTable();
            DataTable dtUserActivation = new DataTable();

            dtUserActivation = connManager.GetDataTable("select * from UserActivation where Emailid = '" + txtEMailId + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                //ViewBag.Ack = "User activation pending";
                ViewBag.Activation = "User activation pending. Resend Activation Code?";
                ViewBag.UserActEMail = txtEMailId;
                return View("../Account/Login");
            }

           DtUserList = connManager.GetDataTable("select * from users where email = '" + txtEMailId + "' and Password = '" + txtPassword + "'");

            if (DtUserList.Rows.Count == 0)
            {
                ViewBag.Ack = "Invalid login credentials, please try again";
                return View("../Account/Login");
            }
            else
            {
                Users user = new Users();
                user.UserId = double.Parse(DtUserList.Rows[0]["UserId"].ToString());
                user.FirstName = DtUserList.Rows[0]["FirstName"].ToString();
                user.LastName = DtUserList.Rows[0]["LastName"].ToString();
                user.Email = DtUserList.Rows[0]["EMail"].ToString();
                user.IsPublisher = bool.Parse(DtUserList.Rows[0]["IsPublisher"].ToString());


                //user.Details = DtUserList.Rows[0]["Details"].ToString();
                Session["User"] = user;
                
                HttpCookie userCookie = new HttpCookie("BooqmarqsLogin");
                userCookie.Values["EMail"] = user.Email;
                userCookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(userCookie);


                if (Session["bookmark"] != null && Request.Form["btnQuickLogin"] != null)
                {
                    AddBookmark(Session["bookmark"], user.UserId.ToString());
                    Session["bookmark"] = null;
                    return View("../Bookmark/BMAdded");
                }
                else if (user.IsPublisher)
                {
                    Session["SiteOwner"] = null;
                    return RedirectToAction("Reports", "Bookmark");
                }
                else
                {
                    return RedirectToAction("MyBookmarks", "Bookmark");
                }
            }
        }

        private void AddBookmark(object bmrk, string userId)
        {
            string url = string.Empty;
            if (bmrk != null)
            {
                url = bmrk.ToString();
                BookmarkCls objBmrk = new Bookmark.BookmarkCls();
                string strCity = string.Empty;
                string strState = string.Empty;
                string strCountry = string.Empty;
                string ipAddr = Utilities.GetIPAddress();
                Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);

                objBmrk.URL = url;
                objBmrk.FolderId = 27;
                objBmrk.CreatedDate = DateTime.Now.ToString();
                objBmrk.CreatedUserId = userId;
                objBmrk.Name = url;
                objBmrk.IsFolder = false;
                objBmrk.IpAddr = ipAddr;
                objBmrk.City = strCity;
                objBmrk.Country = strCountry;
            }

        }

        public ActionResult WebUserReg()
        {
            //ViewData["RegisType"] = "WebUser";
            return Register(null, null, null, null);
        }

        public ActionResult SiteOwnerReg()
        {
            Session["SiteOwner"] = "SiteOwner";
            return Register(null, null, null, null);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session["User"] = null;
            Session["Facebook"] = null;
            Session["bookmark"] = null;
            Session.RemoveAll();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            if (Request.Cookies["BooqmarqsLogin"] != null)
            {
                HttpCookie myCookie = Request.Cookies["BooqmarqsLogin"];
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                myCookie.Values["UserId"] = null;
                myCookie.Values["FirstName"] = null;
                myCookie.Values["LastName"] = null;
                Response.Cookies.Set(myCookie);
            }

            //Response.Redirect("../Home/Index");　
            return View("../Home/Index");
        }

        [AllowAnonymous]
        public ActionResult Choice()
        {
            return View();
        }

        //[ReCaptcha]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Register(string txtFirstName, string txtLastName, string txtEMailId, string txtNewPassword)
        {
            string activationCode = Guid.NewGuid().ToString();
            //AddEdit user
            if (!string.IsNullOrEmpty(txtEMailId))
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

                    if(!IsActivated(txtEMailId))
                    {
                        return View();
                    }

                    if (!IsNewUser(txtEMailId))
                    {
                        ViewBag.Ack = "EMail id already exists. If you have forgotten password, please click forgot password link on the Sign In page.";
                        return View();
                    }

                    double dblUserID = 0;
                    user.OptionID = 1;
                    user.CreatedDateTime = DateTime.Now;
                    user.Password = txtNewPassword;
                    user.Email = txtEMailId;
                    user.FirstName = txtFirstName;
                    user.LastName = txtLastName;
                    user.IsWebUser = true;
                    if (Session["SiteOwner"] != null)
                        user.IsPublisher = true;
                    else
                        user.IsPublisher = false;
                    user.CreateUsers(ref dblUserID);
                    user.CreateUserActivation(user, activationCode, dblUserID);
                    ViewBag.Ack = "User Info Saved Successfully. An activation link has been sent to your email address, please check your inbox and activate your account";
                    SendActivationEMail(user.Email, activationCode);
                    SendEMail(user.Email, user.FirstName, user.LastName);
                    user = new Users();
                    Session["SiteOwner"] = null;
                    return View();
                }
                else
                {
                    ViewBag.Ack = "reCaptcha validation failed";
                    return View();
                }
            }
            else
            {
                return View("../Account/Register");
            }
        }

        public ActionResult ViewUser(Users user)
        {
            string email = string.Empty;
            if (Session["User"] != null)
            {
                user = (Users)Session["User"];
                email = user.Email;
                return View("../Account/ViewUser", user);
            }
            else
            {
                return View("../Account/Login");
            }
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditUser(Users user)
        {
            double dblUserID = 0;
            Users tempUser = new Users();

            tempUser = (Users)Session["User"];
            if (Request.Form["Edit"] != null)
            {
                return View("../Account/EditUser", tempUser);
            }
            else if (Request.Form["ChangePassword"] != null)
            {
                return View("../Account/ChangePassword", tempUser);
            }

            if (Request.Form["EditSubmit"] != null)
            {
                if (ModelState.IsValid)
                {
                    ConnManager con = new ConnManager();
                    DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + tempUser.Email + "'");
                    if (dtUser.Rows.Count > 0)
                    {
                        user.UserId = double.Parse(dtUser.Rows[0]["UserId"].ToString());
                        user.OptionID = 2;
                        user.FirstName = user.FirstName;
                        user.LastName = user.LastName;
                        user.Email = user.Email;
                        user.ModifiedDateTime = DateTime.Now;
                        dblUserID = user.UserId;
                        user.CreateUsers(ref dblUserID);
                        ViewBag.Ack = "User Updated Successfully.";
                        Session["User"] = user;
                    }
                    return RedirectToAction("ViewUser", "Account");     //return Redirect("../Account/ViewUser");
                }
                else
                {
                    user = (Users)Session["User"];
                    return RedirectToAction("ViewUser", "Account");     //return View("../Account/ViewUser", user);
                }
            }
            else if (Request.Form["UpdateAsSiteOwner"] != null)
            {
                return RedirectToAction("ScriptCode", "Bookmark");               
            }
            else
            {
                user = (Users)Session["User"];
                return View("../Account/ViewUser", user);
            }
        }

        public ActionResult ChangePassword(string txtNewPassword)
        {
            if (Request.Form["Cancel"] == null)
            {
                if (Session["User"] != null && !string.IsNullOrEmpty(txtNewPassword))
                {
                    user = (Users)Session["User"];
                    double dblUserID = 0;
                    user.OptionID = 5;
                    user.Password = txtNewPassword;
                    user.ModifiedDateTime = DateTime.Now;
                    dblUserID = user.UserId;
                    user.CreateUsers(ref dblUserID);
                    ViewBag.Ack = "Password changed successfully";
                }
                return View(user);
            }
            else
            {
                if (Session["User"] != null)
                {
                    user = (Users)Session["User"];
                    ViewBag.UserEMail = user.Email;
                }
                return View("../Account/ViewUser", user);
            }
        }

        public ActionResult ForgotPassword(string txtEMailId)
        {
            if (!string.IsNullOrEmpty(txtEMailId))
            {
                ConnManager con = new ConnManager();

                DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where EmailId = '" + txtEMailId + "'");
                if (dtUserActivation.Rows.Count > 0)
                {
                    //ViewBag.Ack = "User activation pending";
                    ViewBag.Activation = "User activation pending. Resend Activation Code?";
                    ViewBag.UserActEMail = txtEMailId;
                    return View();
                }

                DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + txtEMailId + "'");
                if (dtUser.Rows.Count <= 0)
                {
                    ViewBag.Ack = "No such email exists";
                }

                else if (!string.IsNullOrEmpty(dtUser.Rows[0]["Password"].ToString()))
                {
                    Mail mail = new Mail();
                    mail.IsBodyHtml = true;
                    string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                    mail.Body = string.Format(EMailBody, "Forgot Password", "Your Booqmarqs account password is " + dtUser.Rows[0]["Password"].ToString());
                    mail.FromAdd = "admin@booqmarqs.com";
                    mail.Subject = "Booqmarqs account password";
                    mail.ToAdd = dtUser.Rows[0]["EMail"].ToString();
                    mail.SendMail();

                    ViewBag.Ack = "Password has been emailed to you, please check your inbox.";
                }
                else
                {

                    ViewBag.Ack = "You have created your profile thorugh one of the social sites. Please use the same channel to login. Google Or Facebook";
                }
            }
            return View();
        }

        public ActionResult Activate()
        {
            string ActivationCode = Request.QueryString["ActivationCode"];
            Users usr = new global::Bookmark.Users();
            ViewBag.Ack = usr.ActivateUser(ActivationCode);
            return View("../Account/Login");
        }

        public ActionResult ProcessActivationCode()
        {
            string txtEMailId = Request.Form["hfUserEMail"];
            ConnManager con = new ConnManager();
            DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + txtEMailId + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                SendActivationEMail(txtEMailId, dtUserActivation.Rows[0]["ActivationCode"].ToString());
                ViewBag.Activation = "Activation Code Sent. Please check your inbox and click on the activation link.";
                ViewBag.UserActEMail = txtEMailId;
            }
            return View("../Account/Login");
        }

        private void SendEMail(string Email_address, string firstName, string LastName)
        {
            try
            {
                Mail mail = new Mail();
                string strBody = string.Empty;
                strBody = "new user email id " + Email_address + " name " + firstName;

                try
                {
                    strBody += "<br /><br /> IP - " + Utilities.GetUserIP() + "<br /><br />";
                }
                catch
                {

                }

                mail.Body = strBody;
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = "New User registered";

                mail.ToAdd = "admin@booqmarqs.com";
                mail.IsBodyHtml = true;
                //mail.SendMail();
            }
            catch
            {


            }
        }

        private void SendNewUserRegEMail(string email)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://Booqmarqs.com>Booqmarqs</a>";
                mail.Body = string.Format(EMailBody, "New User Register", "Welcome to " + strCA + ". We appreciate your time for posting code that help many.");
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = "Welcome to Booqmarqs - ";
                mail.ToAdd = email;
                mail.IsBodyHtml = true;
                //mail.SendMail();
            }
            catch
            {


            }
        }

        private void SendActivationEMail(string email, string ActivationCode)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                string strActLink = "http://booqmarqs.com/Account/Activate/?ActivationCode=" + ActivationCode;
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com>Booqmarqs</a>";
                mail.Body = string.Format(EMailBody, "User Activation", "Welcome to " + strCA + ". We appreciate your time for posting code that help many. <br/> <br/>Please click <a id=actHere href=http://booqmarqs.com/Account/Activate/?ActivationCode=" + ActivationCode + ">" + strActLink + "</a> to activate your account");
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = "Welcome to Booqmarqs - ";
                mail.ToAdd = email;
                mail.IsBodyHtml = true;
                mail.SendMail();
            }
            catch
            {


            }
        }


        /// <summary>
        /// Social Login Section
        /// </summary>

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {            
            if(Request.Form.Keys.Count > 0)
            {
                if(Request.Form.Keys[1].Contains("Facebook"))
                {
                    provider = "Facebook";
                }
                else if (Request.Form.Keys[1].Contains("Google"))
                {
                    provider = "Google";

                }
                else if (Request.Form.Keys[1].Contains("Twitter"))
                {
                    provider = "Twitter";
                }
            }
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            bool _isNewUser = false;
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ClaimsIdentity claimsIdentities = loginInfo.ExternalIdentity;

                Users user1 = new Users();
                //user1.UserId = 1;
                user1.FirstName = (from c in claimsIdentities.Claims
                                   where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"
                                   select c.Value).Single();
                user1.LastName = (from c in claimsIdentities.Claims
                                  where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"
                                  select c.Value).Single();
                user1.Email = (from c in claimsIdentities.Claims
                               where c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                               select c.Value).Single();


                if (!IsActivated(user1.Email))
                {
                    return View("../Account/Login");
                }


                if (IsNewUser(ref user1))
                {
                    _isNewUser = true;
                    if (Session["SiteOwner"] != null && Session["SiteOwner"].ToString() == "SiteOwner")
                    {
                        user1.IsPublisher = true;
                    }
                    RegisterUser(user1);
                }


                Session["User"] = user1;
                HttpCookie mycookie = new HttpCookie("BooqmarqsLogin");
                mycookie.Values["EMail"] = user1.Email;
                mycookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(mycookie);


                if (Session["bookmark"] != null && Request.Form["btnQuickLogin"] != null)
                {
                    AddBookmark(Session["bookmark"], user1.UserId.ToString());
                    Session["bookmark"] = null;
                    return View("../Bookmark/BMAdded");
                }
                if (_isNewUser)
                { 
                    if(Session["SiteOwner"] != null && Session["SiteOwner"].ToString() == "SiteOwner")
                    {
                     
                        return RedirectToAction("ScriptCode", "Bookmark");
                    }
                    else
                    {
                        return RedirectToAction("MyBookmarks", "Bookmark");
                    }
                }
                else
                {
                    if(user1.IsPublisher)
                    {
                        return RedirectToAction("Reports", "Bookmark");
                    }
                    else
                    {
                        return RedirectToAction("MyBookmarks", "Bookmark");
                    }
                }
            }
        }



        private bool IsNewUser(string email)
        {
            bool userExists = false;
            DataTable dtUser = new DataTable();
            ConnManager con = new ConnManager();
            dtUser = con.GetDataTable("Select * from Users where Email = '" + email + "'");

            if (dtUser.Rows.Count > 0)
            {
                userExists = false;
            }
            else
            {
                userExists = true;
            }
            return userExists;
        }


        private bool IsNewUser(ref Users user1)
        {
            bool userExists = false;
            DataTable dtUser = new DataTable();
            ConnManager con = new ConnManager();            
            dtUser = con.GetDataTable("Select * from Users where Email = '" + user1.Email + "'");           

            if (dtUser.Rows.Count > 0)
            {
                userExists = false;
                user1.UserId = double.Parse(dtUser.Rows[0]["UserId"].ToString());
                user1.FirstName = dtUser.Rows[0]["FirstName"].ToString();
                user1.LastName = dtUser.Rows[0]["LastName"].ToString();
                user1.Email = dtUser.Rows[0]["EMail"].ToString();
                user1.IsPublisher = bool.Parse(dtUser.Rows[0]["IsPublisher"].ToString());
            }
            else
            {
                userExists = true;
            }
            return userExists;
        }


        private void RegisterUser(Users user1)
        {
            user1 = user1.CreateUser(user1.Email, user1.FirstName, user1.LastName, user1.IsPublisher);
            SendEMail(user.Email, user.FirstName, user.LastName);
        }

        private bool IsActivated(string email)
        {
            ConnManager con = new ConnManager();
            DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + email + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                //ViewBag.Ack = "User activation pending";
                ViewBag.Activation = "User activation pending. Resend Activation Code?";
                ViewBag.UserActEMail = email;
                return false;
            }
            else
                return true;
        }

        #region Helpers


        // Sign in the user with this external login provider if the user already has a login
        //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //switch (result)
        //{
        //    case SignInStatus.Success:
        //        return RedirectToLocal(returnUrl);
        //    //case SignInStatus.LockedOut:
        //        //return View("Lockout");
        //    //case SignInStatus.RequiresVerification:
        //        //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //    case SignInStatus.Failure:
        //    default:
        //        // If the user does not have an account, then prompt the user to create an account
        //        ViewBag.ReturnUrl = returnUrl;
        //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //}


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOff()
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    return RedirectToAction("Index", "Home");
        //}


        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }




        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }



}
