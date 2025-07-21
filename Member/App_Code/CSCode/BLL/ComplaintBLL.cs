using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class ComplaintBLL
{
    public int compId { get; set; }
    public string projName { get; set; }
    public string compDate { get; set; }
    public string compTitle { get; set; }
    public string compDesc { get; set; }
    public string compFeedback { get; set; }
    public string compResolved { get; set; }
    public int compProjctId { get; set; }
    public string compCategory { get; set; }
    public string custName { get; set; }
    public string custAddress { get; set; }   
    public string custEmail { get; set; }   
    public string custRegDate { get; set; }    
    public string custCompany{get;set;}
    public string msg { get; set; }
    public int projId { get; set; }
    public int ID { get; set; }
    
    public ComplaintBLL()
	{    //,string custName,string custAddress,string custEmail,string custCompany,string custRegDate
	}
    public ComplaintBLL(string compDate, int compId, string projName, string compTitle, string compResolved, string compCategory, string custName, string custRegDate, string custEmail, string custAddress, string custCompany, int projId, string compDesc, int ID, string compFeedback)
    {
        this.compDate = compDate;
        this.compId = compId;
        this.projName = projName;
        this.compTitle = compTitle;
        this.compResolved = compResolved;        
        this.compCategory = compCategory;
        this.custName = custName;
        this.custRegDate = custRegDate;        
        this.custEmail = custEmail;
        this.custCompany = custCompany;
        this.custAddress = custAddress;
        this.projId = projId;
        this.compDesc = compDesc;     
        this.ID=ID;
        this.compFeedback = compFeedback;
    }  
   
    public static List<ComplaintBLL> getall(string mode,int projId)
    {
        ComplaintDAL objtype = new ComplaintDAL();
        return objtype.GetAllComplaints(mode,projId);
    }  



    public static void deletComplaint(string mode, int compId)
    {
        ComplaintBLL obj = new ComplaintBLL();
        obj.compId = compId;
        ComplaintDAL obj1 = new ComplaintDAL();
        obj1.DeleteComplaint(mode, compId);
    }
    public static void saveComplaint(string mode, int compId,  string compResolved, string compFeedback)
    {
      
        ComplaintBLL obj = new ComplaintBLL();
        obj.compId = compId;       
        obj.compResolved = compResolved;       
        obj.compFeedback = compFeedback;
       
        ComplaintDAL obj1 = new ComplaintDAL();
        obj1.SaveComplaint(mode, obj);

        
    }
    public ComplaintBLL(int ID, string compCategory)
    {
        this.ID = ID;
        this.compCategory =compCategory;
    }

    public static List<ComplaintBLL> getCat(string mode)
    {
        ComplaintDAL objtype = new ComplaintDAL();
        return objtype.getCategoryDetail(mode);
    }
   
}