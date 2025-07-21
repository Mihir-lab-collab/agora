using Customer.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_Module : Authentication
{
   static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo(); 
    }
    [System.Web.Services.WebMethod]
    public static String FillParentModule()
    {
        try
        {
            List<Modules> lstParentModules = Modules.GetParentModules("GetParentModules");
            var data = (from ItemModule in lstParentModules

                        select new
                        {
                            ItemModule.ModuleID_Parent,
                            ItemModule.ModuleID_ParentName
                        }).ToList();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception ex)
        {
            return null;
        }
    }
    [System.Web.Services.WebMethod]
    public static String BindModule(string Type)
    {
        try
        {
            List<Modules> lstmodule = Modules.BindModules("GetModule", Type, 0);
            var data = from moduleItem in lstmodule
                       select new
                       {
                           moduleItem.ModuleID,
                           moduleItem.ModuleID_ParentName,
                           moduleItem.ModuleID_Parent,
                           moduleItem.Menu,
                           moduleItem.Name,
                           moduleItem.EntryPage,
                           moduleItem.Parameter,
                           moduleItem.IsMenuVisible,
                           moduleItem.IsGenric,
                           moduleItem.SortOrder
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
    public static string UpdateModule(int ModuleID, int ModuleParentID, string DisplayName,string MenuName, string PageURL,string Parameter, bool IsVisible, bool IsGeneric, int SortOrder,string type)
    {
        string output = "Update Failed";
        try
        {
            UM = UserMaster.UserMasterInfo(); 
            Modules updatemodule = new Modules();
            updatemodule.ModuleID = ModuleID;
            updatemodule.ModuleID_Parent =  ModuleParentID;
            updatemodule.Type = type;
            updatemodule.Name = DisplayName;
            updatemodule.Menu = MenuName;
            updatemodule.EntryPage = PageURL == "" ? null : PageURL;
            updatemodule.Parameter = Parameter == "" ? null : Parameter;
            updatemodule.IsMenuVisible = IsVisible;
            updatemodule.IsGenric = IsGeneric;
            updatemodule.SortOrder = SortOrder;
            updatemodule.ModifiedBy = UM.EmployeeID;
            updatemodule.mode = "Update";

            bool isupdated = Modules.UpdateModule(updatemodule);
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
    
 
    protected void lnkSaveModule_Click(object sender, EventArgs e)
    {
        string output = "Insert Failed";
        Modules Insmodule = new Modules();
        Insmodule.ModuleID = 0;
        Insmodule.ModuleID_Parent = Convert.ToInt32(hdnParentID.Value);
        Insmodule.Type = ddlType.SelectedValue;
        Insmodule.Name =hdnDisplayName.Value;
        Insmodule.Menu=hdnMenuName.Value;
        Insmodule.EntryPage = hdnPageURL.Value == "" ? null : hdnPageURL.Value;
        Insmodule.Parameter = hndParameter.Value == "" ? null : hndParameter.Value;
        Insmodule.IsMenuVisible = Convert.ToBoolean(hdnChkIsMenuVisible.Value);
        Insmodule.IsGenric = Convert.ToBoolean(hdnChkIsGeneric.Value);

        if (!string.IsNullOrEmpty(hdnSortOrder.Value))
            Insmodule.SortOrder = Convert.ToInt32(hdnSortOrder.Value);

        Insmodule.InsertedBy = UM.EmployeeID;
        Insmodule.mode = "Insert";

        int id = Modules.InsertModule(Insmodule);
        if (id != 0)
        {
            output = "Inserted successfully.";
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('" + output + "')", true);
    }
}