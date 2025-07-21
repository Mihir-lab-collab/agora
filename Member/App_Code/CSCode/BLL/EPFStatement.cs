using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for EPFStatement
/// </summary>
public class EPFStatement
{
    public string EmployeeName;
    public int Basic;
    public int EPSWages; public int PF;
    public int EPSContribution;
    public int BalenceER;
    public string empId;
    //public DataTable dtEPF = new DataTable();

    public EPFStatement()
    {

    }

    public DataSet GetEpfStatement(string data)
    {
        EPFStatementDAL objEPFStatementDAL=new EPFStatementDAL ();
        string[] DataSplite = data.Split('/');
        return objEPFStatementDAL.GetEpfStatement(Convert.ToInt32(DataSplite[0]), Convert.ToInt32(DataSplite[1])); ;
    }
}