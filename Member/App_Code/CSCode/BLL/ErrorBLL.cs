using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ErrorBLL
/// </summary>
public class ErrorBLL
{
    public ErrorBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int Id { get; set; }
    public string Source { get; set; }
    public string LongDateTime { get; set; }
    public string Message { get; set; }
    public string QueryString { get; set; }
    public string TargetSite { get; set; }
    public string StackTrace { get; set; }
    public string ServerName { get; set; }
    public string RequestURL { get; set; }
    public string UserAgent { get; set; }
    public string UserIP { get; set; }
    public string UserName { get; set; }
    public string Mode { get; set; }
    public void SaveErrorToDB(ErrorBLL errorBLL)
    {
        if (errorBLL != null)
        {
            ErrorDAL errorDAL = new ErrorDAL();
            ErrorBLL error = new ErrorBLL()
            {
                Message = errorBLL.Message,
                StackTrace = errorBLL.StackTrace,
                UserAgent = errorBLL.UserAgent,
                ServerName = errorBLL.ServerName,
                RequestURL = errorBLL.RequestURL,
                QueryString = errorBLL.QueryString,
                Source = errorBLL.Source,
                UserName = errorBLL.UserName,
                TargetSite = errorBLL.TargetSite,
                UserIP = errorBLL.UserIP

            };
            errorDAL.LogErrorToDB(error);
        }

    }
    public static void LogExceptionToFile(Exception ex)
    {
        string filePath = "exception_log" + DateTime.Now.Date.ToString("yyyy_MM_dd") + ".txt";
        string finalFilePath = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["ErrorLogFilePath"], filePath);
        if (!File.Exists(finalFilePath))
        {
            File.Create(finalFilePath);
        }

        try
        {
            System.IO.File.AppendAllText(finalFilePath, "[Date/Time]:" + DateTime.Now+ Environment.NewLine);
            System.IO.File.AppendAllText(finalFilePath, "[Exception Type]:" + ex.GetType().FullName + Environment.NewLine);
            System.IO.File.AppendAllText(finalFilePath, "[Message]:" + ex.Message + Environment.NewLine);
            System.IO.File.AppendAllText(finalFilePath, "[StackTrace]:" + ex.StackTrace + Environment.NewLine);
        }
        catch (IOException e)
        {

        }
    }
}
