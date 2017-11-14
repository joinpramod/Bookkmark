using System;
using System.Data;
using System.Data.SqlClient;


namespace Bookkmark
{
    public class BookkmarkCls : ConnManager
    {
        private SqlConnection CmdLCLDBConn;
        private SqlCommand CmdExecute;
        public SqlConnection SetConnection
        {
            get
            {
                return CmdLCLDBConn;
            }
            set
            {
                CmdLCLDBConn = value;
            }
        }
        public int OptID { get; set; }
        public double Id { get; set; }
        public string URL { get; set; }
        public double FolderId { get; set; }
        public string Name { get; set; }
        public string IpAddr { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime ModifiedDate { get; set; }


        public bool SetCommandBookkmark(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Bookkmark_Sp", CmdLCLDBConn);
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


        public bool CreateBookkmark(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            if (SetCommandBookkmark(ref CmdExecute))
            {
                try
                {
                    if (TrTransaction != null)
                    {
                        CmdExecute.Transaction = TrTransaction;
                    }
                    SqlDataReader DATReader = CmdExecute.ExecuteReader();
                    while (DATReader.Read())
                    {
                        NewMasterID = double.Parse(DATReader[0].ToString());
                    }
                    DATReader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

    }
}



