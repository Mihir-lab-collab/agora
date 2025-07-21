using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Holiday : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true,false,true);
    }

    [System.Web.Services.WebMethod]
    public static String BindEvents(int KEID = 0)
    {
        try
        {
            List<CIPBLL> lstEvents = CIPBLL.GetEvents("Select",KEID ,Convert.ToInt32(HttpContext.Current.Session["LocationID"]));
            var data = from EItems in lstEvents
                       select new
                       {
                           EItems.KEID,
                           EItems.EventDate,
                           EItems.EventDateTime,
                           EItems.Description,
                           EItems.Time
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
     
    [System.Web.Services.WebMethod]
    public static void DeleteEvent(int KEID)
    {
        CIPBLL.Delete("Delete", KEID);

    }

    protected void lnkSaveEvents_Click(object sender, EventArgs e)
    {
       
        int LocationId = 0;
        int KEID = 0;
        string EventDate= string.Empty;
        string Description = string.Empty;
        string Time = string.Empty;
        if (Session["LocationID"] != null || Convert.ToString(Session["LocationID"]) != string.Empty)
        {
            LocationId = Convert.ToInt32(Session["LocationID"]);
        }
        KEID = Convert.ToInt32(hdnKEID.Value);
        EventDate = hdnEventDate.Value;
        Description = hdnDescription.Value;
        Time = hdnTime.Value;

        int outputID = CIPBLL.Save("SAVE", KEID, LocationId, EventDate, Description,Time);        
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "call();", true);

        //if (KEID == 0)
        //    SendMail();
        //Response.Redirect("CIP.aspx");
    }

    private void SendMail()
    {
        string toID = string.Empty;
        string subject = string.Empty;
        string msgBody = string.Empty;
        string from = "hr@intelgain.com";

        List<CIPBLL> mailInfo = CIPBLL.GetMailInfo("MAILINFO");
        toID = "elumalai.n@intelgain.com,dipti.c@intelgain.com,pallavi.j@intelgain.com";
        //toID = mailInfo[0].EmailID;
        //toID = "web@intelgain.com";
        subject = "CIP Session on " + mailInfo[0].EventDate.ToString();
        msgBody = getMsgBody(mailInfo[0]);

        CSCode.Global.SendMail(msgBody, subject, toID,from,true,"","");
    }

    private string getMsgBody(CIPBLL obj)
    {
        string strData = string.Empty;
        return strData = "<p><strong>Topic</strong>: " + obj.Description + "</p><p><strong>Date</strong>: " + obj.EventDate + "<br /><strong>Time</strong>: " + obj.Time + "<br /></p>";
    }
       
}