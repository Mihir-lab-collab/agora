using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;


/// <summary>
/// Summary description for MilstoneDueBLL
/// </summary>
public class MilstoneDueBLL
{
    public int MID { get; set; }
    public string Name { get; set; }
    public string Amount { get; set; }
    public string Balance { get; set; }
    public decimal ExRate { get; set; }
    public string DueDate { get; set; }
    public string DueDays { get; set; }
    public int projectMilestoneID { get; set; }
    public int EstHours { get; set; }
    public string Description { get; set; }
    public string isRecurring { get; set; }
    public string InsertedDate { get; set; }
    public string currSymbol { get; set; }
    public int ProjID { get; set; }
    public string ProjName { get; set; }
    public int DueFor { get; set; }

    public MilstoneDueBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public static List<MilstoneDueBLL> GetMilestoneDue(string mode)
    {
        MilstoneDueDAL objDAL = new MilstoneDueDAL();
        return BindMilestoneDue(objDAL.GetMilestoneDue(mode));
    }

    private static List<MilstoneDueBLL> BindMilestoneDue(DataTable dt)
    {
       // CultureInfo cuInfo = new CultureInfo("en-IN");
        List<MilstoneDueBLL> lst = new List<MilstoneDueBLL>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MilstoneDueBLL obj = new MilstoneDueBLL();
                obj.Name = dt.Rows[i]["Name"].ToString();
             //   obj.currSymbol = dt.Rows[i]["currSymbol"].ToString();
                obj.Amount = dt.Rows[i]["Amount"].ToString();
                obj.Balance = dt.Rows[i]["Balance"].ToString();
               // obj.ExRate = Convert.ToDecimal(dt.Rows[i]["ExRate"].ToString());
                obj.DueDate = dt.Rows[i]["DueDate"].ToString();
                obj.DueDays = dt.Rows[i]["DueDays"].ToString();
                obj.EstHours = Convert.ToInt32(dt.Rows[i]["EstHours"].ToString());
                obj.ProjID = Convert.ToInt32(dt.Rows[i]["projId"].ToString());
                //obj.Description = dt.Rows[i]["Description"].ToString();
              //  obj.isRecurring = dt.Rows[i]["isRecurring"].ToString();
               //obj.ProjID = Convert.ToInt32(dt.Rows[i]["projectMilestoneID"].ToString());
              //  obj.DueFor = Convert.ToInt32(dt.Rows[i]["Duefor"].ToString());
                obj.ProjName = dt.Rows[i]["ProjName"].ToString();

               // obj.Amount =obj.currSymbol +" "+ (Convert.ToDecimal(obj.Amount)).ToString("C", cuInfo).Remove(0, 2).Trim();
               // obj.Balance = obj.currSymbol +" "+ (Convert.ToDecimal(obj.Balance)).ToString("C", cuInfo).Remove(0, 2).Trim();
                lst.Add(obj);
            }
        }
        return lst;

    }

}