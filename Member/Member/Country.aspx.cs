using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using Customer.BLL;
using Customer.DAL;



public partial class Member_Country : System.Web.UI.Page
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
    public static String BindCountry()
    {
        try
        {

            List<CountryBLL> lstConfig = CountryBLL.GetCountry("GET");
            var data = from curCountry in lstConfig
                       select new
                       {
                           curCountry.CountryID,
                           curCountry.Country
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
    public static string UpdateCountry(int CountryID, string CountryName)
    {
        string output = "Update Failed";
        try
        {
            int isupdated = CountryBLL.SaveCountry("SAVE", CountryID, CountryName);
            output =Convert.ToString(isupdated);            
        }
        catch (Exception ex)
        {
            output = "2";
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    protected void lnkSaveCountry_Click(object sender, EventArgs e)
    {
        int CountryID = 0;
        string CountryName = string.Empty;
        CountryName = hdnCountry.Value;

        int outID = CountryBLL.SaveCountry("SAVE", CountryID, CountryName);
        if (outID == 1)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('Country already exist')", true);
    }
}