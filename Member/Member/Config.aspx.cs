using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class Member_Config : Authentication
{
    static UserMaster UM;

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
    }

    protected void lnkSaveConfig_Click(object sender, EventArgs e)
    {
        int configID = 0;
        string category, name, value, value1, comment = string.Empty;
        configID = Convert.ToInt32(hfConfigID.Value);
        category = hfCategory.Value;
        name = hfConfigName.Value;
        value = HttpUtility.HtmlEncode(txtValue.Value).Replace("\n", "<br/>");
        value1 = HttpUtility.HtmlEncode(txtValue1.Value).Replace("\n", "<br/>");
        comment = hfComment.Value;
        int InsertedBy = Convert.ToInt32(UM.EmployeeID);
        ConfigBLL.InsertConfig("Insert", configID, category, name, value, value1,comment, InsertedBy);
    }

    [System.Web.Services.WebMethod]
    public static String BindConfig()
    {

        try
        {

            List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig");
            var data = from curConfig in lstConfig
                       select new
                       {
                           curConfig.configID,
                           curConfig.category,
                           curConfig.name,
                           curConfig.comment,
                           curConfig.value,
                           curConfig.value1,
                           curConfig.modifiedOn,
                           curConfig.modifiedBy


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
    public static string UpdateConfig(int configEditID, string categoryEdit, string nameEdit, string valueEdit, string valueEdit1, string commentEdit)
    {
        valueEdit = HttpUtility.HtmlDecode(valueEdit);
        valueEdit1 = HttpUtility.HtmlDecode(valueEdit1);
        string output = "Update Failed";
        try
        {
            int ModifiedBy = Convert.ToInt32(UM.EmployeeID);
            bool isupdated = ConfigBLL.UpdateConfig("Update", configEditID, categoryEdit, nameEdit, valueEdit, valueEdit1, commentEdit, ModifiedBy);
            if (isupdated == true)
            {
                output = "updated successfully.";

            }

        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    [System.Web.Services.WebMethod]
    public static String BindDefaultID()
    {
        try
        {

            List<ConfigBLL> lstConfigDefaultID = ConfigBLL.GetDefaultID("DefaultID");

            var data = from curConfig in lstConfigDefaultID
                       select new
                       {
                           curConfig.configID

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