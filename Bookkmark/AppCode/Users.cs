using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bookmark
{

    public enum RegisType { WebUser, SiteOwner }

    public class Users
    {
        public int OptionID { get; set; }
        public double UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsPublisher { get; set; }
        public bool IsWebUser { get; set; }
        //public string ScriptId { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        public System.DateTime ModifiedDateTime { get; set; }
        public string ImageURL { get; set; }
        //public string Company { get; set; }
        //public string Details { get; set; }
        //public string Address { get; set; }
        public string Status { get; set; }
        public List<Domain> Domains { get; set; }

        public bool CreateUsers(ref double NewMasterID)
        {
            SqlCommand CmdExecute = new SqlCommand();
            if (SetCommandUser(ref CmdExecute))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                {
                    using (CmdExecute)
                    {
                        conn.Open();
                        CmdExecute.Connection = conn;
                        using (SqlDataReader DATReader = CmdExecute.ExecuteReader())
                        {
                            while (DATReader.Read())
                            {
                                NewMasterID = double.Parse(DATReader[0].ToString());
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool SetCommandUser(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("User_Sp");
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamUserId = Cmd.Parameters.Add("@UserId", SqlDbType.Float);
            SqlParameter ParamFirstName = Cmd.Parameters.Add("@FirstName", SqlDbType.VarChar);
            SqlParameter ParamLastName = Cmd.Parameters.Add("@LastName", SqlDbType.VarChar);
            SqlParameter ParamPassword = Cmd.Parameters.Add("@Password", SqlDbType.VarChar);
            SqlParameter ParamEmail = Cmd.Parameters.Add("@Email", SqlDbType.VarChar);
            SqlParameter ParamIsPublisher = Cmd.Parameters.Add("@IsPublisher", SqlDbType.Bit);
            SqlParameter ParamIsWebUser = Cmd.Parameters.Add("@IsWebUser", SqlDbType.VarChar);
            SqlParameter ParamStatus = Cmd.Parameters.Add("@Status", SqlDbType.VarChar);
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
            ParamIsWebUser.Value = IsWebUser;
            ParamIsWebUser.Direction = ParameterDirection.Input;
            //ParamDomainName.Value = DomainName;
            //ParamDomainName.Direction = ParameterDirection.Input;
            ParamStatus.Value = Status;
            ParamStatus.Direction = ParameterDirection.Input;
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


        public Users CreateUser(string strEmail, string strFirstName, string strLastName)
        {
            Users user = new Users();
            double dblUserID = 0;
            user.Email = strEmail.Trim();
            user.FirstName = strFirstName.Trim();
            user.LastName = strLastName.Trim();
            //user.ImageURL = strImageURL.Trim();
            user.OptionID = 1;
            user.CreatedDateTime = DateTime.Now;
            user.CreateUsers(ref dblUserID);
            user.UserId = dblUserID;
            return user;
        }

        public bool UserExists(string strEmail, ref double _userId)
        {
            DataTable dtUserExists = new DataTable();
            ConnManager connManager = new ConnManager();
            dtUserExists = connManager.GetDataTable("Select * from Users where EMail = '" + strEmail + "'");
            if (dtUserExists.Rows.Count > 0)
            {
                _userId = double.Parse(dtUserExists.Rows[0]["Userid"].ToString());
                return true;
            }
            else
            {
                return false;
            }
        }


        public void CreateUserActivation(Users user, string activationCode, double dblUserID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO UserActivation VALUES(@UserId, @EMailId, @ActivationCode)"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId", dblUserID);
                        cmd.Parameters.AddWithValue("@EMailId", user.Email);
                        cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
        }



        public string ActivateUser(string ActivationCode)
        {
            string res = string.Empty;
            ConnManager con = new ConnManager();
            DataTable dtUserActivation = con.GetDataTable("select * from UserActivation where  ActivationCode = '" + ActivationCode + "'");

            if (dtUserActivation != null && dtUserActivation.Rows.Count > 0)
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand("delete from UserActivation where  EMailId = @EMailId"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@EMailId", dtUserActivation.Rows[0]["EMailId"]);
                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                res = "Account activated successfully. Please login with your emaild and password";
            }
            else
            {
                res = "Account could not be activated, please try again";
            }
            return res;
        }


    }
}






//public Users CreateUsers(string strEmail, string strFirstName, string strLastName)
//{
//    return CreateUser(strEmail, strFirstName, strLastName, null);
//}
