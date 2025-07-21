using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;

/// <summary>
/// Summary description for EmployeeCVBLL
/// </summary>
public class EmployeeCVBLL
{
    public EmployeeCVBLL()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public EmployeeCVBLL(int _empid, string _empName, string _SkillDesc, string _ResumePath, string _LastUploadedDate, string _LastUploadedBy, int _empExperience, string _empJoiningDate, string PrimarySkill, string _empAddress, string _projectsWorkingOn, string _skillMatrixs)
    {
        this.empid = _empid;
        this.empName = _empName;
        this.SkillDesc = _SkillDesc;
        this.ResumePath = _ResumePath;
        this.LastUploadedDate = _LastUploadedDate;
        this.LastUploadedBy = _LastUploadedBy;
        this.empExperince = _empExperience;
        this.empJoiningDate = _empJoiningDate;
        this.PrimarySkill = PrimarySkill;
        this.empAddress = _empAddress;
        this.projectsWorkingOn = _projectsWorkingOn;
        this.skillMatrixs = _skillMatrixs;
    }

    public string empName { get; set; }
    public string SkillDesc { get; set; }
    public int empid { get; set; }
    public string ResumePath { get; set; }

    public string LastUploadedDate { get; set; }
    public string LastUploadedBy { get; set; }

    public int empExperince { get; set; }

    public string empJoiningDate { get; set; }
    public string PrimarySkill { get; set; }

    public string empAddress { get; set; }
    public string projectsWorkingOn { get; set; }
    public string skillMatrixs { get; set; }
    public static List<EmployeeCVBLL> GetEmployeeData()
    {
        EmployeeCVDAL objCvUploadL = new EmployeeCVDAL();
        return objCvUploadL.getEmployeeData();
    }

    public static List<EmployeeCVBLL> UpdateEmpData(string ResumePath, int EmpId, int LastUploadedBy)
    {
        EmployeeCVDAL objCvUploadL = new EmployeeCVDAL();
        return objCvUploadL.UpdateEmpData(ResumePath, EmpId, LastUploadedBy);
    }


}