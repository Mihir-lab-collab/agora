using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;

public partial class Member_Filter : Authentication
{
    static UserMaster UM;
    public static System.Data.DataTable  createFilterTable()
    {
           System.Data.DataTable dtFilterSearch = new System.Data.DataTable();
       
            System.Data.DataColumn dcSrNo = new System.Data.DataColumn("SrNo", typeof(System.Int32));
            dcSrNo.AutoIncrement = true;
            dcSrNo.AutoIncrementSeed = 1;
            dcSrNo.AutoIncrementStep = 1;
            dtFilterSearch.Columns.Add(dcSrNo);
            dtFilterSearch.PrimaryKey = new System.Data.DataColumn[] { dcSrNo };
            System.Data.DataColumn dcColumName = new System.Data.DataColumn("ColumnName", typeof(System.String));
            System.Data.DataColumn dcData = new System.Data.DataColumn("Data", typeof(System.String));
            dtFilterSearch.Columns.Add(dcColumName);
            dtFilterSearch.Columns.Add(dcData);
            System.Data.DataRow dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "No";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Task";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Posted On";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Last Modified";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Projects";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Priority";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Assigned To";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);

            dr = dtFilterSearch.NewRow();
            dr["ColumnName"] = "Status";
            dr["Data"] = "ALL";
            dtFilterSearch.Rows.Add(dr);


        return dtFilterSearch;
    }

   // [System.Web.Services.WebMethod]
    //public static String FilterData(String strEmpID, String strColumn)
    //{
        
    //    DataTable dtFilterSearch = createFilterTable();
    //    List<string> lst = new List<string>();
       
    //    var UnqiueKM = from r in dtFilterSearch.AsEnumerable()
    //                   group r by new { KM = r["ColumnName"] } into g
    //                           select g;
        
    //    JavaScriptSerializer jss = new JavaScriptSerializer();
    //    return jss.Serialize(lst);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo(); 
    }



    [System.Web.Services.WebMethod]
    public static String FilterData(String strEmpID, String strColumn)
    {
        clsCommon open = new clsCommon();
        System.Data.DataTable dtFilter = open.GetDataForFilter(strEmpID, strColumn);
        List<string> lst = new List<string>();
        foreach (System.Data.DataRow drItem in dtFilter.Rows)
        {
            lst.Add(drItem[0].ToString());
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(lst);
    }

    [System.Web.Services.WebMethod]
    public static String FilterProject()
    {
        UM = UserMaster.UserMasterInfo(); 
        clsCommon open = new clsCommon();
        System.Data.DataTable dtFilter = open.GetProjectName(UM.EmployeeID.ToString());
        List<projectData> lst = new List<projectData>();
        foreach (System.Data.DataRow drItem in dtFilter.Rows)
        {
            projectData projectdata = new projectData(Convert.ToInt32(drItem[0].ToString()), drItem[1].ToString());
            lst.Add(projectdata);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(lst);
    }

    [System.Web.Services.WebMethod]
    public static String FilterModule(String _parentId)
    {
        clsCommon open = new clsCommon();
        System.Data.DataTable dtFilter = open.GetModuleName(_parentId);
        List<ModuleData> lst = new List<ModuleData>();
        foreach (System.Data.DataRow drItem in dtFilter.Rows)
        {
            ModuleData Moduledata = new ModuleData(Convert.ToInt32(drItem[0].ToString()), drItem[1].ToString());
            lst.Add(Moduledata);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(lst);
    }

    [System.Web.Services.WebMethod]
    public static String FilterModuleByID(String _ModuleId)
    {
        clsCommon open = new clsCommon();
        System.Data.DataTable dtFilter = open.GetModuleNameByID(_ModuleId);
        List<ModuleData> lst = new List<ModuleData>();
        foreach (System.Data.DataRow drItem in dtFilter.Rows)
        {
            ModuleData Moduledata = new ModuleData(Convert.ToInt32(drItem[0].ToString()), drItem[1].ToString());
            Moduledata.ModuleDescription = Convert.ToString(drItem[2]);
            Moduledata.ModuleEstimate = Convert.ToInt32(Convert.ToString(drItem[3]) == "" ? 0 : drItem[3]);
            lst.Add(Moduledata);
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(lst);
    }
}
public class projectData
{
    public projectData(int intprojid, string strprojname)
    {
        projid = intprojid;
        projName = strprojname;
    }

    public int projid
    {
        get;
        set;
    }
    public string projName
    {
        get;
        set;
    }

}

public class ModuleData
{
    public ModuleData(int intmodid, string strModulename)
    {
        modID = intmodid;
        ModuleName = strModulename;
    }

    public int modID
    {
        get;
        set;
    }
    public string ModuleName
    {
        get;
        set;
    }
    public string ModuleDescription
    {
        get;
        set;
    }
    public int ModuleEstimate
    {
        get;
        set;
    }
}