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
            user = (Users)Session["User"];
            if (user == null || user.Email == null || user.Email != "admin@booqmarqs.com")
            {
                Response.Redirect("http://www.booqmarqs.com");
            }
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
                string EMailBody = System.IO.File.ReadAllText(Server.MapPath("~/EMailBody.txt"));
                string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/MyBookmarks>bookmarks</a>";
                mail.Body = string.Format(EMailBody, "My Bookmarks", "You are having your bookmarks always handy. View your " + strCA + " and access them on any machine or browser <br/> <br/> Also you use our browser extension");
                mail.FromAdd = "admin@booqmarqs.com";
                mail.Subject = txtSubject.Text;
                mail.ToAdd = txtEmailId.Text;
                mail.IsBodyHtml = true;
                mail.SendMail();
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
                        string EMailBody = System.IO.File.ReadAllText(Server.MapPath("~/EMailBody.txt"));
                        string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/MyBookmarks>bookmarks</a>";
                        mail.Body = string.Format(EMailBody, "My Bookmarks", "You are having your bookmarks always handy. View your " + strCA + " and access them on any machine or browser.  <br/> <br/> Also you can use our browser extension");
                        mail.FromAdd = "admin@booqmarqs.com";
                        mail.Subject = "Booqmarqs - View your bookmarks";
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
            string EMailBody = System.IO.File.ReadAllText(Server.MapPath("~/EMailBody.txt"));
            string strCA = "<a id=HyperLink1 style=font-size: medium; font-weight: bold; color:White href=http://booqmarqs.com/Bookmark/MyBookmarks>here</a>";
            mail.Body = string.Format(EMailBody, "My Bookmarks", "You can have your bookmarks always handy. Quick import " + strCA + " and access them on any machine or browser <br/> <br/> Also you use our browser extension");
            mail.FromAdd = "admin@booqmarqs.com";
            mail.Subject = "Booqmarqs - View your bookmarks";
            mail.ToAdd = txtEmailId.Text;
            mail.IsBodyHtml = true;
            mail.SendMail();
        }
    }
}