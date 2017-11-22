using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Bookkmark
{
    public class Bookkmark
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
        public bool SetCommandBookkmark(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Bookkmark_Sp");
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


        public bool ManageBookkmark()
        {
            SqlCommand CmdExecute =  new SqlCommand();
            if (SetCommandBookkmark(ref CmdExecute))
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


        public string GetBookkmarkCount(string strlnk)
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

        public bool EditBookkmark(string _userId)
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

        public bool QuickInsertBookkmark(string _userId)
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


        public List<Bookkmark> GetFolders(string search, string sort, string sortdir)
        {
            List<Bookkmark> lstBookkmarks = new List<Bookkmark>();
           Bookkmark _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1;
            _Bookkmark.Name = "Default";
            _Bookkmark.URL = "Forums";
            _Bookkmark.FolderId = 0;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New York";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookkmark.Count = "12";
            lstBookkmarks.Add(_Bookkmark);

            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 102;
            _Bookkmark.Name = "Forums";
            _Bookkmark.URL = "Forums";
            _Bookkmark.FolderId = 1;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "San Francisco";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "10/7/2017 12:24 PM";
            _Bookkmark.Count = "9";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 103;
            _Bookkmark.Name = "News";
            _Bookkmark.URL = "News";
            _Bookkmark.FolderId = 1;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Chicago";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "09/21/2017 10:24 AM";
            _Bookkmark.Count = "20";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 104;
            _Bookkmark.Name = "MyFolder";
            _Bookkmark.URL = "MyFolder";
            _Bookkmark.FolderId = 1;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Texas";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "08/20/2017 10:24 AM";
            _Bookkmark.Count = "4";
            lstBookkmarks.Add(_Bookkmark);

            return lstBookkmarks;
        }


        public List<Bookkmark> GetBookkmarks(string search, string sort, string sortdir, string userId)
        {

            List<Bookkmark> lstBookkmarks = new List<Bookkmark>();
            Bookkmark _Bookkmark = new Bookkmark();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter("Select * from VwBookmarks where CreatedUserId = " + userId + "", conn))
                {
                    sda.Fill(dt);
                }
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _Bookkmark.Id = double.Parse(dt.Rows[i]["Id"].ToString());
                _Bookkmark.Name = dt.Rows[i]["Name"].ToString();
                _Bookkmark.URL = dt.Rows[i]["URL"].ToString();
                _Bookkmark.FolderId = double.Parse(dt.Rows[i]["FolderId"].ToString());
                _Bookkmark.IsFolder = bool.Parse(dt.Rows[i]["IsFolder"].ToString());
                _Bookkmark.IpAddr = dt.Rows[i]["IpAddr"].ToString();
                _Bookkmark.City = dt.Rows[i]["City"].ToString();
                _Bookkmark.Country = dt.Rows[i]["Country"].ToString();
                _Bookkmark.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
                lstBookkmarks.Add(_Bookkmark);
                _Bookkmark = new Bookkmark();
            }


            _Bookkmark.Id = 1;
            _Bookkmark.Name = "Default";
            _Bookkmark.URL = "";
            _Bookkmark.FolderId = 0;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New York";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookkmark.Count = "12";
            lstBookkmarks.Add(_Bookkmark);

            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 102;
            _Bookkmark.Name = "Social";
            _Bookkmark.URL = "";
            _Bookkmark.FolderId = 1;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "San Francisco";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "10/7/2017 12:24 PM";
            _Bookkmark.Count = "9";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 103;
            _Bookkmark.Name = "General";
            _Bookkmark.URL = "";
            _Bookkmark.FolderId = 1;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Chicago";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "09/21/2017 10:24 AM";
            _Bookkmark.Count = "20";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 104;
            _Bookkmark.Name = "MyFolder";
            _Bookkmark.URL = "";
            _Bookkmark.FolderId = 1;
            _Bookkmark.IsFolder = true;
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Texas";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "08/20/2017 10:24 AM";
            _Bookkmark.Count = "4";
            lstBookkmarks.Add(_Bookkmark);


            //Forums
            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1001;
            _Bookkmark.Name = "Facebook";
            _Bookkmark.FolderId = 102;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.facebook.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Bengaluru";
            _Bookkmark.Country = "India";
            _Bookkmark.CreatedDate = "07/25/2017 10:24 AM";
            _Bookkmark.Count = "21";
            lstBookkmarks.Add(_Bookkmark);



            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1003;
            _Bookkmark.Name = "Stats - Quora";
            _Bookkmark.FolderId = 102;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.quora.com/stats";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Los Angeles";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "05/02/2017 10:24 AM";
            _Bookkmark.Count = "15";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1005;
            _Bookkmark.Name = "LinkedIn";
            _Bookkmark.FolderId = 102;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.linkedin.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "North Carolina";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "03/05/2017 10:24 AM";
            _Bookkmark.Count = "24";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1006;
            _Bookkmark.Name = "Twitter";
            _Bookkmark.FolderId = 102;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.twitter.com";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Vegas";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "02/10/2017 10:24 AM";
            _Bookkmark.Count = "28";
            lstBookkmarks.Add(_Bookkmark);

            //CDS
            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1009;
            _Bookkmark.Name = "Google Plus";
            _Bookkmark.FolderId = 102;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://plus.google.com/discover";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New York";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookkmark.Count = "27";
            lstBookkmarks.Add(_Bookkmark);


            //News
            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1004;
            _Bookkmark.Name = "Wikipedia";
            _Bookkmark.FolderId = 103;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.wikipedia.org/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Florida";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "04/21/2017 10:24 AM";
            _Bookkmark.Count = "18";
            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1008;
            _Bookkmark.Name = "Google";
            _Bookkmark.FolderId = 103;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.google.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New York";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookkmark.Count = "17";
            lstBookkmarks.Add(_Bookkmark);

            //My Folder
            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1007;
            _Bookkmark.Name = "Yahoo";
            _Bookkmark.FolderId = 103;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.yahoo.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "Boston";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "01/12/2017 10:24 AM";
            _Bookkmark.Count = "31";
            lstBookkmarks.Add(_Bookkmark);



            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1008;
            _Bookkmark.Name = "Bing";
            _Bookkmark.FolderId = 103;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "http://www.bing.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New York";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookkmark.Count = "17";
            lstBookkmarks.Add(_Bookkmark);




            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1002;
            _Bookkmark.Name = "Amazon";
            _Bookkmark.FolderId = 104;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "https://www.amazon.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New Jersey";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "06/11/2017 10:24 AM";
            _Bookkmark.Count = "19";

            lstBookkmarks.Add(_Bookkmark);


            _Bookkmark = new Bookkmark();
            _Bookkmark.Id = 1010;
            _Bookkmark.Name = "ESPN";
            _Bookkmark.FolderId = 104;
            _Bookkmark.IsFolder = false;
            _Bookkmark.URL = "http://www.espn.com/";
            _Bookkmark.IpAddr = "121.0.0.1";
            _Bookkmark.City = "New York";
            _Bookkmark.Country = "USA";
            _Bookkmark.CreatedDate = "11/8/2017 10:24 AM";
            _Bookkmark.Count = "25";
            lstBookkmarks.Add(_Bookkmark);
            return lstBookkmarks;
        }



    }
}



