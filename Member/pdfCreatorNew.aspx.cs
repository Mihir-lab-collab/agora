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
using WebSupergoo.ABCpdf6;

public partial class Admin_pdfCreator : System.Web.UI.Page
{
    string _strRegID = string.Empty;
    SqlConnection _SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ToString());
    SqlDataAdapter _sqlAdap = new SqlDataAdapter();
    DataSet _dst = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {        
       
        if (Request.QueryString["RegID"] != null)
        {
            _strRegID = Request.QueryString["RegID"].ToString();
            BindRegistrationData(_strRegID);
        }
        else
        {
            Response.Redirect("traineeRegistration.aspx");
        }
    }

    public void BindRegistrationData(string RegID)
    {
        try
        {
            string _strSql = string.Empty;

            _strSql = "SELECT * FROM traineeRegistartion WHERE traineeRegID = " + RegID;

            _sqlAdap = new SqlDataAdapter(_strSql, _SqlCon);

            _dst = new DataSet();

            _sqlAdap.Fill(_dst);

            if (_dst.Tables[0].Rows.Count > 0)
            {
                lblFormNumber.Text = _dst.Tables[0].Rows[0]["traineeConfirmationID"].ToString();
                lblFirstName.Text = _dst.Tables[0].Rows[0]["traineeFirstName"].ToString();
                lblMiddleName.Text = _dst.Tables[0].Rows[0]["traineeMiddleName"].ToString();
                lblLastName.Text = _dst.Tables[0].Rows[0]["traineeLastName"].ToString();
                lblDOB.Text = DateTime.Parse(_dst.Tables[0].Rows[0]["traineeDOB"].ToString()).ToString("dd-MM-yyyy");
                lblGender.Text = _dst.Tables[0].Rows[0]["traineeGender"].ToString();
                lblAddress.Text = _dst.Tables[0].Rows[0]["traineeAddress"].ToString();
                lblState.Text = _dst.Tables[0].Rows[0]["traineeState"].ToString();
                lblCity.Text = _dst.Tables[0].Rows[0]["traineeCity"].ToString();
                lblEmail.Text = _dst.Tables[0].Rows[0]["traineeEmailID"].ToString();

                if (!(string.IsNullOrEmpty(_dst.Tables[0].Rows[0]["traineePhoneNo"].ToString().Trim())))
                {
                    trPhoneNo.Visible = true;
                    lblPhoneNo.Text = _dst.Tables[0].Rows[0]["traineePhoneNo"].ToString();
                }
                else
                {
                    trPhoneNo.Visible = false;
                }

                lblMobileNo.Text = _dst.Tables[0].Rows[0]["traineeMobileNo"].ToString();

                lblEducation.Text = _dst.Tables[0].Rows[0]["traineeEducationDetails"].ToString();

                if (!(string.IsNullOrEmpty(_dst.Tables[0].Rows[0]["traineeOtherEduDetails"].ToString().Trim())))
                {
                    trOther.Visible = true;
                    lblOtherDetails.Text = _dst.Tables[0].Rows[0]["traineeOtherEduDetails"].ToString();
                }
                else
                {
                    trOther.Visible = false;
                }

                lblIntersted.Text = _dst.Tables[0].Rows[0]["traineeIntestedBcoz"].ToString();
               
            }
        }
        catch (Exception ex)
        {

        }
    }
}
