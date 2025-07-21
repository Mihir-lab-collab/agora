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
using System.IO;


public partial class Member_Proposal : Authentication
{
   // ProjectsBLL prj = new ProjectsBLL();
  static  UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
        }
    }
    protected void lnkSaveProject_Click(object sender, EventArgs e)
    {
        string projectTitle,projectDesc,clientMailId=string.Empty;
        projectTitle=hfProjectTitle.Value;
        projectDesc=hfProjectDesc.Value;
        clientMailId=hfClientMailId.Value;
        int InsertedBy = UM.EmployeeID;
        int ProposalMasterID = ProposalsProjectsBLL.InsertProposalsProjects("InsertProposalProject", projectTitle, projectDesc, clientMailId, InsertedBy);
    }

    [System.Web.Services.WebMethod]
    public static string BindProjects()
    {
        try
        {
           UM = UserMaster.UserMasterInfo();
            Boolean IsAdmin = false;
            if (UM.IsAdmin || UM.IsModuleAdmin)
            {
                IsAdmin = true;
            }

            List<ProposalsProjectsBLL> lstGetProjects = ProposalsProjectsBLL.GetProjectDetails(UM.EmployeeID, IsAdmin);
            var data = from curProjects in lstGetProjects
                       select new
                       {
                           curProjects.projectID,
                           curProjects.projectTitle,
                           curProjects.projDesc,
                           curProjects.status,
                           accessCode =Convert.ToString(curProjects.projectID) + "-" + curProjects.createdOn.ToString("hhmm"),
                           curProjects.createdBy,
                           curProjects.createdOn,
                           curProjects.modifiedBy,
                           curProjects.modifiedOn,
                           curProjects.clientMail,
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
    public static string UpdateProposalProjects(int projectID, string projectTitle, string projDesc, string clientMail,string status)
    {
        UM = UserMaster.UserMasterInfo();
         string output = "Update Failed";
         try
         {
             string ModifiedBy = UM.EmployeeID.ToString();
             bool isupdated = ProposalsProjectsBLL.UpdateProposalProjects("UpdateProposalProject", projectID, projectTitle, projDesc, clientMail,status, ModifiedBy);
             if (isupdated == true)
             {
                 output = "projects updated successfully.";
             }
         }
         catch (Exception ex)
         {
         }
         JavaScriptSerializer jss = new JavaScriptSerializer();
         return jss.Serialize(output);
    }
    protected void lnkOK_Click(object sender, EventArgs e)
    {
        string projectTitle, projectDesc, clientMailId = string.Empty;     
        projectTitle = hfProjectTitle.Value;
        projectTitle = projectTitle + "_copy";
        projectDesc = hfProjectDesc.Value;
        clientMailId = hfClientMailId.Value;
        int InsertedBy = UM.EmployeeID;
        int ProposalMasterID = ProposalsProjectsBLL.InsertProposalsProjects("InsertProposalProject", projectTitle, projectDesc, clientMailId, InsertedBy);
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static string SaveProjectDetails(string projectTitle, string projDesc, string clientMail)
    {
        UM = UserMaster.UserMasterInfo();
        string output = "Update Failed";
        try
        {
            int InsertedBy = UM.EmployeeID;
            int ProposalMasterID = ProposalsProjectsBLL.InsertProposalsProjects("InsertProposalProject", projectTitle, projDesc, clientMail, InsertedBy);
        }
        catch (Exception ex)
        {
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }
}

