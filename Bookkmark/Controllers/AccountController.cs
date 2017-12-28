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

            ///////Temporary
            //Users user1 = new Users();
            //user1.UserId = 1;
            //user1.FirstName = "FirstName";
            //user1.LastName = "LastName";
            //user1.Email = txtEMailId;
            //user.IsPublisher = true;
            //Session["User"] = user1;
            //Session["user.Email"] = user1.Email;
            //ViewBag.UserEmail = user1.Email;
            //HttpCookie mycookie = new HttpCookie("BooqmarqsLogin");
            //mycookie.Values["UserId"] = txtEMailId;
            //mycookie.Values["FirstName"] = "FirstName";
            //mycookie.Values["LastName"] = "LastName";
            //mycookie.Expires = System.DateTime.Now.AddDays(180);
            //Response.Cookies.Add(mycookie);

            //if (Session["bookmark"] != null && Request.Form["btnQuickLogin"] != null)
            //{
            //    //AddBookmark(Session["bookmark"]);
            //    Session["bookmark"] = null;
            //    return View("../Bookmark/BMAdded");
            //}
            //else if (user.IsPublisher)
            //{
            //    Session["RegisType"] = null;
            //    return RedirectToAction("Reports", "Bookmark");
            //}
            //else
            //{
            //    return RedirectToAction("MyBookmarks", "Bookmark");
            //}
            //////////




            /////Actual
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
                Session["user.Email"] = user.Email;
                ViewBag.UserEmail = user.Email;
                
                HttpCookie userCookie = new HttpCookie("BooqmarqsLogin");
                userCookie.Values["UserId"] = user.UserId.ToString();
                userCookie.Values["EMail"] = user.Email;
                userCookie.Values["FirstName"] = user.FirstName;
                userCookie.Values["LastName"] = user.LastName;
                userCookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(userCookie);


                if (Session["bookmark"] != null && Request.Form["btnQuickLogin"] != null)
                {
                    //AddBookmark(Session["bookmark"]);
                    Session["bookmark"] = null;
                    return View("../Bookmark/BMAdded");
                }
                else if (user.IsPublisher)
                {
                    Session["RegisType"] = null;
                    return RedirectToAction("Reports", "Bookmark");
                }
                else
                {
                    return RedirectToAction("MyBookmarks", "Bookmark");
                }
            }
        }

        public ActionResult Register()
        {
            return View("../Account/Register", user);
        }

        public ActionResult WebUserReg()
        {
            //ViewData["RegisType"] = "WebUser";
            return Register();
        }

        public ActionResult SiteOwnerReg()
        {
            Session["Session"] = "SiteOwner";
            return Register();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session["User"] = null;
            Session["Facebook"] = null;
            Session["bookmark"] = null;
            Session.RemoveAll();
            ViewBag.UserEmail = null;
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



        [ReCaptcha]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateUser(Users user, string txtPassword)
        {
            string activationCode = Guid.NewGuid().ToString();
            //AddEdit user
            if (Request.Form["Cancel"] == null)
            {
                if (ModelState.IsValid)
                {
                    ConnManager con = new ConnManager();

                    DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  Emailid = '" + user.Email + "'");
                    if (dtUserActivation.Rows.Count > 0)
                    {
                        //ViewBag.Ack = "User activation pending";
                        ViewBag.Activation = "User activation pending. Resend Activation Code?";
                        ViewBag.UserActEMail = user.Email;
                        return View("Users", user);
                    }

                    DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + user.Email + "'");
                    if (dtUser.Rows.Count > 0)
                    {
                        ViewBag.Ack = "EMail id already exists. If you have forgotten password, please click forgot password link on the Sign In page.";
                        return View("Users", user);
                    }

                    double dblUserID = 0;
                    user.OptionID = 1;
                    user.CreatedDateTime = DateTime.Now;
                    user.Password = txtPassword;
                    user.Email = user.Email;
                    user.IsWebUser = true;
                    user.CreateUsers(ref dblUserID);
                    user.CreateUserActivation(user, activationCode, dblUserID);
                    ViewBag.Ack = "User Info Saved Successfully. An activation link has been sent to your email address, please check your inbox and activate your account";
                    SendActivationEMail(user.Email, activationCode);
                    SendEMail(user.Email, user.FirstName, user.LastName);
                    user = new Users();
                    return View("Register", user);
                }
                else
                {
                    ViewBag.Ack = ModelState["ReCaptcha"].Errors[0].ErrorMessage;
                    return View("Register", user);
                }
            }
            else
            {
                return View("../Account/Choice");
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditUser(Users user)
        {
            double dblUserID = 0;
            Users tempUser = new global::Bookmark.Users();

            user = (Users)Session["User"];
            if (Request.Form["Edit"] != null)
            {
                return View("../Account/EditUser", user);
            }
            else if (Request.Form["ChangePassword"] != null)
            {
                ViewBag.OldPassword = user.Password;
                return View("../Account/ChangePassword", user);
            }

            if (Request.Form["EditSubmit"] != null)
            {
                if (ModelState.IsValid)
                {
                    ConnManager con = new ConnManager();
                    DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + user.Email + "'");
                    if (dtUser.Rows.Count > 0)
                    {
                        user.UserId = double.Parse(dtUser.Rows[0]["UserId"].ToString());
                        user.OptionID = 7;
                        tempUser = (Users)Session["User"];                        
                        //user.ImageURL = tempUser.ImageURL;
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
                    return View("../Account/Login");
                }

                DataTable dtUser = con.GetDataTable("Select * from Users where Email = '" + txtEMailId + "'");
                if (dtUser.Rows.Count <= 0)
                {
                    ViewBag.Ack = "No such EMail Id exists";
                }

                if (!string.IsNullOrEmpty(dtUser.Rows[0]["Password"].ToString()))
                {
                    Mail mail = new Mail();
                    mail.IsBodyHtml = true;
                    string EMailBody = System.IO.File.ReadAllText(Server.MapPath("EMailBody.txt"));

                    mail.Body = string.Format(EMailBody, "Your Booqmarqs account password is " + dtUser.Rows[0]["Password"].ToString());
                    mail.FromAdd = "admin@Booqmarqs.com";
                    mail.Subject = "Bookmark account password";
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
                mail.FromAdd = "admin@Booqmarqs.com";
                mail.Subject = "New User registered";

                mail.ToAdd = "admin@Booqmarqs.com";
                mail.IsBodyHtml = true;
                mail.SendMail();
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
                mail.Body = string.Format(EMailBody, "Welcome to " + strCA + ". We appreciate your time for posting code that help many.");
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

        private void SendActivationEMail(string email, string ActivationCode)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                string strActLink = "http://booqmarqs.com/Account/Activate/?ActivationCode=" + ActivationCode;
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com>Booqmarqs</a>";
                mail.Body = string.Format(EMailBody, "Welcome to " + strCA + ". We appreciate your time for posting code that help many. <br/> <br/>Please click <a id=actHere href=http://booqmarqs.com/Account/Activate/?ActivationCode=" + ActivationCode + ">" + strActLink + "</a> to activate your account");
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
                    ViewBag.OldPassword = txtNewPassword;
                }
                return View(user);
            }
            else
            {
                if (Session["User"] != null)
                {
                    user = (Users)Session["User"];
                }
                return View("../Account/ViewUser", user);
            }
        }

        public ActionResult Activate()
        {
            string ActivationCode = Request.QueryString["ActivationCode"];
            Users usr = new global::Bookmark.Users();
            ViewBag.Ack = usr.ActivateUser(ActivationCode);
            return View("../Account/Login");
        }

        public ActionResult ProcessActivationCode(string txtEMailId)
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
            return View("Users", user);
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
                Session["User"] = user1;
                Session["user.Email"] = user1.Email;
                ViewBag.UserEmail = user1.Email;
                HttpCookie mycookie = new HttpCookie("BooqmarqsLogin");
                mycookie.Values["UserId"] = user1.Email;
                mycookie.Values["FirstName"] = user1.FirstName;
                mycookie.Values["LastName"] = user1.LastName;
                mycookie.Expires = System.DateTime.Now.AddDays(180);
                Response.Cookies.Add(mycookie);

                if (IsNewUser(ref user1))
                {
                    _isNewUser = true;
                    RegisterUser(user1);
                }


                if (Session["bookmark"] != null && Request.Form["btnQuickLogin"] != null)
                {
                    //AddBookmark(Session["bookmark"]);
                    Session["bookmark"] = null;
                    return View("../Bookmark/BMAdded");
                }
                if (_isNewUser)
                { 
                    if(Session["RegisType"] != null && Session["RegisType"].ToString() == "SiteOwner")
                    {
                        Session["RegisType"] = null;
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


        private bool IsNewUser(ref Users user1)
        {
            bool userExists = false;
            DataTable dtUser = new DataTable();
            ConnManager con = new ConnManager();            
            dtUser = con.GetDataTable("Select * from Users where Email = '" + user1.Email + "'");           

            if (dtUser.Rows.Count > 0)
            {
                userExists = true;
                user.UserId = double.Parse(dtUser.Rows[0]["UserId"].ToString());
                user.FirstName = dtUser.Rows[0]["FirstName"].ToString();
                user.LastName = dtUser.Rows[0]["LastName"].ToString();
                user.Email = dtUser.Rows[0]["EMail"].ToString();
                user.IsPublisher = bool.Parse(dtUser.Rows[0]["IsPublisher"].ToString());
            }
            else
            {
                userExists = true;
            }
            return userExists;
        }


        private void RegisterUser(Users user1)
        {
            user1.CreateUser(user1.Email, user1.FirstName, user1.LastName);
            SendEMail(user.Email, user.FirstName, user.LastName);
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
