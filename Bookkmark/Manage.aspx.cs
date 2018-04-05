using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Bookmark
{
    public partial class Manage : System.Web.UI.Page
    {
        Users user = new Users();

        protected void Page_Load(object sender, EventArgs e)
        {
            //user = (Users)Session["User"];
            //if (user == null || user.Email == null || user.Email != "admin@booqmarqs.com")
            //{
            //    Response.Redirect("http://www.booqmarqs.com");
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            user = (Users)Session["User"];
            if (user != null && user.Email != null && user.Email == "admin@booqmarqs.com")
            {
                ConnManager connManager = new ConnManager();

                if (txtSQL.Text.ToLower().StartsWith("select"))
                {
                    DataTable DSQuestions = new DataTable();
                    DSQuestions = connManager.GetDataTable(txtSQL.Text);
                    if (DSQuestions != null)
                    {
                        if (DSQuestions.Rows.Count > 0)
                        {
                            GridView1.DataSource = DSQuestions;
                            GridView1.DataBind();
                        }
                        else
                        {
                            GridView1.DataSource = new DataTable();
                            GridView1.DataBind();
                        }
                    }
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(txtSQL.Text, conn))
                        {
                            comm.ExecuteNonQuery();
                        }
                    }                   
                }
            }
            else
            {
                Response.Redirect("http://www.booqmarqs.com");
            }
        }


        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                Mail mail = new Mail();
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("~/EMailBody2.txt"));
                //string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/MyBookmarks>here</a>";
                //string strBrowExt = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/Extensions>here</a>";
                //mail.Body = string.Format(EMailBody, "", "Greetings!! <br/><br/> You are having your bookmarks always with you handy. Access your bookmarks " + strCA + " anytime, any machine and any browser. <br/> <br/> Also you can use our browser extensions from " + strBrowExt + ". <br/><br/> We sincerely appreciate your time for using Booqmarqs and would be glad to help in any ways possible as we always try hard to get the best user experience. You could also reply to email anytime.");

                string strImport = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/Import>here</a>";
                string strBrowExt = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/Extensions>here</a>";
                mail.Body = string.Format(EMailBody, "", "Greetings!!<br/><br/>You are just one step away in having all your bookmarks handy online anytime, any machine and browser.<br/><br/>Use our browser extensions to import all your bookmarks in 1 shot i.e. with one button click from " + strBrowExt + ".<br/><br/>Or do it in 2 easy steps from the website " + strImport + ".  Take a look <br/><br/>Feel the comfort of having all your valuable links with you always.<br/><br/>");


                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = txtSubject.Text;
                mail.ToAdd = txtEmailId.Text;
                mail.IsBodyHtml = true;
                mail.SendMail();
           
                //<p style = "font-family:Calibri;font-size:12px;font-weight:bold">
                //    {0}
                //</p>
                //<pre style = "font-family:Calibri;font-size:12px">
                //    {1}
                //        thanks
                //        Booqmarqs Team
                //</pre>
          
            }
            catch
            {


            }
        }


        protected void btnSendBulkEMails_Click(object sender, EventArgs e)
        {

            ConnManager conn = new Bookmark.ConnManager();
            DataTable dtData = conn.GetDataTable("select count(*), createduser, max(email) as email from VwUserBookmarks group by createduser order by createduser");

            if (dtData != null && dtData.Rows.Count > 0)
            {
                foreach (DataRow dr in dtData.Rows)
                {
                    try
                    {
                        Mail mail = new Mail();
                        string EMailBody = System.IO.File.ReadAllText(Server.MapPath("~/EMailBody2.txt"));
                        string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/MyBookmarks>bookmarks</a>";
                        string strBrowExt = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/Extensions>here</a>";
                        mail.Body = string.Format(EMailBody, "", "You are having your bookmarks always handy. Here are your " + strCA + ", access them on any machine or browser. <br/> <br/> Also you can use our browser extensions from " + strBrowExt);
                        mail.FromAdd = "admin@booqmarqs.com";
                        mail.Subject = "View your bookmarks";
                        mail.ToAdd = dr["EMAil"].ToString();
                        mail.IsBodyHtml = true;
                        mail.SendMail();
                    }
                    catch
                    {


                    }
                }
            }
        }



        protected void btnSendBulkEMailsWhoDontHaveBookmarks_Click(object sender, EventArgs e)
        {
            Mail mail = new Mail();
            string EMailBody = System.IO.File.ReadAllText(Server.MapPath("~/EMailBody2.txt"));
            string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/MyBookmarks>here</a>";
            string strBrowExt = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/Extensions>here</a>";
            mail.Body = string.Format(EMailBody, "", "You can have your bookmarks always handy. Quick import " + strCA + " and access them on any machine or browser. <br/> <br/> Also you use our browser extensions from " + strBrowExt);
            mail.FromAdd = "admin@booqmarqs.com";
            mail.Subject = "View your bookmarks";
            mail.ToAdd = txtEmailId.Text;
            mail.IsBodyHtml = true;
            mail.SendMail();
        }
    }
}