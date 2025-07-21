using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

public partial class Process : System.Web.UI.Page
{

    string strEmailID;
    string strattDate;
    SqlConnection strConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
		string Mode = Request["m"];

		if(Mode == "att")
		{
			string Respo = AttProcess(Request["d"], Request["i"]);
			if (Respo != "")
				Respo = "ERROR: " + Respo;
			Response.Write(Respo);
		}
		else if(Mode == "attsum")
		{
			string Respo = AttSumProcess(Request["d"]);
			if (Respo != "")
				Respo = "ERROR: " + Respo;
			Response.Write(Respo);
		}
		else
		{
			Response.Write("ERROR: Invalid call.");
		}
	}

	private string AttSumProcess(string StrDays)
	{	
		string strResponse = "";
		string SQLStr = "";
		
		int IntDays = 1;
		
		if(int.TryParse(StrDays, out IntDays) == false)
		{
			IntDays = 1;
		}
		
		try
        {
			strConn.Open();
			string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (string.IsNullOrEmpty(ip))
			{
				ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
			}

            SQLStr = "SELECT EmpID, CONVERT(Date, PunchTime, 102) AS AttDate, Min(PunchTime) As AttInTime,  Max(PunchTime) As AttOutTime" +
                " FROM EmpAttLog WHERE CONVERT(Date, PunchTime, 102) > CONVERT(Date, DateAdd(Day, -" + IntDays + ", getDate()), 102) " +
                " GROUP BY EmpID, CONVERT(Date, PunchTime, 102)";


            SqlDataAdapter adapter = new SqlDataAdapter(SQLStr, strConn);
			DataSet RecordSet = new DataSet();
			adapter.Fill(RecordSet, "Records");
			foreach (DataRow rdr in RecordSet.Tables["Records"].Rows)
            {
				SQLStr = "INSERT INTO EmpAtt(empID,attDate,attStatus, attInTime, attOutTime, attIP, adminID) " +
				"VALUES(" + rdr["Empid"]  + ",'" + rdr["AttDate"]  + "','p','" + rdr["AttInTime"]  + "','" + rdr["AttOutTime"]  + "','" + ip + "',1000)";

				SqlCommand cmd = new SqlCommand(SQLStr, strConn);
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					string ErrMsg = e.Message;
					if(ErrMsg.IndexOf("PRIMARY KEY") == -1)
					{
						strResponse = strResponse + "<BR>" + e.Message;
					}
					else
					{
						SQLStr = "UPDATE EmpAtt SET attInTime='" + rdr["AttInTime"] + "', attOutTime='" + rdr["AttOutTime"] + "' WHERE EmpID=" + rdr["Empid"]  + " AND AttDate='" + rdr["AttDate"]  + "'";
						cmd = new SqlCommand(SQLStr, strConn);
						cmd.ExecuteNonQuery();
					}
				}
				//Response.Write (SQLStr + "<BR>");
			}
		}
		catch (Exception ex)
		{
            strResponse = strResponse + "<BR>" + ex.Message;
		}
        return strResponse;
    }
	
	private string AttProcess(string StrLog, string IP)
	{
       
		string strResponse = "";
		XmlDocument xmlDoc = new XmlDocument();
      
        xmlDoc.LoadXml(StrLog);
		string EID, EDate, EType;
		string SQLStr = "";
		try
        {
		
			strConn.Open();
			foreach (XmlNode node in xmlDoc.SelectNodes("DocumentElement/myTable") )
			{
				EID = node.SelectSingleNode("EID").InnerText;
				EDate = node.SelectSingleNode("EDate").InnerText;
				EType = node.SelectSingleNode("EType").InnerText;
				
				SQLStr = " INSERT INTO EmpAttLog (EmpID, PunchTime, IP, Mode) VALUES('" + EID + "','" + EDate + "','" + IP + "','" + EType + "');"; 
				SqlCommand cmd = new SqlCommand(SQLStr, strConn);
				try
				{
					cmd.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					string ErrMsg = e.Message;
					if(ErrMsg.IndexOf("PRIMARY KEY") == -1 && ErrMsg.IndexOf("FOREIGN KEY") == -1 )
					{
						strResponse = strResponse + "<BR>" + e.Message;
					}		
				}	
/*
				SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Add_Att";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empid", EID);
                cmd.Parameters.AddWithValue("@attdate", EDate);
                cmd.Parameters.AddWithValue("@ip", EType);
                strConn.Open();
                cmd.Connection = strConn;
                cmd.ExecuteNonQuery();
                strConn.Close();
*/				
			}
		}
		catch (Exception ex)
		{
            strResponse = strResponse + "<BR>" + ex.Message;
		}
		
        return strResponse;
    }
}