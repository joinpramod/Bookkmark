using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Bookmark
{
    public class Domain
    {

        public int OptID { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string ShowCount { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }


        public bool SetCommandDomain(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Domain_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamId = Cmd.Parameters.Add("@Id", SqlDbType.Float);
            SqlParameter ParamName = Cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            SqlParameter ParamHeight = Cmd.Parameters.Add("@Height", SqlDbType.VarChar);
            SqlParameter ParamWidth = Cmd.Parameters.Add("@Width", SqlDbType.VarChar);
            SqlParameter ParamShowCount = Cmd.Parameters.Add("@ShowCount", SqlDbType.VarChar);
            SqlParameter ParamScript = Cmd.Parameters.Add("@Script", SqlDbType.VarChar);
            SqlParameter ParamCreatedDate = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamCreatedUserId = Cmd.Parameters.Add("@CreatedUser", SqlDbType.VarChar);
            SqlParameter ParamModifiedDate = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);


            ParamOptID.Value = OptID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamId.Value = Id;
            ParamId.Direction = ParameterDirection.Input;
            ParamName.Value = Name;
            ParamName.Direction = ParameterDirection.Input;
            ParamHeight.Value = Height;
            ParamHeight.Direction = ParameterDirection.Input;
            ParamWidth.Value = Width;
            ParamWidth.Direction = ParameterDirection.Input;
            ParamShowCount.Value = ShowCount;
            ParamShowCount.Direction = ParameterDirection.Input;
            ParamScript.Value = Script;
            ParamScript.Direction = ParameterDirection.Input;
            ParamCreatedUserId.Value = CreatedUserId;
            ParamCreatedUserId.Direction = ParameterDirection.Input;


            if (CreatedDate < DateTime.Parse("1-1-2000"))
            {
                ParamCreatedDate.Value = DBNull.Value;
            }
            else
            {
                ParamCreatedDate.Value = CreatedDate;
            }
            ParamCreatedDate.Direction = ParameterDirection.Input;


            if (ModifiedDate < DateTime.Parse("1-1-2000"))
            {
                ParamModifiedDate.Value = DBNull.Value;
            }
            else
            {
                ParamModifiedDate.Value = ModifiedDate;
            }
            ParamModifiedDate.Direction = ParameterDirection.Input;

            CmdSent = Cmd;
            return true;
        }


        public bool ManageDomain(out string DomId)
        {
            SqlCommand CmdExecute = new SqlCommand();
            DomId = string.Empty;
            if (SetCommandDomain(ref CmdExecute))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                {
                    using (CmdExecute)
                    {
                        conn.Open();
                        CmdExecute.Connection = conn;
                        SqlDataReader sqlReader = CmdExecute.ExecuteReader();
                        while (sqlReader.Read())
                        {
                            DomId = sqlReader[0].ToString();
                        }

                    }
                }
            }
            return true;
        }


        public List<Domain> GetDomains(string search, string sort, string sortdir, string userId)
        {
            List<Domain> lstDomains = new List<Domain>();
            Domain _Domain = new Domain();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwDomains where DomainCreatedUser = " + userId, conn))
                {
                    sda.Fill(dt);
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Domain.Id = double.Parse(dt.Rows[i]["DomainId"].ToString());
                _Domain.Name = dt.Rows[i]["DomainName"].ToString();
                _Domain.Script = dt.Rows[i]["Script"].ToString();
                _Domain.Height = dt.Rows[i]["Height"].ToString();
                _Domain.Width = dt.Rows[i]["Width"].ToString();
                _Domain.ShowCount = dt.Rows[i]["ShowCount"].ToString();
                lstDomains.Add(_Domain);
                _Domain = new Domain();
            }

            return lstDomains;
       }


        public List<Domain> GetDomains(string userId, string domainId)
        {
            List<Domain> lstDomains = new List<Domain>();
            Domain _Domain = new Domain();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("select * from VwDomains where DomainCreatedUser = " + userId + " and DomainId = " + domainId, conn))
                {
                    sda.Fill(dt);
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Domain.Id = double.Parse(dt.Rows[i]["DomainId"].ToString());
                _Domain.Name = dt.Rows[i]["DomainName"].ToString();
                _Domain.Script = dt.Rows[i]["Script"].ToString();
                _Domain.Height = dt.Rows[i]["Height"].ToString();
                _Domain.Width = dt.Rows[i]["Width"].ToString();
                _Domain.ShowCount = dt.Rows[i]["ShowCount"].ToString();
                lstDomains.Add(_Domain);
                _Domain = new Domain();
            }

            return lstDomains;
        }

        public List<Domain> GetDomainScript(string userId, string ddDomains)
        {
            List<Domain> lstDomains = new List<Domain>();
            Domain _Domain = new Domain();
            DataTable dt = new DataTable();
            string strQuery = "Select * from VwDomains where DomainCreatedUser = " + userId;
            if(!string.IsNullOrEmpty(ddDomains))
            {
                strQuery += " and DomainId = " + ddDomains + "";
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(strQuery, conn))
                {
                    sda.Fill(dt);
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Domain.Id = double.Parse(dt.Rows[i]["DomainId"].ToString());
                _Domain.Name = dt.Rows[i]["DomainName"].ToString();
                _Domain.Script = dt.Rows[i]["Script"].ToString();
                _Domain.Height = dt.Rows[i]["Height"].ToString();
                _Domain.Width = dt.Rows[i]["Width"].ToString();
                _Domain.ShowCount = dt.Rows[i]["ShowCount"].ToString();
                lstDomains.Add(_Domain);
                _Domain = new Domain();
            }
            
            return lstDomains;
        }

    }
}



