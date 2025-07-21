using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for ErrorDAL
/// </summary>
public class ErrorDAL
{
    DataSet ds = null;
    DataTable dt = null;
    private static string connStr = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public ErrorDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void LogErrorToDB(ErrorBLL logExceptions)
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection sqlConn = new SqlConnection(connStr);
        try
        {
            HttpContext ctxObject = HttpContext.Current;
            //string strLogConnString = ConfigurationManager.ConnectionStrings["DbAgora"].ConnectionString;
            string logDateTime = DateTime.Now.ToString("g");
            string strReqURL = (ctxObject.Request.Url != null) ? ctxObject.Request.Url.ToString() : String.Empty;
            string strReqQS = (ctxObject.Request.QueryString != null) ? ctxObject.Request.QueryString.ToString() : String.Empty;
            string strServerName = String.Empty;
            if (ctxObject.Request.ServerVariables["HTTP_REFERER"] != null)
            {
                strServerName = ctxObject.Request.ServerVariables["HTTP_REFERER"].ToString();
            }
            string strUserAgent = (ctxObject.Request.UserAgent != null) ? ctxObject.Request.UserAgent : String.Empty;
            string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
            string strUserAuthen = (ctxObject.User.Identity.IsAuthenticated.ToString() != null) ? ctxObject.User.Identity.IsAuthenticated.ToString() : String.Empty;
            //string strUserName = (ctxObject.User.Identity.Name != null) ? ctxObject.User.Identity.Name : String.Empty;
            string strUserName = !(string.IsNullOrEmpty(logExceptions.UserName )) ? logExceptions.UserName : String.Empty;
            string strMessage = string.Empty, strSource = string.Empty, strTargetSite = string.Empty, strStackTrace = string.Empty;
            if (logExceptions != null)
            {
                strMessage = logExceptions.Message;
                strSource = logExceptions.Source;
                strTargetSite = logExceptions.TargetSite.ToString();
                strStackTrace = logExceptions.StackTrace;
                //logExceptions = logExceptions.InnerException;
            }
            if (connStr.Length > 0)
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "USP_LogExceptionToDB";
                cmd.Connection = sqlConn;
                sqlConn.Open();
                cmd.Parameters.Add(new SqlParameter("@Source", strSource));
                cmd.Parameters.Add(new SqlParameter("@LogDateTime", logDateTime));
                cmd.Parameters.Add(new SqlParameter("@Message", strMessage));
                cmd.Parameters.Add(new SqlParameter("@QueryString", strReqQS));
                cmd.Parameters.Add(new SqlParameter("@TargetSite", strTargetSite));
                cmd.Parameters.Add(new SqlParameter("@StackTrace", strStackTrace));
                cmd.Parameters.Add(new SqlParameter("@ServerName", string.Empty));
                cmd.Parameters.Add(new SqlParameter("@RequestURL", strServerName));
                cmd.Parameters.Add(new SqlParameter("@UserAgent", strUserAgent));
                cmd.Parameters.Add(new SqlParameter("@UserIP", strUserIP));
                cmd.Parameters.Add(new SqlParameter("@UserName", strUserName));
                cmd.Parameters.Add(new SqlParameter("@Mode", "Insert"));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                sqlConn.Close();
            }
        }
        catch (Exception exc)
        {
            throw exc;

        }
        finally
        {
            cmd.Dispose();
            sqlConn.Close();
        }


    }
}