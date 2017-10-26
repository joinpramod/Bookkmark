using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace Bookkmark
{
    public class ClsSuggestion : ConnManager
    {
        private SqlConnection CmdLCLDBConn;
        private SqlCommand CmdExecute;
        private int IntOptID;
        private double DblSuggestionId;
        private string StrSuggestion;
        private double DblCreatedUser;
        private System.DateTime DtCreatedDate;


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
        public double SuggestionId
        {
            get
            {
                return DblSuggestionId;
            }
            set
            {
                DblSuggestionId = value;
            }
        }


        public string Suggestion
        {
            get
            {
                return StrSuggestion;
            }
            set
            {
                StrSuggestion = value;
            }
        }


        public double CreatedUser
        {
            get
            {
                return DblCreatedUser;
            }
            set
            {
                DblCreatedUser = value;
            }
        }


        public System.DateTime CreatedDate
        {
            get
            {
                return DtCreatedDate;
            }
            set
            {
                DtCreatedDate = value;
            }
        }


        public bool SetCommandSuggestion(ref SqlCommand CmdSent)
        {
            SqlCommand Cmd = new SqlCommand("Suggestion_Sp", CmdLCLDBConn);
            Cmd.CommandType = CommandType.StoredProcedure;


            SqlParameter ParamOptID = Cmd.Parameters.Add("@OptID", SqlDbType.Int);
            SqlParameter ParamSuggestionId = Cmd.Parameters.Add("@SuggestionId", SqlDbType.Float);
            SqlParameter ParamSuggestion = Cmd.Parameters.Add("@Suggestion", SqlDbType.VarChar);
            SqlParameter ParamCreatedUser = Cmd.Parameters.Add("@CreatedUser", SqlDbType.Float);
            SqlParameter ParamCreatedDate = Cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime);

            ParamOptID.Value = IntOptID;
            ParamOptID.Direction = ParameterDirection.Input;
            ParamSuggestionId.Value = DblSuggestionId;
            ParamSuggestionId.Direction = ParameterDirection.Input;
            ParamSuggestion.Value = StrSuggestion;
            ParamSuggestion.Direction = ParameterDirection.Input;
            ParamCreatedUser.Value = DblCreatedUser;
            ParamCreatedUser.Direction = ParameterDirection.Input;
            if (DtCreatedDate < DateTime.Parse("1-1-2000"))
            {
                ParamCreatedDate.Value = DBNull.Value;
            }
            else
            {
                ParamCreatedDate.Value = DtCreatedDate;
            }
            ParamCreatedDate.Direction = ParameterDirection.Input;



            CmdSent = Cmd;
            return true;
        }


        public bool CreateSuggestion(ref double NewMasterID, SqlTransaction TrTransaction)
        {
            if (SetCommandSuggestion(ref CmdExecute))
            {
                try
                {
                    if (TrTransaction != null)
                    {
                        CmdExecute.Transaction = TrTransaction;
                    }
                    SqlDataReader DATReader = CmdExecute.ExecuteReader();
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
