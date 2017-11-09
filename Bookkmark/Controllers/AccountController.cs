using System.Web.Mvc;
using System.Data;
using System;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Bookkmark.AppCode;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Bookkmark.Models;

namespace Bookkmark.Controllers
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

                DataSet dsUser = con.GetData("Select * from Users where Email = '" + txtEMailId + "'");
                con.DisposeConn();
                if (dsUser.Tables[0].Rows.Count <= 0)
                {
                    ViewBag.Ack = "No such EMail Id exists";
                }
             
                if (!string.IsNullOrEmpty(dsUser.Tables[0].Rows[0]["Password"].ToString()))
                {
                    Mail mail = new Mail();
                    mail.IsBodyHtml = true;
                    string EMailBody = System.IO.File.ReadAllText(Server.MapPath("EMailBody.txt"));

                    mail.Body = string.Format(EMailBody, "Your CodeAnalyze account password is " + dsUser.Tables[0].Rows[0]["Password"].ToString());


                    mail.FromAdd = "admin@codeanalyze.com";
                    mail.Subject = "Bookkmark account password";
                    mail.ToAdd = dsUser.Tables[0].Rows[0]["EMail"].ToString();
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

        public ActionResult Activate()
        {
            string ActivationCode = Request.QueryString["ActivationCode"];
            ConnManager con = new ConnManager();
            DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  ActivationCode = '" + ActivationCode + "'");

            if (dtUserActivation != null && dtUserActivation.Rows.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("delete from UserActivation where  EMailId = @EMailId"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EMailId", dtUserActivation.Rows[0]["EMailId"]);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                ViewBag.lblAck = "Account activated successfully. Please login with your emaild and password";
            }
            else
            {
                ViewBag.lblAck = "Account did not activated, please get the activation resent to your inbox and try again";
            }

           
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

        private ActionResult CheckUserLogin(string txtEMailId, string txtPassword)
        {
            Users user1 = new Users();
            user1.UserId = 1;
            user1.FirstName = "FirstName";
            user1.LastName = "LastName";
            user1.Email = "admin@bookkmark.com";
            Session["User"] = user1;
            Session["user.Email"] = user1.Email;
            ViewBag.UserEmail = user1.Email;
            if (Session["bookkmark"] != null)
            {
                //AddBookmark(Session["bookkmark"]);
                //ViewData["Refresh"] = "true";
                return View("../Bookkmark/BMAdded");
            }
            else
            {
                if (txtPassword != null)
                    return RedirectToAction("MyBookkmarks", "Bookkmark");
                else
                    return View("../Account/ViewUser", user);
            }



            ConnManager connManager = new ConnManager();
            connManager.OpenConnection();
            DataTable DSUserList = new DataTable();
            DataTable dtUserActivation = new DataTable();

            dtUserActivation = connManager.GetDataTable("select * from UserActivation where Emailid = '" + txtEMailId + "'");
            if (dtUserActivation.Rows.Count > 0)
            {
                //ViewBag.lblAck = "User activation pending";
                ViewBag.Activation = "User activation pending. Resend Activation Code?";
                ViewBag.UserActEMail = txtEMailId;
                return View("../Account/Login");
            }

            if (!string.IsNullOrEmpty(txtPassword))
            {
                DSUserList = connManager.GetDataTable("select * from users where email = '" + txtEMailId + "' and Password = '" + txtPassword + "'");
            }
            else
            {
                DSUserList = connManager.GetDataTable("select * from users where email = '" + txtEMailId + "'");
            }


            if (DSUserList.Rows.Count == 0)
            {
                ViewBag.lblAck = "Invalid login credentials, please try again";
                return View("../Account/Login");
            }
            else
            {
                Users user = new Users();
                user.UserId = double.Parse(DSUserList.Rows[0]["UserId"].ToString());
                user.FirstName = DSUserList.Rows[0]["FirstName"].ToString();
                user.LastName = DSUserList.Rows[0]["LastName"].ToString();
                user.Email = DSUserList.Rows[0]["EMail"].ToString();
                user.Address = DSUserList.Rows[0]["Address"].ToString();
                //user.ImageURL = DSUserList.Rows[0]["ImageURL"].ToString();
                user.Password = DSUserList.Rows[0]["Password"].ToString();

                //user.ImageURL = user.ImageURL.Replace("~", "");
                //user.ImageURL = user.ImageURL.Replace("/Bookkmark", "");

                user.Details = DSUserList.Rows[0]["Details"].ToString();
                Session["User"] = user;
                Session["user.Email"] = user.Email;
                ViewBag.UserEmail = user.Email;
                connManager.DisposeConn();

                if (Session["bookkmark"] != null)
                {
                    //AddBookmark(Session["bookkmark"]);
                    //ViewData["Refresh"] = "true";
                    return View("../Account/BMAdded");
                }
                else
                {
                    if (txtPassword != null)
                        return RedirectToAction("MyBookkmarks", "Bookkmark");
                    else
                        return View("../Account/ViewUser", user);
                }
            }
        }

        public ActionResult Register()
        {
            if (Session["User"] != null)
            {
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
                return null;
            }
            else
            {
                user = new Users();
                if (ViewData["RegisType"] != null && ViewData["RegisType"].ToString() == "WebUser")
                {
                    return View("../Account/Register", user);
                }
                else
                {
                    return View("../Account/Register", user);
                }
            }
        }

        public ActionResult WebUserReg()
        {
            if (Session["User"] != null)
            {
                return View("../Bookkmark/MyBookkmarks");
            }
            else
            {
                ViewData["RegisType"] = "WebUser";
                return Register();
            }
        }

        public ActionResult SiteOwnerReg()
        {
            if (Session["User"] != null)
            {
                return View("../Bookkmark/Reports");
            }
            else
            {
                ViewData["RegisType"] = "SiteOwner";
                return Register();
            }
        }

        [ReCaptcha]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateEditUser(Users user, HttpPostedFileBase fileUserPhoto, string txtPassword)
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
                        //ViewBag.lblAck = "User activation pending";
                        ViewBag.Activation = "User activation pending. Resend Activation Code?";
                        ViewBag.UserActEMail = user.Email;
                        return View("Users", user);
                    }


                    DataSet dsUser = con.GetData("Select * from Users where Email = '" + user.Email + "'");
                    con.DisposeConn();
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        ViewBag.Ack = "EMail id already exists. If you have forgotten password, please click forgot password link on the Sign In page.";
                        return View("Users", user);
                    }


                    double dblUserID = 0;
                    SqlConnection LclConn = new SqlConnection();
                    SqlTransaction SetTransaction = null;
                    bool IsinTransaction = false;
                    if (LclConn.State != ConnectionState.Open)
                    {
                        user.SetConnection = user.OpenConnection(LclConn);
                        SetTransaction = LclConn.BeginTransaction(IsolationLevel.ReadCommitted);
                        IsinTransaction = true;
                    }
                    else
                    {
                        user.SetConnection = LclConn;
                    }

                    user.OptionID = 1;
                    user.CreatedDateTime = DateTime.Now;
                    user.Password = txtPassword;

                    bool result = user.CreateUsers(ref dblUserID, SetTransaction);
                    if (IsinTransaction && result)
                    {
                        SetTransaction.Commit();
                    }
                    else
                    {
                        SetTransaction.Rollback();
                    }


                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO UserActivation VALUES(@UserId, @EMailId, @ActivationCode)"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@UserId", dblUserID);
                                cmd.Parameters.AddWithValue("@EMailId", user.Email);
                                cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                                cmd.Connection = conn;
                                conn.Open();
                                cmd.ExecuteNonQuery();
                                conn.Close();
                            }
                        }
                    }

                    user.CloseConnection(LclConn);

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
        public ActionResult EditUser(Users user, HttpPostedFileBase fileUserPhoto)
        {
            //AddEdit user
            if (Request.Form["Cancel"] == null)
            {
                if (ModelState.IsValid)
                {
                    ConnManager con = new ConnManager();
                    DataSet dsUser = con.GetData("Select * from Users where Email = '" + user.Email + "'");
                    con.DisposeConn();
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        user.UserId = double.Parse(dsUser.Tables[0].Rows[0]["UserId"].ToString());
                    }


                    double dblUserID = 0;
                    SqlConnection LclConn = new SqlConnection();
                    SqlTransaction SetTransaction = null;
                    bool IsinTransaction = false;
                    if (LclConn.State != ConnectionState.Open)
                    {
                        user.SetConnection = user.OpenConnection(LclConn);
                        SetTransaction = LclConn.BeginTransaction(IsolationLevel.ReadCommitted);
                        IsinTransaction = true;
                    }
                    else
                    {
                        user.SetConnection = LclConn;
                    }

                    user.OptionID = 7;
                    Users tempUser = new Bookkmark.Users();
                    tempUser = (Users)Session["User"];
                    user.ImageURL = tempUser.ImageURL;


                    user.ModifiedDateTime = DateTime.Now;
                    dblUserID = user.UserId;


                    bool result = user.CreateUsers(ref dblUserID, SetTransaction);
                    if (IsinTransaction && result)
                    {
                        SetTransaction.Commit();
                    }
                    else
                    {
                        SetTransaction.Rollback();
                    }
                    user.CloseConnection(LclConn);
                    ViewBag.Ack = "User Updated Successfully.";

                    Session["User"] = user;
                    return Redirect("../Account/ViewUser");
                }
                else
                {
                    user = (Users)Session["User"];
                    return View("../Account/ViewUser", user);
                }
            }
            else
            {
                user = (Users)Session["User"];
                return View("../Account/ViewUser", user);
            }
        }

        private void SendActivationEMail(string email, string ActivationCode)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("../EMailBody.txt"));
                string strActLink = "http://codeanalyze.com/Account/Activate/?ActivationCode=" + ActivationCode;
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://codeanalyze.com>CodeAnalyze</a>";
                mail.Body = string.Format(EMailBody, "Welcome to " + strCA + ". We appreciate your time for posting code that help many. <br/> <br/>Please click <a id=actHere href=http://codeanalyze.com/Account/Activate/?ActivationCode=" + ActivationCode + ">" + strActLink + "</a> to activate your account");
                mail.FromAdd = "admin@codeanalyze.com";
                mail.Subject = "Welcome to CodeAnalyze - ";
                mail.ToAdd = email;
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
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://codeanalyze.com>CodeAnalyze</a>";
                mail.Body = string.Format(EMailBody, "Welcome to " + strCA + ". We appreciate your time for posting code that help many.");
                mail.FromAdd = "admin@codeanalyze.com";
                mail.Subject = "Welcome to CodeAnalyze - ";
                mail.ToAdd = email;
                mail.IsBodyHtml = true;
                mail.SendMail();
            }
            catch
            {


            }
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
                mail.FromAdd = "admin@codeanalyze.com";
                mail.Subject = "New User registered";

                mail.ToAdd = "admin@codeanalyze.com";
                mail.IsBodyHtml = true;
                mail.SendMail();
            }
            catch
            {


            }
        }

        public ActionResult ViewUser(Users user)
        {

            string email = string.Empty;

            if (Session["User"] != null)
            {
                user = (Users)Session["User"];
                email = user.Email;
                if (Request.Url.ToString().Contains("localhost"))
                {
                    if (user.ImageURL != null && !user.ImageURL.Contains("/Bookkmark/"))
                    {
                        user.ImageURL = "/Bookkmark/" + user.ImageURL.Replace("~/", "");
                    }
                }
                else //if (user.ImageURL != null && !user.ImageURL.Contains("/codeanalyze.com/"))
                {
                    user.ImageURL = user.ImageURL.Replace("~", "");
                    user.ImageURL = user.ImageURL.Replace("/Bookkmark", "");
                }

            }
            else if (Request.Form.Keys.Count > 0)
            {
                email = Request.Form.Keys[0].ToString();
                string name = Request.Form.Keys[1].ToString();
                string imageurl = Request.Form.Keys[2].ToString();
                //string type = Request.Form.Keys[3].ToString();
                double _userId = 0;

                if (!user.UserExists(email, ref _userId))
                {
                    user = user.CreateUser(email, name, "", imageurl);
                    SendNewUserRegEMail(email);
                    SendEMail(email, name, "");
                }
                // else
                //{
                //    user.Email = email;
                //    user.UserId = _userId;
                //}

                // Session["User"] = user;

            }
            return CheckUserLogin(email, null);
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session["User"] = null;
            Session["Facebook"] = null;
            Session.RemoveAll();
            ViewBag.UserEmail = null;

            Response.Redirect("../Home/Index");

　
            return View("../Home/Index");
        }

        [AllowAnonymous]
        public ActionResult Choice()
        {
            return View();
        }
        public ActionResult ChangePassword(string txtNewPassword)
        {

            if (Request.Form["Cancel"] == null)
            {
                if (Session["User"] != null && !string.IsNullOrEmpty(txtNewPassword))
                {
                    user = (Users)Session["User"];
                    double dblUserID = 0;
                    SqlConnection LclConn = new SqlConnection();
                    SqlTransaction SetTransaction = null;
                    bool IsinTransaction = false;
                    if (LclConn.State != ConnectionState.Open)
                    {
                        user.SetConnection = user.OpenConnection(LclConn);
                        SetTransaction = LclConn.BeginTransaction(IsolationLevel.ReadCommitted);
                        IsinTransaction = true;
                    }
                    else
                    {
                        user.SetConnection = LclConn;
                    }
                    user.OptionID = 5;
                    user.Password = txtNewPassword;
                    user.ModifiedDateTime = DateTime.Now;
                    dblUserID = user.UserId;

                    bool result = user.CreateUsers(ref dblUserID, SetTransaction);
                    if (IsinTransaction && result)
                    {
                        SetTransaction.Commit();
                    }
                    else
                    {
                        SetTransaction.Rollback();
                    }

                    user.CloseConnection(LclConn);
                    //lblUserRegMsg.Visible = true;
                    ViewBag.Ack = "Password changed successfully";
                    //txtPassword.Text = "";
                    //txtConfirmPassword.Text = "";

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
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

  
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

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


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

        #region Helpers
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
