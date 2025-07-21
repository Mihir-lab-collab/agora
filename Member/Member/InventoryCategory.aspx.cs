using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using CommonFunctionLib;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Configuration;

public partial class InventoryCategory : Authentication
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        Session["InventoryCatID"] = null;
        if (!IsPostBack)
        {
            BindData();
            if (UM.IsModuleAdmin || UM.IsAdmin)
            {
                gvItemList.Columns[0].Visible = true;
                gvItemList.ShowFooter = true;
                gvItemList.Columns[2].Visible = false;
            }
        }
    }

    private void BindData()
    {
        List<CategoryBLL> lstCat = CategoryBLL.BindCategory("ItemList");
        if (lstCat.Count > 0)
        {
            gvItemList.DataSource = lstCat;
            gvItemList.DataBind();
        }
    }
   
    protected void gvItemList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvItemList.EditIndex = -1;
        BindData();
       
    }
    protected void gvItemList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvItemList.EditIndex = e.NewEditIndex;
        BindData();
        
    }
    protected void gvItemList_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (this.gvItemList.EditIndex != -1)
        {
            HiddenField txtftrCategoryNameEdit = (HiddenField)this.gvItemList.Rows[this.gvItemList.EditIndex].FindControl("hdnEditCategoryID");
            TextBox txtEditName =(TextBox)this.gvItemList.Rows[this.gvItemList.EditIndex].FindControl("txtftrCategoryNameEdit");

            int intCategoryId = Convert.ToInt16(txtftrCategoryNameEdit.Value);
            CategoryBLL objcat = new CategoryBLL();
            objcat.CategoryID = intCategoryId;
            objcat.Name = txtEditName.Text;
            objcat.ModifiedBy = UM.EmployeeID;
            objcat.mode = "Update";
            bool isupdated = CategoryBLL.UpdateCategory(objcat);
          
            if (isupdated == true)
            {
                lblResult.ForeColor = Color.Green;
                lblResult.Text = txtEditName.Text + " Details updated successfully";
                gvItemList.EditIndex = -1;
                BindData();
            }
        }
    }

    protected void gvItemList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int intCategoryID = Convert.ToInt16(gvItemList.DataKeys[e.RowIndex].Values["CategoryID"].ToString());
        CategoryBLL objcat = new CategoryBLL();
        objcat.CategoryID = intCategoryID;
        objcat.mode = "Delete";
        bool isDeleted = CategoryBLL.DeleteCategory(objcat);
        if (isDeleted == true)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Deleted Successfully!');", true);
            BindData();
        }
    }
    protected void lblCategoryName_Click(object sender, EventArgs e)
    {
        //int intCategoryID = Convert.ToInt16(gvItemList.DataKeys[].Values["CategoryID"].ToString());
        //Response.Redirect("~//InventoryAttribute.aspx?id=" + intCategoryId + " &Name=" + txtftrCategoryName.Text.Trim());
    }
    protected void gvItemList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            if (ViewState["Pager"] != null)
            {
                CreatePagerLinkButton(e, gvItemList);
            }
            else
            {
                ArrayList alPager = new ArrayList();
                if (gvItemList.PageCount >= 5)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                else
                {
                    for (int i = 1; i <= gvItemList.PageCount; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                ViewState.Add("Pager", alPager);
                CreatePagerLinkButton(e, gvItemList);
            }
        }
    }
    private void CreatePagerLinkButton(GridViewRowEventArgs e, GridView gvTemp)
    {
        ArrayList alPager = (ArrayList)ViewState["Pager"];
        if (alPager.Contains((gvTemp.PageIndex + 1).ToString()))
        {
            foreach (string page in alPager)
            {
                LinkButton lnkbtnPage = new LinkButton();
                lnkbtnPage.ID = "Page" + page;
                lnkbtnPage.CommandName = "page";
                lnkbtnPage.CommandArgument = (Convert.ToInt32(page) - 1).ToString();
                lnkbtnPage.Click += lnkbtnPage_Click;
                lnkbtnPage.Text = page;
                lnkbtnPage.CssClass = "number";
                if (gvTemp.PageIndex == (Convert.ToInt32(page) - 1))
                {
                    lnkbtnPage.CssClass = "active";
                }
                e.Row.FindControl("divListBottons").Controls.Add(lnkbtnPage);
            }
        }
        else
        {
            if (alPager.Contains((gvTemp.PageIndex - 1).ToString()))
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) + 1).ToString();
                }
            }
            else if (alPager.Contains((gvTemp.PageIndex + 1).ToString()))
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) - 1).ToString();
                }
            }
            else if (Convert.ToInt32(alPager[alPager.Count - 1]) > gvTemp.PageIndex + 1)
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) - 1).ToString();
                }
            }
            else if (Convert.ToInt32(alPager[0]) < gvTemp.PageIndex + 1)
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) + 1).ToString();
                }
            }
            ViewState["Pager"] = alPager;
            CreatePagerLinkButton(e, gvTemp);
        }
    }

    void lnkbtnPage_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        gvItemList.PageIndex = Convert.ToInt32(lnk.CommandArgument);
        BindData();
    }
    protected void imgbtnAdd_Click1(object sender, EventArgs e)
    {
        clsCommon open = new clsCommon();
        TextBox txtftrCategoryName = (TextBox)gvItemList.FooterRow.FindControl("txtftrCategoryName");
        CategoryBLL objCat = new CategoryBLL();
        var category = CategoryBLL.BindCategory("Name", txtftrCategoryName.Text);
        if (!(category.Count > 0))
        {
            objCat.Name = txtftrCategoryName.Text;
            objCat.InsertedBy = UM.EmployeeID;
            objCat.mode = "Insert";
            int id = CategoryBLL.InsertCategory(objCat);
          
            BindData();
            lblResult.ForeColor = Color.Green;
            lblResult.Text = txtftrCategoryName.Text + " Details inserted successfully";

            category = CategoryBLL.BindCategory("Name", txtftrCategoryName.Text);
            int intCategoryId = Convert.ToInt16(category.FirstOrDefault().CategoryID);
            Session["InventoryCatID"] = intCategoryId;
            Response.Redirect("~/Member/InventoryAttribute.aspx");
        }
        else
        {
            BindData();
            lblResult.Text = txtftrCategoryName.Text + " Already Saved";
        }
    }
    protected void lnkCatID_Click(object sender, EventArgs e)
    {
        Session["InventoryCatID"] = ((LinkButton)sender).CommandArgument;
        Response.Redirect("./InventoryAttribute.aspx");
    }
}