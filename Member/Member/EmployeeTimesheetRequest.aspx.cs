using Customer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Member_EmpReqtime : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {

        UM = UserMaster.UserMasterInfo();

        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true, false, true);
        if (!IsPostBack)
        {

            BindEmployee();
        }
    }


    private void BindEmployee()
    {
        List<KeyValueModel> lstEmp = new List<KeyValueModel>();
        lstEmp = SkillMatrixBLL.GetEmployee("GetEmployee");


        lstEmployee.DataSource = lstEmp;
        lstEmployee.DataTextField = "Key";
        lstEmployee.DataValueField = "Value";
        lstEmployee.DataBind();

    }

    // Code by AP ends
    //  private void BindEmployee()
    //{
    //    List<KeyValueModel> lstEmp = new List<KeyValueModel>();
    //     lstEmp = SkillMatrixBLL.GetEmployee("GetEmployee");
    //     ddlEmployee.DataSource = lstEmp;
    //   ddlEmployee.DataValueField = "Value";
    //      ddlEmployee.DataTextField = "Key";
    //      ddlEmployee.DataBind();

    //}

    //protected void lnkSaveEvents_Click(object sender, EventArgs e)
    //{

    //    string Description = string.Empty;
    //    DateTime RequestDate = DateTime.ParseExact(hdnEventDate.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    Description = hdnDescription.Value;

    //    int empid = Convert.ToInt32(hdnEmpId.Value);

    //    int outputID = EmployeeTimesheetRequestBLL.Save(empid, RequestDate, Description, UM.EmployeeID);
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "call();", true);
    //}


    protected void lnkSaveEvents_Click(object sender, EventArgs e)
    {

        string Description = string.Empty;
        DateTime RequestDate = DateTime.ParseExact(hdnEventDate.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        Description = hdnDescription.Value;

        string empid = hdnEmpId.Value.Replace("'", "''");
        string[] empRequested = empid.Split(',');
        for (int i = 0; i < empRequested.Length; i++)
        {

            if (empRequested[i].Trim() != "")
            {
                int outputID = EmployeeTimesheetRequestBLL.Save(Convert.ToInt32(empRequested[i].ToString()), RequestDate, Description, UM.EmployeeID);

            }
        }

    }

    [System.Web.Services.WebMethod]
    public static String BindEvents(int KEID = 0)
    {
        try
        {
            List<EmployeeTimesheetRequestBLL> lstEvents = EmployeeTimesheetRequestBLL.GetAllEmployeeTimesheetRequestList();
            if (KEID == 1)
            {
                lstEvents = lstEvents.Where(x => Convert.ToDateTime(x.InsertedOn).Date == DateTime.Now.Date).ToList();
            }
            else if (KEID == 2)
            {
                lstEvents = lstEvents.Where(x => Convert.ToDateTime(x.InsertedOn).Date < DateTime.Now.Date).ToList();
            }
            var data = from EItems in lstEvents
                       select new
                       {
                           EItems.Id,
                           EItems.EmpId,
                           EItems.EmployeeName,
                           EItems.Requestdate1,
                           EItems.Descripation,
                           EItems.ApprovedBy,
                           EItems.InsertedOn,
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var hss = jss.Serialize(data);
            return jss.Serialize(data);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}