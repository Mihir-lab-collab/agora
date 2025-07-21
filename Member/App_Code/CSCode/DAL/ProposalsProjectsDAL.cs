using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
using Customer.DAL;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
/// <summary>
/// Summary description for ProposalsProjectsDAL
/// </summary>
public class ProposalsProjectsDAL
{
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;

    public ProposalsProjectsDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public List<ProposalsProjectsBLL> GetProjectDetails(int EmpiID, Boolean IsAdmin)
    {
        List<ProposalsProjectsBLL> CurProposalsProjects = new List<ProposalsProjectsBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("spProposalDetail", con);
        cmd.Parameters.AddWithValue("@EmpID", EmpiID );
        cmd.Parameters.AddWithValue("@IsAdmin", IsAdmin);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CurProposalsProjects.Add(new ProposalsProjectsBLL(
                     reader["projectID"].ToString() == "" ? 0 : Convert.ToInt32(reader["projectID"].ToString()),
                    reader["projectTitle"].ToString() == "" ? "" : Convert.ToString(reader["projectTitle"].ToString()),
                     reader["projectDesc"].ToString() == "" ? "" : Convert.ToString(reader["projectDesc"].ToString()),
                     reader["Status"].ToString() == "" ? "" : ProposalStatus(Convert.ToString(reader["Status"].ToString())),
                    reader["CreatedBy"].ToString() == "" ? "" : Convert.ToString(reader["CreatedBy"].ToString()),
                    reader["CreatedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["CreatedOn"].ToString()),
                     reader["ModifiedBy"].ToString() == "" ? "" : Convert.ToString(reader["ModifiedBy"].ToString()),
                     reader["ModifiedOn"].ToString() == "" ? DateTime.Today : Convert.ToDateTime(reader["ModifiedOn"].ToString()),
                     reader["clientMail"].ToString() == "" ? "" : Convert.ToString(reader["clientMail"].ToString())
                    ));
            }
        }
        return CurProposalsProjects;
    }

    public int InsertProposalsProjects(ProposalsProjectsBLL objInsert)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("spProposalMaster", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@projTitle", objInsert.projectTitle);
        cmd.Parameters.AddWithValue("@projDesc", objInsert.projDesc);
        cmd.Parameters.AddWithValue("@ClientMail", objInsert.clientMail);
        cmd.Parameters.AddWithValue("@UserID", objInsert.insertedBy);
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }


    public bool UpdateProposalProjects(ProposalsProjectsBLL objInsert)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("spProposalMaster", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsert.mode);
        cmd.Parameters.AddWithValue("@ProposalMasterID", objInsert.proposalID);
        cmd.Parameters.AddWithValue("@projTitle", objInsert.projectTitle);
        cmd.Parameters.AddWithValue("@projDesc", objInsert.projDesc);
        cmd.Parameters.AddWithValue("@ClientMail", objInsert.clientMail);
        cmd.Parameters.AddWithValue("@UserID", objInsert.modifiedBy);
        cmd.Parameters.AddWithValue("@Status", objInsert.status);


        try
        {
            using (con)
            {
                con.Open();
                int output = Convert.ToInt32(cmd.ExecuteScalar());
                if (output == 1)
                    updated = true;
            }
        }
        catch (Exception ex)
        { }
        return updated;
    }

    public string ProposalStatus(string StatusCode)
    {
        string StatusName = StatusCode;

        if (StatusCode == "p")
        {
            StatusName = "Published";
        }
        else if (StatusCode == "d")
        {
            StatusName = "Draft";
        } 
        else if (StatusCode == "a")
        {
            StatusName = "Acquired";
        }
        else if (StatusCode == "r")
        {
            StatusName = "Rejected";
        }
        else
        {
            StatusName = "Undefined";
        }
        return StatusName;
    }


    public List<ProposalsProjectsBLL> GetProjectCSDetails(string mode)
    {
        List<ProposalsProjectsBLL> CurProposalsCSProjects = new List<ProposalsProjectsBLL>();
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("spProposal", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", mode);
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                CurProposalsCSProjects.Add(new ProposalsProjectsBLL(
                     reader["ProposalCSDefaultID"].ToString() == "" ? 0 : Convert.ToInt32(reader["ProposalCSDefaultID"].ToString()),
                    reader["ProjectTitle"].ToString() == "" ? "" : Convert.ToString(reader["ProjectTitle"].ToString()),
                     reader["ProjectUrl"].ToString() == "" ? "" : Convert.ToString(reader["ProjectUrl"].ToString()),
                     reader["ProjectDesc"].ToString() == "" ? "" : Convert.ToString(reader["ProjectDesc"].ToString()),
                     reader["ImageName"].ToString() == "" ? "" :Convert.ToString(reader["ImageData"].ToString())              
                    ));
            }
        }
        return CurProposalsCSProjects;
    }

    public int InsertProposalsCSProjects(ProposalsProjectsBLL objInsertCS)
    {
        int outputid = 0;
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("spProposalMaster", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objInsertCS.mode);
        cmd.Parameters.AddWithValue("@projTitle", objInsertCS.projectTitle);
        cmd.Parameters.AddWithValue("@ProjectUrl", objInsertCS.ProjectUrl);
        cmd.Parameters.AddWithValue("@projDesc", objInsertCS.projDesc);
        cmd.Parameters.AddWithValue("@ImageName",objInsertCS.ImageName);
        cmd.Parameters.AddWithValue("@ImageContentType", objInsertCS.contentType);
        cmd.Parameters.AddWithValue("@ImageData", objInsertCS.bytes);
      
        try
        {
            using (con)
            {
                outputid = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

            }
        }
        catch (Exception ex)
        { }
        return outputid;
    }


    public bool UpdateProposalCSProjects(ProposalsProjectsBLL objUpdateCS)
    {
        bool updated = false;
        SqlConnection con = new SqlConnection(_strConnection);
        SqlCommand cmd = new SqlCommand("spProposalMaster", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mode", objUpdateCS.mode);
        cmd.Parameters.AddWithValue("@ProposalCSDefaultID", objUpdateCS.proposalID);
        cmd.Parameters.AddWithValue("@projTitle", objUpdateCS.projectTitle);
        cmd.Parameters.AddWithValue("@ProjectUrl", objUpdateCS.ProjectUrl);
        cmd.Parameters.AddWithValue("@projDesc", objUpdateCS.projDesc);
        cmd.Parameters.AddWithValue("@ImageName", objUpdateCS.ImageName);
        cmd.Parameters.AddWithValue("@ImageContentType", objUpdateCS.contentType);
     
        try
        {
            using (con)
            {
                con.Open();
                int output = Convert.ToInt32(cmd.ExecuteScalar());
                if (output == 1)
                    updated = true;
            }
        }
        catch (Exception ex)
        { }
        return updated;
    }
}