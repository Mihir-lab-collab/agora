using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.DAL;

/// <summary>
/// Summary description for UploadPolicyBLL
/// </summary>
public class UploadPolicyBLL
{
    public string FileName { get; set; }
    public int FileTypeId { get; set; }
    public string CustomFileName { get; set; }
    public string FileUploadPath { get; set; }  
    public string DisplayFileURL { get; set; }  
    public int IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public UploadPolicyBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static int SaveUploadPolicyFile(string mode, string FileName, string CustomFilename, string FileuploadPath,string DisplayFileURL, int FileTypeId)
    {
        UploadPolicyBLL objuploadPolicyBLL = new UploadPolicyBLL();
        objuploadPolicyBLL.FileName = FileName;
        objuploadPolicyBLL.CustomFileName = CustomFilename;
        objuploadPolicyBLL.FileUploadPath = FileuploadPath;
        objuploadPolicyBLL.DisplayFileURL = DisplayFileURL;
        objuploadPolicyBLL.FileTypeId = FileTypeId;
        UploadPolicyDAL objDAL = new UploadPolicyDAL();
        return objDAL.SaveUploadPolicyFile(mode, objuploadPolicyBLL);
    }

    public static List<UploadPolicyBLL> GetHrPolicyFilesDetails(string mode)
    {
        UploadPolicyDAL objDAL = new UploadPolicyDAL();
        return objDAL.GetHrPolicyFilesDetails(mode);
    }
}