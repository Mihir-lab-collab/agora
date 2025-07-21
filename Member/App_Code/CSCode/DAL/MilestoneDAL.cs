using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;
using Customer.Model;
using System.Globalization;
using System.IO;
using CSCode;

/// <summary>
/// Summary description for mileStoneDAL
/// </summary>

public class mileStoneDAL
{

    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public mileStoneDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /***************************************************************** MILESTONE STARTS ***************************************************/
    public List<mileStone> getMileStone(string mode, int projid)
    {
        List<mileStone> curMileStone = new List<mileStone>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_ProjectMilestone", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", mode);
            cmd.Parameters.AddWithValue("@ProjID", projid);

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mileStone obj = new mileStone();
                        obj.projID = Convert.ToInt32(reader["projId"]);
                        obj.projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"]);
                        obj.name = Convert.ToString(reader["Name"]);
                        obj.amount = Convert.ToString(Global.GetCurrencyFormat(Convert.ToDouble(reader["Amount"])));
                        obj.ExRate = Convert.ToDecimal(reader["ExRate"]);
                        obj.currExRate = Convert.ToDecimal(reader["currExRate"]);
                        obj.DeliveryDate = Convert.ToString(reader["DeliveryDate"]);
                        obj.dueDate = Convert.ToString(reader["DueDate"]);
                        obj.EstHours = Convert.ToInt32(reader["EstHours"]);
                        obj.Description = Convert.ToString(reader["Description"]);
                        obj.BalAmount = Convert.ToInt32(reader["BalanceAmount"]) != 0 ? Convert.ToString(Global.GetCurrencyFormat(Convert.ToDouble(reader["BalanceAmount"]))) : "0";
                        obj.IsRecurring = (reader["IsRecurring"] == DBNull.Value) ? false : Convert.ToBoolean(reader["IsRecurring"]);
                        obj.RecurringMSID = Convert.ToInt32(reader["ProjectMSReccuringID"]);
                        // obj.MaxDueDate = Convert.ToString(reader["Max_DueDate"]);
                        obj.MaxDueDate = Convert.ToString(reader["Max_DeliveryDate"]);
                        curMileStone.Add(obj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }
        return curMileStone;
    }


    public List<mileStone> getRecurringMilestone()
    {
        List<mileStone> recMileStone = new List<mileStone>();
        try
        {
            SqlConnection con = new SqlConnection(_strConnection);

            SqlCommand cmd = new SqlCommand("SP_ProjectMilestone", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mode", "GetRecurring");

            using (con)
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mileStone obj = new mileStone();
                        obj.projID = Convert.ToInt32(reader["projId"]);
                        obj.projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"]);
                        obj.name = Convert.ToString(reader["Name"]);
                        obj.amount = Convert.ToString(reader["Amount"]);
                        obj.ExRate = Convert.ToDecimal(reader["ExRate"]);
                        obj.insertedBy = Convert.ToInt32(reader["InsertedBy"]);
                        obj.dueDate = Convert.ToString(reader["DueDate"]);
                        obj.DeliveryDate = Convert.ToString(reader["DeliveryDate"]);
                        obj.EstHours = Convert.ToInt32(reader["EstHours"]);
                        obj.Description = Convert.ToString(reader["Description"]);
                        obj.BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"]);
                        obj.IsRecurring = (reader["IsRecurring"] == DBNull.Value) ? false : Convert.ToBoolean(reader["IsRecurring"]);
                        obj.RecurringMSID = Convert.ToInt32(reader["ProjectMSReccuringID"]);
                        recMileStone.Add(obj);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }

        return recMileStone;
    }


    public DataSet getMilestoneDue(int days)
    {
        //List<mileStoneDue> recMileStonedue = new List<mileStoneDue>();
        //try
        //{
        //    SqlConnection con = new SqlConnection(_strConnection);
        //    SqlCommand cmd = new SqlCommand("SP_ProjectMilestone", con);
        //cmd.Parameters.AddWithValue("@mode", "DueDate");
        //cmd.Parameters.AddWithValue("@DueDaysAdd", 2);
        //cmd.CommandType = CommandType.StoredProcedure;
        //using (con)
        //{
        //    con.Open();
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            mileStoneDue obj = new mileStoneDue();
        //            obj.ProjectMilestoneID = Convert.ToInt32(reader["ProjectMilestoneID"]);
        //            obj.Name = Convert.ToString(reader["Name"]);
        //            obj.projId = Convert.ToInt32(reader["projId"]);
        //            obj.Amount = Convert.ToDecimal(reader["Amount"]);
        //            obj.projManagerId = Convert.ToInt32(reader["projManagerId"]);
        //            obj.AccountMgrId = Convert.ToInt32(reader["AccountMgrId"]);
        //            obj.ProjectManager = Convert.ToString(reader["ProjectManager"]);
        //            obj.ProjectManagerEmail = Convert.ToString(reader["ProjectManagerEmail"]);
        //            obj.AccountManager = Convert.ToInt32(reader["AccountManager"]);
        //            obj.AccountManagerEmail = Convert.ToString(reader["AccountManagerEmail"]);
        //            obj.DueDate = Convert.ToString(reader["DueDate"]);
        //            recMileStonedue.Add(obj);
        //        }
        //    }
        //}
        //}
        //catch (Exception ex)
        //{
        //    ex.WriteErrorLog();
        //}

        DataSet dt = new DataSet();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                SqlCommand cmd = new SqlCommand("SP_ProjectMilestone", con);
                // cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@mode", "DueDate");
                cmd.Parameters.AddWithValue("@DueDaysAdd", days);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                // ds = new DataSet();
                da.Fill(dt);
                con.Close();
                // return dt;
            }
        }
        catch (Exception ex)
        {
            //  return dt;
        }

        return dt;
    }

    public int insertMileStoneData(mileStone objInsert)
    {
        int outputid = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ProjectMilestone", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", objInsert.mode);
                cmd.Parameters.AddWithValue("@ProjID", objInsert.projID);
                cmd.Parameters.AddWithValue("@ProjectMileStoneID", objInsert.projMilestoneID);
                cmd.Parameters.AddWithValue("@Name", objInsert.name);
                cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(objInsert.amount));
                cmd.Parameters.AddWithValue("@ExRate", Convert.ToDecimal(objInsert.ExRate));
                CultureInfo provider = CultureInfo.CreateSpecificCulture("en-UK");
                if (objInsert.dueDate != "")
                {

                    DateTime DueDate = Convert.ToDateTime(objInsert.dueDate);
                    cmd.Parameters.AddWithValue("@DueDate", DueDate);
                }
                if (objInsert.DeliveryDate != "")
                {

                    DateTime DeliveryDate = Convert.ToDateTime(objInsert.DeliveryDate);
                    cmd.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
                }
                cmd.Parameters.AddWithValue("@EstHours", objInsert.EstHours);
                cmd.Parameters.AddWithValue("@description", objInsert.Description);

                cmd.Parameters.AddWithValue("@UserID", objInsert.insertedBy);
                cmd.Parameters.AddWithValue("@IsRecurring", Convert.ToBoolean(objInsert.IsRecurring));
                cmd.Parameters.AddWithValue("@ProjectMSReccuringID", Convert.ToInt32(objInsert.RecurringMSID));


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }
        return outputid;
    }

    public void DeleteMilestone(string mode, int ProjectMileStoneID)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_ProjectMilestone", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@ProjectMileStoneID", ProjectMileStoneID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable ds = new DataTable();
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }
    }

    /***************************************************************** MILESTONE ENDS ***************************************************/

    public List<mileStoneModel> getInvoiceMileStone(string projid)
    {
        int locationID = 0;
        string[] IDs = projid.Split('@'); // projID and LocationID
        int pID = Convert.ToInt32(IDs[0]);
        if (IDs.Length > 1)
            locationID = Convert.ToInt32(IDs[1]);
        List<mileStoneModel> curMileStone = new List<mileStoneModel>();
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SP_InvoiceGetProjectDetail", con);
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GETINVOICEDATA");
                cmd.Parameters.AddWithValue("@ProjID", pID);
                cmd.Parameters.AddWithValue("@ProjectMilestoneID", 0);
                cmd.Parameters.AddWithValue("@LocationID", locationID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            foreach (DataRow reader in dt.Rows)
            {
                curMileStone.Add(new mileStoneModel()
                {
                    projID = Convert.ToInt32(reader["projId"].ToString()),
                    projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"].ToString()),
                    name = reader["Name"].ToString() == "" ? "" : Convert.ToString(reader["Name"].ToString()),
                    currID = Convert.ToInt32(reader["currID"].ToString()),
                    currSymbol = reader["currSymbol"].ToString() == "" ? "" : Convert.ToString(reader["currSymbol"].ToString()),
                    custAddress = reader["custAddress"].ToString() == "" ? "" : Convert.ToString(reader["custAddress"].ToString()),
                    custName = reader["custCompany"].ToString() == "" ? "" : Convert.ToString(reader["custCompany"].ToString()),
                    MileDescription = reader["MileDescription"].ToString() == "" ? "" : Convert.ToString(reader["MileDescription"].ToString()),
                    Amount = Convert.ToDecimal(reader["Amount"].ToString()),
                    OriginalAmount = Convert.ToDecimal(reader["OriginalAmount"].ToString()),
                    BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"].ToString()),
                    currExRate = Convert.ToDecimal(reader["ExRate"].ToString()),
                    PInvoiceNo = reader["InvoiceNo"].ToString()
                }
                         );
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }
        return curMileStone;
    }

    public List<mileStoneModel> getProformaInvoiceMileStone(string projid)
    {
        int locationID = 0;
        string[] IDs = projid.Split('@'); // projID and LocationID
        int pID = Convert.ToInt32(IDs[0]);
        if (IDs.Length > 1)
            locationID = Convert.ToInt32(IDs[1]);
        List<mileStoneModel> curMileStone = new List<mileStoneModel>();
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mode", "GetInvoiceData");
                cmd.Parameters.AddWithValue("@ProjID", pID);
                cmd.Parameters.AddWithValue("@ProjectMilestoneID", 0);
                cmd.Parameters.AddWithValue("@LocationID", locationID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            foreach (DataRow reader in dt.Rows)
            {
                curMileStone.Add(new mileStoneModel()
                {
                    projID = Convert.ToInt32(reader["projId"].ToString()),
                    projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"].ToString()),
                    name = reader["Name"].ToString() == "" ? "" : Convert.ToString(reader["Name"].ToString()),
                    currID = Convert.ToInt32(reader["currID"].ToString()),
                    currSymbol = reader["currSymbol"].ToString() == "" ? "" : Convert.ToString(reader["currSymbol"].ToString()),
                    custAddress = reader["custAddress"].ToString() == "" ? "" : Convert.ToString(reader["custAddress"].ToString()),
                    custName = reader["custCompany"].ToString() == "" ? "" : Convert.ToString(reader["custCompany"].ToString()),
                    MileDescription = reader["MileDescription"].ToString() == "" ? "" : Convert.ToString(reader["MileDescription"].ToString()),
                    Amount = Convert.ToDecimal(reader["Amount"].ToString()),
                    OriginalAmount = Convert.ToDecimal(reader["OriginalAmount"].ToString()),
                    BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"].ToString()),
                    currExRate = Convert.ToDecimal(reader["ExRate"].ToString()),
                    PInvoiceNo = reader["InvoiceNo"].ToString()
                }
                         );
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }
        return curMileStone;
    }


    public DataSet GetProformaInvoiceDetails(string invoiceId, string projId)
    {
        int locationID = 0;
        string[] IDs = projId.Split('@'); // projID and LocationID
        int pID = Convert.ToInt32(IDs[0]);
        if (IDs.Length > 1)
            locationID = Convert.ToInt32(IDs[1]);
        SqlConnection con = new SqlConnection(_strConnection);

        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "InvoiceDetails");
        cmd.Parameters.AddWithValue("@ProformaInvoiceID", Convert.ToInt32(invoiceId));
        cmd.Parameters.AddWithValue("@projId", pID);
        cmd.Parameters.AddWithValue("@LocationID", locationID);
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }



    public List<mileStoneModel> GetMileStoneDetails(int projid, int ProjectMilestoneID)
    {
        List<mileStoneModel> curMileStone = new List<mileStoneModel>();
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(_strConnection))
            {
                con.Open();
                //SqlCommand cmd = new SqlCommand("SP_InvoiceGetProjectDetail", con);
                SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GETINVOICEDATA");
                cmd.Parameters.AddWithValue("@ProjID", projid);
                cmd.Parameters.AddWithValue("@ProjectMilestoneID", ProjectMilestoneID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            foreach (DataRow reader in dt.Rows)
            {
                curMileStone.Add(new mileStoneModel()
                {
                    projID = Convert.ToInt32(reader["projId"].ToString()),
                    projMilestoneID = Convert.ToInt32(reader["ProjectMileStoneID"].ToString()),
                    MileDescription = reader["MileDescription"].ToString() == "" ? "" : Convert.ToString(reader["MileDescription"].ToString()),
                    Amount = Convert.ToDecimal(reader["Amount"].ToString()),
                    OriginalAmount = Convert.ToDecimal(reader["OriginalAmount"].ToString()),
                    BalanceAmount = Convert.ToDecimal(reader["BalanceAmount"].ToString()),
                    CalBalance = Convert.ToDecimal(reader["BalanceAmount"].ToString())
                }
                         );
            }
        }
        catch (Exception ex)
        {
            ex.WriteErrorLog();
        }
        return curMileStone;
    }

    public DataSet GetInvoiceDetails(string invoiceId)
    {

        SqlConnection con = new SqlConnection(_strConnection);
        //SqlCommand cmd = new SqlCommand("USP_GetInvoiceDetails", con);
        SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "INVOICE_DETAILS");
        cmd.Parameters.AddWithValue("@ProjectInvoiceID", Convert.ToInt32(invoiceId));
        using (con)
        {
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
    }

    public List<KeyValueModel> GetAllProjects()
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        //SqlCommand cmd = new SqlCommand("sp_GetAllprojectMaster", con);
        SqlCommand cmd = new SqlCommand("SP_ProjectInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetAllproject");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            List<KeyValueModel> list = new List<KeyValueModel>();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KeyValueModel()
                {
                    Key = reader["projName"].ToString(),
                    Value = Convert.ToInt32(reader["projId"].ToString())
                });
            }
            return list;
        }
    }

    public List<KeyValueModel> GetProjects()
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        //SqlCommand cmd = new SqlCommand("sp_GetAllprojectMaster", con);
        SqlCommand cmd = new SqlCommand("SP_ProformaInvoice", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Mode", "GetAllproject");
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            List<KeyValueModel> list = new List<KeyValueModel>();

            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KeyValueModel()
                {
                    Key = reader["projName"].ToString(),
                    Value = Convert.ToInt32(reader["projId"].ToString())
                });
            }
            return list;
        }
    }

}

