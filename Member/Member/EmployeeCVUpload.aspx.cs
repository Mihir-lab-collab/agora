using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;

public partial class Member_EmployeeCVUpload : Authentication
{
    string TargetDir = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    [System.Web.Services.WebMethod]
    public static String GeEmployeeData()
    {
        try
        {
            List<EmployeeCVBLL> lstforInit = new List<EmployeeCVBLL>();
            lstforInit = EmployeeCVBLL.GetEmployeeData();

            var data = (from CurGetProjDetailsByProjId in lstforInit
                        select new EmployeeCVBLL
                        {
                            empid = CurGetProjDetailsByProjId.empid,
                            empName = CurGetProjDetailsByProjId.empName,
                            SkillDesc = CurGetProjDetailsByProjId.SkillDesc,
                            ResumePath = CurGetProjDetailsByProjId.ResumePath,
                            LastUploadedDate = CurGetProjDetailsByProjId.LastUploadedDate,
                            LastUploadedBy = CurGetProjDetailsByProjId.LastUploadedBy,
                            empExperince = CurGetProjDetailsByProjId.empExperince,
                            empJoiningDate = CurGetProjDetailsByProjId.empJoiningDate,
                            PrimarySkill = CurGetProjDetailsByProjId.PrimarySkill,
                            empAddress = CurGetProjDetailsByProjId.empAddress.Replace(System.Environment.NewLine, "<BR>"),
                            projectsWorkingOn = CurGetProjDetailsByProjId.projectsWorkingOn,
                            skillMatrixs=CurGetProjDetailsByProjId.skillMatrixs

                        }).ToList();


            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(data);
        }

        catch (Exception ex)
        {
            return null;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string fileExtension = "";
        string strResume = "";
        int intEmpID = 0;
        string empName = lblempName.Text;
        if (hdnempId.Value != "")
            intEmpID = Convert.ToInt32(hdnempId.Value.ToString());
        try
        {
            string filePath = FileAttachment.PostedFile.FileName;
            string filename = Path.GetFileName(filePath);
            fileExtension = Path.GetExtension(filename).ToLower();
            string contenttype = String.Empty;

            if ((FileAttachment.PostedFile != null) && (FileAttachment.PostedFile.ContentLength > 0))
            {
                switch (fileExtension)
                {
                    case ".doc":
                        contenttype = "application/vnd.ms-word";
                        break;

                    case ".docx":
                        contenttype = "application/vnd.ms-word";
                        break;
                    case ".pdf":
                        contenttype = "application/pdf";
                        break;
                }
                if (contenttype != String.Empty)
                {
                    int index = filename.IndexOf(".");
                    if (index > 0)
                        filename = filename.Substring(0, index);


                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }

        if (string.IsNullOrEmpty(hdnDocName.Value))
            hdnDocName.Value = FileAttachment.FileName;
        strResume = hdnDocName.Value;
        hdnDocName.Value = String.Empty;

        if ((FileAttachment.PostedFile != null) && (FileAttachment.PostedFile.ContentLength > 0))
            if (fileExtension.ToUpper().Contains(".DOC") || fileExtension.ToUpper().Contains(".PDF"))
            {

                TargetDir = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\CVCollection\";

                if (!Directory.Exists(TargetDir))
                {
                    Directory.CreateDirectory(TargetDir);
                }

                FileAttachment.SaveAs(TargetDir + intEmpID + fileExtension);
                EmployeeCVBLL.UpdateEmpData(strResume, intEmpID, 1000); //<----login empid


            }

    }
}