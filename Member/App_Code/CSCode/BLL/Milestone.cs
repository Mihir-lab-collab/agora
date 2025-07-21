using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Customer.Model;
/// <summary>
/// Summary description for mileStone
/// </summary>
/// 
public class mileStoneModel
{
    public int projID { get; set; }
    public int projMilestoneID { get; set; }
    public string name { get; set; }
    public string custName { get; set; }
    public string custAddress { get; set; }
    public int currID { get; set; }
    public string currSymbol { get; set; }
    public string MileDescription { get; set; }
    public decimal BalanceAmount { get; set; }
    public decimal Amount { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal CalBalance { get; set; }
    public decimal currExRate { get; set; }
    public string PInvoiceNo { get; set; }
    public bool isDiabled { get; set; }
}

public class mileStoneDue
{
    public int ProjectMilestoneID { get; set; }
    public string Name { get; set; }
    public int projId { get; set; }
    public decimal Amount { get; set; }
    public int projManagerId { get; set; }
    public int AccountMgrId { get; set; }
    public string ProjectManager { get; set; }
    public string ProjectManagerEmail { get; set; }
    public int AccountManager { get; set; }
    public string AccountManagerEmail { get; set; }
    public string DueDate { get; set; }


    public static DataSet getMilestoneDue(int days)
    {
        mileStoneDAL objrecmile = new mileStoneDAL();
        return objrecmile.getMilestoneDue(days);
    }
}

public class mileStone
{
    public string mode { get; set; }
    public int projID { get; set; }
    public int projMilestoneID { get; set; }
    public string name { get; set; }
    public string amount { get; set; }
    public string DeliveryDate { get; set; }
    public string dueDate { get; set; }
    public int EstHours { get; set; }
    public decimal ExRate { get; set; }
    public decimal currExRate { get; set; }
    public string Description { get; set; }
    public int insertedBy { get; set; }
    public int modifiedBy { get; set; }
    public string xml { get; set; }
    public decimal BalanceAmount { get; set; }
    public decimal OriginalAmount { get; set; }
    public bool IsRecurring { get; set; }
    public int RecurringMSID { get; set; }
    public string MaxDueDate { get; set; }
    public string BalAmount { get; set; }


    public mileStone()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public mileStone(int projID, int projMilestoneID, string name, string amount, decimal ExRate, decimal currExRate, string dueDate, int EstHours, string Description, decimal BalanceAmount, decimal OriginalAmount, bool IsRecurring, int RecurringMSID)
    {
        this.projID = projID;
        this.projMilestoneID = projMilestoneID;
        this.name = name;
        this.amount = amount;
        this.ExRate = ExRate;
        this.currExRate = currExRate;
        this.dueDate = dueDate;
        this.EstHours = EstHours;
        this.Description = Description;
        this.BalanceAmount = BalanceAmount;
        this.OriginalAmount = OriginalAmount;
        this.IsRecurring = IsRecurring;
        this.RecurringMSID = RecurringMSID;
    }

    public mileStone(int projID, int projMilestoneID, string name)
    {
        // TODO: Complete member initialization
        this.projID = projID;
        this.projMilestoneID = projMilestoneID;
        this.name = name;
    }

    public mileStone(int projMilestoneID, string name)
    {
        // TODO: Complete member initialization
        this.projMilestoneID = projMilestoneID;
        this.name = name;
    }

    public static List<mileStone> getMileStone(string mode, int projid)
    {
        mileStoneDAL objmil = new mileStoneDAL();
        return objmil.getMileStone(mode, projid);

    }

    public static void DeleteMilestone(string mode, int ProjectMileStoneID)
    {
        mileStone curMilestone = new mileStone();
        curMilestone.projMilestoneID = ProjectMileStoneID;
        mileStoneDAL objDelete = new mileStoneDAL();
        objDelete.DeleteMilestone(mode, ProjectMileStoneID);
    }

    public int insertMileStoneData(string mode, int ProjID, int projMilestoneID, string name, string amount, decimal ExRate, string dueDate, string DeliveryDate, int EstHours, string Description, int insertedBy, decimal BalanceAmount, bool IsRecurring, int RecurringMSID)
    {
        mileStone objInsert = new mileStone();
        objInsert.mode = mode;
        objInsert.projID = ProjID;
        objInsert.projMilestoneID = projMilestoneID;
        objInsert.name = name;
        objInsert.amount = amount;
        objInsert.ExRate = ExRate;
        objInsert.dueDate = dueDate;
        objInsert.DeliveryDate = DeliveryDate;
        objInsert.EstHours = EstHours;
        objInsert.Description = Description;
        objInsert.insertedBy = insertedBy;
        objInsert.BalanceAmount = BalanceAmount;
        objInsert.IsRecurring = IsRecurring;
        objInsert.RecurringMSID = RecurringMSID;
        mileStoneDAL objInsertInto = new mileStoneDAL();
        return objInsertInto.insertMileStoneData(objInsert);
    }


    public static List<mileStone> getRecurringMilestone()
    {
        mileStoneDAL objrecmile = new mileStoneDAL();
        return objrecmile.getRecurringMilestone();
    }



    public List<mileStoneModel> getInvoiceMileStone(string projid)
    {
        mileStoneDAL objmil = new mileStoneDAL();
        return objmil.getInvoiceMileStone(projid);
    }


    public List<mileStoneModel> getProformaInvoiceMileStone(string projid)
    {
        mileStoneDAL objmil = new mileStoneDAL();
        return objmil.getProformaInvoiceMileStone(projid);
    }

    public List<mileStoneModel> GetMileStoneDetails(int projid, int ProjectMilestoneID)
    {
        mileStoneDAL objmil = new mileStoneDAL();
        return objmil.GetMileStoneDetails(projid, ProjectMilestoneID);
    }

    public static List<KeyValueModel> GetAllProjects()
    {
        mileStoneDAL objprojectMile = new mileStoneDAL();
        return objprojectMile.GetAllProjects();
    }

    public static List<KeyValueModel> GetProjects()
    {
        mileStoneDAL objprojectMile = new mileStoneDAL();
        return objprojectMile.GetProjects();
    }


    public DataSet GetInvoiceDetails(string invId)
    {
        mileStoneDAL objEdCorp = new mileStoneDAL();
        return objEdCorp.GetInvoiceDetails(invId);
    }


    public DataSet GetProformaInvoiceDetails(string invId, string projId)
    {
        mileStoneDAL objEdCorp = new mileStoneDAL();
        return objEdCorp.GetProformaInvoiceDetails(invId, projId);
    }

}