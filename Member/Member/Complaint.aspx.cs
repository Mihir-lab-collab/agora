using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using CSCode;

using System.Web.Script.Serialization;





public partial class Complaint : Authentication
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false);

    }

    [System.Web.Services.WebMethod]
    public static string BindComplaints()
    {
        try
        {
            List<ComplaintBLL> lsComplaints = new List<ComplaintBLL>();
            lsComplaints = ComplaintBLL.getall("Get", Convert.ToInt32(HttpContext.Current.Session["ProjectId"]));   
            var data = from pr in lsComplaints
                       select new
                       {
                           pr.compDate,
                           pr.compId,                           
                           pr.projName,
                           pr.compTitle,                           
                           pr.compResolved,
                           pr.compCategory,
                           pr.custName,
                           pr.custRegDate,
                           pr.custEmail,
                           pr.custAddress,
                           pr.custCompany,
                           pr.projId,
                           pr.compDesc,
                           pr.compFeedback                                                   

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
    public static void DeleteComplaint(int compId)
    {
        ComplaintBLL.deletComplaint("Delete", compId); //new ComplaintBLL();             
    }

    [System.Web.Services.WebMethod]
    //UpdateComplaint(compId, compDate, compTitle, compCategory,compDesc, compResolved)
    public static string UpdateComplaint(int compId, string compResolved, string compFeedback)
    {
        string output = "Update Failed";
        try
        {
            ComplaintBLL.saveComplaint("update", compId, compResolved,compFeedback);
        }
        catch (Exception ex)
        {

        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }


    [System.Web.Services.WebMethod]
    public static string BindCategoryDetails()
    {
        try
        {
            List<ComplaintBLL> lsproj = new List<ComplaintBLL>();
            lsproj = ComplaintBLL.getCat("GetCategory");
            var data = from pr in lsproj
                       select new
                       {                        
                           pr.compCategory,
                           pr.ID
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
    public static String GetProjectNameByProjId()
    {
        try
        {
            List<ComplaintBLL> lstGetEmpDetailsByProjId = new List<ComplaintBLL>();
            lstGetEmpDetailsByProjId = ComplaintBLL.getall("Get", Convert.ToInt32(HttpContext.Current.Session["ProjectId"]));

            var data = (from CurGetProjDetailsByProjId in lstGetEmpDetailsByProjId
                        select new
                        {
                            CurGetProjDetailsByProjId.projId,
                            CurGetProjDetailsByProjId.projName
                        }).ToList();
            ;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);

        }
        catch (Exception)
        {
            return null;
        }
    }
    

    
}