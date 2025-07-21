using AgoraBL.Common;
using AgoraBL.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraBL.BAL
{
    public class EmpWFHApprovalBAL
    {
        public EmpWFHApprovalBAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public string EmpWFHID { get; set; }
        public string EmpID { get; set; }
        public string LoginEmpId { get; set; }
        public int LocationID { get; set; }
        public string EmpName { get; set; }
        public string EmpNameID { get; set; }
        public string WFHType { get; set; }
        public string WFHFrom { get; set; }
        public string WFHTo { get; set; }
        public string WFHAppliedOn { get; set; }
        public string WFHReason { get; set; }
        public string AdminComment { get; set; }
        public string WFHStatus { get; set; }
        public string WFHSanctionOn { get; set; }
        public string WFHSanctionBy { get; set; }
        public string IsApproved { get; set; }
        public int IsTeam { get; set; }
        public int TotalWFH { get; set; }
        public string WFHEntryDate { get; set; }
        public string AttOutTime { get; set; }
        public string AttInTime { get; set; }
        public string EmployeeWFHCount { get; set; }

        public DataSet UpdateEmpWFHStatus(string mode, string empID, string empWFHID, string wFHStatus, string adminComment, string sanctionedBy, string fDate, string tDate)
        {
            EmpWFHBAL objEmpWFHBAL = new EmpWFHBAL();
            EmpWFHApprovalDAL objEmpWFEmpWFHApprovalDALL = new EmpWFHApprovalDAL();
            string empWFHStatus = string.Empty;
            DataTable dtResult = objEmpWFEmpWFHApprovalDALL.GETWFHByEMPWFHID("GETWFHByEMPWFHID", Convert.ToInt32(empWFHID));
            DataSet dsEmpWFHUpdate = new DataSet();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                empWFHStatus = Convert.ToString(dtResult.Rows[0]["WFHStatus"]);
                if (!string.IsNullOrEmpty(empWFHStatus))
                {
                    if (!(string.Compare(empWFHStatus, "p", true) == 0))
                    {
                        DataTable dt = new DataTable("WFHStatus");
                        DataColumn idColumn = new DataColumn("ID", typeof(int));
                        DataColumn WFHUpdatedColumn = new DataColumn("WFHUpdated", typeof(string));
                        dt.Columns.Add(idColumn);
                        dt.Columns.Add(WFHUpdatedColumn);
                        dt.PrimaryKey = new DataColumn[] { idColumn };
                        DataRow row = dt.NewRow();
                        row["ID"] = 1;
                        row["WFHUpdated"] = "WFHUpdated";
                        dt.Rows.Add(row);
                        dsEmpWFHUpdate.Tables.Add(dt);
                        return dsEmpWFHUpdate;
                    }
                }
            }
            string strReturnString = string.Empty;
            string ip = "0.0.0.0";
            EmpWFHApprovalBAL objBLL = new EmpWFHApprovalBAL();
            EmpWFHApprovalDAL objDAL = new EmpWFHApprovalDAL();
            objBLL.EmpID = empID;
            objBLL.EmpWFHID = empWFHID;
            objBLL.WFHStatus = wFHStatus;
            objBLL.AdminComment = adminComment;
            objBLL.WFHSanctionBy = sanctionedBy;
            objBLL.WFHFrom = fDate;
            objBLL.WFHTo = tDate;
            dsEmpWFHUpdate = objEmpWFEmpWFHApprovalDALL.UpdateEmpWFHStatus(mode, ip, objBLL);

            if (dsEmpWFHUpdate != null && dsEmpWFHUpdate.Tables[0].Rows.Count > 0)
            {
                strReturnString = "WFH updated successfully.";
                string strEmpWFHStatus = ClsConstant.EmpWFHStatus.Pending;
                string DBEmpWFHStatus = string.Empty;
                string DBUpdateDate = string.Empty;
                string DBUpdatedBy = string.Empty;
                string DBReason = string.Empty;
                if (dsEmpWFHUpdate.Tables.Count > 2 && dsEmpWFHUpdate.Tables[2] != null && dsEmpWFHUpdate.Tables[2].Rows.Count > 0)
                {
                    DBEmpWFHStatus = Convert.ToString(dsEmpWFHUpdate.Tables[2].Rows[0]["WFHStatus"]);
                    DBUpdatedBy = Convert.ToString(dsEmpWFHUpdate.Tables[2].Rows[0]["UpdatedBy"]);
                    DBUpdateDate = Convert.ToString(dsEmpWFHUpdate.Tables[2].Rows[0]["UpdatedOn"]);
                    DBReason = Convert.ToString(dsEmpWFHUpdate.Tables[2].Rows[0]["WFHDesc"]);
                }
                if (string.Compare(DBEmpWFHStatus, "a", true) == 0)
                {
                    strEmpWFHStatus = ClsConstant.EmpWFHStatus.Approved;
                }
                else if (string.Compare(DBEmpWFHStatus, "r", true) == 0)
                {
                    strEmpWFHStatus = ClsConstant.EmpWFHStatus.Rejected;
                }
                else if (string.Compare(DBEmpWFHStatus, "p", true) == 0)
                {
                    strEmpWFHStatus = ClsConstant.EmpWFHStatus.Pending;
                }
                else
                {
                    strEmpWFHStatus = ClsConstant.EmpWFHStatus.Pending;
                }
                Task.Run(() => objEmpWFHBAL.SendWFHNotificationToHrThroughAI("WFHUpdate", Convert.ToInt32(empID), fDate, DBReason, tDate, dsEmpWFHUpdate, EmpName:string.Empty, EmpWFHStatus: strEmpWFHStatus, UpdateDate: DBUpdateDate, UpdatedBy: DBUpdatedBy, WFHId: Convert.ToInt32(objBLL.EmpWFHID), UpdatedReason: adminComment));
                //objEmployeeMasterBAL.SendWFHNotificationToHrThroughAI(Convert.ToInt32(empID), fDate, WFHType, WFHReason, tDate, dsEmpWFHUpdate, EmpName, EmpWFHStatus: strEmpWFHStatus, UpdateDate: DBUpdateDate, UpdatedBy: DBUpdatedBy);

            }
            else
            {
                strReturnString = "Failed! to update WFH";
            }
            return dsEmpWFHUpdate;
        }       
       
    }
}
