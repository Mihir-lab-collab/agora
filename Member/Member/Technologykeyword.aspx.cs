using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Technologykeyword : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
    }

    [System.Web.Services.WebMethod]
    public static string BindTechnologykeyword(string techName)
    {
        try
        {
            List<TechnologykeywordBLL> lsttechnology = new List<TechnologykeywordBLL>();
            lsttechnology = TechnologykeywordBLL.getall("Get", techName);
            var data = from t in lsttechnology
                       select new
                       {
                           t.techId,
                           t.techName,  
                           t.subTechName
                       };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected void lnkSaveNew_Click(object sender, EventArgs e)
    {
        string techname = string.Empty;
        string subtechname = string.Empty;
        int techId = 0;

        techname = hfTechname.Value;
        if (techname != "" )
        {
            TechnologykeywordBLL _addtech = new TechnologykeywordBLL();
            _addtech.AddNewTechnology(techId, techname);
            messageBox("Technology inserted successfully");
        }
        else
        {
            messageBox("Insert failed");
        }        
    }

    [System.Web.Services.WebMethod(EnableSession = true)]//In order to store session state in web method we use enablesession
    public static void UpdateTechnologyId(int techId, string techName, string subTechName)
    {
        TechnologykeywordBLL _techbll = new TechnologykeywordBLL();
        _techbll.UpdateTechnology(techId, techName, subTechName);
       
    }


    [System.Web.Services.WebMethod(EnableSession = true)]//In order to store session state in web method we use enablesession
    public static void DeleteTechnologyId(int techId)
    {
        TechnologykeywordBLL _techbll = new TechnologykeywordBLL();
        _techbll.DeleteTechnology(techId);
    }



    private void messageBox(string message)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('" + message + "');", true);
    }
}