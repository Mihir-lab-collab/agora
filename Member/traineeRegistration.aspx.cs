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


public partial class Admin_traineeRegistration : System.Web.UI.Page
{
    SqlConnection _SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ToString());   
    string _strLastInsertRegID = string.Empty;
    public char[] _splitter = { '/', '.' };

    protected void Page_Load(object sender, EventArgs e)
    {    
    }

    #region Insert Registartion Details :
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string _strSql = string.Empty;

            try
            {
                               
                    _strSql = "INSERT INTO traineeRegistartion (traineeConfirmationID,traineeFirstName,traineeMiddleName,traineeLastName,traineeDOB,traineeGender , " +
                                         " traineeAddress,traineeState,traineeCity,traineeEmailID,traineePhoneNo,traineeMobileNo,traineeEducationDetails,traineeOtherEduDetails,traineeIntestedBcoz )" +
                                         " VALUES (@traineeConfirmationID,@traineeFirstName,@traineeMiddleName,@traineeLastName,@traineeDOB,@traineeGender , " +
                                         " @traineeAddress,@traineeState,@traineeCity,@traineeEmailID,@traineePhoneNo,@traineeMobileNo,@traineeEducationDetails,@traineeOtherEduDetails,@traineeIntestedBcoz) SELECT @@IDENTITY";

                    SqlCommand _sqlCmdInsert = new SqlCommand(_strSql, _SqlCon);

                    //=====================================================
                    //DELECRING PARAMETERS
                    //=====================================================
                    _sqlCmdInsert.Parameters.Add("@traineeConfirmationID", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeFirstName", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeMiddleName", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeLastName", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeDOB", SqlDbType.SmallDateTime, 4);
                    _sqlCmdInsert.Parameters.Add("@traineeGender", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeAddress", SqlDbType.VarChar, 100);
                    _sqlCmdInsert.Parameters.Add("@traineeState", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeCity", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeEmailID", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineePhoneNo", SqlDbType.VarChar, 10);
                    _sqlCmdInsert.Parameters.Add("@traineeMobileNo", SqlDbType.VarChar,10);
                    _sqlCmdInsert.Parameters.Add("@traineeEducationDetails", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeOtherEduDetails", SqlDbType.VarChar, 50);
                    _sqlCmdInsert.Parameters.Add("@traineeIntestedBcoz", SqlDbType.NVarChar, 500);

                    //=====================================================
                    //ASSIGNING VALUES TO THE PARAMETERS
                    //=====================================================
                    _sqlCmdInsert.Parameters["@traineeConfirmationID"].Value = "";
                    _sqlCmdInsert.Parameters["@traineeFirstName"].Value = txtFirstName.Text.Replace("'", "").Trim();
                    _sqlCmdInsert.Parameters["@traineeMiddleName"].Value = txtMiddleName.Text.Replace("'", "").Trim();
                    _sqlCmdInsert.Parameters["@traineeLastName"].Value = txtLastName.Text.Replace("'", "").Trim();
                    _sqlCmdInsert.Parameters["@traineeDOB"].Value = FunConvertDateTime(txtDOB.Value.ToString()); 
                    _sqlCmdInsert.Parameters["@traineeGender"].Value = drpGender.SelectedItem.Text.ToString();
                    _sqlCmdInsert.Parameters["@traineeAddress"].Value = txtAddress.Text.Replace("'", "").Trim();
                    _sqlCmdInsert.Parameters["@traineeState"].Value = drpState.SelectedItem.Text.ToString();
                    _sqlCmdInsert.Parameters["@traineeCity"].Value = txtCity.Text.Replace("'", "").Trim();
                    _sqlCmdInsert.Parameters["@traineeEmailID"].Value = txtEmailID.Text.Replace("'", "").Trim();

                    if (!(string.IsNullOrEmpty(txtPhoneNo.Text)))
                    {
                        _sqlCmdInsert.Parameters["@traineePhoneNo"].Value = txtPhoneNo.Text.Trim();
                    }
                    else
                    {
                        _sqlCmdInsert.Parameters["@traineePhoneNo"].Value = "";
                    }
                 
                    _sqlCmdInsert.Parameters["@traineeMobileNo"].Value = txtMobNo.Text.Trim();
                    _sqlCmdInsert.Parameters["@traineeEducationDetails"].Value = drpEducation.SelectedItem.Text.ToString();

                    if (!(string.IsNullOrEmpty(txtOtherEducation.Text)))
                    {
                        _sqlCmdInsert.Parameters["@traineeOtherEduDetails"].Value = txtOtherEducation.Text.Replace("'", "").Trim();
                    }
                    else
                    {
                        _sqlCmdInsert.Parameters["@traineeOtherEduDetails"].Value = "";
                    }

                    _sqlCmdInsert.Parameters["@traineeIntestedBcoz"].Value = txtInterstedBcoz.Text.Replace("'", "").Trim();

                    _sqlCmdInsert.Connection.Open();
                    object objLastInsertID = _sqlCmdInsert.ExecuteScalar();
                    _sqlCmdInsert.Connection.Close();
                    _sqlCmdInsert.Dispose();
                    _strLastInsertRegID = Convert.ToString(objLastInsertID);
               
                    Session["LastInsertRegID"] = _strLastInsertRegID;
                    

                    if (isNumeric(_strLastInsertRegID, System.Globalization.NumberStyles.Integer))
                    {
                        if (Convert.ToInt32(_strLastInsertRegID) > 0)
                        {
                            string _strUpdate = string.Empty;

                            if (_strLastInsertRegID.Length == 1)
                            {
                                _strUpdate = "UPDATE traineeRegistartion SET traineeConfirmationID='DWTTR000" + _strLastInsertRegID + "' WHERE traineeRegID=" + _strLastInsertRegID;
                            }
                            else if (_strLastInsertRegID.Length == 2)
                            {
                                _strUpdate = "UPDATE traineeRegistartion SET traineeConfirmationID='DWTTR00" + _strLastInsertRegID + "' WHERE traineeRegID=" + _strLastInsertRegID;
                            }
                            else if (_strLastInsertRegID.Length == 3)
                            {
                                _strUpdate = "UPDATE traineeRegistartion SET traineeConfirmationID='DWTTR0" + _strLastInsertRegID + "' WHERE traineeRegID=" + _strLastInsertRegID;
                            }
                            else if (_strLastInsertRegID.Length == 4)
                            {
                                _strUpdate = "UPDATE traineeRegistartion SET traineeConfirmationID='DWTTR" + _strLastInsertRegID + "' WHERE traineeRegID=" + _strLastInsertRegID;
                            }

                            SqlCommand _sqlCmdUpdate = new SqlCommand(_strUpdate, _SqlCon);

                            _sqlCmdUpdate.Connection.Open();
                            _sqlCmdUpdate.ExecuteNonQuery();
                            _sqlCmdUpdate.Connection.Close();
                            _sqlCmdUpdate.Dispose();

                            Response.Redirect("thanksRegistration.aspx");
                        }
                    }                          
            }
            catch (Exception ex)
            {
                Response.Write("Error      " + ex.Message.ToString());
                Response.End();
            }
        }
    }  
    #endregion

    //FUNCTION TO CONVERT string "dd/MM/YYYY" to DateTime _productType (MM/dd/yyyyy) 
    protected DateTime FunConvertDateTime(string strDate)
    {
        //DATE CONVERSION OF txtProIssueDt TO Datetime TO INSERT IN DATABASE :
        string _strDateDt = strDate;
        string[] _arDateDt = new string[2];
        _arDateDt = _strDateDt.Split(_splitter);
        int _DateDtDay = Convert.ToInt32(_arDateDt[0]);
        int _DateDtMonth = Convert.ToInt32(_arDateDt[1]);
        int _DateDtYear = Convert.ToInt32(_arDateDt[2]);
        DateTime _dtmDateDt = new DateTime(_DateDtYear, _DateDtMonth, _DateDtDay);
        return _dtmDateDt;
    }


    #region Check numeric data :
    public static bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle, System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    #endregion

    #region Reset  :
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearAllControl();
    }
    #endregion

    #region TO Clear All Control's :
    public void ClearAllControl()
    {
        txtFirstName.Text = string.Empty;
        txtMiddleName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtDOB.Value = string.Empty;
        txtAddress.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtEmailID.Text = string.Empty;
        txtOtherEducation.Text = string.Empty;
        txtInterstedBcoz.Text = string.Empty;
        drpState.SelectedIndex = 0;
        drpGender.SelectedIndex = 0;
        drpEducation.SelectedIndex = 0;
    }
    #endregion

}
