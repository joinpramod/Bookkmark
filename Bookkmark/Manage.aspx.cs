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
            if (user != null && user.Email != null && user.Email == "admin@booqmarqs.com")
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
    }
}