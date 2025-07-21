using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_TdsEmployee : Authentication
{
    //static string CurrUserid1 = string.Empty;
    static string CurrUserid = string.Empty;
    static string UserProfile = string.Empty;
    static string TdsYear = string.Empty;
    static List<TdsEmployeeBLL> objListTds = new List<TdsEmployeeBLL>();
    static UserMaster UM;
    protected void Page_Load(object sender, EventArgs e)
    {

        UM = UserMaster.UserMasterInfo();
        CurrUserid = UM.EmployeeID.ToString();

        Session["EmployeeId"] = UM.EmployeeID.ToString();
        txtEmpName.Value = UM.Name;
        hdnEmpId.Value = CurrUserid;
    }

    [System.Web.Services.WebMethod]
    public static List<TdsEmployeeBLL> GetTds(string Year)
    {
        // UserMaster UM;
        // UM = UserMaster.UserMasterInfo();
        //CurrUserid = UM.EmployeeID.ToString();

        TdsYear = Year;
        TdsEmployeeBLL objTdsBll = new TdsEmployeeBLL();
        try
        {
            DataTable dt = new DataTable();
            dt = objTdsBll.GetEmpTds(HttpContext.Current.Session["EmployeeId"].ToString(), Year);//CurrUserid
            if (dt.Rows.Count > 0)
            {
                objListTds.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TdsEmployeeBLL objTdsBll1 = new TdsEmployeeBLL();

                    objTdsBll1.Id = Convert.ToInt32(dt.Rows[i][0]);
                    objTdsBll1.TdsName = dt.Rows[i][1].ToString();
                    objTdsBll1.Amount = Convert.ToInt32(dt.Rows[i][2]);
                    objTdsBll1.Comment = dt.Rows[i][3].ToString();
                    objTdsBll1.InsertedOn = dt.Rows[i][4].ToString();
                    objTdsBll1.Flag = dt.Rows[i][5].ToString();
                    objTdsBll1.FatherName = dt.Rows[i][6].ToString();
                    objTdsBll1.PanNo = dt.Rows[i][7].ToString();
                    objTdsBll1.Type = dt.Rows[i][8].ToString();
                    objTdsBll1.Regime = Convert.ToBoolean(dt.Rows[i][9]);
                    objTdsBll1.Deslaimer = Convert.ToBoolean(dt.Rows[i][10].ToString());
                    objTdsBll1.RegimeStatus= dt.Rows[i][11].ToString();
                    objListTds.Add(objTdsBll1);
                }

            }
        }

        catch (Exception Ex)
        {

            throw;
        }
        return objListTds;
    }

    [System.Web.Services.WebMethod]
    public static void SaveTDS(List<TdsEmployeeBLL> GridData, string FatherName, string PanNo, string IsRegime, bool IsDeclaimer = true)//, int InsertedBy)
    {
        TdsEmployeeBLL objTdsBll = new TdsEmployeeBLL();

        try
        {
            int intOffMonthset = 0;//-1
            if (DateTime.Now.Day < 10)
                intOffMonthset = -1;//-2
            string displayYear = string.Empty;
            string displayDate = DateAndTime.DateAdd("m", intOffMonthset, DateAndTime.Now).ToString();


            if (DateTime.Now.Month < 4)
            {
                displayYear = Convert.ToString(Convert.ToInt32((Convert.ToDateTime(displayDate).Year) - 1));
            }
            else
            {
                displayYear = Convert.ToString((Convert.ToDateTime(displayDate).Year));
            }
            bool Regime = false;

            if (IsRegime == "rdNewRegime")
            {
                Regime = true;
            }
            else if (IsRegime == "rdOldRegime")
            {
                Regime = false;
            }
            //foreach (var item in GridData1)
            //{
            //  GridData.Add(item);
            //}

            objTdsBll.SaveTDS(GridData, HttpContext.Current.Session["EmployeeId"].ToString(), displayYear, FatherName, PanNo, Convert.ToInt32(HttpContext.Current.Session["EmployeeId"]), Regime, IsDeclaimer);//CurrUserid,InsertedBy



        }
        catch (Exception ex)
        {

            throw;
        }
    }


    [System.Web.Services.WebMethod]
    public static List<TdsEmployeeBLL> CheckExistingTdsEmployee(string Year)
    {
        TdsEmployeeBLL objTdsBll = new TdsEmployeeBLL();
        DataTable dt = new DataTable();
        try
        {
            dt = objTdsBll.CheckExistingTdsEmployee(HttpContext.Current.Session["EmployeeId"].ToString(), Year);//CurrUserid
            objListTds.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TdsEmployeeBLL objTdsBllList = new TdsEmployeeBLL();
                    objTdsBllList.Year = Convert.ToInt32(dt.Rows[i][0]);
                    objTdsBllList.InsertedOn = dt.Rows[i][1].ToString();
                    objListTds.Add(objTdsBllList);
                }
            }

        }
        catch (Exception Ex)
        {
            objListTds.Clear();
        }
        return objListTds;
    }


}