using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Authentication
{
    UserMaster UM;
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UM = UserMaster.UserMasterInfo();
            BindingGridViewData();
        }
    }

    protected void gvItemList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            this.gvItemList.PageIndex = e.NewPageIndex;
            BindingGridViewData();
        }
        catch
        {
        }
    }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~//Member/Inventory.aspx");
    }

    protected void gvItemList_Sorting(object sender, GridViewSortEventArgs e)
    {

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
        BindingGridViewData();
    }
    #endregion

    #region Methods
    public void BindingGridViewData()
    {
        try
        {
            int categoryID = Convert.ToInt32(Session["CategoryID"]);
            List<InventoryBLL> lstInvoiceCat = InventoryBLL.BindInvoiceDetailsByCategory("CategoryWise", categoryID);
            if (lstInvoiceCat.Count > 0)
            {
                this.gvItemList.DataSource = lstInvoiceCat;
                this.gvItemList.DataBind();
            }
            else
            {
                this.gvItemList.DataSource = null;
                this.gvItemList.DataBind();
            }
            lblItemDetails.Text = "Item details for category : " + lstInvoiceCat.FirstOrDefault().CategoryName;
        }
        catch
        {
        }
    }
    #endregion    
   
    protected void lnkInvoiceItem_Click(object sender, EventArgs e)
    {
        Session["ItemInvoiceID"] = ((LinkButton)sender).CommandArgument;
      
        Response.Redirect("./InventoryInvoiceDetails.aspx?pr=1");
    }
}