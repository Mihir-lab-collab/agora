using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

public class ProjectsReviewDAL
{
    public ProjectsReviewDAL()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string _strConnection = WebConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    public void SaveProjectMeeting(string mode, PeojectsReviewBLL objProjectReview)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("[sp_ProjectReviewMeeting]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MODE", mode);
        cmd.Parameters.AddWithValue("@MeetingId", objProjectReview.MeetingId);
        cmd.Parameters.AddWithValue("@MeetingDate", objProjectReview.MeetingDate);
        cmd.Parameters.AddWithValue("@CalledBy", objProjectReview.CalledById);
        cmd.Parameters.AddWithValue("@Attendees", objProjectReview.Attendees);
        cmd.Parameters.AddWithValue("@MeetingType", objProjectReview.MeetingType);
        cmd.Parameters.AddWithValue("@AgendaTopic", objProjectReview.AgendaTopic);
        cmd.Parameters.AddWithValue("@Facilitator", objProjectReview.FacilitatorId);
        cmd.Parameters.AddWithValue("@TimeAlloted", objProjectReview.TimeAlloted);
        cmd.Parameters.AddWithValue("@InsertedBy", objProjectReview.InsertedBy);

        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                //outputid = Convert.ToInt32(cmd.ExecuteScalar());

                string BCC_EMail = string.Empty; int MeetingId = 0;
                SqlDataReader ServiceReader = cmd.ExecuteReader();
                while (ServiceReader.Read())
                {
                    MeetingId = Convert.ToInt32(ServiceReader.GetValue(0));
                    BCC_EMail = ServiceReader.GetValue(1).ToString();
                }
                Send_MeetingInvitationMail(MeetingId, BCC_EMail);
                con.Close();
            }
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }
    }

    public List<PeojectsReviewBLL> Get_ProjectReviewMeetingList(string Mode, int MeetingId)
    {
        List<PeojectsReviewBLL> curproject = new List<PeojectsReviewBLL>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
        SqlCommand cmd = new SqlCommand("sp_ProjectReviewMeeting", con);
        cmd.Parameters.AddWithValue("@MODE", Mode);
        cmd.Parameters.AddWithValue("@MeetingId", MeetingId);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlDataReader reader = null;
        using (con)
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                curproject.Add(new PeojectsReviewBLL
                    (
                        Convert.ToInt32(reader["MeetingId"]),
                        Convert.ToDateTime(reader["MeetingDate"]),
                        Convert.ToInt32(reader["CalledById"]),
                        reader["CalledByName"].ToString(),
                        reader["Attendees"].ToString(),
                        reader["AttendeesId"].ToString(),
                        reader["MeetingType"].ToString(),
                        reader["AgendaTopic"].ToString(),
                        Convert.ToInt32(reader["FacilitatorId"]),
                        reader["FacilitatorName"].ToString(),
                        reader["TimeAlloted"].ToString(),
                        Convert.ToDateTime(reader["InsertedOn"]),
                        Convert.ToInt32(reader["InsertedBy"]),
                        Convert.ToInt32(reader["MeetingStatus"])
                    ));
            }
        }
        return curproject;
    }

    public void Cancel_Meeting(string mode, int MeetingId)
    {
        SqlConnection con = new SqlConnection(_strConnection);
        con.Open();
        SqlCommand cmd = new SqlCommand("[sp_ProjectReviewMeeting]", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@MODE", mode);
        cmd.Parameters.AddWithValue("@MeetingId", MeetingId);
        try
        {
            using (con)
            {
                cmd.ExecuteNonQuery();
                //outputid = Convert.ToInt32(cmd.ExecuteScalar());

                string BCC_EMail = string.Empty;
                SqlDataReader ServiceReader = cmd.ExecuteReader();
                while (ServiceReader.Read())
                {
                    BCC_EMail = ServiceReader.GetValue(0).ToString();
                }
                Send_MeetingInvitationMail(MeetingId, BCC_EMail);
                con.Close();
            }
        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }
    }
    public UserMaster Send_MeetingInvitationMail(int MeetingId, string BCC_EMail)
    {
        UserMaster objUserMaster = new UserMaster();
        string Message = string.Empty;
        try
        {
            if (MeetingId > 0)
            {
                List<ConfigBLL> lstConfig = ConfigBLL.GetConfigDetails("GetConfig", 46);
                string EmailBody = lstConfig[0].value.ToString();
                //EmailBody = EmailBody.Replace("<%UserFName%>", objUserMaster.Name);
                string strBody, strSubject, result;
                strBody = EmailBody;
                strSubject = "Agora: Internal Team Meeting Invitation";
                //result = CSCode.Global.SendMail(strBody, strSubject, BCC_EMail, "", true, "", "");
                objUserMaster.Message = "Meeting Invitation Sent.";
            }
            else
            {
                Message = "Invalid Meeting.";
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
        return objUserMaster;
    }
}