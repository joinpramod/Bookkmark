using System;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;
using Bookkmark.Models;
using System.Text.RegularExpressions;

namespace Bookkmark
{
    public class ConnManager
    {

        public DataSet GetData(string sqlQuery)
        {
            DataSet DS = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(sqlQuery, conn))
                {
                    sda.Fill(DS);
                }
            }        
            return DS;
        }

        public DataTable GetDataTable(string sqlQuery)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLCON"].ToString()))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(sqlQuery, conn))
                {
                    sda.Fill(dt);
                }
            }
            return dt;
        }



        private string CleanTitle(string v)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(v, "");
        }


        //public List<QuestionType> GetQuestionType()
        //{
        //    OpenConnection();
        //    DataTable DSQuestions = new DataTable();
        //    DSQuestions = GetDataTable("Select * from QuestionType");
        //    DisposeConn();

        //    List<QuestionType> types = new List<QuestionType>();
        //    QuestionType type;
        //    foreach (DataRow row in DSQuestions.Rows)
        //    {
        //        type = new QuestionType();
        //        type.TypeId = row["QuestionTypeId"].ToString();
        //        type.Type = row["QuestionType"].ToString();
        //        types.Add(type);
        //    }
        //    return types;
        //}

        //public List<ArticleModel> GetArticles(string sqlQuery)
        //{
        //    OpenConnection();
        //    DataTable DSQuestions = new DataTable();
        //    DSQuestions = GetDataTable(sqlQuery);
        //    DisposeConn();

        //    List<ArticleModel> articles = new List<ArticleModel>();
        //    ArticleModel article;
        //    foreach (DataRow row in DSQuestions.Rows)
        //    {
        //        article = new ArticleModel();
        //        article.ArticleID = row["ArticleID"].ToString();
        //        article.ArticleDetails = row["ArticleDetails"].ToString();
        //        article.ArticleTitle = row["ArticleTitle"].ToString();
        //        article.InsertedDate = row["InsertedDate"].ToString();
        //        article.ThumbsUp = row["ThumbsUp"].ToString();
        //        article.ThumbsDown = row["ThumbsDown"].ToString();
        //        article.Views = row["Views"].ToString();
        //        articles.Add(article);
        //    }
        //    return articles;
        //}

        //public List<QuestionModel> GetQuestions(string sqlQuery)
        //{
        //    OpenConnection();
        //    DataTable DSQuestions = new DataTable();
        //    DSQuestions = GetDataTable(sqlQuery);
        //    DisposeConn();

        //    List<QuestionModel> questions = new List<QuestionModel>();
        //    QuestionModel question;
        //    foreach (DataRow row in DSQuestions.Rows)
        //    {
        //        question = new QuestionModel();
        //        question.QuestionID = row["QuestionID"].ToString();
        //        question.QuestionType = row["QuestionTypeID"].ToString();
        //        question.QuestionTitle = row["QuestionTitle"].ToString();
        //        question.Question = row["Question"].ToString();
        //        question.AskedUser = row["AskedUser"].ToString();
        //        question.AskedDateTime = row["AskedDateTime"].ToString();
        //        questions.Add(question);
        //    }
        //    return questions;
        //}

        //public List<VwSolutionsModel> GetSolns(string sqlQuery)
        //{
        //    OpenConnection();
        //    DataTable DSQuestions = new DataTable();
        //    DSQuestions = GetDataTable(sqlQuery);
        //    DisposeConn();

        //    List<VwSolutionsModel> solns = new List<VwSolutionsModel>();
        //    VwSolutionsModel soln;
        //    foreach (DataRow row in DSQuestions.Rows)
        //    {
        //        soln = new VwSolutionsModel();
        //        soln.QuestionID = row["QuestionID"].ToString();
        //        soln.QuestionTitle = row["QuestionTitle"].ToString();
        //        soln.AskedUser = row["AskedUser"].ToString();
        //        soln.RepliedUser = row["RepliedUser"].ToString();
        //        solns.Add(soln);
        //    }
        //    return solns;
        //}


        //public void DeleteReply(string replyId)
        //{
        //    ConnManager connManager = new ConnManager();
        //    connManager.OpenConnection();
        //    string strQuery = "Delete from Replies where ReplyId = " + replyId;
        //    SqlCommand command = new SqlCommand(strQuery, connManager.DataCon);
        //    command.ExecuteNonQuery();
        //}

        //public void UpdateUserAsWebUser(double userId)
        //{
        //    ConnManager connManager = new ConnManager();
        //    connManager.OpenConnection();
        //    string strQuery = "Update Users set IsWebUser = 1 where Id = " + userId;
        //    SqlCommand command = new SqlCommand(strQuery, connManager.DataCon);
        //    command.ExecuteNonQuery();
        //}

        //public DataTable GetArticle(string sqlArticleId)
        //{
        //    DataTable dtArticle = new DataTable();
        //    using (SqlCommand _cmd = new SqlCommand("GetArticle_Sp", DataCon))
        //    {
        //        _cmd.CommandType = CommandType.StoredProcedure;

        //        _cmd.Parameters.Add(new SqlParameter("@ArticleId", SqlDbType.Int));
        //        _cmd.Parameters["@ArticleId"].Value = sqlArticleId;

        //        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);

        //        _dap.Fill(dtArticle);
        //    }
        //    return dtArticle;
        //}

        //public DataTable GetQuestion(string sqlQuestionId)
        //{
        //    DataTable dtQuestion = new DataTable();
        //    using (SqlCommand _cmd = new SqlCommand("GetQuestion_Sp", DataCon))
        //    {
        //        _cmd.CommandType = CommandType.StoredProcedure;

        //        _cmd.Parameters.Add(new SqlParameter("@QuestionId", SqlDbType.Int));
        //        _cmd.Parameters["@QuestionId"].Value = sqlQuestionId;

        //        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);

        //        _dap.Fill(dtQuestion);
        //    }
        //    return dtQuestion;
        //}

        //public void DisposeConn()
        //{
        //    if (DataCon.State == ConnectionState.Open)
        //    {
        //        DataCon.Close();            
        //    }
        //    DataCon.Dispose();
        //}




    }
}