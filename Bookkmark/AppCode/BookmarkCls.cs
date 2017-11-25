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
            SqlCommand Cmd = new SqlCommand("Bookmark_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamId = Cmd.Parameters.Add("@Id", SqlDbType.Float);
            SqlParameter ParamURL = Cmd.Parameters.Add("@URL", SqlDbType.VarChar);
            SqlParameter ParamCategoryId = Cmd.Parameters.Add("@FolderId", SqlDbType.Int);
            SqlParameter ParamName = Cmd.Parameters.Add("@Name", SqlDbType.VarChar);
            SqlParameter ParamIpAddr = Cmd.Parameters.Add("@IpAddr", SqlDbType.VarChar);
            SqlParameter ParamCity = Cmd.Parameters.Add("@City", SqlDbType.VarChar);
            SqlParameter ParamCountry = Cmd.Parameters.Add("@Country", SqlDbType.Float);
            SqlParameter ParamCreatedDate = Cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
            SqlParameter ParamCreatedUserId = Cmd.Parameters.Add("@CreatedUserId", SqlDbType.Int);
            SqlParameter ParamModifiedDate = Cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime);


            ParamOptID.Value = OptID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamId.Value = Id;
            ParamId.Direction = ParameterDirection.Input;
            ParamURL.Value = URL;
            ParamURL.Direction = ParameterDirection.Input;
            ParamCategoryId.Value = FolderId;
            ParamCategoryId.Direction = ParameterDirection.Input;
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


        public string GetBookmarkCount(string strlnk)
        {
            string bmrkCount = string.Empty;
            DataTable dtUserExists = new DataTable();
            ConnManager connManager = new ConnManager();
            dtUserExists = connManager.GetDataTable("Select * from Bookmark where url  = '" + strlnk + "'");
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
            dtBmrkExists = connManager.GetDataTable("Select * from VwBookmarks where URL= '"+ URL + "' and  UserId = " + _userId + "");
            if (dtBmrkExists!= null && dtBmrkExists.Rows.Count > 0)
            {
                URL = dtBmrkExists.Rows[0]["URL"].ToString();
                FolderId  = double.Parse(dtBmrkExists.Rows[0]["FolderId"].ToString());
                Name = dtBmrkExists.Rows[0]["Name"].ToString();
                CreatedUserId = dtBmrkExists.Rows[0]["CreatedUserId"].ToString();
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
            dtBmrkExists = connManager.GetDataTable("Select * from VwBookmarks where URL= '" + URL + "' and  UserId = " + _userId + "");
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
            dtBmrkExists = connManager.GetDataTable("Select * from VwBookmarks where URL= '" + URL + "' and  UserId = " + _userId + "");
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

            //DataTable dt = new DataTable();
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            //{
            //    conn.Open();
            //    using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwBookmarks where IsFolder = 1 and CreatedUserId = " + userId + "", conn))
            //    {
            //        sda.Fill(dt);
            //    }
            //}

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
            //    _Bookmark.Name = dt.Rows[i]["Name"].ToString();
            //    _Bookmark.URL = dt.Rows[i]["URL"].ToString();
            //    _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
            //    _Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
            //    _Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
            //    _Bookmark.City = dt.Rows[i]["City"].ToString();
            //    _Bookmark.Country = dt.Rows[i]["Country"].ToString();
            //    _Bookmark.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
            //    lstBookmarks.Add(_Bookmark);
            //    _Bookmark = new BookmarkCls();
            //}


            _Bookmark.Id = 1;
            _Bookmark.Name = "Default";
            _Bookmark.URL = "Forums";
            _Bookmark.FolderId = 0;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New York";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookmark.Count = "12";
            lstBookmarks.Add(_Bookmark);

            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 102;
            _Bookmark.Name = "Forums";
            _Bookmark.URL = "Forums";
            _Bookmark.FolderId = 1;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "San Francisco";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "10/7/2017 12:24 PM";
            _Bookmark.Count = "9";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 103;
            _Bookmark.Name = "News";
            _Bookmark.URL = "News";
            _Bookmark.FolderId = 1;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Chicago";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "09/21/2017 10:24 AM";
            _Bookmark.Count = "20";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 104;
            _Bookmark.Name = "MyFolder";
            _Bookmark.URL = "MyFolder";
            _Bookmark.FolderId = 1;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Texas";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "08/20/2017 10:24 AM";
            _Bookmark.Count = "4";
            lstBookmarks.Add(_Bookmark);

            return lstBookmarks;
        }


        public List<BookmarkCls> GetBookmarks(string search, string sort, string sortdir, string userId)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            //DataTable dt = new DataTable();
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            //{
            //    conn.Open();
            //    using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwBookmarks where CreatedUserId = " + userId + "", conn))
            //    {
            //        sda.Fill(dt);
            //    }
            //}

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
            //    _Bookmark.Name = dt.Rows[i]["Name"].ToString();
            //    _Bookmark.URL = dt.Rows[i]["URL"].ToString();
            //    _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
            //    _Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
            //    _Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
            //    _Bookmark.City = dt.Rows[i]["City"].ToString();
            //    _Bookmark.Country = dt.Rows[i]["Country"].ToString();
            //    _Bookmark.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
            //    lstBookmarks.Add(_Bookmark);
            //    _Bookmark = new Bookmark();
            //}


            _Bookmark.Id = 1;
            _Bookmark.Name = "Default";
            _Bookmark.URL = "";
            _Bookmark.FolderId = 0;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New York";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookmark.Count = "12";
            lstBookmarks.Add(_Bookmark);

            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 102;
            _Bookmark.Name = "Social";
            _Bookmark.URL = "";
            _Bookmark.FolderId = 1;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "San Francisco";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "10/7/2017 12:24 PM";
            _Bookmark.Count = "9";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 103;
            _Bookmark.Name = "General";
            _Bookmark.URL = "";
            _Bookmark.FolderId = 1;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Chicago";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "09/21/2017 10:24 AM";
            _Bookmark.Count = "20";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 104;
            _Bookmark.Name = "MyFolder";
            _Bookmark.URL = "";
            _Bookmark.FolderId = 1;
            _Bookmark.IsFolder = true;
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Texas";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "08/20/2017 10:24 AM";
            _Bookmark.Count = "4";
            lstBookmarks.Add(_Bookmark);


            //Forums
            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1001;
            _Bookmark.Name = "Facebook";
            _Bookmark.FolderId = 102;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.facebook.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Bengaluru";
            _Bookmark.Country = "India";
            _Bookmark.CreatedDate = "07/25/2017 10:24 AM";
            _Bookmark.Count = "21";
            lstBookmarks.Add(_Bookmark);



            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1003;
            _Bookmark.Name = "Quora";
            _Bookmark.FolderId = 102;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.quora.com/stats";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Los Angeles";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "05/02/2017 10:24 AM";
            _Bookmark.Count = "15";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1005;
            _Bookmark.Name = "LinkedIn";
            _Bookmark.FolderId = 102;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.linkedin.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "North Carolina";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "03/05/2017 10:24 AM";
            _Bookmark.Count = "24";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1006;
            _Bookmark.Name = "Twitter";
            _Bookmark.FolderId = 102;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.twitter.com";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Vegas";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "02/10/2017 10:24 AM";
            _Bookmark.Count = "28";
            lstBookmarks.Add(_Bookmark);

            //CDS
            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1009;
            _Bookmark.Name = "Google Plus";
            _Bookmark.FolderId = 102;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://plus.google.com/discover";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New York";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookmark.Count = "27";
            lstBookmarks.Add(_Bookmark);


            //News
            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1004;
            _Bookmark.Name = "Wikipedia";
            _Bookmark.FolderId = 103;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.wikipedia.org/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Florida";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "04/21/2017 10:24 AM";
            _Bookmark.Count = "18";
            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1008;
            _Bookmark.Name = "Google";
            _Bookmark.FolderId = 103;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.google.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New York";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookmark.Count = "17";
            lstBookmarks.Add(_Bookmark);

            //My Folder
            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1007;
            _Bookmark.Name = "Yahoo";
            _Bookmark.FolderId = 103;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.yahoo.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Boston";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "01/12/2017 10:24 AM";
            _Bookmark.Count = "31";
            lstBookmarks.Add(_Bookmark);



            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1008;
            _Bookmark.Name = "Bing";
            _Bookmark.FolderId = 103;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "http://www.bing.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New York";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookmark.Count = "17";
            lstBookmarks.Add(_Bookmark);




            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1002;
            _Bookmark.Name = "Amazon";
            _Bookmark.FolderId = 104;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.amazon.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New Jersey";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "06/11/2017 10:24 AM";
            _Bookmark.Count = "19";

            lstBookmarks.Add(_Bookmark);


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1010;
            _Bookmark.Name = "ESPN";
            _Bookmark.FolderId = 104;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "http://www.espn.com/";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "New York";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookmark.Count = "25";
            lstBookmarks.Add(_Bookmark);
            return lstBookmarks;
        }


        public BookmarkCls GetBookmarkFromId(string bmrkId, double userId)
        {

            List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
            BookmarkCls _Bookmark = new BookmarkCls();
            //DataTable dt = new DataTable();
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            //{
            //    conn.Open();
            //    using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwBookmarks where Id = " + bmrkId + " and CreatedUserId = " + userId.ToString() + "", conn))
            //    {
            //        sda.Fill(dt);
            //    }
            //}

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    _Bookmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
            //    _Bookmark.Name = dt.Rows[i]["Name"].ToString();
            //    _Bookmark.URL = dt.Rows[i]["URL"].ToString();
            //    _Bookmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
            //    _Bookmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
            //    _Bookmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
            //    _Bookmark.City = dt.Rows[i]["City"].ToString();
            //    _Bookmark.Country = dt.Rows[i]["Country"].ToString();
            //    _Bookmark.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
            //    lstBookmarks.Add(_Bookmark);
            //    _Bookmark = new Bookmark();
            //}


            _Bookmark = new BookmarkCls();
            _Bookmark.Id = 1003;
            _Bookmark.Name = "Quora";
            _Bookmark.FolderId = 102;
            _Bookmark.IsFolder = false;
            _Bookmark.URL = "https://www.quora.com/stats";
            _Bookmark.IpAddr = "121.0.0.1";
            _Bookmark.City = "Los Angeles";
            _Bookmark.Country = "USA";
            _Bookmark.CreatedDate = "05/02/2017 10:24 AM";
            _Bookmark.Count = "15";
            lstBookmarks.Add(_Bookmark);


            
            return _Bookmark;
        }


    }
}



