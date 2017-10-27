using System.Data.SqlClient;
using System.Data;
using System;

namespace Bookkmark
{
    public class Users : ConnManager
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
        public int OptionID { get; set; }
        public double UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsPublisher { get; set; }
        public Domain DomainName { get; set; }
        public string ScriptCode { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }

        public string ImageURL { get; set; }
        public string Company { get; set; }
        public string Details { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }

        public bool SetCommandUser(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("User_Sp", CmdLCLDBConn);
            Cmd.CommandType = CommandType.StoredProcedure;

　
            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamUserId = Cmd.Parameters.Add("@UserId", SqlDbType.Float);
            SqlParameter ParamFirstName = Cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
            SqlParameter ParamLastName = Cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
            SqlParameter ParamPassword = Cmd.Parameters.Add("@Password", SqlDbType.VarChar);
            SqlParameter ParamEmail = Cmd.Parameters.Add("@Email", SqlDbType.VarChar);
            SqlParameter ParamIsPublisher = Cmd.Parameters.Add("@IsPublisher", SqlDbType.Bit);
            SqlParameter ParamDomainName = Cmd.Parameters.Add("@DomainName", SqlDbType.VarChar);
            SqlParameter ParamScriptCode = Cmd.Parameters.Add("@ScriptCode", SqlDbType.VarChar);

            //SqlParameter ParamImageURL = Cmd.Parameters.Add("@ImageURL", SqlDbType.VarChar);
            //SqlParameter ParamCompany = Cmd.Parameters.Add("@Company", SqlDbType.VarChar);
            //SqlParameter ParamDetails = Cmd.Parameters.Add("@Details", SqlDbType.VarChar);
            //SqlParameter ParamAddress = Cmd.Parameters.Add("@Address", SqlDbType.VarChar);
            //SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = OptionID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamUserId.Value = UserId;
            ParamUserId.Direction = ParameterDirection.Input;
            ParamFirstName.Value = FirstName;
            ParamFirstName.Direction = ParameterDirection.Input;
            ParamLastName.Value = LastName;
            ParamLastName.Direction = ParameterDirection.Input;
            ParamPassword.Value = Password;
            ParamPassword.Direction = ParameterDirection.Input;
            ParamEmail.Value = Email;
            ParamEmail.Direction = ParameterDirection.Input;

            ParamIsPublisher.Value = IsPublisher;
            ParamIsPublisher.Direction = ParameterDirection.Input;
            ParamDomainName.Value = DomainName;
            ParamDomainName.Direction = ParameterDirection.Input;
            ParamScriptCode.Value = ScriptCode;
            ParamScriptCode.Direction = ParameterDirection.Input;
            //ParamImageURL.Value = StrImageURL;
            //ParamImageURL.Direction = ParameterDirection.Input;
            //ParamCompany.Value = StrCompany;
            //ParamCompany.Direction = ParameterDirection.Input;
            //ParamDetails.Value = StrDetails;
            //ParamDetails.Direction = ParameterDirection.Input;
            //ParamStatus.Value = StrStatus;
            //ParamStatus.Direction = ParameterDirection.Input;
            //ParamAddress.Value = StrAddress;
            //ParamAddress.Direction = ParameterDirection.Input;

            if (CreatedDateTime < DateTime.Parse("1-1-2000"))
            {
                ParamCreatedDateTime.Value = DBNull.Value;
            }
            else
            {
                ParamCreatedDateTime.Value = CreatedDateTime;
            }
            ParamCreatedDateTime.Direction = ParameterDirection.Input;

　
            if (ModifiedDateTime < DateTime.Parse("1-1-2000"))
            {
                ParamModifiedDateTime.Value = DBNull.Value;
            }
            else
            {
                ParamModifiedDateTime.Value = ModifiedDateTime;
            }
            ParamModifiedDateTime.Direction = ParameterDirection.Input;

            CmdSent = Cmd;
            return true;
        }

        public bool CreateUsers(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            if (SetCommandUser(ref CmdExecute))
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
            return true;
        }

        public Users CreateUsers(string strEmail, string strFirstName, string strLastName)
        {
            return CreateUser(strEmail, strFirstName, strLastName, null);
        }

        public Users CreateUser(string strEmail, string strFirstName, string strLastName, string strImageURL)
        {
            Users user = new Users();
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
                user.Email = strEmail.Trim();
                user.FirstName = strFirstName.Trim();
                user.LastName = strLastName.Trim();
                //user.ImageURL = strImageURL.Trim();
                user.OptionID = 1;
                user.CreatedDateTime = DateTime.Now;
                bool result = user.CreateUsers(ref dblUserID, SetTransaction);
                if (IsinTransaction && result)
                {
                    SetTransaction.Commit();
                    user.Id = dblUserID;
                }
                else
                {
                    SetTransaction.Rollback();
                }
                user.CloseConnection(LclConn);
            return user;
        }

        public bool UserExists(string strEmail, ref double _userId)
        {
            ConnManager connManager = new ConnManager();
            connManager.OpenConnection();
            DataSet dsUserExists = connManager.GetData("Select * from Users where EMail = '" + strEmail + "'");
            connManager.DisposeConn();
            if (dsUserExists.Tables[0].Rows.Count > 0)
            {
                _userId = double.Parse(dsUserExists.Tables[0].Rows[0]["Userid"].ToString());
                return true;
            }
            else
                return false;

        }

    }
}
