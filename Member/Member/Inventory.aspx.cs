using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class _Default : Authentication
{
    UserMaster UM;
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        BindData();
        BindInvoiceDetailsData();
    }

    protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        TextBox txtftrCategoryName = (TextBox)gvItemList.FooterRow.FindControl("txtftrCategoryName");
        var category = CategoryBLL.BindCategory("Name", txtftrCategoryName.Text);
        CategoryBLL objCat = new CategoryBLL();
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
            Response.Redirect("~/Member/InventoryAttribute.aspx?id=" + intCategoryId + " &Name=" + txtftrCategoryName.Text.Trim());
        }
        else
        {
            BindData();
            lblResult.Text = txtftrCategoryName.Text + " Already Saved";
        }
    }

    protected void lblCategoryName_Click(object sender, EventArgs e)
    {
    }

    protected void gvItemList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemList.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void txtSlider_TextChanged(object sender, EventArgs e)
    {
        GridViewRow rowPager = gvItemList.BottomPagerRow;
        TextBox txtSliderExt = (TextBox)rowPager.Cells[0].FindControl("txtSlider");
        gvItemList.PageIndex = Int32.Parse(txtSliderExt.Text) - 1;
        BindData();
    }

    protected void gvItemList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Everytime you want to add new rows header, you creat new formatcells variable
            SortedList formatCells = new SortedList();

            formatCells.Add("1", "Inventory Items,1,2");
            formatCells.Add("2", "Quantity,3,1");
            formatCells.Add("3", "Last Purchased Date,1,2");
            formatCells.Add("4", "Last Purchased Price,1,2");
            SortedList formatcells2 = new SortedList();
            formatcells2.Add("1", "Purchased,1,1");
            formatcells2.Add("2", "Expired,1,1");
            formatcells2.Add("3", "Balance,1,1");
            GetMultiRowHeader(e, formatcells2);
            GetMultiRowHeader(e, formatCells);
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
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

    protected void BtnInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("~//Member/InventoryInvoiceDetails.aspx");
    }

    protected void gvInvoiceList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            this.gvInvoiceList.PageIndex = e.NewPageIndex;
            BindInvoiceDetailsData();
            BindData();
        }
        catch { }
    }

    public void GetMultiRowHeader(GridViewRowEventArgs e, System.Collections.SortedList GetCels)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow row = null;
            IDictionaryEnumerator enumCels = GetCels.GetEnumerator();
            row = new GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);
            while (enumCels.MoveNext())
            {
                string[] count = enumCels.Value.ToString().Split(Convert.ToChar(","));
                TableHeaderCell Cell = new TableHeaderCell();
                Cell.RowSpan = Convert.ToInt16(count[2].ToString());
                Cell.ColumnSpan = Convert.ToInt16(count[1].ToString());
                Cell.Controls.Add(new System.Web.UI.LiteralControl(count[0].ToString()));
                Cell.HorizontalAlign = HorizontalAlign.Center;
                // Cell.Attributes.Add("text-align", "center");
                Cell.Style.Add("text-align", "center");
                row.Cells.Add(Cell);
            }
            e.Row.Parent.Controls.AddAt(0, row);
        }
    }

    protected void gvInvoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Everytime you want to add new rows header, you creat new formatcells variable
            SortedList formatCells = new SortedList();
            formatCells.Add("1", "Invoice No,1,1");
            formatCells.Add("2", "Total Price,1,1");
            formatCells.Add("3", "Purchase Date,1,1");
            formatCells.Add("4", "Supplier,1,1");
            formatCells.Add("5", "Note,1,1");
            GetMultiRowHeader(e, formatCells);
        }
        else if (e.Row.RowType == DataControlRowType.Pager)
        {
            if (ViewState["Pager1"] != null)
            {
                CreatePagerLinkButton(e, gvInvoiceList);
            }
            else
            {
                ArrayList alPager = new ArrayList();
                if (gvInvoiceList.PageCount >= 5)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                else
                {
                    for (int i = 1; i <= gvInvoiceList.PageCount; i++)
                    {
                        alPager.Add(i.ToString());
                    }
                }
                ViewState.Add("Pager1", alPager);
                CreatePagerLinkButton(e, gvInvoiceList);
            }
        }
    }

    private void CreatePagerLinkButton(GridViewRowEventArgs e, GridView gvTemp)
    {
        ArrayList alPager = new ArrayList();
        if (gvTemp.ID == "gvInvoiceList")
            alPager = (ArrayList)ViewState["Pager1"];
        else
            alPager = (ArrayList)ViewState["Pager"];

        if (alPager.Contains((gvTemp.PageIndex + 1).ToString()))
        {
            foreach (string page in alPager)
            {
                LinkButton lnkbtnPage = new LinkButton();
                lnkbtnPage.ID = "Page" + page;
                lnkbtnPage.CommandName = "page";
                lnkbtnPage.CommandArgument = (Convert.ToInt32(page) - 1).ToString();
                if (gvTemp.ID == "gvInvoiceList")
                    lnkbtnPage.Click += lnkbtnPagegvInvoiceList_Click;
                else if (gvTemp.ID == "gvItemList")
                    lnkbtnPage.Click += lnkbtnPagegvItemList_Click;

                lnkbtnPage.Text = page;
                lnkbtnPage.CssClass = "number";
                if (gvTemp.PageIndex == (Convert.ToInt32(page) - 1))
                {
                    lnkbtnPage.CssClass = "active";
                }
                if (gvTemp.ID == "gvItemList")
                    e.Row.FindControl("divListBottons1").Controls.Add(lnkbtnPage);
                else
                    e.Row.FindControl("divListBottons2").Controls.Add(lnkbtnPage);
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

            if (gvTemp.ID == "gvInvoiceList")
                ViewState["Pager1"] = alPager;
            else
                ViewState["Pager"] = alPager;
            CreatePagerLinkButton(e, gvTemp);
        }
    }

    void lnkbtnPagegvInvoiceList_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        
        gvInvoiceList.PageIndex = Convert.ToInt32(lnk.CommandArgument);
        BindInvoiceDetailsData();
    }

    void lnkbtnPagegvItemList_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;

        gvItemList.PageIndex = Convert.ToInt32(lnk.CommandArgument);
        BindData();
    }
    #endregion

    #region Methods
    private void BindData()
    {
        List<CategoryBLL> lstCat = CategoryBLL.BindCategory("ItemList");
        if (lstCat.Count > 0)
        {
            DataTable dtItemInventoryList = new DataTable("ItemInventoryList");
            dtItemInventoryList.Columns.Add(new DataColumn("Name", typeof(string)));
            dtItemInventoryList.Columns.Add(new DataColumn("CategoryID", typeof(string)));
            dtItemInventoryList.Columns.Add(new DataColumn("PurchasedQuantity", typeof(string)));
            dtItemInventoryList.Columns.Add(new DataColumn("ExpiredQuantity", typeof(string)));
            dtItemInventoryList.Columns.Add(new DataColumn("BalanceQuantity", typeof(string)));
            dtItemInventoryList.Columns.Add(new DataColumn("LastPurchasedDate", typeof(string)));
            dtItemInventoryList.Columns.Add(new DataColumn("LastPurchasedPrice", typeof(string)));

            List<InventoryBLL> lstExpireQnt = InventoryBLL.BindCatExpireQuantity("ExpQuan");
            List<InventoryBLL> lstLastPDP = InventoryBLL.BindLastPDP("LastPDP");

            foreach (var drCategoryItem in lstCat)
            {
                DataRow drItemInventoryList = dtItemInventoryList.NewRow();
                drItemInventoryList["CategoryID"] = drCategoryItem.CategoryID;
                drItemInventoryList["Name"] = drCategoryItem.Name;
                drItemInventoryList["PurchasedQuantity"] = drCategoryItem.Quantity;

                    lstExpireQnt =   lstExpireQnt.Where(a => a.CategoryID == drCategoryItem.CategoryID).ToList();
                
                    if (lstExpireQnt.Count() > 0)
                        drItemInventoryList["ExpiredQuantity"] = lstExpireQnt.FirstOrDefault().Quantity;
                    else
                        drItemInventoryList["ExpiredQuantity"] = 0;

                drItemInventoryList["BalanceQuantity"] = Convert.ToInt16(drItemInventoryList["PurchasedQuantity"]) - Convert.ToInt16(drItemInventoryList["ExpiredQuantity"]);

                lstLastPDP = lstLastPDP.Where(a => a.CategoryID == drCategoryItem.CategoryID).ToList();
                if (lstLastPDP.Count() > 0)
                {
                    drItemInventoryList["LastPurchasedDate"] = lstLastPDP.FirstOrDefault().PurchaseDate;
                    drItemInventoryList["LastPurchasedPrice"] = lstLastPDP.FirstOrDefault().Price;
                }
                else
                {
                    drItemInventoryList["LastPurchasedDate"] = "";
                    drItemInventoryList["LastPurchasedPrice"] = "";
                }
                dtItemInventoryList.Rows.Add(drItemInventoryList);
            }
            dtItemInventoryList.DefaultView.Sort = "Name";
            dtItemInventoryList.AcceptChanges();

            gvItemList.DataSource = dtItemInventoryList;
            gvItemList.DataBind();
            ViewState.Add("ItemInventoryList", dtItemInventoryList);
        }
    }

    public void BindInvoiceDetailsData()
    {
        try
        {
            List<InventoryBLL> lstInvoice = InventoryBLL.BindInvoiceDetails("SelectAll");
            if (lstInvoice != null)
            {
                if (lstInvoice.Count > 0)
                {
                    this.gvInvoiceList.DataSource = lstInvoice;
                    this.gvInvoiceList.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    protected void lnkcatId_Click(object sender, EventArgs e)
    {
       Session["CategoryID"] = Convert.ToInt32(((LinkButton)sender).CommandArgument);
       Response.Redirect("./ItemDetails.aspx");
    }
    protected void lnkInvoiceItem_Click(object sender, EventArgs e)
    {
        Session["ItemInvoiceID"] = Convert.ToInt32(((LinkButton)sender).CommandArgument);
        Response.Redirect("./InventoryInvoiceDetails.aspx");
    }
}