using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Customer.BLL
{
    /// <summary>
    /// Summary description for CustUsers
    /// </summary>
    public class BITS
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PM { get; set; }
        public string BA { get; set; }
        public string AccManager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal BudgetedCost { get; set; }
        public decimal ActualCost { get; set; }
        public decimal ActualPayment { get; set; }
        public decimal BudgetedHour { get; set; }
        public decimal ActualHour { get; set; }
        public decimal UnApprovedHours { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusComment { get; set; }
        public DateTime InsertedOn { get; set; }

        public string Status { get; set; }
        public string Duration { get; set; }
        public string Module { get; set; }
        public string Month { get; set; }
        public string Hours { get; set; }
        public string strUnApprovedHours { get; set; }
        public decimal ProjectHealth_Effort { get; set; }
        public decimal ProjectHealth_Cost { get; set; }
        public DateTime Reportdate { get; set; }

        public string TSYear { get; set; }
        public string TSMonth { get; set; }
        public decimal Cost { get; set; }
        //public decimal TotalHoursConsumed { get; set; }
        public string Percentage_Effort { get; set; }
        public string DevelopmentTeam { get; set; }
        public BITS()
        {

        }

        public BITS(int _projId, string _projName, string _projManager, string _BA, string _AccManager, string duration, decimal _projBudgetedHour, decimal _projActualHour, string _projstrUnApprovedHours,
                    string _projStatus, decimal _projBudgetedCost, decimal _projActualCost, decimal _projActualPayment, decimal _projHealth_Effort, decimal _projHealth_Cost, DateTime _reptdate)
        {
            this.ID = _projId;
            this.Name = _projName;
            this.PM = _projManager;
            this.BA = _BA;
            this.AccManager = _AccManager;
            this.Duration = duration;
            this.BudgetedHour = _projBudgetedHour;
            this.ActualHour = _projActualHour;
            this.strUnApprovedHours = _projstrUnApprovedHours;/*this.UnApprovedHours = _projUnApprovedHours;*/
            this.Status = _projStatus;
            this.BudgetedCost = _projBudgetedCost;
            this.ActualCost = _projActualCost;
            this.ActualPayment = _projActualPayment;
            this.ProjectHealth_Effort = _projHealth_Effort;
            this.ProjectHealth_Cost = _projHealth_Cost;
            this.Reportdate = _reptdate;
            //this.TotalHoursConsumed = _projTotalHoursConsumed;
            //this.PayReceivedIncreased = _PayReceivedIncreased;
            //this.ActualCostIncreased = _ActualCostIncreased;
        }

        public BITS(int _projId, string Module, string TSYear, string TSMonth, string Percentage_Effort, decimal Hours, decimal Cost)
        {
            this.ID = _projId;
            this.Module = Module;
            this.TSYear = TSYear;
            this.TSMonth = TSMonth;
            this.Percentage_Effort = Percentage_Effort;
            this.ActualHour = Hours;
            //this.UnApprovedHours = UnApprovedHours;
            this.Cost = Cost;
        }

        public BITS(string Module, string Percentage_Effort, decimal Hours, decimal Cost)
        {
            this.Module = Module;
            this.Percentage_Effort = Percentage_Effort;
            this.ActualHour = Hours;
            this.Cost = Cost;
            //this.TotalHoursConsumed = _projTotalHoursConsumed;
        }

        public static List<BITS> GetProjectDetails(int PMID, bool? showOverHeads, bool? Added, bool? Initiated, bool? InProgress, bool? UnderUAT, bool? OnHold, bool? CompletedClosed, bool? Cancelled, bool? UnderWarranty, bool? TNM, bool? FixedCost)
        {
            BITSDAL objCustProjects = new BITSDAL();
            return objCustProjects.GetProjectDetails(PMID, showOverHeads, Added, Initiated, InProgress, UnderUAT, OnHold, CompletedClosed, Cancelled, UnderWarranty, TNM, FixedCost);
        }

        public static List<BITS> GetTSBreakupDetails(int prjId)
        {
            BITSDAL objCustProjects = new BITSDAL();
            return objCustProjects.GetTSBreakupDetails(prjId);
        }

        public static List<BITS> GetTimesheetDetailsMonthwise(int prjId)
        {
            BITSDAL objCustProjects = new BITSDAL();
            return objCustProjects.GetTimesheetDetailsMonthwise(prjId);
        }

        public static List<BITS> GetTimesheetDetailsWorkwise(int prjId, int year, int month)
        {
            BITSDAL objCustProjects = new BITSDAL();
            return objCustProjects.GetTimesheetDetailsWorkwise(prjId, year, month);
        }
    }
}