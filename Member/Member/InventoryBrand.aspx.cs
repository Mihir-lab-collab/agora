using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InventoryBrand : Authentication
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            BindBrandData();
            if (UM.IsModuleAdmin || UM.IsAdmin)
            {
                gvDetails.Columns[0].Visible = true;
                gvDetails.ShowFooter = true;
            }
        }
    }
    private void BindBrandData()
    {
        List<BrandBLL> lstBrand = BrandBLL.BindBrand("Select");
        if (lstBrand.Count > 0)
        {
            gvDetails.DataSource = lstBrand;
            gvDetails.DataBind();
        }
    }
    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindBrandData();
    }
    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int BrandID = Convert.ToInt16(gvDetails.DataKeys[e.RowIndex].Values["BrandID"].ToString());
        BrandBLL objBr = new BrandBLL();
        objBr.BrandID = BrandID;
        objBr.mode = "Delete";
        BrandBLL.DeleteBrand(objBr);
        BindBrandData();
    }
    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDetails.EditIndex = e.NewEditIndex;
        BindBrandData();
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (this.gvDetails.EditIndex != -1)
        {
            TextBox txtEditName = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("txtEditName") as TextBox;
            TextBox txtDescEdit = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("txtDescEdit") as TextBox;
            HiddenField hdnEditBrandID = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("hdnEditBrandID") as HiddenField;

            BrandBLL objBr = new BrandBLL();
            objBr.BrandID = Convert.ToInt32(hdnEditBrandID.Value);
            objBr.Name = txtEditName.Text;
            objBr.Description = txtDescEdit.Text;
            objBr.ModifiedBy = UM.EmployeeID;
            objBr.mode = "Update";
            BrandBLL.UpdateBrand(objBr);
     
            lblResult.ForeColor = Color.Green;
            lblResult.Text = txtEditName.Text + " Details updated successfully";
            gvDetails.EditIndex = -1;
            BindBrandData();
        }
    }
    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddNew"))
        {
            TextBox txtName = (TextBox)gvDetails.FooterRow.FindControl("txtftrname");
            TextBox txtDesc = (TextBox)gvDetails.FooterRow.FindControl("txtFtDesc");

            BrandBLL objBr = new BrandBLL();
            objBr.BrandID = 0;
            objBr.Name = txtName.Text;
            objBr.Description = txtDesc.Text;
            objBr.InsertedBy = UM.EmployeeID ;
            objBr.mode = "Insert";
            BrandBLL.InsertBrand(objBr);
     
            lblResult.ForeColor = Color.Green;
            lblResult.Text = txtName.Text + " Details inserted successfully";
            BindBrandData();
        }
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Pager)
        {
            if (ViewState["Pager"] != null)
            {
                CreatePagerLinkButton(e);
            }
            else
            {
                ArrayList alPager = new ArrayList();
                if (gvDetails.PageCount >= 5)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                else
                {
                    for (int i = 1; i <= gvDetails.PageCount; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                ViewState.Add("Pager", alPager);
                CreatePagerLinkButton(e);
            }
        }
    }
    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDetails.PageIndex = e.NewPageIndex;
        BindBrandData();
    }
    private void CreatePagerLinkButton(GridViewRowEventArgs e)
    {
        ArrayList alPager = (ArrayList)ViewState["Pager"];
        if (alPager.Contains((gvDetails.PageIndex + 1).ToString()))
        {
            foreach (string page in alPager)
            {
                LinkButton lnkbtnPage = new LinkButton();
                lnkbtnPage.ID = "Page" + page;
                lnkbtnPage.CommandName = "page";
                lnkbtnPage.CommandArgument = (Convert.ToInt32(page) - 1).ToString();
                lnkbtnPage.Click += lnkbtnPage_Click;
                lnkbtnPage.Text = page;
                lnkbtnPage.CausesValidation = false;
                lnkbtnPage.CssClass = "number";
                if (gvDetails.PageIndex == (Convert.ToInt32(page) - 1))
                {
                    lnkbtnPage.CssClass = "active";
                }
                e.Row.FindControl("divListBottons").Controls.Add(lnkbtnPage);
            }
        }
        else
        {
            if (alPager.Contains((gvDetails.PageIndex - 1).ToString()))
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) + 1).ToString();
                }
            }
            else if (alPager.Contains((gvDetails.PageIndex + 1).ToString()))
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) - 1).ToString();
                }
            }
            else if (Convert.ToInt32(alPager[alPager.Count - 1]) > gvDetails.PageIndex + 1)
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) - 1).ToString();
                }
            }
            else if (Convert.ToInt32(alPager[0]) < gvDetails.PageIndex + 1)
            {
                for (int i = 0; i < alPager.Count; i++)
                {
                    alPager[i] = (Convert.ToInt32(alPager[i]) + 1).ToString();
                }
            }
            ViewState["Pager"] = alPager;
            CreatePagerLinkButton(e);
        }
    }

    void lnkbtnPage_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        gvDetails.PageIndex = Convert.ToInt32(lnk.CommandArgument);
        gvDetails.DataBind();
    }
}