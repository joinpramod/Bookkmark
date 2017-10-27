using System.Web.Mvc;
using Bookkmark.Models;
using System.Data;
using System.Collections.Generic;
using Bookkmark.AppCode;
using System;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace Bookkmark.Controllers
{

    public class AccountController : BaseController
    {
        private Users user = new Users();

        public ActionResult Login()
        {
            return View();
        }

        //public ActionResult Google()
        //{
        //    return View();
        //}

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
                user.ImageURL = DSUserList.Rows[0]["ImageURL"].ToString();
                user.Password = DSUserList.Rows[0]["Password"].ToString();

                user.ImageURL = user.ImageURL.Replace("~", "");
                user.ImageURL = user.ImageURL.Replace("/Bookkmark", "");


                DataSet dsQuestions = new DataSet();
                DataSet dsAnswers = new DataSet();
                DataSet dsArticles = new DataSet();

                dsQuestions = connManager.GetData("Select Count(*) from Question where AskedUser = " + user.UserId + "");
                if (dsQuestions.Tables.Count > 0 && dsQuestions.Tables[0].Rows.Count > 0)
                    user.QuestionsPosted = dsQuestions.Tables[0].Rows[0][0].ToString();

                dsAnswers = connManager.GetData("Select Count(*) from Replies where RepliedUser = " + user.UserId + "");
                if (dsAnswers.Tables.Count > 0 && dsAnswers.Tables[0].Rows.Count > 0)
                    user.AnswersPosted = dsAnswers.Tables[0].Rows[0][0].ToString();

                dsArticles = connManager.GetData("Select Count(*) from CodeArticles where UserId = " + user.UserId + "");
                if (dsArticles.Tables.Count > 0 && dsArticles.Tables[0].Rows.Count > 0)
                    user.ArticlesPosted = dsArticles.Tables[0].Rows[0][0].ToString();
                else
                    user.ArticlesPosted = "0";

                user.Details = DSUserList.Rows[0]["Details"].ToString();
                Session["User"] = user;
                Session["user.Email"] = user.Email;
                ViewBag.UserEmail = user.Email;
                connManager.DisposeConn();
                return View("../Account/ViewUser", user);

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
                return View(user);
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
                    if (fileUserPhoto != null && fileUserPhoto.ContentLength > 1048576)
                    {
                        ViewBag.Ack = "Image file size should be less than 1 mb";
                        //return;
                    }
                    else
                    {
                        //try
                        //{
                        //if(!user.Email.ToLower().Contains("mail.ru"))
                        //{
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

                                if (fileUserPhoto != null && fileUserPhoto.FileName != "")
                                {
                                    try
                                    {
                                        string fileName = System.IO.Path.GetFileNameWithoutExtension(fileUserPhoto.FileName);
                                        string fileExt = System.IO.Path.GetExtension(fileUserPhoto.FileName);
                                        string fullFileName = System.IO.Path.GetFileName(fileUserPhoto.FileName);

                                        if (!System.IO.File.Exists(Server.MapPath("~\\Images\\") + fullFileName))
                                        {
                                            fileUserPhoto.SaveAs(Server.MapPath("~\\Images\\") + fullFileName);
                                        }
                                        else
                                        {
                                            fullFileName = fileName + DateTime.Now.ToString("HHmmss") + fileExt;
                                            while (System.IO.File.Exists(fullFileName))
                                            {
                                                fileName = fileName + DateTime.Now.ToString("HHmmss");
                                                fullFileName = fileName + fileExt;
                                            }
                                            fileUserPhoto.SaveAs(Server.MapPath("~\\Images\\") + fullFileName);
                                        }
                                        user.ImageURL = "~/Images/" + fullFileName;
                                    }
                                    catch (Exception ex)
                                    {
                                        //ViewBag.Ack = "Please try again";
                                        user.ImageURL = "~/Images/Person.JPG";
                                    }
                                }
                                else
                                {
                                    user.ImageURL = "http://www.gravatar.com/avatar/" + Utilities.HashEmailAddress(user.Email) + "?s=80&r=g&d=Identicon";
                                    //user.ImageURL = "~/Images/Person.JPG";
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

                                //ViewBag.Ack = "User Registered Successfully. Please login.";
                                ViewBag.Ack = "User Info Saved Successfully. An activation link has been sent to your email address, please check your inbox and activate your account";
                                //SendNewUserRegEMail(user.Email);
                                SendActivationEMail(user.Email, activationCode);
                                SendEMail(user.Email, user.FirstName, user.LastName);
                        //}
                    }
                    //Session["User"] = user;
                    //return View("ViewUser", user);
                    //return Redirect("../Account/ViewUser");
                    user = new Users();
                    return View("Users", user);
                }
                else
                {
                    ViewBag.Ack = ModelState["ReCaptcha"].Errors[0].ErrorMessage;
                    return View("Users", user);
                }
            }
            else
            {
                return View("Users", user);
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
                    if (fileUserPhoto != null && fileUserPhoto.ContentLength > 1048576)
                    {
                        ViewBag.Ack = "Image file size should be less than 1 mb";
                        //return;
                    }
                    else
                    {
                        //try
                        //{
                            ConnManager con = new ConnManager();
                            DataSet dsUser = con.GetData("Select * from Users where Email = '" + user.Email + "'");
                            con.DisposeConn();
                            if (dsUser.Tables[0].Rows.Count > 0)
                            {
                                //if (Session["User"] == null)
                                //{
                                //    ViewBag.Ack = "EMail id already exists. If you have forgotten password, please click forgot password link on the Sign In page.";
                                //    //return;
                                //}
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

                            if (fileUserPhoto != null && fileUserPhoto.FileName != "")
                            {
                                try
                                {
                                    string fileName = System.IO.Path.GetFileNameWithoutExtension(fileUserPhoto.FileName);
                                    string fileExt = System.IO.Path.GetExtension(fileUserPhoto.FileName);
                                    string fullFileName = System.IO.Path.GetFileName(fileUserPhoto.FileName);

                                    if (!System.IO.File.Exists(Server.MapPath("~\\Images\\") + fullFileName))
                                    {
                                        fileUserPhoto.SaveAs(Server.MapPath("~\\Images\\") + fullFileName);
                                    }
                                    else
                                    {
                                        fullFileName = fileName + DateTime.Now.ToString("HHmmss") + fileExt;
                                        while (System.IO.File.Exists(fullFileName))
                                        {
                                            fileName = fileName + DateTime.Now.ToString("HHmmss");
                                            fullFileName = fileName + fileExt;
                                        }
                                        fileUserPhoto.SaveAs(Server.MapPath("~\\Images\\") + fullFileName);
                                    }
                                    user.ImageURL = "~/Images/" + fullFileName;
                                }
                                catch (Exception ex)
                                {
                                //ViewBag.Ack = "Please try again";
                                user.ImageURL = "~/Images/Person.JPG";
                            }
                                user.OptionID = 5;
                            }
                            else
                            {
                                user.OptionID = 7;
                                Users tempUser = new Bookkmark.Users();
                                tempUser = (Users)Session["User"];
                                user.ImageURL = tempUser.ImageURL;
                            }


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
                        //}
                        //catch
                        //{

                        //}
                    }
                    Session["User"] = user;
                    //return View("ViewUser", user);
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

        //public ActionResult GoogleUser()
        //{
        //    // string txtEMailId = (string)this.RouteData.Values["id"].ToString();
        //    // return CheckUserLogin(txtEMailId, null);
        //    ViewBag.EMailId= Request.QueryString["id"].ToString();
        //    Response.Redirect("../Account/ForgotPassword");
        //    return null;
        //}


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

        //public ActionResult MyQues()
        //{
        //    List<QuestionModel> questions = new List<QuestionModel>();
        //    questions = GetMyQues(questions);
        //    PagingInfo info = new PagingInfo();
        //    info.SortField = " ";
        //    info.SortDirection = " ";
        //    info.PageSize = 20;
        //    info.PageCount = Convert.ToInt32(Math.Ceiling((double)(questions.Count() / info.PageSize)));
        //    info.CurrentPageIndex = 0;
        //    var query = questions.OrderBy(c => c.QuestionID).Take(info.PageSize);
        //    ViewBag.PagingInfo = info;
        //    return View(query.ToList());
        //}

        //private List<QuestionModel> GetMyQues(List<QuestionModel> questions)
        //{
        //    user = (Users)Session["User"];
        //    if (ModelState.IsValid)
        //    {
        //        string strSQL = string.Empty;
        //        strSQL = "SELECT * FROM VwQuestions WHERE AskedUser = " + user.UserId;
        //        ConnManager connManager = new ConnManager();
        //        questions = connManager.GetQuestions(strSQL);
        //    }
        //    return questions;
        //}

        //[HttpPost]
        //public ActionResult MyQues(PagingInfo info)
        //{
        //    if (Request.Form["Cancel"] == null)
        //    {
        //        List<QuestionModel> questions = new List<QuestionModel>();
        //        questions = GetMyQues(questions);

        //        IQueryable<QuestionModel> query = questions.AsQueryable();
        //        query = query.Skip(info.CurrentPageIndex * info.PageSize).Take(info.PageSize);
        //        ViewBag.PagingInfo = info;
        //        List<QuestionModel> model = query.ToList();
        //        return View(model);
        //    }
        //    else
        //    {
        //        if (Session["User"] != null)
        //        {
        //            user = (Users)Session["User"];
        //        }
        //        return View("../Account/ViewUser", user);

        //    }
        //}



        //public ActionResult MyAns()
        //{

        //    List<VwSolutionsModel> solns = new List<VwSolutionsModel>();
        //    solns = GetMyAns(solns);
        //    PagingInfo info = new PagingInfo();
        //    info.SortField = " ";
        //    info.SortDirection = " ";
        //    info.PageSize = 20;
        //    info.PageCount = Convert.ToInt32(Math.Ceiling((double)(solns.Count() / info.PageSize)));
        //    info.CurrentPageIndex = 0;
        //    var query = solns.OrderBy(c => c.QuestionID).Take(info.PageSize);
        //    ViewBag.PagingInfo = info;
        //    return View(query.ToList());

        //}

        //private List<VwSolutionsModel> GetMyAns(List<VwSolutionsModel> solns)
        //{
        //    user = (Users)Session["User"];
        //    if (ModelState.IsValid)
        //    {
        //        string strSQL = string.Empty;
        //        strSQL = "SELECT * FROM VwSolutions WHERE RepliedUser = " + user.UserId;
        //        ConnManager connManager = new ConnManager();
        //        solns = connManager.GetSolns(strSQL);
        //    }
        //    return solns;
        //}

        //[HttpPost]
        //public ActionResult MyAns(PagingInfo info)
        //{
        //    if (Request.Form["Cancel"] == null)
        //    {
        //        List<VwSolutionsModel> questions = new List<VwSolutionsModel>();
        //        questions = GetMyAns(questions);

        //        IQueryable<VwSolutionsModel> query = questions.AsQueryable();
        //        query = query.Skip(info.CurrentPageIndex * info.PageSize).Take(info.PageSize);
        //        ViewBag.PagingInfo = info;
        //        List<VwSolutionsModel> model = query.ToList();
        //        return View(model);
        //    }
        //    else
        //    {
        //        if (Session["User"] != null)
        //        {
        //            user = (Users)Session["User"];
        //        }
        //        return View("../Account/ViewUser", user);

        //    }
        //}




        //public ActionResult MyArts()
        //{

        //    List<ArticleModel> solns = new List<ArticleModel>();
        //    solns = GetMyArts(solns);
        //    PagingInfo info = new PagingInfo();
        //    info.SortField = " ";
        //    info.SortDirection = " ";
        //    info.PageSize = 20;
        //    info.PageCount = Convert.ToInt32(Math.Ceiling((double)(solns.Count() / info.PageSize)));
        //    info.CurrentPageIndex = 0;
        //    var query = solns.OrderBy(c => c.ArticleID).Take(info.PageSize);
        //    ViewBag.PagingInfo = info;
        //    return View(query.ToList());

        //}

        //private List<ArticleModel> GetMyArts(List<ArticleModel> solns)
        //{
        //    user = (Users)Session["User"];
        //    if (ModelState.IsValid)
        //    {
        //        string strSQL = string.Empty;
        //        strSQL = "SELECT * FROM VwArticles WHERE UserId = " + user.UserId;
        //        ConnManager connManager = new ConnManager();
        //        solns = connManager.GetArticles(strSQL);
        //    }
        //    return solns;
        //}

        //[HttpPost]
        //public ActionResult MyArts(PagingInfo info)
        //{
        //    if (Request.Form["Cancel"] == null)
        //    {
        //        List<ArticleModel> arts = new List<ArticleModel>();
        //        arts = GetMyArts(arts);

        //        IQueryable<ArticleModel> query = arts.AsQueryable();
        //        query = query.Skip(info.CurrentPageIndex * info.PageSize).Take(info.PageSize);
        //        ViewBag.PagingInfo = info;
        //        List<ArticleModel> model = query.ToList();
        //        return View(model);
        //    }
        //    else
        //    {
        //        if (Session["User"] != null)
        //        {
        //            user = (Users)Session["User"];
        //        }
        //        return View("../Account/ViewUser", user);

        //    }
        //}


        //public ActionResult ViewUserProfile(string UserId)
        //{
        //    Users userProfile = new Users();
        //    string strSQL = string.Empty;
        //    strSQL = "SELECT * FROM Users WHERE UserId = " + user.UserId;
        //    ConnManager connManager = new ConnManager();
        //    solns = connManager.GetArticles(strSQL);

        //    userProfile.UserId = double.Parse(DSUserList.Rows[0]["UserId"].ToString());
        //    userProfile.FirstName = DSUserList.Rows[0]["FirstName"].ToString();
        //    userProfile.LastName = DSUserList.Rows[0]["LastName"].ToString();
        //    //userProfile.Email = DSUserList.Rows[0]["EMail"].ToString();
        //    userProfile.Address = DSUserList.Rows[0]["Address"].ToString();
        //    userProfile.ImageURL = DSUserList.Rows[0]["ImageURL"].ToString();
        //    user.ImageURL = user.ImageURL.Replace("~", "");
        //    user.ImageURL = user.ImageURL.Replace("/Bookkmark", "");

        //    return View(userProfile);
        //}


    }
}
