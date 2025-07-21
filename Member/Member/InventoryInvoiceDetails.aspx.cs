using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_InventoryInvoiceDetails : Authentication
{
    #region Variables
    UserMaster UM;
    DateTime dtExpire;
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            LoadPage();
        }
        BindData();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearUserEnteredValues();
        btnSave.Text = "SAVE";
        panelItemDetails.Visible = false;
        btnAdd.Enabled = true;
        CategoryName.Enabled = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                Random r = new Random();
                if (hdnAutoSupplierID.Value == "0")
                {
                    SupplierBLL SupIns = new SupplierBLL();
                    SupIns.SupplierID = 0;
                    SupIns.Name = txtSupplier.Text;
                    SupIns.mode = "Insert";
                    SupIns.InsertedBy = UM.EmployeeID;
                    int id = SupplierBLL.InsertSupplier(SupIns);
                    hdnAutoSupplierID.Value = Convert.ToString(id);
                }

                if (hdnAutoBrandID.Value == "0")
                {
                    BrandBLL objBr = new BrandBLL();
                    objBr.BrandID = 0;
                    objBr.Name = txtBrand.Text;
                   // objBr.Description = txtDesc.Text;
                    objBr.InsertedBy = UM.EmployeeID;
                    objBr.mode = "Insert";
                   int BrandId = BrandBLL.InsertBrand(objBr);
                   hdnAutoBrandID.Value = Convert.ToString(BrandId);
                }
               
                dtExpire = DateTime.ParseExact(txtPurchaseDate.Text.Trim() == "" ? DateTime.Now.ToString("dd/MM/yyyy") : txtPurchaseDate.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dtExpire = dtExpire.AddYears(Convert.ToInt16(txtYear.Text.Trim() == "" ? "0" : txtYear.Text.Trim()));
                dtExpire = dtExpire.AddMonths(Convert.ToInt16(txtMonth.Text == "" ? "0" : txtMonth.Text.Trim()));
                dtExpire = dtExpire.AddDays(Convert.ToInt16(txtDay.Text == "" ? "0" : txtDay.Text.Trim()));

                if (Convert.ToInt16(hdnItemInvoiceID.Value) > 0)
                {

                    DataTable dtItemId = null;
                    dtItemId = new DataTable();
                    dtItemId.Columns.Add("ItemID");
                    DataRow drNew = dtItemId.NewRow();
                    if (btnSave.Text == "SAVE")
                    {
                        InventoryBLL objItem = new InventoryBLL();
                        objItem.ItemID =0;
                        objItem.BrandID =Convert.ToInt32(hdnAutoBrandID.Value);
                        objItem.CategoryID = Convert.ToInt16(CategoryName.SelectedValue);
                        objItem.ItemInvoiceID=Convert.ToInt16(hdnItemInvoiceID.Value);
                        objItem.SupplierID = Convert.ToInt32(hdnAutoSupplierID.Value);
                        objItem.Description = txtDescription.Text.Trim();
                        objItem.Price = Convert.ToSingle(txtPrice.Text);
                        objItem.ExpiryDate = dtExpire;
                        objItem.Quantity = Convert.ToInt32(txtQuantity1.Text);
                        objItem.SerialNo = txtSerialNo.Text == "" ? "Null" : txtSerialNo.Text;
                        objItem.PurchaseDate = DateTime.ParseExact(txtpurdate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objItem.InsertedBy = UM.EmployeeID;
                        objItem.mode = "Insert";
                        int ItemId = InventoryBLL.InsertItem(objItem);
                        drNew[0] = ItemId;
                    
                    }
                    else if (btnSave.Text == "UPDATE")
                    {
                        InventoryBLL objItem = new InventoryBLL();
                        objItem.ItemID = Convert.ToInt32(btnSave.CommandArgument);
                        objItem.BrandID = Convert.ToInt32(hdnAutoBrandID.Value);
                        objItem.CategoryID = Convert.ToInt32(CategoryName.SelectedValue);
                        objItem.ItemInvoiceID = Convert.ToInt32(btnSave.CommandArgument);
                        objItem.SupplierID = Convert.ToInt32(hdnAutoSupplierID.Value);
                        objItem.Description = txtDescription.Text.Trim();
                        objItem.Price = Convert.ToSingle(txtPrice.Text);
                        objItem.ExpiryDate =dtExpire; 
                        objItem.Quantity = Convert.ToInt32(txtQuantity1.Text);
                        objItem.SerialNo = txtSerialNo.Text == "" ? "Null" : txtSerialNo.Text;
                        objItem.PurchaseDate =  DateTime.ParseExact(txtpurdate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objItem.InsertedBy = UM.EmployeeID;
                        objItem.mode = "Update";
                        bool isUpdated = InventoryBLL.UpdateItem(objItem);

                        drNew[0] = btnSave.CommandArgument;
                        btnSave.Text = "SAVE";
                    }
                    dtItemId.Rows.Add(drNew);
                    dtItemId.AcceptChanges();

                    if (dtItemId.Rows.Count > 0)
                    {
                        foreach (GridViewRow grdRow in gvItemDetails.Rows)
                        {
                            Label lblID = (Label)(gvItemDetails.Rows[grdRow.RowIndex].Cells[0].FindControl("lblID"));
                            HiddenField hfValues = (HiddenField)(gvItemDetails.Rows[grdRow.RowIndex].Cells[0].FindControl("hfValuesMain"));
                            if (lblID.Text != null && lblID.Text != "")
                            {
                                List<InventoryBLL> lstItemAttribute = InventoryBLL.BindItemAttribute("SelectItemAtt", Convert.ToInt32(dtItemId.Rows[0][0].ToString()), Convert.ToInt32(lblID.Text.ToString()));
                                if (lstItemAttribute.Count > 0)
                                {
                                    InventoryBLL objItemAttribute = new InventoryBLL();
                                    objItemAttribute.ItemID=lstItemAttribute.FirstOrDefault().ItemID;
                                    objItemAttribute.AttributeID=Convert.ToInt16(lblID.Text.ToString());
                                    objItemAttribute.Value=Convert.ToString(hfValues.Value);
                                    objItemAttribute.ModifiedBy=UM.EmployeeID;
                                    objItemAttribute.mode="Update";
                                    InventoryBLL.InsertItemAttribute(objItemAttribute);
                                }
                                else
                                {
                                    InventoryBLL objItemAttribute = new InventoryBLL();
                                    objItemAttribute.ItemID = Convert.ToInt32(dtItemId.Rows[0][0].ToString());
                                    objItemAttribute.AttributeID = Convert.ToInt32(lblID.Text.ToString());
                                    objItemAttribute.Value = Convert.ToString(hfValues.Value);
                                    objItemAttribute.InsertedBy = UM.EmployeeID;
                                    objItemAttribute.mode = "Insert";
                                    InventoryBLL.InsertItemAttribute(objItemAttribute);
                                }
                            }
                        }
                    }
                    ClearUserEnteredValues();
                    DisplayInvoiceItemDetails();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('No attributes! To add Attributes click Inventory');</script>", false);
                }
            }
        }
        catch
        {
        }
    }


    void txt_add_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow row = (GridViewRow)txt.NamingContainer;
        int index = row.RowIndex;
        HiddenField hfValues = (HiddenField)(gvItemDetails.Rows[index].Cells[0].FindControl("hfValuesMain"));
        hfValues.Value = ((TextBox)sender).Text;
    }

    public void ddl_add_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is DropDownList)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int index = row.RowIndex;

            HiddenField hfValues = (HiddenField)(gvItemDetails.Rows[index].Cells[0].FindControl("hfValuesMain"));
            hfValues.Value = ((DropDownList)sender).SelectedValue.ToString().Trim();
        }
        else if (sender is CheckBoxList)
        {
            CheckBoxList chk = (CheckBoxList)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            int index = row.RowIndex;
            HiddenField hfValues = (HiddenField)(gvItemDetails.Rows[index].Cells[0].FindControl("hfValuesMain"));
            hfValues.Value = "";
            foreach (ListItem lstItem in ((CheckBoxList)sender).Items)
            {
                if (lstItem.Selected)
                {
                    hfValues.Value += ", " + lstItem.Text.ToString();
                }
            }
        }
    }

    private string ConvertSortDirectionToSql()
    {
        string newSortDirection = String.Empty;

        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;

            case "DESC":
                GridViewSortDirection = "ASC";

                break;
        }
        return GridViewSortDirection;
    }

    protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int intItemID = Convert.ToInt16(((ImageButton)sender).CommandArgument);
            Session.Add("ItemID", intItemID);
            List<InventoryBLL> lstItem = InventoryBLL.BindItem("ItemSelect", intItemID);
            if (lstItem.Count > 0)
            {
                btnSave.Text = "UPDATE";
                btnSave.CommandArgument = intItemID.ToString();
                txtBrand.Text = lstItem.FirstOrDefault().BrandName;
                txtSupplier.Text = lstItem.FirstOrDefault().Supplier;
                hdnAutoSupplierID.Value = Convert.ToString(lstItem.FirstOrDefault().SupplierID);
                hdnAutoBrandID.Value = Convert.ToString(lstItem.FirstOrDefault().BrandID);
                txtSerialNo.Text = lstItem.FirstOrDefault().SerialNo;
                txtDescription.Text = lstItem.FirstOrDefault().Description;
                txtQuantity1.Text = Convert.ToString(lstItem.FirstOrDefault().Quantity);
                txtPrice.Text = Convert.ToString(lstItem.FirstOrDefault().Price);
                if (!string.IsNullOrEmpty(Convert.ToString(lstItem.FirstOrDefault().PurchaseDate)))
                    txtpurdate.Text = Convert.ToDateTime(lstItem.FirstOrDefault().PurchaseDate).ToString("dd/MM/yyyy");
                else
                    txtpurdate.Text = "";
                DateTime dtPurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dtExpDate = DateTime.Parse((lstItem.FirstOrDefault().ExpiryDate.ToString()));

                TimeSpan tsInDays = dtExpDate.Subtract(dtPurchaseDate);
                txtYear.Text = Convert.ToInt16(tsInDays.Days / 365) == 0 ? "" : Convert.ToInt16(tsInDays.Days / 365).ToString();
                txtMonth.Text = Convert.ToInt16((tsInDays.Days % 365) / 30) == 0 ? "" : Convert.ToInt16((tsInDays.Days % 365) / 30).ToString();
                txtDay.Text = Convert.ToInt16((tsInDays.Days % 365) % 30) == 0 ? "" : Convert.ToInt16((tsInDays.Days % 365) % 30).ToString();
                CategoryName.SelectedValue = Convert.ToString(lstItem.FirstOrDefault().CategoryID);

                List<InventoryBLL> lstItemAttr = InventoryBLL.BindItemAttribute("SelectAtt", Convert.ToInt32(btnSave.CommandArgument));
                gvItemDetails.DataSource = lstItemAttr;
                gvItemDetails.DataBind();
                DisplayExistingItemAttributesBindData();

                panelItemDetails.Visible = true;
                CategoryName.Enabled = true;
                btnAdd.Enabled = false;
            }
        }
        catch
        {
        }
    }

    protected void CategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (this.CategoryName.Text == "---Select---")
            {
                gvItemDetails.DataSource = "";
                gvItemDetails.DataBind();
                return;
            }
            List<AttributeBLL> lstAttr = AttributeBLL.BindAttribute("SelectCatID", Convert.ToInt16(CategoryName.SelectedValue));
            //panelItemDetails.Visible = true;
            gvItemDetails.DataSource = lstAttr;
            gvItemDetails.DataBind();
            BindData();
        }
        catch
        {
        }
    }

    protected void gvItemList_RowEditing(object sender, GridViewEditEventArgs e)
    {
        panelItemDetails.Visible = true;
        btnAdd.Enabled = false;
        CategoryName.Enabled = false;
    }
    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (CategoryName.SelectedIndex != 0)
        {
            panelItemDetails.Visible = true;
            btnAdd.Enabled = false;
            CategoryName.Enabled = true;
        }
    }

    protected void BtnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["pr"] != null && Request.QueryString["pr"].ToString() == "1")
        {
           // Session["CategoryID"] = Request.QueryString["Itemid"];
            Response.Redirect("~//Member/ItemDetails.aspx");
        }
            
        else
            Response.Redirect("~//Member/Inventory.aspx");
    }

    protected void btnEditUpdateSave_Click(object sender, EventArgs e)
    {
        if (btnEditUpdateSave.Text == "SAVE" || btnEditUpdateSave.Text == "UPDATE")
        {
            string _ItemInvoiceID = Convert.ToString(Session["ItemInvoiceID"]);
            DataTable dtresult = new DataTable();
            //Commented By Nikhil Shetye for GST implementation
            //decimal vat = 0;
            //decimal serviceTax = 0;

            //if (!string.IsNullOrEmpty(txtVat.Text))
            //    vat = Convert.ToDecimal(txtVat.Text);

            //if(!string.IsNullOrEmpty(txtServiceTax.Text))
            //    serviceTax=Convert.ToDecimal(txtServiceTax.Text);
            decimal CGST = 0;
            decimal SGST = 0;

            if (!string.IsNullOrEmpty(txtCGST.Text))
                CGST = Convert.ToDecimal(txtCGST.Text);

            if (!string.IsNullOrEmpty(txtSGST.Text))
                SGST = Convert.ToDecimal(txtSGST.Text);

            List<InventoryBLL> exitingInvoiceNo = InventoryBLL.GetInvoiceIdByInvoiceNo("Select", InvoiceNo.Text.Trim());
            if (!string.IsNullOrEmpty(_ItemInvoiceID))
            {
                string _existingItemInvoiceID = string.Empty;
                if (exitingInvoiceNo.Count != 0)
                {
                    _existingItemInvoiceID = Convert.ToString(exitingInvoiceNo.FirstOrDefault().ItemInvoiceID);
                }
                if ((_ItemInvoiceID == _existingItemInvoiceID) | (_ItemInvoiceID != null && _existingItemInvoiceID == string.Empty))
                {
                    InventoryBLL objInvoice = new InventoryBLL();
                    objInvoice.ItemInvoiceID = Convert.ToInt32(hdnItemInvoiceID.Value);
                    objInvoice.SupplierID = Convert.ToInt32(hdnAutoSupplierID.Value);
                    objInvoice.InvoiceNo = InvoiceNo.Text.Trim();
                    objInvoice.PurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    objInvoice.Note = Note.Text;
                    //Commented By Nikhil Shetye for GST implementation
                    //objInvoice.VatPercentage = Convert.ToDecimal(vat);
                    //objInvoice.STPercentage=Convert.ToDecimal(serviceTax);
                    objInvoice.CGSTPercentage = Convert.ToDecimal(CGST);
                    objInvoice.SGSTPercentage = Convert.ToDecimal(SGST);
                    objInvoice.InsertedBy = UM.EmployeeID;
                    objInvoice.mode = "Update";
                    InventoryBLL.UpdateItemInvoice(objInvoice);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Already Invoice Number is Present.');</script>", false);
                    return;
                }
            }
            else
            {
                if (exitingInvoiceNo.Count == 0)
                {
                    if (Convert.ToInt16(hdnItemInvoiceID.Value) > 0)
                    {
                        InventoryBLL objInvoice = new InventoryBLL();
                        objInvoice.ItemInvoiceID = Convert.ToInt32(hdnItemInvoiceID.Value);
                        objInvoice.SupplierID = Convert.ToInt32(hdnAutoSupplierID.Value);
                        objInvoice.InvoiceNo = InvoiceNo.Text.Trim();
                        objInvoice.PurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objInvoice.Note = Note.Text;
                        //Commented By Nikhil Shetye for GST implementation
                        //objInvoice.VatPercentage = Convert.ToDecimal(vat);
                        //objInvoice.STPercentage=Convert.ToDecimal(serviceTax);
                        objInvoice.CGSTPercentage = Convert.ToDecimal(CGST);
                        objInvoice.SGSTPercentage = Convert.ToDecimal(SGST);
                        objInvoice.InsertedBy = UM.EmployeeID;
                        objInvoice.mode = "Update";
                        InventoryBLL.UpdateItemInvoice(objInvoice);
                    }
                    else
                    {
                        if (hdnAutoSupplierID.Value == "0")
                        {
                            SupplierBLL SupIns = new SupplierBLL();
                            SupIns.SupplierID = 0;
                            SupIns.Name = txtSupplier.Text;
                            SupIns.mode = "Insert";
                            SupIns.InsertedBy = UM.EmployeeID;
                            int id = SupplierBLL.InsertSupplier(SupIns);
                            hdnAutoSupplierID.Value = Convert.ToString(id);
                        }


                        InventoryBLL objInvoice = new InventoryBLL();
                        objInvoice.ItemInvoiceID = Convert.ToInt32(hdnItemInvoiceID.Value);
                        objInvoice.SupplierID = Convert.ToInt32(hdnAutoSupplierID.Value);
                        objInvoice.InvoiceNo = InvoiceNo.Text.Trim();
                        objInvoice.PurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objInvoice.Note = Note.Text;
                        //Commented By Nikhil Shetye for GST implementation
                        //objInvoice.VatPercentage = Convert.ToDecimal(vat);
                        //objInvoice.STPercentage=Convert.ToDecimal(serviceTax);
                        objInvoice.CGSTPercentage = Convert.ToDecimal(CGST);
                        objInvoice.SGSTPercentage = Convert.ToDecimal(SGST);
                        objInvoice.InsertedBy = UM.EmployeeID;
                        objInvoice.mode = "Insert";
                        int ItemInvoiceID = InventoryBLL.InsertItemInvoice(objInvoice);
                        hdnItemInvoiceID.Value = Convert.ToString(ItemInvoiceID);
                    }
                }
                else
                {
                    if (hdnItemInvoiceID.Value == Convert.ToString(exitingInvoiceNo.FirstOrDefault().ItemInvoiceID))
                    {
                        InventoryBLL objInvoice = new InventoryBLL();
                        objInvoice.ItemInvoiceID = Convert.ToInt32(hdnItemInvoiceID.Value);
                        objInvoice.SupplierID = Convert.ToInt32(hdnAutoSupplierID.Value);
                        objInvoice.InvoiceNo = InvoiceNo.Text.Trim();
                        objInvoice.PurchaseDate = DateTime.ParseExact(txtPurchaseDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        objInvoice.Note = Note.Text;
                        //Commented By Nikhil Shetye for GST implementation
                        //objInvoice.VatPercentage = Convert.ToDecimal(vat);
                        //objInvoice.STPercentage = Convert.ToDecimal(serviceTax);
                        objInvoice.CGSTPercentage = Convert.ToDecimal(CGST);
                        objInvoice.SGSTPercentage = Convert.ToDecimal(SGST);
                        objInvoice.InsertedBy = UM.EmployeeID;
                        objInvoice.mode = "Update";
                        InventoryBLL.UpdateItemInvoice(objInvoice);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "Save", "<script>alert('Already Invoice Number is Present.');</script>", false);
                        return;
                    }
                }
            }
            Table1.Rows[5].Visible = true;
            btnEditUpdateSave.Text = "EDIT";
            //txtVat.Text = vat.ToString();  //Commented By Nikhil Shetye for GST implementation
            txtCGST.Text = CGST.ToString();
            txtSupplier.Enabled = false;
            Note.Enabled = false;
            //Commented By Nikhil Shetye for GST implementation
            //txtVat.Enabled = false;
            //txtServiceTax.Enabled = false;
            txtCGST.Enabled = false;
            txtSGST.Enabled = false;
            InvoiceNo.Enabled = false;
            txtPurchaseDate.Enabled = false;
            gvItemList.Enabled = true;
            CategoryName.Enabled = true;
            btnAdd.Enabled = true;
            Table1.Rows[2].Cells[2].Visible = true;
            Table1.Rows[2].Cells[3].Visible = true;
//added by AP
            decimal amount = Convert.ToDecimal(lblTaxExemptAmount.Text);
            //Commented By Nikhil Shetye for GST implementation
            //decimal vatAmount = amount * (vat / 100);
            //decimal serviceAmount = amount * (serviceTax / 100);
            decimal CGSTAmount = amount * (CGST / 100);
            decimal SGSTAmount = amount * (SGST / 100);
            //Commented By Nikhil Shetye for GST implementation
            //decimal total = amount + vatAmount+serviceAmount;
            //lblAmount.Text =(Math.Round(vatAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>Service Tax  Amount</b> " + (Math.Round(serviceAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>Total Amount</b> " + (Math.Round(total, 2, MidpointRounding.AwayFromZero)).ToString();
            decimal total = amount + CGSTAmount + SGSTAmount;
            lblAmount.Text = (Math.Round(CGSTAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>SGST Amount</b> " + (Math.Round(SGSTAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>Total Amount</b> " + (Math.Round(total, 2, MidpointRounding.AwayFromZero)).ToString();
        }
        else if (btnEditUpdateSave.Text == "EDIT")
        {
            txtSupplier.Enabled = true;
            Note.Enabled = true;
            InvoiceNo.Enabled = true;
            txtPurchaseDate.Enabled = true;
            //Commented By Nikhil Shetye for GST implementation
            //txtVat.Enabled = true;
            //txtServiceTax.Enabled = true;
            txtCGST.Enabled = true;
            txtSGST.Enabled = true;
            //Table1.Rows[4].Visible = false;
            Table1.Rows[5].Visible = false;
            btnEditUpdateSave.Text = "UPDATE";
            gvItemList.Enabled = false;
            panelItemDetails.Visible = false;
        }
    }

    protected void gvItemList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dataTable = (DataTable)Session["InventoryDetails"];

        if (dataTable != null)
        {

            DataView dataView = new DataView(dataTable);
            e.SortDirection = e.SortDirection;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql();

            gvItemList.DataSource = dataView;
            gvItemList.DataBind();
            Session["InventoryDetails"] = dataView.Table;
        }

    }
    #endregion

    #region Methods
    [WebMethod]
    public static string[] GetCompletionSupplierList(string prefixText)
    {
        List<SupplierBLL> listSup = SupplierBLL.BindSupplierPrefix("Prefix", prefixText);
        List<string> items = new List<string>();
        foreach (var item in listSup)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(item.Name, Convert.ToString(item.SupplierID)));
        }

        return items.ToArray();
    }
    [WebMethod]
    public static string[] GetCompletionBrandList(string prefixText)
    {
        List<BrandBLL> listBr = BrandBLL.BindBrandPrefix("Prefix", prefixText);
        List<string> items = new List<string>();
        foreach (var item in listBr)
        {
            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(item.Name, Convert.ToString(item.BrandID)));
        }

        return items.ToArray();
    }
    
    private void LoadPage()
    {
        try
        {
            List<CategoryBLL> lstcat = CategoryBLL.BindCategory("Select");
            this.CategoryName.DataSource = lstcat;
            this.CategoryName.DataTextField = "Name";
            this.CategoryName.DataValueField = "CategoryID";
            this.CategoryName.DataBind();
            this.CategoryName.Items.Insert(0, new ListItem("---Select---", "---Select---"));
            this.CategoryName.SelectedIndex = -1;

            if (!string.IsNullOrEmpty(Convert.ToString(Session["ItemInvoiceID"])))
            {
                hdnItemInvoiceID.Value = Convert.ToString(Session["ItemInvoiceID"]);
                Session["ItemInvoiceID"] = null;
                DisplyExistingInvoiceItemDetails();
            }
            else
            {
                Table1.Rows[2].Cells[2].Visible = false;
                Table1.Rows[2].Cells[3].Visible = false;
            }
        }
        catch
        {
        }
    }

    public void BindData()
    {
        try
        {
            foreach (GridViewRow item in gvItemDetails.Rows)
            {
                Label lblID = item.Cells[0].FindControl("lblID") as Label;
                if (lblID.Text.Trim() != "")
                {
                    List<AttributeBLL> lstAttr = AttributeBLL.BindAttribute("SelectCatIDAttID", Convert.ToInt32(CategoryName.SelectedValue), Convert.ToInt32(lblID.Text.ToString()));
                    List<AttributeBLL> lstCatAttr = AttributeBLL.BindCatAttribute("SelectAttID", Convert.ToInt32(lblID.Text.ToString()));
                    if (lstAttr.Count == 0)
                        return;
                    switch (Convert.ToString(lstAttr.FirstOrDefault().Type))
                    {
                        case "Dropdown":
                            DropDownList ddl_add = new DropDownList();
                            ddl_add.Visible = true;
                            ddl_add.Width = 100;
                            ddl_add.DataSource = lstCatAttr;
                            ddl_add.DataTextField = "Value";
                            ddl_add.DataValueField = "Value";
                            ddl_add.SelectedIndexChanged += ddl_add_SelectedIndexChanged;
                            ddl_add.AutoPostBack = true;
                            ddl_add.DataBind();
                            ddl_add.Items.Insert(0, "None");
                            if (Convert.ToString(lstAttr.FirstOrDefault().DefaultValue) != "")
                            {
                                for (int i = 0; i < ddl_add.Items.Count; i++)
                                {
                                    if (ddl_add.Items[i].ToString().Trim() == (Convert.ToString(lstAttr.FirstOrDefault().DefaultValue).Trim()))
                                    {
                                        ddl_add.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            item.Cells[1].Controls.Add(ddl_add);
                            break;
                        case "Textbox":
                            TextBox txt_add = new TextBox();
                            txt_add.Text = Convert.ToString(lstAttr.FirstOrDefault().DefaultValue).Trim();
                            txt_add.Visible = true;
                            txt_add.AutoPostBack = true;
                            txt_add.TextChanged += txt_add_TextChanged;
                            item.Cells[1].Controls.Add(txt_add);
                            break;
                        case "Checkbox":
                            CheckBoxList chklst_add = new CheckBoxList();
                            chklst_add.Visible = true;
                            chklst_add.DataSource = lstCatAttr;
                            chklst_add.DataTextField = "Value";
                            chklst_add.DataValueField = "Value";
                            chklst_add.SelectedIndexChanged += ddl_add_SelectedIndexChanged;
                            chklst_add.AutoPostBack = true;
                            chklst_add.DataBind();

                            if (Convert.ToString(lstAttr.FirstOrDefault().DefaultValue) != "")
                            {
                                for (int i = 0; i < chklst_add.Items.Count; i++)
                                {
                                    if (chklst_add.Items[i].ToString().Trim() == (Convert.ToString(lstAttr.FirstOrDefault().DefaultValue).Trim()))
                                    {
                                        chklst_add.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }

                            item.Cells[1].Controls.Add(chklst_add);
                            break;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void DisplayExistingItemAttributesBindData()
    {
        try
        {
            foreach (GridViewRow item in gvItemDetails.Rows)
            {
                Label lblID = item.Cells[0].FindControl("lblID") as Label;
                if (lblID.Text.Trim() != "")
                {
                    string itemID = Convert.ToString(Session["ItemID"]);
                    List<InventoryBLL> lstItemAttr = InventoryBLL.BindItemAttribute("SelectAtt", Convert.ToInt32(itemID));
                    int AttrId = Convert.ToInt32(lblID.Text);
                    var lstDataRow = lstItemAttr.Where(a => a.AttributeID == AttrId).ToList();//dtAttributeDetails.Select("AttributeID ='" + lblID.Text + "'");
                    List<AttributeBLL> lstAttrCat = AttributeBLL.BindCatAttribute("SelectAttID", Convert.ToInt32(lblID.Text.ToString()));

                    if (lstItemAttr.Count == 0)
                        return;

                    switch (Convert.ToString(lstDataRow.FirstOrDefault().Type))
                    {
                        case "Dropdown":
                            DropDownList ddl_add = new DropDownList();
                            ddl_add.Visible = true;
                            ddl_add.Width = 100;
                            ddl_add.DataSource = lstAttrCat;
                            ddl_add.DataTextField = "Value";
                            ddl_add.DataValueField = "Value";
                            ddl_add.SelectedIndexChanged += ddl_add_SelectedIndexChanged;
                            ddl_add.AutoPostBack = true;
                            ddl_add.DataBind();
                            ddl_add.Items.Insert(0, "None");
                            if (Convert.ToString(lstDataRow.FirstOrDefault().Value) != "")
                            {
                                for (int i = 0; i < ddl_add.Items.Count; i++)
                                {
                                    if (ddl_add.Items[i].ToString().Trim() == (Convert.ToString(lstDataRow.FirstOrDefault().Value).Trim()))
                                    {
                                        ddl_add.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            item.Cells[1].Controls.Add(ddl_add);
                            break;

                        case "Textbox":
                            TextBox txt_add = new TextBox();
                            txt_add.Text = Convert.ToString(lstDataRow.FirstOrDefault().Value).Trim();
                            txt_add.Visible = true;
                            txt_add.AutoPostBack = true;
                            txt_add.TextChanged += txt_add_TextChanged;
                            item.Cells[1].Controls.Add(txt_add);
                            break;
                        case "Checkbox":
                            CheckBoxList chklst_add = new CheckBoxList();
                            chklst_add.Visible = true;
                            chklst_add.DataSource = lstAttrCat;
                            chklst_add.DataTextField = "Value";
                            chklst_add.DataValueField = "Value";
                            chklst_add.SelectedIndexChanged += ddl_add_SelectedIndexChanged;
                            chklst_add.AutoPostBack = true;
                            chklst_add.DataBind();

                            if (Convert.ToString(lstDataRow.FirstOrDefault().Value) != "")
                            {
                                for (int i = 0; i < chklst_add.Items.Count; i++)
                                {
                                    string value = Convert.ToString(chklst_add.Items[i]);

                                    string value1 = Convert.ToString(lstDataRow.FirstOrDefault().Value);
                                    string[] value2 = value1.Split(',');

                                    foreach (string strvalue in value2)
                                    {
                                        if (value == strvalue.Trim())
                                        {
                                            chklst_add.Items[i].Selected = true;
                                        }
                                    }
                                }
                            }

                            item.Cells[1].Controls.Add(chklst_add);
                            break;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void ClearUserEnteredValues()
    {
        try
        {
            txtBrand.Text = string.Empty;
            //txtSupplier.Text = string.Empty;
            txtSerialNo.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQuantity1.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtDay.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtYear.Text = string.Empty;

            panelItemDetails.Visible = false;
            btnAdd.Enabled = true;
            CategoryName.Enabled = true;
            txtpurdate.Text = string.Empty;
        }
        catch { }
    }

    public void DisplayInvoiceItemDetails()
    {
        try
        {
            //double amount = 0, vatAmount = 0,serviceTaxAmount=0; //Commented By Nikhil Shetye for GST implementation
            double amount = 0, CGSTAmount = 0, SGSTAmount = 0;
            List<InventoryBLL> lstInvoice = InventoryBLL.GetInvoiceIdByInvoiceNo("Select", InvoiceNo.Text);
            List<InventoryBLL> lstItemInventory = InventoryBLL.BindItem("Select", Convert.ToInt16(hdnItemInvoiceID.Value));
            if (!(lstItemInventory.Count > 0))
            {
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("ItemID", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("CategoryName", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("Brand", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("Supplier", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("Price", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("Quantity", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("ExpDate", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("Description", typeof(string)));

                if (dtTemp.Rows.Count != 0)
                {
                    dtTemp.Rows.Add(dtTemp.NewRow());
                    //Store the DataTable in ViewState
                    gvItemList.DataSource = dtTemp;
                    gvItemList.DataBind();

                    int columncount = gvItemList.Rows[0].Cells.Count;
                    gvItemList.Rows[0].Cells.Clear();
                    gvItemList.Rows[0].Cells.Add(new TableCell());
                    gvItemList.Rows[0].Cells[0].ColumnSpan = columncount;
                    gvItemList.Rows[0].Cells[0].Text = "No Records";
                }
            }
            else
            {
                Session.Add("InventoryDetails", lstItemInventory);
                gvItemList.DataSource = lstItemInventory;
                gvItemList.DataBind();

                amount = lstItemInventory.Sum(item => item.Price * item.Quantity);
                //Commented By Nikhil Shetye for GST implementation
                //vatAmount = amount * (Convert.ToDouble(txtVat.Text) / 100);
                //serviceTaxAmount = amount * (Convert.ToDouble(txtServiceTax.Text) / 100);
                CGSTAmount = amount * (Convert.ToDouble(txtCGST.Text) / 100);
                SGSTAmount = amount * (Convert.ToDouble(txtSGST.Text) / 100);


                //amount = lstItemInventory.AsEnumerable().Sum(
                //      row => decimal.Parse(lstItemInventory.FirstOrDefault().Price.ToString()) * decimal.Parse(lstItemInventory.FirstOrDefault().Quantity.ToString()));
                //vatAmount = amount * (Convert.ToDecimal(txtVat.Text) / 100);
            }
            lblTaxExemptAmount.Text = amount.ToString();
            //double total = amount + vatAmount + serviceTaxAmount;
            //lblAmount.Text = (Math.Round(vatAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>Service Tax  Amount</b> " + (Math.Round(serviceTaxAmount, 2, MidpointRounding.AwayFromZero)).ToString()  +"&nbsp;&nbsp;&nbsp;<b>Total Amount</b> " + (Math.Round(total, 2, MidpointRounding.AwayFromZero)).ToString();
            //Commented By Nikhil Shetye for GST implementation
            double total = amount + CGSTAmount + SGSTAmount;
            lblAmount.Text = (Math.Round(CGSTAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>SGST Amount</b> " + (Math.Round(SGSTAmount, 2, MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>Total Amount</b> " + (Math.Round(total, 2, MidpointRounding.AwayFromZero)).ToString();

            //lblAmount.Text = (Math.Round(vatAmount,2,MidpointRounding.AwayFromZero)).ToString() + "&nbsp;&nbsp;&nbsp;<b>Total Amount</b> " + ( Math.Round((amount + vatAmount),2,MidpointRounding.AwayFromZero)).ToString();
        }
        catch
        {
        }
    }

    public void DisplyExistingInvoiceItemDetails()
    {
        try
        {
            int _ItemInvoiceID = Convert.ToInt32(hdnItemInvoiceID.Value);
            List<InventoryBLL> lstItemInvoice = InventoryBLL.BindInvoiceDetailsById("SelectItemInvoiceID", _ItemInvoiceID);
            if (lstItemInvoice != null)
            {
                if (lstItemInvoice.Count > 0)
                {
                    foreach (var Item in lstItemInvoice)
                    {
                        this.InvoiceNo.Text = Convert.ToString(Item.InvoiceNo);
                        this.txtPurchaseDate.Text = Item.PurchaseDate.Date.ToString("dd/MM/yyyy");
                        this.Note.Text = Convert.ToString(Item.Note);
                        this.txtSupplier.Text = Convert.ToString(Item.Supplier);
                        //Commented By Nikhil Shetye for GST implementation
                        //this.txtVat.Text = Convert.ToDecimal(Item.VatPercentage).ToString();
                        //this.txtServiceTax.Text = Convert.ToDecimal(Item.STPercentage).ToString();
                        this.txtCGST.Text = Convert.ToDecimal(Item.CGSTPercentage).ToString();
                        this.txtSGST.Text = Convert.ToDecimal(Item.SGSTPercentage).ToString();
                        hdnAutoSupplierID.Value = Convert.ToString(Item.SupplierID);
                        Table1.Rows[5].Visible = true;
                        btnEditUpdateSave.Text = "EDIT";
                        gvItemList.Enabled = true;
                        txtSupplier.Enabled = false;
                        Note.Enabled = false;
                        InvoiceNo.Enabled = false;
                        txtPurchaseDate.Enabled = false;
                        //txtVat.Enabled = false;
                        //txtServiceTax.Enabled = false;
                        txtCGST.Enabled = false;
                        txtSGST.Enabled = false;
                        break;
                    }
                    DisplayInvoiceItemDetails();
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Properties
    private string GridViewSortDirection
    {
        get { return ViewState["SortDirection"] as string ?? "ASC"; }
        set { ViewState["SortDirection"] = value; }
    }
    #endregion
    protected void gvItemDetails_409edIndexChanged(object sender, EventArgs e)
    {
    }
}