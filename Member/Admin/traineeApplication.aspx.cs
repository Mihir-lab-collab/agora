using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public partial class Admin_traineeApplication : Authentication
{
    #region Variable Declartion :
    SqlConnection _SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ToString());
    SqlDataAdapter _sqlAdapTrainee = new SqlDataAdapter();
    DataSet _dstTrainee = new DataSet();
    #endregion

    #region Page Load Event :
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DynoEmpSession"] != null)
        {
        }
        else
        {
            Response.Redirect("/emp/empLogin.aspx");
        }

        if (!IsPostBack)
        {
            BindGridViewData();
        }
    }
    #endregion
   
    #region Binding GridView  :
    public void BindGridViewData()
    {
        string _strSqlTrainee = string.Empty;
        string _strWithSearchText = txtSearch.Text.Trim();
        string _strWithoutSearchText = string.Empty;

        if (!(string.IsNullOrEmpty(_strWithSearchText)))
        {
            _strWithSearchText = " AND traineeConfirmationID = '" + txtSearch.Text.Trim() + "'";
            _strWithoutSearchText = " where traineeConfirmationID = '" + txtSearch.Text.Trim() + "'";
        }
        else
        {
            _strWithSearchText = "";
            _strWithoutSearchText = "";
        }

        if (Request.QueryString["search"] != null)
        {
            if (Request.QueryString["search"].Trim() == "Confirmed")
            {
                _strSqlTrainee = "SELECT  traineeRegID AS RegID, traineeConfirmationID AS FormNo, " +
                          "( traineeFirstName + ' ' + traineeLastName) AS Name, CONVERT(nvarchar,traineeDOB,103) As DOB, " +
                          " traineeState AS State, traineeCity AS City, traineeEmailID AS EmailID, traineeMobileNo AS MobileNo, " +
                          " (traineeEducationDetails + '   ' + traineeOtherEduDetails) AS EductaionDetails, traineeIntestedBcoz As InterstedBcoz, traineeStatus AS Status " +
                          " FROM  traineeRegistartion WHERE traineeStatus <> 0 " + _strWithSearchText;
            }
            else if (Request.QueryString["search"].Trim() == "Pending")
            {
                _strSqlTrainee = "SELECT  traineeRegID AS RegID, traineeConfirmationID AS FormNo, " +
                        "( traineeFirstName + ' ' + traineeLastName) AS Name, CONVERT(nvarchar,traineeDOB,103) As DOB, " +
                        " traineeState AS State, traineeCity AS City, traineeEmailID AS EmailID, traineeMobileNo AS MobileNo, " +
                        " (traineeEducationDetails + '   ' + traineeOtherEduDetails) AS EductaionDetails, traineeIntestedBcoz As InterstedBcoz, traineeStatus AS Status " +
                         " FROM  traineeRegistartion WHERE traineeStatus = 0  " + _strWithSearchText;
            }
            else if (Request.QueryString["search"].Trim() == "All")
            {
                _strSqlTrainee = "SELECT  traineeRegID AS RegID, traineeConfirmationID AS FormNo, " +
                          "(traineeFirstName + ' ' + traineeLastName) AS Name, CONVERT(nvarchar,traineeDOB,103) As DOB, " +
                          " traineeState AS State, traineeCity AS City, traineeEmailID AS EmailID, traineeMobileNo AS MobileNo, " +
                          " (traineeEducationDetails + '   ' + traineeOtherEduDetails) AS EductaionDetails, traineeIntestedBcoz As InterstedBcoz, traineeStatus AS Status " +
                          " FROM  traineeRegistartion  " + _strWithoutSearchText;
            }
        }

        _sqlAdapTrainee = new SqlDataAdapter(_strSqlTrainee, _SqlCon);
        _dstTrainee = new DataSet();

        _sqlAdapTrainee.Fill(_dstTrainee);

        if (_dstTrainee.Tables[0].Rows.Count > 0)
        {
            grdTraineeDetails.DataSource = _dstTrainee;
            grdTraineeDetails.DataBind();
        }
        else
        {
            lblMsg.Visible = true;
            lblMsg.Text = "No Record Found ! Please Try again ... ";
            txtSearch.Text = string.Empty;
        }


    }
    #endregion

    #region Get the status of entity.
    public static string GetStatus(bool status)
    {
        if (status) return "Confirmed";      // if status is true returns Confirmed :
        else return "Pending";               // if status is false returns Pending :
    }
    #endregion

    #region Search Criteria  :
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGridViewData();
    }
    #endregion

    #region GridVIew Event's  :
    protected void grdTraineeDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Pager)
        {
            Label lblRowIndex = new Label();
            lblRowIndex = (Label)e.Row.FindControl("lblRowIndex");
            lblRowIndex.Text = Convert.ToString(e.Row.RowIndex + 1);
        }
    }
    protected void grdTraineeDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdTraineeDetails.EditIndex = e.NewEditIndex;
        BindGridViewData();
    }
    protected void grdTraineeDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdTraineeDetails.EditIndex = -1;
        BindGridViewData();
    }
    protected void grdTraineeDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        try
        {
            grdTraineeDetails.EditIndex = -1;
            GridViewRow row = grdTraineeDetails.Rows[e.RowIndex];
            Label lblRegID = (Label)(row.FindControl("lblRegID"));			            //find id        
            TextBox txtMobileNo=(TextBox)(row.FindControl("txtMobileNo"));
            CheckBox chkStatus = (CheckBox)(row.FindControl("chkStatus"));          //find status

            string _strUpdate = string.Empty;

            _strUpdate = "UPDATE traineeRegistartion SET traineeMobileNo='" + txtMobileNo.Text.Trim() + "', traineeStatus='" + chkStatus.Checked.ToString() + "' WHERE traineeRegID=" + lblRegID.Text.ToString().Trim();

            SqlCommand _sqlCmdUpdate = new SqlCommand(_strUpdate, _SqlCon);

            _sqlCmdUpdate.Connection.Open();
            _sqlCmdUpdate.ExecuteNonQuery();
            _sqlCmdUpdate.Connection.Close();
            _sqlCmdUpdate.Dispose();

            BindGridViewData();
        }
        catch (Exception ex)
        {
            lblMsg.Visible = true;
            lblMsg.Text = ex.StackTrace.ToString();

        }
    }
    protected void grdTraineeDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CratePdf")
        {
            int _intRegID = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("adminPdfConverter.aspx?RegID=" + _intRegID.ToString());
        }       
    }
    protected void grdTraineeDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string _strDelete = string.Empty;

            grdTraineeDetails.EditIndex = -1;
            GridViewRow row = grdTraineeDetails.Rows[e.RowIndex];
            Label lblRegID = (Label)(row.FindControl("lblRegID"));	     //find RegID           

            _strDelete = "DELETE traineeRegistartion WHERE traineeRegID = " + lblRegID.Text;

            SqlCommand _sqlCmdDelete = new SqlCommand(_strDelete, _SqlCon);

            _sqlCmdDelete.Connection.Open();
            _sqlCmdDelete.ExecuteNonQuery();
            _sqlCmdDelete.Connection.Close();
            _sqlCmdDelete.Dispose();

            BindGridViewData();
        }
        catch (Exception ex)
        {
            Response.Write("Error   " + ex.Message.ToString());
            Response.End();
        }
    }
    #endregion
   
}
