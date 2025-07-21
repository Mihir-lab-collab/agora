using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_PayProcess : Authentication
{
    clsCommon objCommon = new clsCommon();
    CultureInfo cuInfo = new CultureInfo("en-IN");
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PayId"] = null;
        UM = UserMaster.UserMasterInfo();
        if (UM == null)
        {
            Session.Abandon();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SiteRoot"] + "Login.aspx");
        }
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(false, true, false, false);

        if (!IsPostBack)
        {
            if (UM == null)
                return;

            hdLocationId.Value = HttpContext.Current.Session["LocationID"].ToString(); 
            //BindLocation();
       }
    }

    [System.Web.Services.WebMethod]
    public static string BindPayrProcessDetails(int EmpId, int? year, int? month)
    {
        try
        {
            List<PayrollBLL> lsPayroll = new List<PayrollBLL>();

            lsPayroll = PayrollBLL.GetSalaryDetails("GETDETAIL", EmpId, year, month, Convert.ToInt32(HttpContext.Current.Session["LocationID"].ToString()));

            var data = from pr in lsPayroll
                       select new
                       {
                           pr.EmpID,
                           pr.EmpName,
                           pr.Gross,
                           pr.IsPF,
                           pr.PT,
                           pr.Insurance,
                           pr.Basic,
                           pr.HRA,
                           pr.Conveyance,
                           pr.Medical,
                           pr.Food,
                           pr.Special,
                           pr.LTA,
                           pr.PF,
                           pr.Days,
                           pr.Leaves,
                           pr.Loan,
                           pr.Advance,
                           pr.Presents,
                           pr.Tax,
                           pr.Deduction,
                           pr.Bonus,
                           pr.Addition,
                           pr.Net,
                           pr.CTC,
                           pr.TotalAddition,
                           pr.TotalDeduction,
                           pr.Ded_Leave,
                           pr.Remark,
                           pr.CalcGross,
                           pr.CalcBasic,
                           pr.Comment,
                           pr.PayId
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }

    }

    //protected void dlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    hdLocationId.Value = dlLocation.SelectedValue;
    //}

    //private void BindLocation()
    //{
    //    DataTable dtEmployeeLocation = new DataTable();
    //    dtEmployeeLocation = objCommon.EmployeeLocationList();
    //    dlLocation.DataSource = dtEmployeeLocation;
    //    dlLocation.DataTextField = "Name";
    //    dlLocation.DataValueField = "LocationID";
    //    dlLocation.DataBind();
    //    dlLocation.SelectedValue = "10";
    //    hdLocationId.Value = dlLocation.SelectedValue;
    //    dlLocation.Visible = true;
    //}
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("/Member/PayAdd.aspx");
    }
    protected void btnBankStatement_Click(object sender, EventArgs e)
    {
        //string date = hdnMonth.Value + "-" + hdnYear.Value;
        Session["PayId"] = hdnPayID.Value;
        Response.Redirect("/Member/PayStatement.aspx");
    }
}