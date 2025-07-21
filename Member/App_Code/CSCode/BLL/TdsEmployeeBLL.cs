using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TdsEmployeeBLL
/// </summary>
public class TdsEmployeeBLL
{
 
    public int Id = 0;
    public int Year = 0;
    public string TdsName = string.Empty;
    public int Amount = 0;
    public string Comment = string.Empty;
    public string InsertedOn = string.Empty;
    public string Flag = string.Empty;
    public string FatherName = string.Empty;
    public string PanNo = string.Empty;
    public string Type = string.Empty;
    public bool Regime { get; set; }
    public bool Deslaimer { get; set; }
    public int InsertedBy { get; set; }
    public string RegimeStatus = string.Empty;

    public DataTable GetEmpTds(string EmpId,string Year)
    {
        TdsEmployeeDAL objTdsDal=new TdsEmployeeDAL ();
        return objTdsDal.GetEmpTds(EmpId, Year);
    }

    public void SaveTDS(List<TdsEmployeeBLL> GridData, string EmpID, string Year, string FatherName, string PanNo, int InsertedBy, bool IsRegime, bool IsDeclaimer)
    {
        TdsEmployeeDAL objTdsDal = new TdsEmployeeDAL();
        try
        {
             objTdsDal.SaveTDS(GridData, EmpID, Year, FatherName, PanNo, InsertedBy, IsRegime,IsDeclaimer); 
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public  DataTable CheckExistingTdsEmployee(string EmpId,string Year)
    {
        TdsEmployeeDAL objTdsDal = new TdsEmployeeDAL();
        DataTable dt = new DataTable();
        try
        {
            dt = objTdsDal.CheckExistingTdsEmployee(EmpId, Year);
        }
        catch (Exception Ex)
        {

            throw;
        }
        return dt;
    }

    public DataTable GetEmplist(string Year)
    {
        TdsEmployeeDAL objTdsDal = new TdsEmployeeDAL();
        return objTdsDal.GetEmplist(Year);
    }
}
