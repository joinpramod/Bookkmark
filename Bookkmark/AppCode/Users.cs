using System.Data.SqlClient;
using System.Data;
using System;

namespace Bookkmark
{
    public class Users : ConnManager
    {
        private SqlConnection CmdLCLDBConn;
        private SqlCommand CmdExecute;
        private int IntOptID;
        private double DblUserId;
        private string StrFirstName;
        private string StrLastName;
        private string StrPassword;
        private string StrEmail;
        private string StrImageURL;
        private string StrCompany;
        private string StrDetails;
        private string StrAddress;
        private string StrStatus;
        //private string StrArticlesPosted;
        //private string StrQuestionsPosted;
        //private string StrAnswersPosted;
        private System.DateTime DtCreatedDateTime;
        private System.DateTime DtModifiedDateTime;


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


        public int OptionID
        {
            get
            {
                return IntOptID;
            }
            set
            {
                IntOptID = value;
            }
        }

        public double UserId
        {
            get
            {
                return DblUserId;
            }
            set
            {
                DblUserId = value;
            }
        }

        public string FirstName
        {
            get
            {
                return StrFirstName;
            }
            set
            {
                StrFirstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return StrLastName;
            }
            set
            {
                StrLastName = value;
            }
        }

        public string Password
        {
            get
            {
                return StrPassword;
            }
            set
            {
                StrPassword = value;
            }
        }

        public string Email
        {
            get
            {
                return StrEmail;
            }
            set
            {
                StrEmail = value;
            }
        }

        public string ImageURL
        {
            get
            {
                return StrImageURL;
            }
            set
            {
                StrImageURL = value;
            }
        }

        public string Company
        {
            get
            {
                return StrCompany;
            }
            set
            {
                StrCompany = value;
            }
        }

        public string Details
        {
            get
            {
                return StrDetails;
            }
            set
            {
                StrDetails = value;
            }
        }

        public string Address
        {
            get
            {
                return StrAddress;
            }
            set
            {
                StrAddress = value;
            }
        }

        public string Status
        {
            get
            {
                return StrStatus;
            }
            set
            {
                StrStatus = value;
            }
        }

        public string ArticlesPosted { get; set; }
        public string QuestionsPosted { get; set; }
        public string AnswersPosted { get; set; }


        public System.DateTime CreatedDateTime
        {
            get
            {
                return DtCreatedDateTime;
            }
            set
            {
                DtCreatedDateTime = value;
            }
        }

        public System.DateTime ModifiedDateTime
        {
            get
            {
                return DtModifiedDateTime;
            }
            set
            {
                DtModifiedDateTime = value;
            }
        }

        public bool SetCommandUsers(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Users_Sp", CmdLCLDBConn);
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamUserId = Cmd.Parameters.Add("@UserId", SqlDbType.Float);
            SqlParameter ParamFirstName = Cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
            SqlParameter ParamLastName = Cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
            SqlParameter ParamPassword = Cmd.Parameters.Add("@Password", SqlDbType.VarChar);
            SqlParameter ParamEmail = Cmd.Parameters.Add("@Email", SqlDbType.VarChar);
            SqlParameter ParamImageURL = Cmd.Parameters.Add("@ImageURL", SqlDbType.VarChar);
            SqlParameter ParamCompany = Cmd.Parameters.Add("@Company", SqlDbType.VarChar);
            SqlParameter ParamDetails = Cmd.Parameters.Add("@Details", SqlDbType.VarChar);
            SqlParameter ParamAddress = Cmd.Parameters.Add("@Address", SqlDbType.VarChar);
            SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
            SqlParameter ParamCreatedDateTime = Cmd.Parameters.Add("@CreatedDateTime", SqlDbType.DateTime);
            SqlParameter ParamModifiedDateTime = Cmd.Parameters.Add("@ModifiedDateTime", SqlDbType.DateTime);

            ParamOptID.Value = IntOptID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamUserId.Value = DblUserId;
            ParamUserId.Direction = ParameterDirection.Input;
            ParamFirstName.Value = StrFirstName;
            ParamFirstName.Direction = ParameterDirection.Input;
            ParamLastName.Value = StrLastName;
            ParamLastName.Direction = ParameterDirection.Input;
            ParamPassword.Value = StrPassword;
            ParamPassword.Direction = ParameterDirection.Input;
            ParamEmail.Value = StrEmail;
            ParamEmail.Direction = ParameterDirection.Input;
            ParamImageURL.Value = StrImageURL;
            ParamImageURL.Direction = ParameterDirection.Input;
            ParamCompany.Value = StrCompany;
            ParamCompany.Direction = ParameterDirection.Input;
            ParamDetails.Value = StrDetails;
            ParamDetails.Direction = ParameterDirection.Input;
            ParamStatus.Value = StrStatus;
            ParamStatus.Direction = ParameterDirection.Input;
            ParamAddress.Value = StrAddress;
            ParamAddress.Direction = ParameterDirection.Input;




            if (DtCreatedDateTime < DateTime.Parse("1-1-2000"))
            {
                ParamCreatedDateTime.Value = DBNull.Value;
            }
            else
            {
                ParamCreatedDateTime.Value = DtCreatedDateTime;
            }
            ParamCreatedDateTime.Direction = ParameterDirection.Input;


            if (DtModifiedDateTime < DateTime.Parse("1-1-2000"))
            {
                ParamModifiedDateTime.Value = DBNull.Value;
            }
            else
            {
                ParamModifiedDateTime.Value = DtModifiedDateTime;
            }
            ParamModifiedDateTime.Direction = ParameterDirection.Input;

            CmdSent = Cmd;
            return true;
        }

        public bool CreateUsers(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            if (SetCommandUsers(ref CmdExecute))
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

        public Users CreateUser(string strEmail, string strFirstName, string strLastName)
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
                user.ImageURL = strImageURL.Trim();
                user.OptionID = 1;
                user.CreatedDateTime = DateTime.Now;
                bool result = user.CreateUsers(ref dblUserID, SetTransaction);
                if (IsinTransaction && result)
                {
                    SetTransaction.Commit();
                    user.UserId = dblUserID;
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

