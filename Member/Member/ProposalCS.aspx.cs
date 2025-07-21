using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Customer.BLL;
using System.Configuration;
using System.IO;
using System.Text;


public partial class Member_ProposalCS : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo(); 
    }

    protected void lnkSaveCSProject_Click(object sender, EventArgs e)
    {
        string projectCSTitle, projectCSUrl, projectCSDesc = string.Empty;
        projectCSTitle = hfProjectCSTitle.Value;
        projectCSUrl = hfProjectCSUrl.Value;
        projectCSDesc = hfProjectCSDesc.Value;
        int InsertedBy = UM.EmployeeID;
        string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
        string contentType = FileUpload1.PostedFile.ContentType;

        if (FileUpload1.HasFile)
        {
            using (Stream fs = FileUpload1.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    int ProposalMasterID = ProposalsProjectsBLL.InsertProposalsCSProjects("InsertProposalCSProject", projectCSTitle, projectCSUrl, projectCSDesc, fileName, contentType, bytes);
                }
            }
        }
    }


    [System.Web.Services.WebMethod]
    public static String BindCSProjects()
    {
        try
        {
            List<ProposalsProjectsBLL> lstGetCSProjects = ProposalsProjectsBLL.GetProjectCSDetails("GetProposalCSProjects");

            var data = from curCSProjects in lstGetCSProjects
                       select new
                       {
                           curCSProjects.ProposalCSDefaultID,
                           curCSProjects.ProjectTitle,
                           curCSProjects.ProjectUrl,
                           curCSProjects.ProjectDesc,
                           curCSProjects.ImageName,
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
    public static string UpdateProposalCSProjects(int projectCSID, string projectCSTitle, string projCSUrl, string projectEditCSDesc, string ImageName, string contentType)
    {
        string output = "Update Failed";

        try
        {
            bool isupdated = ProposalsProjectsBLL.UpdateProposalCSProjects("UpdateProposalCSProject", projectCSID, projectCSTitle, projCSUrl, projectEditCSDesc, ImageName, contentType);
        }
        catch (Exception ex)
        {
        }
        JavaScriptSerializer jss = new JavaScriptSerializer();
        return jss.Serialize(output);
    }

    public static System.Drawing.Image byteArrayToImage(byte[] byteBLOBData)
    {
        MemoryStream ms = new MemoryStream(byteBLOBData);
        System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);       
        return returnImage;
    }
}