using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Customer.BLL;

public partial class Member_UploadPolicy : System.Web.UI.Page
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlConnection con = new SqlConnection(_strConnection);
            string com = "select  * from UploadFileType ";
            SqlDataAdapter adpt = new SqlDataAdapter(com, con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlfiletype.DataSource = dt;
            ddlfiletype.DataBind();
            ddlfiletype.DataTextField = "FileType";
            ddlfiletype.DataValueField = "Id";
            ddlfiletype.DataBind();
            ddlfiletype.Items.Insert(0, new ListItem("Select FileType", "0"));
           
        }
        //Response.Redirect("UploadPolicy.aspx");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (ddlfiletype.SelectedValue == "0")
        {
            int FileType = Convert.ToInt32(ddlfiletype.SelectedValue);

            lblMsg.Text = "Please select appropriate file type";
        }


        if (ddlfiletype.SelectedValue == "1" && FileUpload1.HasFile==false)
        {
            lblMsg.Text = "Please Browse File";
        }

        if (ddlfiletype.SelectedValue == "2" && FileUpload1.HasFile == false)
        {
            lblMsg.Text = "Please Browse File";
        }
        if (ddlfiletype.SelectedValue == "3" && FileUpload1.HasFile == false)
        {
            lblMsg.Text = "Please Browse File";
        }
        if (FileUpload1.HasFile)
        {
            //if (ddlfiletype.SelectedValue == "0")

            //{
            //    //int FileTypeId = Convert.ToInt32(ddlfiletype.SelectedValue);

            //    lblMsg.Text = "Please select appropriate file type";

            //}

                if (ddlfiletype.SelectedValue == "1")
            {
                
                DateTime Today = DateTime.Now;
                var Todaydate = Today.ToString("dd/MMM/yyyy hh:mm:ss");
                string MDFY1 = Todaydate.Replace('/', '_');
                string MDFY2 = MDFY1.Replace(' ', '_');
                string MDFY = MDFY2.Replace(':', '_');
                
                //string s1 = "HR_Policy";
                //bool b1 = s1.StartsWith("HR_Policy");
                //look at this but remember the best way is to do this with javascript
                string extension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string[] validFileTypes = { "pdf" };
                bool isValidFile = false;

                string fileName = FileUpload1.PostedFile.FileName;
                for (int i = 0; i < validFileTypes.Length; i++)

                {

                    if (extension == "." + validFileTypes[i])

                    {

                        isValidFile = true;

                        break;

                    }

                }
                //string fileName = "HR_Policy.pdf";
                string CustomFilename = MDFY + "_" + fileName;
                string FileuploadPath = "Member/HrPoliciesFiles/" + MDFY + "_" + fileName; // stored in solution folder
                string DisplayFileURL = "HrPoliciesFiles/" + MDFY + "_" + fileName; // Remove Member/ coz display URL of file
                int FileTypeId = Convert.ToInt32(ddlfiletype.SelectedValue);
                int isupdated = 0;
                FileUpload1.PostedFile.SaveAs(MapPath("~") + "Member/HrPoliciesFiles/" + CustomFilename);
                
                if ( fileName.StartsWith("HR_Policy", StringComparison.InvariantCultureIgnoreCase))
                {
                    isupdated = UploadPolicyBLL.SaveUploadPolicyFile("SAVE", fileName, CustomFilename, FileuploadPath, DisplayFileURL, FileTypeId);
                }
                
                if (isupdated == 1 )
                {
                    lblMsg.Text = fileName + " " + "File Successfully Uploaded";
                    ddlfiletype.ClearSelection();
                }
               else if (!isValidFile)

                {



                    lblMsg.Text = "Invalid File. Please upload  " +

                                   string.Join(",", validFileTypes)+ " "+"file only";

                     ddlfiletype.ClearSelection();

                }

                else
                {
                    lblMsg.Text = "FileName should begin with HR_Policy";
                    //lblMsg.Text = fileName + " " + "Cannot be Inserted" + "  " + "FileName should begin with HR_Policy";
                }
            }

            else if (ddlfiletype.SelectedValue == "2")
                {
                
                DateTime Today = DateTime.Now;
                var Todaydate = Today.ToString("dd/MMM/yyyy hh:mm:ss");
                string MDFY1 = Todaydate.Replace('/', '_');
                string MDFY2 = MDFY1.Replace(' ', '_');
                string MDFY = MDFY2.Replace(':', '_');

                //look at this but remember the best way is to do this with javascript
                string extension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string[] validFileTypes = { "pdf" };
                bool isValidFile = false;

                string fileName = FileUpload1.PostedFile.FileName;
                for (int i = 0; i < validFileTypes.Length; i++)

                {

                    if (extension == "." + validFileTypes[i])

                    {

                        isValidFile = true;

                        break;

                    }

                }
               
                string CustomFilename = MDFY + "_" + fileName;
                string FileuploadPath = "Member/HrPoliciesFiles/" + MDFY + "_" + fileName; // stored in solution folder
                string DisplayFileURL = "HrPoliciesFiles/" + MDFY + "_" + fileName; // Remove Member/ coz display URL of file
                int FileTypeId = Convert.ToInt32(ddlfiletype.SelectedValue);
                int isupdated = 0;
                FileUpload1.PostedFile.SaveAs(MapPath("~") + "Member/HrPoliciesFiles/" + CustomFilename);
                if (fileName.StartsWith("Mediclaim_Policy", StringComparison.InvariantCultureIgnoreCase))
                {
                    isupdated = UploadPolicyBLL.SaveUploadPolicyFile("SAVE", fileName, CustomFilename, FileuploadPath, DisplayFileURL, FileTypeId);
                }
                if (isupdated == 1)
                {
                    lblMsg.Text = fileName + " " + "File Successfully Uploaded";
                    ddlfiletype.ClearSelection();
                }

               else if (!isValidFile)

                {



                    lblMsg.Text = "Invalid File. Please upload  " +

                                   string.Join(",", validFileTypes) + " " + "file only";
                    ddlfiletype.ClearSelection();
                }
                else
                {
                    lblMsg.Text = "FileName should begin with Mediclaim_Policy";
                    //lblMsg.Text = fileName + " " + "Cannot be Inserted" + "  " + "FileName should begin with Mediclaim_Policy";
                }
            }
            else if (ddlfiletype.SelectedValue == "3")
                    {
                        DateTime Today = DateTime.Now;
                        var Todaydate = Today.ToString("dd/MMM/yyyy hh:mm:ss");
                        string MDFY1 = Todaydate.Replace('/', '_');
                        string MDFY2 = MDFY1.Replace(' ', '_');
                        string MDFY = MDFY2.Replace(':', '_');

                        //look at this but remember the best way is to do this with javascript
                        string extension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string[] validFileTypes = { "pdf" };
                bool isValidFile = false;

                string fileName = FileUpload1.PostedFile.FileName;
                for (int i = 0; i < validFileTypes.Length; i++)

                {

                    if (extension == "." + validFileTypes[i])

                    {

                        isValidFile = true;

                        break;

                    }

                }
                //string fileName = FileUpload1.PostedFile.FileName;
                        string CustomFilename = MDFY + "_" + fileName;
                        string FileuploadPath = "Member/HrPoliciesFiles/" + MDFY + "_" + fileName; // stored in solution folder
                        string DisplayFileURL = "HrPoliciesFiles/" + MDFY + "_" + fileName; // Remove Member/ coz display URL of file
                        int FileTypeId = Convert.ToInt32(ddlfiletype.SelectedValue);
                        int isupdated = 0;
                        FileUpload1.PostedFile.SaveAs(MapPath("~") + "Member/HrPoliciesFiles/" + CustomFilename);
                if (fileName.StartsWith("ASH_Policy", StringComparison.InvariantCultureIgnoreCase))
                {
                    isupdated = UploadPolicyBLL.SaveUploadPolicyFile("SAVE", fileName, CustomFilename, FileuploadPath, DisplayFileURL, FileTypeId);
                }
                        if (isupdated == 1)
                        {
                            lblMsg.Text = fileName + " " + "File Successfully Uploaded";
                             ddlfiletype.ClearSelection();
                        }
               else if (!isValidFile)

                {



                    lblMsg.Text = "Invalid File. Please upload  " +

                                   string.Join(",", validFileTypes) + " " + "file only";

                    ddlfiletype.ClearSelection();

                }
                else
                        {
                              lblMsg.Text = "FileName should begin with ASH_Policy";

                    //lblMsg.Text = fileName + " " + "Cannot be Inserted"+"  " +"FileName should begin with ASH_Policy";
                }
                    }
            else if (ddlfiletype.SelectedValue == "0")
            {


                //look at this but remember the best way is to do this with javascript
                string extension = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName);
                string fileName = FileUpload1.PostedFile.FileName;

                int FileTypeId = Convert.ToInt32(ddlfiletype.SelectedValue);

                lblMsg.Text = "Please select appropriate file type";
            }
        }
        
    }

    protected void ddlfiletype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Id"] = ddlfiletype.SelectedItem.Value.ToString();
        Session["FileType"] = ddlfiletype.SelectedItem.Text.Trim();
        

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("UploadPolicy.aspx");
    }
}