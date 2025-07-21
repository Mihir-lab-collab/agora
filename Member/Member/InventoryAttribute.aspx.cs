using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using CommonFunctionLib;
using System.Web.UI.WebControls;
using System.Web.Configuration;

public partial class Member_InventoryAttribute : Authentication
{
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {
        UM = UserMaster.UserMasterInfo();
        if (!IsPostBack)
        {
            if (Session["InventoryCatID"] != null)
            {
                SetInitialRow();
                DataSet dsCategory = (DataSet)ViewState["DataSetCategory"];
                DataTable CurrentTable = dsCategory.Tables["Category"];
                List<AttributeBLL> lstAttCat = AttributeBLL.BindAttribute("SelectCatID", Convert.ToInt32(Session["InventoryCatID"]));
                foreach (var drAttribute in lstAttCat)
                {
                    DataRow drNew = CurrentTable.NewRow();
                    drNew["Name"] = drAttribute.Name;
                    drNew["DefaultValue"] = drAttribute.DefaultValue;
                    drNew["Type"] = drAttribute.Type;
                    drNew["AttributeID"] = drAttribute.AttributeID;
                    CurrentTable.Rows.Add(drNew);
                    CurrentTable.AcceptChanges();

                    DataTable AttributeCategory = dsCategory.Tables["AttributeCategory"];
                    List<AttributeBLL> lstAttribute = AttributeBLL.BindCatAttribute("SelectAttID", lstAttCat[0].AttributeID);
                    foreach (var value in lstAttribute)
                    {
                        DataRow drAttributeCategory = AttributeCategory.NewRow();
                        drAttributeCategory["SrFNo"] = drNew[0].ToString();
                        drAttributeCategory["Value"] = value.Value.ToString();
                        AttributeCategory.Rows.Add(drAttributeCategory);
                    }
                    AttributeCategory.AcceptChanges();
                }
                ViewState["DataSetCategory"] = dsCategory;
                BindAttributeDetails();
            }
        }
    }

    private void SetInitialRow()
    {
        DataSet ds = new DataSet("CategoryDetails");

        DataTable AttributeCategory = new DataTable("AttributeCategory");
        DataTable CurrentTable = new DataTable("Category");

        AttributeCategory.Columns.Add(new DataColumn("SrFNo", typeof(int)));
        AttributeCategory.Columns.Add(new DataColumn("Value", typeof(string)));

        DataRow CurrentRow = null;
        CurrentTable.Columns.Add(new DataColumn("SrNo", typeof(int)));
        CurrentTable.Columns.Add(new DataColumn("Name", typeof(string)));
        CurrentTable.Columns.Add(new DataColumn("DefaultValue", typeof(string)));
        CurrentTable.Columns.Add(new DataColumn("Type", typeof(string)));
        CurrentTable.Columns.Add(new DataColumn("AttributeID", typeof(string)));
        CurrentTable.Columns[0].AutoIncrement = true;
        CurrentTable.Columns[0].AutoIncrementSeed = 1;
        CurrentTable.Columns[0].AutoIncrementStep = 1;
        CurrentTable.PrimaryKey = new DataColumn[] { CurrentTable.Columns[0] };
        CurrentRow = CurrentTable.NewRow();
        CurrentTable.Rows.Add(CurrentRow);
        //Store the DataTable in ViewState
        gvDetails.DataSource = CurrentTable;
        gvDetails.DataBind();
        int columncount = gvDetails.Rows[0].Cells.Count;
        gvDetails.Rows[0].Cells.Clear();
        gvDetails.Rows[0].Cells.Add(new TableCell());
        gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
        gvDetails.Rows[0].Cells[0].Text = "No Records";

        CurrentTable.Rows.RemoveAt(0);
        CurrentTable.AcceptChanges();

        ds.Tables.Add(CurrentTable);
        ds.Tables.Add(AttributeCategory);

        DataRelation drelation = new DataRelation("CategoryRelation", CurrentTable.Columns[0], AttributeCategory.Columns[0]);
        ds.Relations.Add(drelation);

        ViewState.Add("DataSetCategory", ds);
    }

