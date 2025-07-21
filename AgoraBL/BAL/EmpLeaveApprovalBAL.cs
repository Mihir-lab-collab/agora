using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Globalization;
using AgoraBL.DAL;
using AgoraBL.Models;
using AgoraBL.Common;
using System.Threading.Tasks;

/// <summary>
/// Summary description for EmpLeaveApprovalBAL
/// </summary>
namespace AgoraBL.BAL
{
    public class EmpLeaveApprovalBAL
    {
        public string EmpLeaveID { get; set; }
        public string EmpID { get; set; }
        public int LocationID { get; set; }
        public string EmpName { get; set; }
        public string EmpNameID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveFrom { get; set; }
        public string LeaveTo { get; set; }
        public string LeaveAppliedOn { get; set; }
        public string LeaveReason { get; set; }
        public string AdminComment { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveSanctionOn { get; set; }
        public string LeaveSanctionBy { get; set; }
        public string BalanceLeave { get; set; }
        public string IsApproved { get; set; }
        public int IsTeam { get; set; }

        public int TotalLeave { get; set; }

        public EmpLeaveApprovalBAL()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<EmpLeaveApprovalBAL> GetEmpLeaves(string mode, string status, string name, string from, string to, int locationID, int loginID, int includeArchive)
        {
            EmpLeaveApprovalBAL objBLL = new EmpLeaveApprovalBAL();
            EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
            objBLL.LeaveStatus = status;
            objBLL.EmpName = name;
            objBLL.LeaveFrom = from;
            objBLL.LeaveTo = to;
            objBLL.LocationID = locationID;
            return BindLeaves(objDAL.GetEmpLeaves(mode, objBLL, loginID, includeArchive));
        }

        private List<EmpLeaveApprovalBAL> BindLeaves(DataTable dt)
        {
            List<EmpLeaveApprovalBAL> lst = new List<EmpLeaveApprovalBAL>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EmpLeaveApprovalBAL obj = new EmpLeaveApprovalBAL();
                    obj.EmpLeaveID = dt.Rows[i]["EmpLeaveID"].ToString();
                    obj.EmpID = dt.Rows[i]["EmpID"].ToString();
                    obj.EmpName = dt.Rows[i]["EmpName"].ToString();
                    obj.EmpNameID = dt.Rows[i]["EmpName"].ToString() + "(" + dt.Rows[i]["EmpID"].ToString() + ")";
                    obj.LeaveType = dt.Rows[i]["LeaveType"].ToString();
                    obj.LeaveFrom = dt.Rows[i]["LeaveFrom"].ToString();
                    obj.LeaveTo = dt.Rows[i]["LeaveTo"].ToString();
                    obj.LeaveAppliedOn = dt.Rows[i]["LeaveEntryDate"].ToString();
                    obj.LeaveReason = dt.Rows[i]["leavedesc"].ToString();
                    obj.AdminComment = dt.Rows[i]["leaveComment"].ToString();
                    obj.LeaveStatus = dt.Rows[i]["leavestatus"].ToString();
                    obj.LeaveSanctionOn = dt.Rows[i]["LeaveSenctionedDate"].ToString();
                    obj.LeaveSanctionBy = dt.Rows[i]["LeaveSanctionBy"].ToString();
                    obj.BalanceLeave = dt.Rows[i]["BalanceLeave"].ToString();
                    obj.IsApproved = dt.Rows[i]["isApproved"].ToString();
                    obj.TotalLeave = Convert.ToInt32(dt.Rows[i]["Totleave"].ToString());
                    obj.IsTeam = Convert.ToInt32(dt.Rows[i]["isTeam"].ToString());

                    lst.Add(obj);
                }
            }
            return lst;
        }

        public DataTable GetLeaveType(string mode)
        {
            DataTable dt = new DataTable();
            EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
            dt = objDAL.GtLeaveType(mode);
            return dt;
        }

        public string GetProfile(string mode, string empID)
        {

            EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();

            return objDAL.GetProfile(mode, empID);
        }

