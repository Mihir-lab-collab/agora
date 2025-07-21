using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Customer.BLL;
using Customer.DAL;
using dwtDAL;
using System.Configuration;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Member_Designation : Authentication
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

   static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo(); 
        }
    }

    [System.Web.Services.WebMethod]
    public static String BindDesignation()
    {
        try
        {

            List<DesignationBLL> lstConfig = DesignationBLL.GetDesignation("GET");
            var data = from curDesig in lstConfig
                       select new
                       {
                           curDesig.DesigID,
                           curDesig.Designation
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
    public static string UpdateDesignation(int desigID, string designation)
    {
        string output = "Update Failed";
        try
        {
            int isupdated = DesignationBLL.SaveDesignation("SAVE", desigID, designation);

        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    protected void lnkSaveDesignation_Click(object sender, EventArgs e)
    {
        int desigID = 0;
        string designation = string.Empty;
        designation = hdnDesignation.Value;

        int outID = DesignationBLL.SaveDesignation("SAVE", desigID, designation);
        if (outID == 1)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Designation already exist')", true);
    }
}