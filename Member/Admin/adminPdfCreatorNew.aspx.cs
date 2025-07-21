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

public partial class Admin_pdfCreator : Authentication
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
            Response.Redirect("traineeApplication.aspx");
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
            }
        }
        catch (Exception ex)
        {

        }
    }
}