        public DataSet UpdateEmpLeaveStatus(string mode, string empID, string empLeaveID, string leaveStatus, string LeaveType, string AdminComment, string SanctionedBy, string fDate, string tDate)
        {
            EmployeeMasterBAL objEmployeeMasterBAL = new EmployeeMasterBAL();
            string empLeaveStatus = string.Empty;
            DataTable dtResult = objEmployeeMasterBAL.GETLeaveByEMPLeaveID("GETLeaveByEMPLeaveID",Convert.ToInt32(empLeaveID));
            DataSet dsEmpLeaveUpdate = new DataSet();
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                empLeaveStatus = Convert.ToString(dtResult.Rows[0]["leaveStatus"]);
                if (!string.IsNullOrEmpty(empLeaveStatus))
                {
                    if (!(string.Compare(empLeaveStatus,"p",true)==0))
                    {
                        DataTable dt = new DataTable("LeaveStatus");
                        DataColumn idColumn = new DataColumn("ID", typeof(int));
                        DataColumn LeaveUpdatedColumn = new DataColumn("LeaveUpdated", typeof(string));
                        dt.Columns.Add(idColumn);
                        dt.Columns.Add(LeaveUpdatedColumn);
                        dt.PrimaryKey = new DataColumn[] { idColumn };
                        DataRow row = dt.NewRow();
                        row["ID"] = 1;
                        row["LeaveUpdated"] = "LeaveUpdated";
                        dt.Rows.Add(row);
                        dsEmpLeaveUpdate.Tables.Add(dt);
                        return dsEmpLeaveUpdate;
                    }
                }
            }
            string strReturnString = string.Empty;
            string ip = "0.0.0.0";
            EmpLeaveApprovalBAL objBLL = new EmpLeaveApprovalBAL();
            EmpLeaveApprovalDAL objDAL = new EmpLeaveApprovalDAL();
            objBLL.EmpID = empID;
            objBLL.EmpLeaveID = empLeaveID;
            objBLL.LeaveStatus = leaveStatus;
            objBLL.LeaveType = LeaveType;
            objBLL.AdminComment = AdminComment;
            objBLL.LeaveSanctionBy = SanctionedBy;
            objBLL.LeaveFrom = fDate;
            objBLL.LeaveTo = tDate;
            dsEmpLeaveUpdate = objDAL.UpdateEmpLeaveStatus(mode, ip, objBLL);
            
           if (dsEmpLeaveUpdate != null && dsEmpLeaveUpdate.Tables[0].Rows.Count > 0)
            {
                strReturnString = "Leave updated successfully.";
                string strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Pending;
                //Added for send leave notification through AI
                //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(Convert.ToInt32(userIdentityData.EmpID), userIdentityData.LeaveFrom, userIdentityData.LeaveType,
                //    userIdentityData.LeaveReason, userIdentityData.LeaveTo, dsEmpLeave, userIdentityData.EmpName);

                string DBEmpLeaveStatus = string.Empty;
                string DBUpdateDate = string.Empty;
                string DBUpdatedBy = string.Empty;
                string DBReason = string.Empty;
                if (dsEmpLeaveUpdate.Tables.Count>2 && dsEmpLeaveUpdate.Tables[2]!=null && dsEmpLeaveUpdate.Tables[2].Rows.Count>0)
                {
                    DBEmpLeaveStatus = Convert.ToString(dsEmpLeaveUpdate.Tables[2].Rows[0]["leaveStatus"]);
                    DBUpdatedBy = Convert.ToString(dsEmpLeaveUpdate.Tables[2].Rows[0]["UpdatedBy"]);
                    DBUpdateDate = Convert.ToString(dsEmpLeaveUpdate.Tables[2].Rows[0]["UpdatedOn"]);
                    DBReason = Convert.ToString(dsEmpLeaveUpdate.Tables[2].Rows[0]["leaveDesc"]);
                }
                if (string.Compare(DBEmpLeaveStatus,"a",true)==0)
                {
                    strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Approved;
                }
                else if (string.Compare(DBEmpLeaveStatus, "r", true) == 0)
                {
                    strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Rejected;
                }
                else if (string.Compare(DBEmpLeaveStatus, "p", true) == 0)
                {
                    strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Pending;
                }
                else
                {
                    strEmpLeaveStatus = ClsConstant.EmpLeaveStatus.Pending;
                }
                Task.Run(() => objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI("LeaveUpdate", Convert.ToInt32(empID), fDate, LeaveType, DBReason, tDate, dsEmpLeaveUpdate, EmpName, EmpLeaveStatus: strEmpLeaveStatus, UpdateDate: DBUpdateDate, UpdatedBy: DBUpdatedBy, leaveId: Convert.ToInt32(objBLL.EmpLeaveID),UpdatedReason: AdminComment));
                //objEmployeeMasterBAL.SendLeaveNotificationToHrThroughAI(Convert.ToInt32(empID), fDate, LeaveType, LeaveReason, tDate, dsEmpLeaveUpdate, EmpName, EmpLeaveStatus: strEmpLeaveStatus, UpdateDate: DBUpdateDate, UpdatedBy: DBUpdatedBy);

                //SendMail(dsEmpLeave.Tables[0],
                //leaveFrom,
                //               leaveTo,
                //               Reason, out IsEmailSent);

            }
            else
            {
                strReturnString = "Failed! to update Leave";
            }
            return dsEmpLeaveUpdate;
        }
    } 
}