    protected void BindAttributeDetails()
    {
        DataSet dsCategory = (DataSet)ViewState["DataSetCategory"];
        DataTable CurrentTable = dsCategory.Tables["Category"];
        if (CurrentTable.Rows.Count > 0)
        {
            gvDetails.DataSource = CurrentTable;
            gvDetails.DataBind();

            foreach (GridViewRow grdRow in gvDetails.Rows)
            {
                DropDownList bind_dropdownlist = (DropDownList)(gvDetails.Rows[grdRow.RowIndex].Cells[3].FindControl("ddlValueShow"));
                Label lblSrn = (Label)(gvDetails.Rows[grdRow.RowIndex].Cells[0].FindControl("SrNo"));
                DropDownList ddlTypeEdit = (DropDownList)(gvDetails.Rows[grdRow.RowIndex].Cells[2].FindControl("ddlTypeEdit"));
                if (lblSrn == null)
                    lblSrn = (Label)(gvDetails.Rows[grdRow.RowIndex].Cells[0].FindControl("SrEditNo"));

                if (bind_dropdownlist == null)
                    bind_dropdownlist = (DropDownList)(gvDetails.Rows[grdRow.RowIndex].Cells[3].FindControl("ddlDefaultValueEdit"));

                DataTable AttributeCategory = dsCategory.Tables["AttributeCategory"];

                DataRow[] drResult = AttributeCategory.Select("SrFNo =" + lblSrn.Text);

                if (drResult.Length > 0)
                {
                    foreach (DataRow drAttribute in drResult)
                    {
                        bind_dropdownlist.Items.Add(drAttribute["Value"].ToString().Trim());
                    }
                }

                if (bind_dropdownlist != null && bind_dropdownlist.ID == "ddlDefaultValueEdit")
                {
                    bind_dropdownlist.Items.Insert(0, "None");
                }

                if (ddlTypeEdit != null)
                {
                    DataRow[] drSelected = CurrentTable.Select("SrNo='" + lblSrn.Text.Trim() + "'");
                    if (drSelected.Count() > 0)
                    {
                        ddlTypeEdit.SelectedValue = drSelected[0]["Type"].ToString().Trim();
                        bind_dropdownlist.SelectedValue = drSelected[0]["DefaultValue"].ToString().Trim();
                    }
                }
                if (bind_dropdownlist != null)
                {
                    bind_dropdownlist.Visible = true;
                }
            }
        }
        else
        {
            bindEmptyData();
        }
    }
    private void bindEmptyData()
    {
        DataRow CurrentRow = null;
        DataTable CurrentTable = ((DataSet)ViewState["DataSetCategory"]).Tables["Category"];
        CurrentRow = CurrentTable.NewRow();
        CurrentTable.Rows.Add(CurrentRow);
        //Store the DataTable in ViewState
        gvDetails.DataSource = CurrentTable;
        gvDetails.DataBind();
        int columncount = gvDetails.Rows[0].Cells.Count;
        gvDetails.Rows[0].Cells.Clear();
        gvDetails.Rows[0].Cells.Add(new TableCell());
        gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
        gvDetails.Rows[0].Cells[0].Text = "No Records";
        CurrentTable.Rows.RemoveAt(0);
        CurrentTable.AcceptChanges();
    }

    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDetails.EditIndex = e.NewEditIndex;
        BindAttributeDetails();
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        if (ViewState["DataSetCategory"] != null)
        {
            string srNo = gvDetails.DataKeys[e.RowIndex].Values["SrNo"].ToString();
            DataSet dsCategory = (DataSet)ViewState["DataSetCategory"];
            DataTable CurrentTable = dsCategory.Tables["Category"];

            DataRow[] drList = CurrentTable.Select("SrNo = '" + srNo + "'");
            string strName = string.Empty;
            if (drList.Length > 0)
            {
                DataRow CurrentNewRow = drList[0];
                strName = drList[0]["Name"].ToString();

                if (this.gvDetails.EditIndex != -1)
                {
                    TextBox txtAttributename = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("txtEditattname") as TextBox;
                    TextBox txtValue = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("txtValue") as TextBox;
                    DropDownList ddlDefaultValue = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("ddlDefaultValueEdit") as DropDownList;
                    DropDownList ddlListType = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("ddlTypeEdit") as DropDownList;
                    HiddenField hndAttributeID = this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("hndAttributeID") as HiddenField;

                    CurrentNewRow["Name"] = txtAttributename.Text;
                    CurrentNewRow["DefaultValue"] = ddlDefaultValue.SelectedItem == null ? "" : ddlDefaultValue.SelectedItem.Text.Trim() == "None" ? "" : ddlDefaultValue.SelectedItem.Text;
                    CurrentNewRow["Type"] = ddlListType.SelectedItem.Text;

                    AttributeBLL objAtt = new AttributeBLL();
                    objAtt.AttributeID = Convert.ToInt32(hndAttributeID.Value);
                    objAtt.Name = txtAttributename.Text;
                    objAtt.DefaultValue = ddlDefaultValue.SelectedItem == null ? "" : ddlDefaultValue.SelectedItem.Text.Trim() == "None" ? "" : ddlDefaultValue.SelectedItem.Text;
                    objAtt.Type = ddlListType.SelectedItem.Text;
                    objAtt.ModifiedBy = UM.EmployeeID;
                    objAtt.mode = "Update";
                    AttributeBLL.UpdateAttribute(objAtt);

                    List<AttributeBLL> lstAttribute = AttributeBLL.BindCatAttribute("SelectAttID", Convert.ToInt32(hndAttributeID.Value));

                    foreach (ListItem lstItem in ddlDefaultValue.Items)
                    {
                        if (lstItem.ToString() != "None")
                        {
                            var drFound = lstAttribute.Where(a => a.Value == lstItem.Value);
                            if (!(drFound.Count() > 0))
                            {
                                AttributeBLL objCatAtt = new AttributeBLL();
                                objCatAtt.AttributeID = Convert.ToInt32(hndAttributeID.Value);
                                objCatAtt.Name = lstItem.ToString();
                                objCatAtt.InsertedBy = UM.EmployeeID;
                                objCatAtt.mode = "InsertCatAtt";
                                int id = AttributeBLL.InsertCategoryAttribute(objCatAtt);

                                DataTable AttributeCategory = dsCategory.Tables["AttributeCategory"];

                                DataRow dr = AttributeCategory.NewRow();
                                dr[0] = srNo;
                                dr[1] = lstItem.ToString();

                                AttributeCategory.Rows.Add(dr);
                                AttributeCategory.AcceptChanges();
                            }
                        }
                    }

                    lblresult.ForeColor = Color.Green;
                    lblresult.Text = "&nbsp; Record Updated successfully";
                    gvDetails.EditIndex = -1;
                    CurrentTable.AcceptChanges();
                    ViewState["CurrentTable"] = CurrentTable;
                    BindAttributeDetails();
                }
            }
        }
    }
    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindAttributeDetails();
    }
    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["CurrentTable"] != null)
        {
            string srNo = gvDetails.DataKeys[e.RowIndex].Values["SrNo"].ToString();
            DataTable CurrentTable = (DataTable)ViewState["CurrentTable"];

            DataRow[] drList = CurrentTable.Select("SrNo = '" + srNo + "'");
            string strName = string.Empty;
            if (drList.Length > 0)
                strName = drList[0]["Name"].ToString();
            CurrentTable.Rows.RemoveAt(e.RowIndex);
            CurrentTable.AcceptChanges();
            BindAttributeDetails();
            lblresult.ForeColor = Color.Red;
            lblresult.Text = "&nbsp; Record deleted successfully";
            ViewState["CurrentTable"] = CurrentTable;
        }
    }
    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddNew"))
        {
            TextBox txtAttributename = (TextBox)gvDetails.FooterRow.FindControl("txtftrattname");
            TextBox txtValue = (TextBox)gvDetails.FooterRow.FindControl("txtftrValue");
            DropDownList ddlDefaultValue = (DropDownList)gvDetails.FooterRow.FindControl("dldefaultValue");
            DropDownList ddlType = (DropDownList)gvDetails.FooterRow.FindControl("ddlAttributeTypeDataEntry");

            if (ViewState["DataSetCategory"] != null)
            {
                DataSet dsCategory = (DataSet)ViewState["DataSetCategory"];
                DataTable CurrentTable = dsCategory.Tables["Category"];
                DataRow[] drValuesFound = CurrentTable.Select("Name ='" + txtAttributename.Text + "'");
                if (!(drValuesFound.Count() > 0))
                {
                    DataRow CurrentNewRow = CurrentTable.NewRow();
                    CurrentNewRow["Name"] = txtAttributename.Text;
                    CurrentNewRow["DefaultValue"] = ddlDefaultValue.SelectedItem == null ? "" : ddlDefaultValue.SelectedItem.Text.Trim() == "None" ? "" : ddlDefaultValue.SelectedItem.Text;
                    CurrentNewRow["Type"] = ddlType.SelectedItem.Text;
                    CurrentTable.Rows.Add(CurrentNewRow);
                    CurrentTable.AcceptChanges();

                    DataTable AttributeCategory = dsCategory.Tables["AttributeCategory"];

                    foreach (var value in ddlDefaultValue.Items)
                    {
                        if (value.ToString().Trim() != "None")
                        {
                            DataRow drAttributeCategory = AttributeCategory.NewRow();
                            drAttributeCategory["SrFNo"] = CurrentNewRow[0].ToString();
                            drAttributeCategory["Value"] = value.ToString();
                            AttributeCategory.Rows.Add(drAttributeCategory);
                        }
                    }
                    AttributeCategory.AcceptChanges();

                    lblresult.ForeColor = Color.Green;
                    lblresult.Text = "&nbsp; Record inserted successfully";
                    ViewState["DataSetCategory"] = dsCategory;

                    List<AttributeBLL> lstAttr = AttributeBLL.BindAttribute("SelectCatID", Convert.ToInt32(Session["InventoryCatID"]),0,txtAttributename.Text.Trim());
                    if (!(lstAttr.Count > 0))
                    {
                        AttributeBLL ObjIns = new AttributeBLL();
                        ObjIns.CategoryID = Convert.ToInt32(Session["InventoryCatID"]);
                        ObjIns.AttributeID = 0;
                        ObjIns.Name = txtAttributename.Text;
                        ObjIns.DefaultValue = ddlDefaultValue.SelectedItem == null ? "" : ddlDefaultValue.SelectedItem.Text.Trim() == "None" ? "" : ddlDefaultValue.SelectedItem.Text;
                        ObjIns.Type = ddlType.SelectedItem.Text;
                        ObjIns.InsertedBy = UM.EmployeeID;
                        ObjIns.mode = "Insert";
                        int id = AttributeBLL.InsertAttribute(ObjIns);

                        foreach (var item in ddlDefaultValue.Items)
                        {
                            if (item.ToString().Trim() != "None")
                            {
                                AttributeBLL objCat = new AttributeBLL();
                                objCat.AttributeID = id;
                                objCat.Name = item.ToString();
                                objCat.InsertedBy = UM.EmployeeID;
                                objCat.mode = "InsertCatAtt";
                                AttributeBLL.InsertCategoryAttribute(objCat);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Attribute already exists.!')", true);
                    }
                }
                else
                {
                    lblresult.ForeColor = Color.Red;
                    lblresult.Text = txtAttributename.Text + " Already inserted";
                }
                ViewState["DataSetCategory"] = dsCategory;
                BindAttributeDetails();
            }
        }
    }

    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView dRowView = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlType = (DropDownList)e.Row.FindControl("ddlTypeEdit");
                if (ViewState["CurrentTable"] != null)
                {
                    string srNo = gvDetails.DataKeys[e.Row.RowIndex].Values["SrNo"].ToString();
                    DataTable CurrentTable = (DataTable)ViewState["CurrentTable"];

                    DataRow[] drList = CurrentTable.Select("SrNo = '" + srNo + "'");
                    string strName = string.Empty;
                    if (drList.Length > 0)
                        strName = drList[0]["Type"].ToString();
                    ddlType.SelectedValue = strName;
                }
            }
        }
    }
    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDetails.PageIndex = e.NewPageIndex;
        BindAttributeDetails();
        gvDetails.DataBind();
    }

    protected void btnValues_Click(object sender, EventArgs e)
    {
        TextBox txtValue = (TextBox)gvDetails.FooterRow.FindControl("txtftrValue");
        DropDownList dldefaultValue = (DropDownList)gvDetails.FooterRow.FindControl("dldefaultValue");
        if (txtValue.Text.Trim() != "")
        {
            if (dldefaultValue.Items.FindByText(txtValue.Text.Trim()) == null)
            {
                dldefaultValue.Items.Add(txtValue.Text.Trim());
                txtValue.Text = "";
            }
        }

        if (gvDetails.Rows[0].Cells[0].Text == "No Records")
        {
            int columncount = gvDetails.Rows[0].Cells.Count;
            gvDetails.Rows[0].Cells.Clear();
            gvDetails.Rows[0].Cells.Add(new TableCell());
            gvDetails.Rows[0].Cells[0].ColumnSpan = columncount;
            gvDetails.Rows[0].Cells[0].Text = "No Records";
        }
    }
    protected void ddlAttributeTypeDataEntry_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList rdlObject = (DropDownList)sender;
        if (rdlObject.SelectedValue.ToString() != "Textbox")
        {
            TextBox txtValue = (TextBox)gvDetails.FooterRow.FindControl("txtftrValue");
            txtValue.Visible = true;
            Button btnValues = (Button)gvDetails.FooterRow.FindControl("btnValues");
            btnValues.Visible = true;
            DropDownList dldefaultValue = (DropDownList)gvDetails.FooterRow.FindControl("dldefaultValue");
            dldefaultValue.Visible = true;
        }
    }
    protected void btnValueEdit_Click(object sender, EventArgs e)
    {
        TextBox txtValue = (TextBox)this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("txtValueEdit");
        DropDownList dldefaultValue = (DropDownList)this.gvDetails.Rows[this.gvDetails.EditIndex].FindControl("ddlDefaultValueEdit");
        if (txtValue.Text.Trim() != "")
        {
            if (dldefaultValue.Items.FindByText(txtValue.Text.Trim()) == null)
            {
                dldefaultValue.Items.Add(txtValue.Text.Trim());
                txtValue.Text = "";
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~//Member/InventoryCategory.aspx");
    }
}