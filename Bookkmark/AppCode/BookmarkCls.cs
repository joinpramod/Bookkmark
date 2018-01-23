using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Bookmark
{
    public class BookmarkCls
    {
     
        public int OptID { get; set; }
        public double Id { get; set; }
        public string URL { get; set; }
        public double FolderId { get; set; }
        public bool IsFolder { get; set; }
        public string Name { get; set; }
        public string IpAddr { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Count { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedUserId { get; set; }
        public string ModifiedDate { get; set; }

        public bool SetCommandBookmark(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Bookmarks_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamId = Cmd.Parameters.Add("@Id", SqlDbType.Float);
            SqlParameter ParamURL = Cmd.Parameters.Add("@URL", SqlDbType.VarChar);
            SqlParameter ParamCategoryId = Cmd.Parameters.Add("@FolderId", SqlDbType.Float);
            SqlParameter ParamIsFolder = Cmd.Parameters.Add("@IsFolder", SqlDbType.Bit);
            SqlParameter ParamName = Cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            SqlParameter ParamIpAddr = Cmd.Parameters.Add("@IpAddr", SqlDbType.VarChar);
            SqlParameter ParamCity = Cmd.Parameters.Add("@City", SqlDbType.VarChar);
            SqlParameter ParamCountry = Cmd.Parameters.Add("@Country", SqlDbType.VarChar);
            SqlParameter ParamCreatedDate = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamCreatedUserId = Cmd.Parameters.Add("@CreatedUser", SqlDbType.VarChar);
            SqlParameter ParamModifiedDate = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);


            ParamOptID.Value = OptID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamId.Value = Id;
            ParamId.Direction = ParameterDirection.Input;
            ParamURL.Value = URL;
            ParamURL.Direction = ParameterDirection.Input;
            ParamCategoryId.Value = FolderId;
            ParamCategoryId.Direction = ParameterDirection.Input;
            ParamIsFolder.Value = IsFolder;
            ParamIsFolder.Direction = ParameterDirection.Input;
            ParamName.Value = Name;
            ParamName.Direction = ParameterDirection.Input;
            ParamIpAddr.Value = IpAddr;
            ParamIpAddr.Direction = ParameterDirection.Input;
            ParamCity.Value = City;
            ParamCity.Direction = ParameterDirection.Input;
            ParamCountry.Value = Country;
            ParamCountry.Direction = ParameterDirection.Input;
            ParamCreatedUserId.Value = CreatedUserId;
            ParamCreatedUserId.Direction = ParameterDirection.Input;
            ParamCreatedDate.Value = CreatedDate;
            ParamCreatedDate.Direction = ParameterDirection.Input;
            ParamModifiedDate.Value = ModifiedDate;
            ParamModifiedDate.Direction = ParameterDirection.Input;

            CmdSent = Cmd;
            return true;
        }


        public bool ManageBookmark()
        {
            SqlCommand CmdExecute =  new SqlCommand();
            if (SetCommandBookmark(ref CmdExecute))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                {
                    using (CmdExecute)
                    {
                        conn.Open();
                        CmdExecute.Connection = conn;
                        CmdExecute.ExecuteReader();
                    }
                }
            }
            return true;
        }


        public bool ManageBookmark(out string bmrkID)
        {
            SqlCommand CmdExecute = new SqlCommand();
            bmrkID = string.Empty;
            if (SetCommandBookmark(ref CmdExecute))
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
                            bmrkID = sqlReader[0].ToString();
                        }
                    }
                }
            }
            return true;
        }


        public string GetBookmarkCount(string strlnk)
        {
            string bmrkCount = string.Empty;
            DataTable dtUserExists = new DataTable();
            ConnManager connManager = new ConnManager();
            dtUserExists = connManager.GetDataTable("Select * from Bookmarks where url  = '" + strlnk + "'");
            if (dtUserExists.Rows.Count > 0)
            {
                bmrkCount = dtUserExists.Rows.Count.ToString();
            }
            dtUserExists.Dispose();
            return bmrkCount;
        }


        public bool BookmarkExists(string _userId)
        {
            DataTable dtBmrkExists = new DataTable();
            ConnManager connManager = new ConnManager();
            dtBmrkExists = connManager.GetDataTable("Select * from VwUserBookmarks where URL= '"+ URL + "' and  CreatedUser = " + _userId + "");
            if (dtBmrkExists!= null && dtBmrkExists.Rows.Count > 0)
            {
                Id = double.Parse(dtBmrkExists.Rows[0]["Id"].ToString());
                URL = dtBmrkExists.Rows[0]["URL"].ToString();
                FolderId  = double.Parse(dtBmrkExists.Rows[0]["FolderId"].ToString());
                Name = dtBmrkExists.Rows[0]["Name"].ToString();
                CreatedUserId = dtBmrkExists.Rows[0]["CreatedUser"].ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditBookmark(string _userId)
        {
            DataTable dtBmrkExists = new DataTable();
            ConnManager connManager = new ConnManager();
            dtBmrkExists = connManager.GetDataTable("Select * from VwUserBookmarks where URL= '" + URL + "' and  UserId = " + _userId + "");
            if (dtBmrkExists != null && dtBmrkExists.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool QuickInsertBookmark(string _userId)
        {
            DataTable dtBmrkExists = new DataTable();
            ConnManager connManager = new ConnManager();
            dtBmrkExists = connManager.GetDataTable("Select * from VwUserBookmarks where URL= '" + URL + "' and  UserId = " + _userId + "");
            if (dtBmrkExists != null && dtBmrkExists.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public List<BookmarkCls> GetFolders(string search, string sort, string sortdir, string userId)
        {
            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
           BookmarkCls _Bookmark = new BookmarkCls();

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwUserBookmarks where IsFolder = 1 and CreatedUser = " + userId + "", conn))
                {
                    sda.Fill(dt);
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
                _Bookmark.Name = dt.Rows[i]["Name"].ToString();
                _Bookmark.URL = dt.Rows[i]["URL"].ToString();
                _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
                _Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
                _Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
                _Bookmark.City = dt.Rows[i]["City"].ToString();
                _Bookmark.Country = dt.Rows[i]["Country"].ToString();
                _Bookmark.CreatedDate = dt.Rows[i]["CreatedDateTime"].ToString();
                lstBookmarks.Add(_Bookmark);
                _Bookmark = new BookmarkCls();
            }            

            return lstBookmarks;
        }


        public List<BookmarkCls> GetBookmarks(string search, string sort, string sortdir, string userId, string txtCreatedFrom, string txtCreatedTo)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            string strWhere = string.Empty;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                if (string.IsNullOrEmpty(search))
                { 
                    strWhere = "Select * from VwUserBookmarks where CreatedUser = " + userId + " and id <> 27 order by isfolder desc";
                }
                else
                {
                    strWhere = "Select * from VwUserBookmarks where CreatedUser = " + userId + " and id <> 27 and url like '%" + search + "%' order by isfolder desc";
                }

                using (SqlDataAdapter sda = new SqlDataAdapter(strWhere, conn))
                {
                    sda.Fill(dt);
                }
            }

            _Bookmark.Id = 27;
            _Bookmark.Name = "Default";
            _Bookmark.FolderId = 0;
            _Bookmark.IsFolder = true;
            lstBookmarks.Add(_Bookmark);
            _Bookmark = new BookmarkCls();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
                _Bookmark.Name = dt.Rows[i]["Name"].ToString();
                _Bookmark.URL = dt.Rows[i]["URL"].ToString();
                _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
                _Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
                _Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
                _Bookmark.City = dt.Rows[i]["City"].ToString();
                _Bookmark.Country = dt.Rows[i]["Country"].ToString();
                _Bookmark.CreatedDate = dt.Rows[i]["CreatedDateTime"].ToString();
                lstBookmarks.Add(_Bookmark);
                _Bookmark = new BookmarkCls();
            }

            return lstBookmarks;
        }

        public List<BookmarkCls> GetReports(string search, string sort, string sortdir, string userId, string txtCreatedFrom, string txtCreatedTo, string ddDomains, out string total)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.Connection = conn;
                com.CommandText = "Reports_Sp";
                com.Parameters.Add("DomainCreatedUser", SqlDbType.Decimal).Value = userId;
                com.Parameters.Add("@DomainId", SqlDbType.Decimal).Value = ddDomains;
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    sda.Fill(dt);
                }
            }
            total = "0";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
                //_Bookmark.Name = dt.Rows[i]["Name"].ToString();
                _Bookmark.URL = dt.Rows[i]["URL"].ToString();
                _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
                //_Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
                //_Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
                _Bookmark.City = dt.Rows[i]["City"].ToString();
                _Bookmark.Count = dt.Rows[i]["URLCount"].ToString();
                _Bookmark.Country = dt.Rows[i]["Country"].ToString();
                _Bookmark.CreatedDate = dt.Rows[i]["CreatedDateTime"].ToString();
                lstBookmarks.Add(_Bookmark);
                _Bookmark = new BookmarkCls();
                total = dt.Rows[i]["Total_Count"].ToString();
            }

            return lstBookmarks;
        }


        public List<BookmarkCls> GetCharts(string search, string sort, string sortdir, string userId, string txtCreatedFrom, string txtCreatedTo, string ddDomains, out string total, string optId)
        {
            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.Connection = conn;
                com.CommandText = "Charts_Sp";
                com.Parameters.Add("DomainCreatedUser", SqlDbType.Decimal).Value = userId;
                com.Parameters.Add("@DomainId", SqlDbType.Decimal).Value = ddDomains;
                com.Parameters.Add("@OptId", SqlDbType.Int).Value = int.Parse(optId);
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    sda.Fill(dt);
                }
            }
            total = "0";

            if (int.Parse(optId) == 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _Bookmark.URL = dt.Rows[i]["URL"].ToString();
                    _Bookmark.Count = dt.Rows[i]["URLCount"].ToString();
                    lstBookmarks.Add(_Bookmark);
                    _Bookmark = new BookmarkCls();
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _Bookmark.City = dt.Rows[i]["City"].ToString();
                    _Bookmark.Count = dt.Rows[i]["CityCount"].ToString();
                    lstBookmarks.Add(_Bookmark);
                    _Bookmark = new BookmarkCls();
                }
            }

            return lstBookmarks;
        }

        public List<BookmarkCls> GetReports(string strQuery, out string total)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.Text;
                com.Connection = conn;
                com.CommandText = strQuery;
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    sda.Fill(dt);
                }
            }
            total = "0";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
                //_Bookmark.Name = dt.Rows[i]["Name"].ToString();
                _Bookmark.URL = dt.Rows[i]["URL"].ToString();
                _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
                _Bookmark.Count = dt.Rows[i]["URLCount"].ToString();
                //_Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
                //_Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
                _Bookmark.City = dt.Rows[i]["City"].ToString();
                _Bookmark.Country = dt.Rows[i]["Country"].ToString();
                _Bookmark.CreatedDate = dt.Rows[i]["CreatedDateTime"].ToString();
                lstBookmarks.Add(_Bookmark);
                _Bookmark = new BookmarkCls();
                total = dt.Rows[i]["Total_Count"].ToString();

            }

            return lstBookmarks;
        }

        public List<BookmarkCls> GetURLReports(string strQuery, out string total)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.Text;
                com.Connection = conn;
                com.CommandText = strQuery;
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    sda.Fill(dt);
                }
            }
            total = "0";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.URL = dt.Rows[i]["URL"].ToString(); 
                _Bookmark.Count = dt.Rows[i]["URLCount"].ToString();
                lstBookmarks.Add(_Bookmark);
                _Bookmark = new BookmarkCls();

            }

            return lstBookmarks;
        }

        public List<BookmarkCls> GetCityReports(string strQuery, out string total)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.CommandType = CommandType.Text;
                com.Connection = conn;
                com.CommandText = strQuery;
                using (SqlDataAdapter sda = new SqlDataAdapter(com))
                {
                    sda.Fill(dt);
                }
            }
            total = "0";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.Count = dt.Rows[i]["CityCount"].ToString();
                _Bookmark.City = dt.Rows[i]["City"].ToString();
                lstBookmarks.Add(_Bookmark);
                _Bookmark = new BookmarkCls();
            }

            return lstBookmarks;
        }


        public BookmarkCls GetBookmarkFromId(string bmrkId, double userId)
        {
            BookmarkCls _Bookmark = new BookmarkCls();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwUserBookmarks where Id = " + bmrkId + " and CreatedUser = " + userId.ToString() + "", conn))
                {
                    sda.Fill(dt);
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
                _Bookmark.Name = dt.Rows[i]["Name"].ToString();
                _Bookmark.URL = dt.Rows[i]["URL"].ToString();
                _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
                _Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
                _Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
                _Bookmark.City = dt.Rows[i]["City"].ToString();
                _Bookmark.Country = dt.Rows[i]["Country"].ToString();
                _Bookmark.CreatedDate = dt.Rows[i]["CreatedDateTime"].ToString();
            }

            return _Bookmark;
        }




    }
}



