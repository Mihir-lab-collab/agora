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
        MasterPage.MasterInit(false, true);
    }

    [System.Web.Services.WebMethod]
    public static String BindHoliday()
    {
        try
        {
            List<Holiday> lstHoliday = Holiday.GetHolidayDetails("Select", Convert.ToInt32(HttpContext.Current.Session["LocationID"]));
            var data = from HolidayItem in lstHoliday
                       select new
                       {
                           HolidayItem.HolidayId,
                           HolidayItem.HolidayDate,
                           HolidayItem.Narration
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string UpdateHoliday(int HolidayId, int LocationId, string holidaydate, string narration)
    {
        string output = "Update Failed";
        try
        {
            int isupdated = Holiday.SaveHoliday("SAVE", HolidayId, LocationId, holidaydate, narration);
        }
        catch (Exception ex)
        {
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    [System.Web.Services.WebMethod]
    public static void DeleteHoliday(int HolidayId)
    {
        Holiday.DeleteHoliday("Delete", HolidayId);
    }

    protected void lnkSaveHoliday_Click(object sender, EventArgs e)
    {
        int LocationId = 0;
        int HolidayId = 0;
        string holidaydate = string.Empty;
        string narration = string.Empty;
        if (Session["LocationID"] != null || Convert.ToString(Session["LocationID"]) != string.Empty)
        {
            LocationId = Convert.ToInt32(Session["LocationID"]);
        }
        holidaydate = hdnHolidayDate.Value;
        narration = hdnNarration.Value;

        int outputID = Holiday.SaveHoliday("SAVE", HolidayId, LocationId, holidaydate, narration);
        if (outputID == 1)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Holiday already exist')", true);
    }
}