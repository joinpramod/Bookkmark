using System;
using System.Data;
using System.Data.SqlClient;


namespace Bookkmark
{
    public class Publisher : ConnManager
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
        private int OptID { get; set; }
        private double Id { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Password { get; set; }
        private string Email { get; set; }
        private string DomainName { get; set; }
        private string ScriptCode { get; set; }
        public string CreatedUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }

        public bool SetCommandPublisher(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Publisher_Sp", CmdLCLDBConn);
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamPublisherId = Cmd.Parameters.Add("@PublisherId", SqlDbType.Float);
            SqlParameter ParamFirstName = Cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
            SqlParameter ParamLastName = Cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
            SqlParameter ParamPassword = Cmd.Parameters.Add("@Password", SqlDbType.VarChar);
            SqlParameter ParamEmail = Cmd.Parameters.Add("@Email", SqlDbType.VarChar);
            SqlParameter ParamDomainName = Cmd.Parameters.Add("@DomainName", SqlDbType.Int);
            SqlParameter ParamScriptCode = Cmd.Parameters.Add("@ScriptCode", SqlDbType.VarChar);
            SqlParameter ParamCreatedDate = Cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);
            SqlParameter ParamCreatedUserId = Cmd.Parameters.Add("@CreatedUserId", SqlDbType.Int);
            SqlParameter ParamModifiedDate = Cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime);

            ParamOptID.Value = OptID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamPublisherId.Value = Id;
            ParamPublisherId.Direction = ParameterDirection.Input;
            ParamFirstName.Value = FirstName;
            ParamFirstName.Direction = ParameterDirection.Input;
            ParamLastName.Value = LastName;
            ParamLastName.Direction = ParameterDirection.Input;
            ParamPassword.Value = Password;
            ParamPassword.Direction = ParameterDirection.Input;
            ParamEmail.Value = Email;
            ParamEmail.Direction = ParameterDirection.Input;
            ParamDomainName.Value = DomainName;
            ParamDomainName.Direction = ParameterDirection.Input;
            ParamScriptCode.Value = ScriptCode;
            ParamScriptCode.Direction = ParameterDirection.Input;

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

        public bool CreatePublisher(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            if (SetCommandPublisher(ref CmdExecute))
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

        public Publisher CreatePublisher(string strEmail, string strFirstName, string strLastName)
        {
            return CreatePublisher(strEmail, strFirstName, strLastName, null);
        }

        public Publisher CreatePublisher(string strEmail, string strFirstName, string strLastName, string strImageURL)
        {
            Publisher publisher = new Publisher();
            double dblPublisherID = 0;

            SqlConnection LclConn = new SqlConnection();
            SqlTransaction SetTransaction = null;
            bool IsinTransaction = false;
            if (LclConn.State != ConnectionState.Open)
            {
                publisher.SetConnection = publisher.OpenConnection(LclConn);
                SetTransaction = LclConn.BeginTransaction(IsolationLevel.ReadCommitted);
                IsinTransaction = true;
            }
            else
            {
                publisher.SetConnection = LclConn;
            }
            publisher.Email = strEmail.Trim();
            publisher.FirstName = strFirstName.Trim();
            publisher.LastName = strLastName.Trim();
            publisher.OptID = 1;
            publisher.CreatedDate = DateTime.Now;
            bool result = publisher.CreatePublisher(ref dblPublisherID, SetTransaction);
            if (IsinTransaction && result)
            {
                SetTransaction.Commit();
                publisher.Id = dblPublisherID;
            }
            else
            {
                SetTransaction.Rollback();
            }
            publisher.CloseConnection(LclConn);
            return publisher;
        }

        public bool PublisherExists(string strEmail, ref double _publisherId)
        {
            ConnManager connManager = new ConnManager();
            connManager.OpenConnection();
            DataSet dsPublisherExists = connManager.GetData("Select * from Publisher where EMail = '" + strEmail + "'");
            connManager.DisposeConn();
            if (dsPublisherExists.Tables[0].Rows.Count > 0)
            {
                _publisherId = double.Parse(dsPublisherExists.Tables[0].Rows[0]["Id"].ToString());
                return true;
            }
            else
                return false;

        }
    }
}


