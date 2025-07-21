using CSCode;
using Customer.BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Member_Milestone : System.Web.UI.Page //Authentication
{
    public static int projid = 0;
    UserMaster UM;
    public string empID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["projid"] != null && Convert.ToString(Request.QueryString["projid"]) != String.Empty)
            Session["ProjectID"] = Request.QueryString["projid"].ToString();

        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false, true);

        hdnProjID.Value = Session["ProjectID"].ToString();


        int projid = Convert.ToInt32(hdnProjID.Value.ToString());
        if (projid == 0)
        {
            dvall.Visible = true;
            MilestoneID.Visible = false;
            dvbg.Style.Add("display", "block");
        }
        else
        {
            dvall.Visible = false;
            MilestoneID.Visible = true;
            dvbg.Style.Add("display", "none");
            if (Session["CurrExRate"] == null)
            {
                hdnExRate.Value = "1";
            }
            else
            {
                hdnExRate.Value = Session["CurrExRate"].ToString();
            }
            UM = UserMaster.UserMasterInfo();
            empID = UM.EmployeeID.ToString();
            hdnEmpId.Value = empID;
            BindData();
            if (!Page.IsPostBack)
            {
                BindMileStone(Convert.ToInt32(hdnProjID.Value));
            }
        }


    }


    [System.Web.Services.WebMethod]
    public static String BindMileStone(int projid)
    {
        try
        {
            List<mileStone> lstGetMileStone = mileStone.getMileStone("Select", projid);

            var data = from curMileStone in lstGetMileStone
                       select new
                       {
                           curMileStone.name,
                           curMileStone.amount,
                           curMileStone.dueDate,
                           curMileStone.Description,
                           curMileStone.ExRate,
                           curMileStone.EstHours,
                           curMileStone.projID,
                           curMileStone.projMilestoneID,
                           curMileStone.BalAmount
                       };
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static String GetAllcurrencyMaster()
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(currencyMaster.GetAllcurrencyMaster());
    }

    [System.Web.Services.WebMethod]
    public static List<mileStone> GetData(string id)
   {
        try
        {
            List<mileStone> lstGetMileStone = mileStone.getMileStone("Select", Convert.ToInt32(id));
            return lstGetMileStone;
        }

        catch (Exception)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod]
    public static string PostData(string hdnJSONData)
    {
        ////  Member_Default helper = new Member_Default();
        List<mileStone> milestone = new List<mileStone>();
        try
        {
            //milestone = JsonConvert.DeserializeObject<List<mileStone>>((hdnJSONData.Replace("\"\"","\"\\\"")).Replace("\"\\\",","\\\"\","));
            milestone = JsonConvert.DeserializeObject<List<mileStone>>(hdnJSONData);
            foreach (mileStone prime in milestone)
            {
                int result = new mileStone().insertMileStoneData("InsertOrUpdate", prime.projID, prime.projMilestoneID, prime.name, prime.amount, prime.ExRate, prime.dueDate, prime.DeliveryDate, prime.EstHours, prime.Description, prime.insertedBy, prime.BalanceAmount, prime.IsRecurring, prime.RecurringMSID);
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(milestone);
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
            return null;
        }
    }


    [System.Web.Services.WebMethod]
    public static void DeleteMile(string ProjectMileStoneID)
    {
        try
        {
            mileStone.DeleteMilestone("Delete", Convert.ToInt32(ProjectMileStoneID));
        }

        catch (Exception)
        {

        }
    }


    private void BindData()
    {
        bool All = false;
        int CustID = 0;
        string status = "";
        int ProjID = Convert.ToInt32(hdnProjID.Value);
        projid = ProjID;

        List<projectMaster> PM = new List<projectMaster>();
        PM = projectMaster.Projects(All, CustID, status, ProjID);
        if (PM.Count != 0)
        {
            if (ProjID != 0)
            {
                lblProject.Text = PM[0].projName;
                lblDuration.Text = PM[0].projDuration;
                lblCost.Text = Convert.ToString(Global.GetCurrencyFormat(Convert.ToDouble(PM[0].projCost)));
                lblInitProjectCost.Text = Convert.ToString(Convert.ToDecimal(PM[0].InitialProjectCost));
                lblTotalInvoiced.Text = Convert.ToInt32(PM[0].TotalInvoiced) == 0 ? "0" : Convert.ToString(Global.GetCurrencyFormat(Convert.ToDouble(PM[0].TotalInvoiced)));
                lblTotalRecieved.Text = Convert.ToInt32(PM[0].TotalRecieved) == 0 ? "0" : Convert.ToString(Global.GetCurrencyFormat(Convert.ToDouble(PM[0].TotalRecieved)));
                lblStartDate.Text = PM[0].projStartDate.ToShortDateString();
                if (lblStartDate.Text == "1/1/0001")
                {
                    lblStartDate.Text = string.Empty;
                }
            }
            else if (ProjID == 0)
            {
                lblProject.Text = string.Empty;
                lblDuration.Text = string.Empty;
                lblCost.Text = string.Empty;
                lblStartDate.Text = string.Empty;
            }
        }
    }
}