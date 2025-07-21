using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Qualification : Authentication
{
   static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();

    }
    [System.Web.Services.WebMethod]
    public static String BindQualification()
    {
        try
        {
            List<QualificationBLL> lstQual = QualificationBLL.GetQualificationDetails("GetQualification");
            var data = from QualItem in lstQual
                       select new
                       {
                           QualItem.QID,
                           QualItem.QualDesc,
                           QualItem.QualType,
                           QualItem.modifiedBy
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
    public static string UpdateQualification(int QualId, string QualDesc, string QualType)
    {
        string output = "Update Failed";
        try
        {
            int ModifiedBy = UM.EmployeeID;
            bool isupdated = QualificationBLL.UpdateQualification("Update", QualId, QualDesc, QualType,ModifiedBy);
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
    protected void lnkSaveQualification_Click(object sender, EventArgs e)
    {
        string output = "Insert Failed";
        int QID = 0;
        string QualDesc, QualType;
        QID = Convert.ToInt32(hdnQualID.Value);
        QualDesc = hdnQualDesp.Value;
        QualType = hdnQualType.Value;

        int InsertedBy = UM.EmployeeID;

        int id = QualificationBLL.InsertQualification("Insert", QID, QualDesc, QualType, InsertedBy);
        if (id != 0)
        {
            output = "Inserted successfully.";
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + output + "')", true);
    }
}