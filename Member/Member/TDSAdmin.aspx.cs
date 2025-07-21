using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Member_TDSAdmin : Authentication
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [System.Web.Services.WebMethod]
    public static List<EmplistData> getEmpList(string Year)
    {
        TdsEmployeeBLL objTdsBal = new TdsEmployeeBLL();
        List<EmplistData> objList = new List<EmplistData>();
        DataTable dt = objTdsBal.GetEmplist(Year);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                EmplistData objListData = new EmplistData();
                objListData.EmpId = dt.Rows[i][0].ToString();
                objListData.EmpName = dt.Rows[i][1].ToString();
                objListData.ModifiedOn = dt.Rows[i][2].ToString();
                objList.Add(objListData);
            }
        }
        return objList;
    }

    [System.Web.Services.WebMethod]
    public static List<TdsEmployeeBLL> getSelectedTdsData(string EmpId, string Year)
    {
        TdsEmployeeBLL objTdsBll = new TdsEmployeeBLL();
        List<TdsEmployeeBLL> objListTds = new List<TdsEmployeeBLL>();
        try
        {
            DataTable dt = new DataTable();
            dt = objTdsBll.GetEmpTds(EmpId, Year);
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
                    objTdsBll1.Type = dt.Rows[i][8].ToString();
                    objTdsBll1.Regime = Convert.ToBoolean(dt.Rows[i][9]);
                    objTdsBll1.Deslaimer = Convert.ToBoolean(dt.Rows[i][10].ToString());
                    objTdsBll1.RegimeStatus = dt.Rows[i][11].ToString();
                    objTdsBll1.PanNo = dt.Rows[i][7].ToString();
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
}

public class EmplistData
{
    public string EmpId { set; get; }
    public string EmpName { set; get; }
    public string ModifiedOn { set; get; }
}

