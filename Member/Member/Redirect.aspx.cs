using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSCode;
using System.Configuration;

public partial class Member_ProposalCMS_Redirect : Authentication
{
    UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo(); 
        string strGUID = CreateSessionInTextFile();
       
        string p = Convert.ToString(Request.QueryString["p"]);
       
        string Path = ConfigurationManager.AppSettings["ProposalPath"].ToString();

        Response.Redirect(Path + "?p=" + p + "&s=" + strGUID );
       
       
    }

    public string CreateSessionInTextFile()
    {
        Guid obj = Guid.NewGuid();
        string strGUID = Convert.ToString(obj);
        string path = @"D:\TFS\Proposal\Temp\";
        string logFile = path + strGUID + ".txt";
        try
        {
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
            FileInfo MyFile = new FileInfo(logFile);
            MyFile.Create().Close();
            using (StreamWriter sw = new StreamWriter(logFile))
            {
                //sw.WriteLine(strGUID);
                sw.WriteLine(UM.EmployeeID);
                //File.AppendAllText(path, strGUID + ";" + UM.EmployeeID);
                sw.Close();
            }
        }
        catch (Exception ex)
        {

            HttpContext.Current.Response.Write(ex.Message);
        }
        return strGUID;
    }

   
}