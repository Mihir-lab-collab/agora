using CSCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if(!IsPostBack)
            {
                if(Session["GlobalErrorMessage"] != null)
                {
                    Exception exception = Session["GlobalErrorMessage"] as Exception; ;
                    //exception.Message=Session[GlobalErrorMessage].Mess
                    if(exception != null)
                    {
                        UserMaster UM = UserMaster.UserMasterInfo();
                        lblError.Text = Convert.ToString(exception.Message) + Environment.NewLine + Convert.ToString(exception.StackTrace);
                        ErrorBLL errorBLL = new ErrorBLL()
                        {
                            StackTrace = Convert.ToString(exception.StackTrace),
                            Message = Convert.ToString(exception.Message),
                            Source = Convert.ToString(exception.Source),
                            TargetSite = Convert.ToString(exception.TargetSite),
                            UserName=UM.EmployeeID.ToString()
                        };
                        string status = System.Configuration.ConfigurationManager.AppSettings["CheckStatus"];
                        string appenvironment = System.Configuration.ConfigurationManager.AppSettings["AppEnvironment"];
                         errorBLL.SaveErrorToDB(errorBLL);
                        if (string.Compare(status, "true", true) == 0)
                        {
                            //string currentUrl = HttpContext.Current.Request.Url.ToString();
                            string previousUrlString = string.Empty;
                            Uri previousUrl = HttpContext.Current.Request.UrlReferrer;
                            if (previousUrl != null)
                            {
                                previousUrlString = previousUrl.ToString();
                            }
                            string subject = "[Alert] Error on : Environment URL(" + previousUrlString+"("+appenvironment+")" + ")";
                            string toMailId = System.Configuration.ConfigurationManager.AppSettings["ToMailSend"];
                            string fromMailId = System.Configuration.ConfigurationManager.AppSettings["FromMailSend"];
                            StringBuilder mailbody = new StringBuilder();
                            mailbody.Append("<p>This exception indicates a potential issue that requires urgent investigation and resolution. Below are the details of the exception:</br>");
                            mailbody.Append("Timestamp:" + DateTime.UtcNow.ToLongDateString() + "</br>");
                            mailbody.Append("Exception Type:" + exception.GetType().FullName + "</br>");
                            mailbody.Append("Exception Message:" + exception.StackTrace + "</P>");
                            Global.SendMail(mailbody.ToString(), subject, toMailId, fromMailId,true,"","");
                        }
                    }
                }
            }
        }
        catch(Exception ex)
        {
            ErrorBLL.LogExceptionToFile(ex);
        }
    }
}