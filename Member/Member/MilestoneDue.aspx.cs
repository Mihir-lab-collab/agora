using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MilestoneDue : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
       // MasterPage.MasterInit(true, false,true,false);
    }

    [System.Web.Services.WebMethod]
    public static String BindMilestoneDue(string mDate="")
    {
        try
        {
            List<MilstoneDueBLL> lstMDue = MilstoneDueBLL.GetMilestoneDue("Select");
            var data = from EItems in lstMDue
                       select new
                       {
                           EItems.Name,
                           EItems.Amount,
                           EItems.Balance,
                           EItems.Description,
                           EItems.ExRate,
                           EItems.EstHours,
                           EItems.isRecurring,
                           EItems.DueDate,
                           EItems.DueDays,
                           EItems.ProjID,
                           EItems.ProjName,
                           EItems.DueFor
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }
  
}