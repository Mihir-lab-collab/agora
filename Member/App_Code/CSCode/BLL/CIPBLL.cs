using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CIPBLL
/// </summary>
public class CIPBLL
{
    public int KEID { get; set; }
    public int LocationId { get; set; }
    public string Location { get; set; }
    public string EventDate { get; set; }
    public string EventDateTime { get; set; }
    public string Description { get; set; }
    public string Time { get; set; }
    public string Mode { get; set; }
    public string EmailID { get; set; }

	public CIPBLL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static List<CIPBLL> GetEvents(string mode, int KEID,int locationID)
    {
        CIPDAL objCIPDAL = new CIPDAL();
        CIPBLL objCIPBLL = new CIPBLL();
        objCIPBLL.Mode = mode;
        objCIPBLL.KEID = KEID;
        objCIPBLL.LocationId = locationID;

        DataTable dt = new DataTable();
        dt = objCIPDAL.GetEvents(objCIPBLL);

        return objCIPBLL.BindEventList(dt,"ALL");
    }

    private List<CIPBLL> BindEventList(DataTable dt ,string type)
    {
        List<CIPBLL> lstCIPBLL = new List<CIPBLL>();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CIPBLL objCIPBLL = new CIPBLL();

                objCIPBLL.KEID = Convert.ToInt32(dt.Rows[i]["KEID"].ToString());
                objCIPBLL.LocationId = Convert.ToInt32(dt.Rows[i]["LocationId"].ToString());
                objCIPBLL.EventDate = dt.Rows[i]["EventDate"].ToString();
                objCIPBLL.EventDateTime = dt.Rows[i]["EventDateTime"].ToString();
                objCIPBLL.Description = dt.Rows[i]["EventDesc"].ToString();
                objCIPBLL.Time = dt.Rows[i]["EventTime"].ToString();
                if(type == "MAILID")
                    objCIPBLL.EmailID = dt.Rows[i]["EmailID"].ToString();

                lstCIPBLL.Add(objCIPBLL);
            }
        }
        return lstCIPBLL;
    }
    public static int Save(string mode, int KEID, int locationID, string eventDate, string description,string time)
    {
        CIPDAL objCIPDAL = new CIPDAL();
        CIPBLL objCIPBLL = new CIPBLL();
        objCIPBLL.Mode = mode;
        objCIPBLL.KEID = KEID;
        objCIPBLL.LocationId = locationID;
        objCIPBLL.EventDate = eventDate;
        objCIPBLL.Description = description;
        objCIPBLL.Time = time;

        return objCIPDAL.Save(objCIPBLL);
    }
    public static void Delete(string mode, int KEID)
    {
        CIPDAL objCIPDAL = new CIPDAL();
        CIPBLL objCIPBLL = new CIPBLL();
        objCIPBLL.Mode = mode;
        objCIPBLL.KEID = KEID;

        objCIPDAL.Delete(objCIPBLL);
    }

    public static List<CIPBLL> GetMailInfo(string mode)
    {
        CIPDAL objCIPDAL = new CIPDAL();
        CIPBLL objCIPBLL = new CIPBLL();

        DataTable dt = new DataTable();
        dt = objCIPDAL.GetMailInfo(mode);
        
        return objCIPBLL.BindEventList(dt,"MAILID");  
    }
}