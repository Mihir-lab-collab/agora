using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;

public partial class Member_ProjectModule : Authentication
{
    UserMaster UM;
    clsCommon common = new clsCommon();
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Admin MasterPage = (Admin)Page.Master;
        MasterPage.MasterInit(true, false);
        lblmsgpop.Text = "";

        if (!IsPostBack)
        {
            FillModuleType();
        }
    }

    private void FillModuleType()
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);


        SqlCommand cmd = new SqlCommand("Select ProjectModuleTypeID,Name from ProjectModuleType order by Name", con);

        SqlDataAdapter da = new SqlDataAdapter(cmd);

        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlModuleType.DataTextField = ds.Tables[0].Columns["Name"].ToString();
        ddlModuleType.DataValueField = ds.Tables[0].Columns["ProjectModuleTypeID"].ToString();
        ddlModuleType.DataSource = ds.Tables[0];
        ddlModuleType.DataBind();
        ddlModuleType.Items.Insert(0, new ListItem("--Select--", "0"));
        con.Close();
    }

    [WebMethod]
    public static string checkModuleNameExists(int ProjID, string RefProjID, string ModName)
    {
        clsCommon open = new clsCommon();
        string msg = "";
        bool ModNameExists = open.checkModName(Convert.ToInt16(ProjID), RefProjID, ModName);
        if (ModNameExists)
            msg = "Module Name already exists.";

        return msg;
    }

    #region TreeDisplay

    protected void btnShowtree_Click(object sender, EventArgs e)
    {

        ShowTreeData();

    }

    public void ShowTreeData()
    {
        string str = hfProjID.Value;
        bool IsActive=false;
        lnkprojname.Text = hfdProjName.Value;
        if (str != "")
        {
            pnlTree.Style.Add("display", "normal");


            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);



            SqlCommand cmdStates = new SqlCommand("SELECT projectModuleMaster.moduleId As Id,IsNull(projectModuleMaster.moduleName,'') AS StateName,IsNull(ProjectModuleType.Name,'') AS ModuleType,projectModuleMaster.IsActive FROM projectModuleMaster LEFT JOIN ProjectModuleType ON projectModuleMaster.ProjectModuleTypeID = ProjectModuleType.ProjectModuleTypeID where moduleId=moduleRefId and projid=" + str + "", con);
            DataSet dsStates = new DataSet();
            SqlDataAdapter daStates = new SqlDataAdapter(cmdStates);
            daStates.Fill(dsStates);

            int StatesCount = dsStates.Tables[0].Rows.Count;
            string[] Names = new string[StatesCount];
            int i = 0;

            TreeViewProjectModeles.Nodes.Clear();
            foreach (DataRow drStates in dsStates.Tables[0].Rows)
            {
                string StateName = Convert.ToString(drStates["StateName"]);
                if (Convert.ToString(drStates["ModuleType"]) != "")
                {
                    StateName += " " + "[" + Convert.ToString(drStates["ModuleType"]) + "]";
                }
                if (Convert.ToBoolean(drStates["IsActive"]) == false)
                {
                    StateName = "<span style=\"color: #D8CCCC\">" + StateName + "</span>";
                }
                TreeViewProjectModeles.Nodes.Add(new TreeNode(StateName, drStates["Id"].ToString()));//drStates["StateName"].ToString()
                Names[i] = drStates["id"].ToString();
                i++;
            }
            if (i > 0)
            {

                string DistrictsQuery = "(";
                i = 0;
                for (int j = 0; j < StatesCount; j++)
                {
                    if (j == 0)
                        DistrictsQuery += "moduleRefId = " + Names[j];
                    else
                        DistrictsQuery += " or moduleRefId = " + Names[j];

                }
                DistrictsQuery += ")";
                DistrictsQuery += " and moduleId<>moduleRefId and projid=" + str;


                string sqlstr = "Select projectModuleMaster.moduleid as Id,projectModuleMaster.moduleRefId as StateId,IsNull(projectModuleMaster.moduleName,'') AS StateName,IsNull(ProjectModuleType.Name,'') AS ModuleType,projectModuleMaster.IsActive from projectmodulemaster LEFT JOIN ProjectModuleType ON projectModuleMaster.ProjectModuleTypeID = ProjectModuleType.ProjectModuleTypeID where " + DistrictsQuery;

                SqlCommand cmdDistricts = new SqlCommand(sqlstr, con);
                DataSet dsDistricts = new DataSet();
                SqlDataAdapter daDistricts = new SqlDataAdapter(cmdDistricts);
                daDistricts.Fill(dsDistricts);

                StatesCount = dsDistricts.Tables[0].Rows.Count;
                Names = new string[StatesCount];

                foreach (DataRow drDistricts in dsDistricts.Tables[0].Rows)
                {
                    string StateName = Convert.ToString(drDistricts["StateName"]);
                    if (Convert.ToString(drDistricts["ModuleType"]) != "")
                    {
                        StateName += " " + "[" + Convert.ToString(drDistricts["ModuleType"]) + "]";
                    }
                    IsActive = CheckForParentIsActive(drDistricts["StateId"].ToString());
                    if(IsActive==false || Convert.ToBoolean(drDistricts["IsActive"])==false)
                    {
                        StateName = "<span style=\"color: #D8CCCC\">" + StateName + "</span>";
                    }
                    TreeNode child = new TreeNode
                    {
                        Text = StateName,
                        Value = drDistricts["Id"].ToString()
                    };

                    TreeViewProjectModeles.FindNode(drDistricts["StateId"].ToString()).ChildNodes.Add(child);
                    PopulatesubchildLevel2(drDistricts["Id"].ToString(), child,IsActive);
                }
            }

            TreeViewProjectModeles.ExpandAll();


        }
        else
        {
            pnlTree.Style.Add("display", "none");
        }

    }
    private void PopulatesubchildLevel2(string parentId, TreeNode tn,bool isactive)
    {
        DataTable dtChilds = GetDataForChilds(parentId);
        bool isActive = false;
        foreach (DataRow row in dtChilds.Rows)
        {
            string str1 = Convert.ToString(row["Name"]);
            if (Convert.ToString(row["ModuleType"]) != "")
            {
                str1 += " " + "[" + Convert.ToString(row["ModuleType"]) + "]";
            }
            isActive = CheckForParentIsActive(parentId);
            if (isActive == false || isactive == false || Convert.ToBoolean(row["IsActive"]) == false)
            {
                str1 = "<span style=\"color: #D8CCCC\">" + str1 + "</span>";
            }
            TreeNode child = new TreeNode
            {
                Text = str1,
                Value = row["Id"].ToString()
            };
            tn.ChildNodes.Add(child);

            DataTable dtChild = this.GetDataForChilds(child.Value);
            PopulatesubchildLevel3(child.Value, child, isActive, isactive);
        }
    }
    private void PopulatesubchildLevel3(string parentId, TreeNode tn,bool perentActive,bool rootActive)
    {
        DataTable dtChilds = GetDataForChilds(parentId);
        bool isActive = false;
        foreach (DataRow row in dtChilds.Rows)
        {
            string str1 = Convert.ToString(row["Name"]);
            if (Convert.ToString(row["ModuleType"]) != "")
            {
                str1 += " " + "[" + Convert.ToString(row["ModuleType"]) + "]";
            }
            isActive = CheckForParentIsActive(parentId);
            if (isActive == false || perentActive == false || rootActive == false || Convert.ToBoolean(row["IsActive"]) == false)
            {
                str1 = "<span style=\"color: #D8CCCC\">" + str1 + "</span>";
            }
            TreeNode child = new TreeNode
            {
                Text = str1,
                Value = row["Id"].ToString()
            };
            tn.ChildNodes.Add(child);
            DataTable dtChild = this.GetDataForChilds(child.Value);
            PopulatesubchildLevel4(child.Value, child,isActive,perentActive,rootActive);
        }
    }
    private void PopulatesubchildLevel4(string parentId, TreeNode tn,bool perantActive1,bool perentActive,bool rootActive)
    {
        DataTable dtChilds = GetDataForChilds(parentId);
        bool isActive;
        foreach (DataRow row in dtChilds.Rows)
        {
            string str1 = Convert.ToString(row["Name"]);
            if (Convert.ToString(row["ModuleType"]) != "")
            {
                str1 += " " + "[" + Convert.ToString(row["ModuleType"]) + "]";
            }
            isActive = CheckForParentIsActive(parentId);
            if (isActive == false || perentActive == false || rootActive == false || perantActive1 == false || Convert.ToBoolean(row["IsActive"]) == false)
            {
                str1 = "<span style=\"color: #D8CCCC\">" + str1 + "</span>";
            }
            TreeNode child = new TreeNode
            {
                Text = str1,
                Value = row["Id"].ToString()
            };
            tn.ChildNodes.Add(child);
            DataTable dtChild = this.GetDataForChilds(child.Value);
            PopulatesubchildLevel5(child.Value, child, isActive, perantActive1, perentActive, rootActive);
        }
    }
    private void PopulatesubchildLevel5(string parentId, TreeNode tn, bool perantActive2, bool perantActive1, bool perentActive, bool rootActive)
    {
        DataTable dtChilds = GetDataForChilds(parentId);
        bool isActive;
        foreach (DataRow row in dtChilds.Rows)
        {
            string str1 = Convert.ToString(row["Name"]);
            if (Convert.ToString(row["ModuleType"]) != "")
            {
                str1 += " " + "[" + Convert.ToString(row["ModuleType"]) + "]";
            }
            isActive = CheckForParentIsActive(parentId);
            if (isActive == false || perentActive == false || rootActive == false || perantActive1 == false || perantActive2 == false || Convert.ToBoolean(row["IsActive"]) == false)
            {
                str1 = "<span style=\"color: #D8CCCC\">" + str1 + "</span>";
            }

            TreeNode child = new TreeNode
            {
                Text = str1,
                Value = row["Id"].ToString()
            };
            tn.ChildNodes.Add(child);

        }
    }
    private DataTable GetDataForChilds(String parentId)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);

        SqlCommand cmdStates = new SqlCommand("Select projectModuleMaster.moduleid as Id,IsNull(projectModuleMaster.moduleName,'') AS Name,IsNull(ProjectModuleType.Name,'') AS ModuleType,projectmodulemaster.IsActive from projectmodulemaster LEFT JOIN ProjectModuleType ON projectModuleMaster.ProjectModuleTypeID = ProjectModuleType.ProjectModuleTypeID where moduleRefId=" + parentId + "", con);
        DataSet dsChilds = new DataSet();
        SqlDataAdapter daChild = new SqlDataAdapter(cmdStates);
        daChild.Fill(dsChilds);
        return dsChilds.Tables[0];
    }
    #endregion

    #region TreeAction
    protected void TreeViewProjectModeles_SelectedNodeChanged(object sender, EventArgs e)
    {
        lblmsgpop.Text = "";
        btneditPop.Style.Add("display", "none");
        txtModEditPop.Style.Add("display", "none");
        txtEstEditPop.Style.Add("display", "none");
        txtDescEditPop.Style.Add("display", "none");

        string NodVal = TreeViewProjectModeles.SelectedNode.Value;
        //if (TreeViewProjectModeles.SelectedNode.Parent != null)
        //{
        //    string parentId = TreeViewProjectModeles.SelectedNode.Parent.Value;
        //    CheckForParentIsActive(parentId);
        //}
        HttpContext.Current.Session["NodeVal"] = NodVal;
        lblNamepop.Text = TreeViewProjectModeles.SelectedNode.Text;
        ddlModuleType.SelectedIndex = 0;
        
        GetForEdit(NodVal);

        ModalPopupExtender1.Show();


    }
    protected bool CheckForParentIsActive(string parentId)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        string sqlquery = "select IsActive from projectmodulemaster where moduleid=" + parentId + "";
        SqlCommand cmdStates = new SqlCommand(sqlquery, con);
        con.Open();
        bool Isactive = (bool)cmdStates.ExecuteScalar();
        return Isactive;

    }
    protected void lnkprojname_Click(object sender, EventArgs e)
    {
        lblmsgpop.Text = "";
        btneditPop.Style.Add("display", "none");
        txtModEditPop.Style.Add("display", "none");
        txtEstEditPop.Style.Add("display", "none");
        txtDescEditPop.Style.Add("display", "none");
        lblNamepop.Text = lnkprojname.Text;
        HttpContext.Current.Session["NodeVal"] = "";
        ModalPopupExtender1.Show();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ClientScript", "hideforEdit();", true);
    }
    private void GetForEdit(string nodid)
    {


        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        string sqlquery = "select  moduleEstimate,moduleDescription,ProjectModuleTypeID,moduleName,IsActive from projectmodulemaster where moduleid=" + nodid + "";
        SqlCommand cmdStates = new SqlCommand(sqlquery, con);
        con.Open();
        SqlDataReader reader = cmdStates.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {

                txtEstEditPop.Text = Convert.ToString(reader.GetValue(0));
                txtDescEditPop.Text = Convert.ToString(reader.GetValue(1));
                hdnModuletype.Value = Convert.ToString(reader.GetValue(2));
                txtModEditPop.Text = Convert.ToString(reader.GetValue(3));
                if(Convert.ToBoolean(reader.GetValue(4))==true)
                {
                    btnInactive.Text = "INACTIVE";
                }
                else
                {
                    btnInactive.Text = "ACTIVE";
                }
            }
        }
        else
        {
            txtEstEditPop.Text = "";
            txtDescEditPop.Text = "";
            txtModEditPop.Text = "";

        }
        reader.Close();

    }
    #endregion

    #region Modelpopup_Actions

    protected void btnclose_Click(object sender, EventArgs e)
    {
        txtmodnamepopup.Text = "";
        txtestimate.Text = "";
        txtdesc.Text = "";
        lblmsgpop.Text = "";
        if (TreeViewProjectModeles.SelectedNode != null)
        {
            TreeViewProjectModeles.SelectedNode.Selected = false;
        }
        ModalPopupExtender1.Hide();
    }

    protected void btndadds_Click(object sender, EventArgs e)
    {
        string RefModID = "0";
        try
        {
            if (hfProjID.Value != "" && txtmodnamepopup.Text != "" && txtdesc.Text.Length < 255)
            {
                if (HttpContext.Current.Session["NodeVal"].ToString() != "")
                {
                    RefModID = HttpContext.Current.Session["NodeVal"].ToString();
                }

                string msg = checkModuleNameExists(Convert.ToInt16(hfProjID.Value), RefModID, txtmodnamepopup.Text);
                if (string.IsNullOrEmpty(msg))
                {
                    lblmsgpop.Text = "";
                    if (txtestimate.Text == "")
                    {
                        txtestimate.Text = "0";
                    }

                    if (RefModID == "0")
                    {
                        common.InsertModuleDetails(Convert.ToUInt16(ddlModuleType.SelectedItem.Value), Convert.ToUInt16(hfProjID.Value), Convert.ToUInt16(10), Convert.ToUInt16(txtestimate.Text), txtmodnamepopup.Text, txtdesc.Text);
                        string ModuleID = common.GetModuleID();
                        if (ModuleID != "")
                        {
                            common.UpdateModuleRefID(Convert.ToUInt16(ModuleID));
                        }
                    }
                    else
                    {
                        common.InsertModuleDetails(Convert.ToUInt16(ddlModuleType.SelectedItem.Value), Convert.ToUInt16(hfProjID.Value), Convert.ToUInt16(RefModID),
                            Convert.ToUInt16(txtestimate.Text), txtmodnamepopup.Text, txtdesc.Text);
                    }

                    btnShowtree_Click(sender, e);
                }
                else
                {

                    lblmsgpop.Text = msg;

                }
            }

        }

        catch (Exception ex)
        {
            throw (ex);
        }
        txtmodnamepopup.Text = "";
        txtdesc.Text = "";
        txtestimate.Text = "";
        if (TreeViewProjectModeles.SelectedNode != null)
        {
            TreeViewProjectModeles.SelectedNode.Selected = false;
        }
    }

    protected void btneditPop_Click(object sender, EventArgs e)
    {

        if (txtModEditPop.Text != "" && Convert.ToString(HttpContext.Current.Session["NodeVal"]) != "" && txtDescEditPop.Text.Length < 256)
        {
            if (txtEstEditPop.Text == "")
            {
                txtEstEditPop.Text = "0";
            }
            common.UpdateModule(Convert.ToUInt16(ddlModuleType.SelectedItem.Value), txtModEditPop.Text, txtDescEditPop.Text, Convert.ToUInt16(txtEstEditPop.Text), Convert.ToUInt16(HttpContext.Current.Session["NodeVal"]));

            if (TreeViewProjectModeles.SelectedNode != null)
            {
                TreeViewProjectModeles.SelectedNode.Selected = false;
            }
            lblmsgpop.Text = "Record updated successfully for module : " + txtModEditPop.Text + "";
        }
        ShowTreeData();
    }

    protected void btndelpop_Click(object sender, EventArgs e)
    {

        if (hfProjID.Value != "")
        {
            string sqlquery = string.Empty;


            if (Convert.ToString(HttpContext.Current.Session["NodeVal"]) != "")
            {
                sqlquery = "Delete  from projectModuleMaster where moduleId=" + HttpContext.Current.Session["NodeVal"] + "";
            }

            if (Convert.ToString(HttpContext.Current.Session["NodeVal"]) == "")
            {
                sqlquery = "Delete from projectModuleMaster where projId=" + hfProjID.Value + "";
            }
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                SqlCommand cmdStates = new SqlCommand(sqlquery, con);
                cmdStates.ExecuteNonQuery();
                lblmsgpop.Text = "Record deleted successfully.";
                btnShowtree_Click(sender, e);
            }
            catch (Exception ex)
            {
                lblmsgpop.Text = "Can not delete because of referential integrity.";
                if (TreeViewProjectModeles.SelectedNode != null)
                {
                    TreeViewProjectModeles.SelectedNode.Selected = false;
                }
            }

        }

    }

    protected void btnInactive_Click(object sender, EventArgs e)
    {
        if (hfProjID.Value != "")
        {
            string sqlquery = string.Empty;
            int inactive = 0;
            string msg = string.Empty;
            if (btnInactive.Text == "ACTIVE")
            {
                inactive = 1;
                msg = "Record Active successfully.";
            }
            else
            {
                inactive = 0;
                msg = "Record Inactive successfully.";
            }

            if (Convert.ToString(HttpContext.Current.Session["NodeVal"]) != "")
            {
                sqlquery = "update projectModuleMaster set IsActive="+ inactive +" where moduleId=" + HttpContext.Current.Session["NodeVal"] + "";
            }

            if (Convert.ToString(HttpContext.Current.Session["NodeVal"]) == "")
            {
                sqlquery = "update projectModuleMaster set IsActive=" + inactive + " where projId=" + hfProjID.Value + "";
            }
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                con.Open();
                SqlCommand cmdStates = new SqlCommand(sqlquery, con);
                cmdStates.ExecuteNonQuery();
                lblmsgpop.Text = msg;
                btnShowtree_Click(sender, e);
            }
            catch (Exception ex)
            {
                lblmsgpop.Text = "Can not Inactive because of referential integrity.";
                if (TreeViewProjectModeles.SelectedNode != null)
                {
                    TreeViewProjectModeles.SelectedNode.Selected = false;
                }
            }

        }
    }
    #endregion
}