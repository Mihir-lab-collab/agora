using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Designation
/// </summary>
public class DesignationBLL
{
    public int DesigID { get; set; }
    public string Designation { get; set; }
    //public int SecurityLevel { get; set;}
    public DesignationBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public DesignationBLL(int DesigID, string Designation)
    {
        this.DesigID = DesigID;
        this.Designation = Designation;
    }
    public static List<DesignationBLL> GetDesignation(string mode)
    {       
        return GetDesignation(mode, 0);
    }

    private static List<DesignationBLL> GetDesignation(string mode, int desigID)
    {
        DesignationDAL objDesignationDAL = new DesignationDAL();
        return objDesignationDAL.GetDesignationDetails(mode, desigID);
    }

    public static int SaveDesignation(string mode, int desigID, string designation)
    {
        DesignationBLL objDesig = new DesignationBLL();
        objDesig.DesigID = desigID;
        objDesig.Designation = designation;
        DesignationDAL objDesignationDAL = new DesignationDAL();
        return objDesignationDAL.SaveDesignation(mode, objDesig);
    }

    
}