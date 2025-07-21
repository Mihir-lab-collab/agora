using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ProjectTeam
/// </summary>
public class ProjectTeam
{
    public int projId { get; set; }
    public int empId { get; set; }
    public int MemberId { get; set; }
    public string projName { get; set; }
    public string empName { get; set; }
    public int Discount { get; set; }
    public DateTime ModifiedOn { get; set; }
    public int Isactive { get; set; }
    public DateTime InsertedOn { get; set; }
    public string EmpAddress { get; set; }
    public string Mode { get; set; }

	public ProjectTeam()
	{
		
	}
    public ProjectTeam(string _projName,int _projId,int _empId, string _empName, int _Discount, DateTime _Modifieddate,int _memberid)//, int _isactive)
    {
        this.empName = _empName;
        this.projName = _projName;
        this.ModifiedOn = _Modifieddate;
       // this.Isactive = _isactive;
        this.Discount = _Discount;
        this.projId = _projId;
        this.empId = _empId;
        this.MemberId = _memberid;
    }
    public ProjectTeam(int _empId, string _empName)//, string _empAddress,DateTime _insertedOn, DateTime _Modifieddate)//, int _isactive)
    {
        this.empName = _empName;
        this.empId = _empId;
        //this.ModifiedOn = _Modifieddate;
        //this.InsertedOn = _insertedOn;
        //this.EmpAddress = _empAddress;
    }
    public static List<ProjectTeam> Projects()
    {
        ProjectTeamDAL objProjectTeamDAL = new ProjectTeamDAL();
        return objProjectTeamDAL.Projects();
    }

    public static List<ProjectTeam> GetEmployees()
    {
        ProjectTeamDAL objProjectTeamDAL = new ProjectTeamDAL();
        return objProjectTeamDAL.GetEmployees();
    }

    public static string UpdateProjectTeam(int _projId,int _empId,int _discount,int _Isactive,DateTime _modifieddate,string _mode,int Id)
    {
        ProjectTeam objteam = new ProjectTeam();
        objteam.projId = _projId;
        objteam.empId = _empId;
        objteam.Discount = _discount;
        objteam.Isactive = _Isactive;
        objteam.ModifiedOn = _modifieddate;
        objteam.Mode = _mode;
        objteam.MemberId = Id;

        ProjectTeamDAL objdal = new ProjectTeamDAL();
        return objdal.UpdateProjectTeam(objteam);
    }

    public static List<ProjectTeam> Projects(string mode, int _projId)
    {
        ProjectTeamDAL objdal = new ProjectTeamDAL();
        return objdal.GetProjectListById(mode, _projId);
    }
}