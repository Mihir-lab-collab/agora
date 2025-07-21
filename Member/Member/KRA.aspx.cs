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

public partial class Member_KRA : Authentication
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethod]
    public static String BindKRA()
    {
        try
        {
            List<KRABLL> lstConfig = KRABLL.GetkAR("GET", 0);
            var data = from curKRA in lstConfig
                       select new
                       {
                           curKRA.KRAID,
                           curKRA.KRANames
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
    public static string UpdateKRA(int KRAID, string KRANames)
    {
        string output = "Update Failed";
        try
        {
            int isupdated = KRABLL.SaveKRA("SAVE", KRAID, KRANames);
        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    protected void lnkSaveDesignation_Click(object sender, EventArgs e)
    {
        int KRAID = 0;
        string KRANames = string.Empty;
        KRANames = hdnKRA.Value;

        int outID = KRABLL.SaveKRA("SAVE", KRAID, KRANames);
        if (outID == 1)
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", "alert('KRA already exist')", true);
    }

}