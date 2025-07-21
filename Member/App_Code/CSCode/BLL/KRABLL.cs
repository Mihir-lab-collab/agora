using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KRABLL
/// </summary>
public class KRABLL
{
    public int KRAID { get; set; }
    public string KRANames { get; set; }
    public int ProfileId { get; set; }

    public KRABLL()
    {
    }

    public KRABLL(int KRAID, string KRANames)
    {
        this.KRAID = KRAID;
        this.KRANames = KRANames;
    }

    public static List<KRABLL> GetkAR(string mode, int KRAID)
    {
        KRADAL objKRADAL = new KRADAL();
        return objKRADAL.GetkAR(mode, KRAID);
    }

    public static int SaveKRA(string mode, int KRAID, string KRANames)
    {
        KRABLL objKRA = new KRABLL();
        objKRA.KRAID = KRAID;
        objKRA.KRANames = KRANames;
        KRADAL objKRADAL = new KRADAL();
        return objKRADAL.SaveKRA(mode, objKRA);
    }

    public static int DeleteKRAProfile(int ProfileId)
    {
        KRABLL curKRAProfile = new KRABLL();
        curKRAProfile.ProfileId = ProfileId;
        // curprojectMember.empid = empid;
        KRADAL objprojectMember = new KRADAL();
        return objprojectMember.DeleteKRAProfile(curKRAProfile);
    }

    public static int InsertUpdateKRAProfile(int ProfileId, int KRAID)
    {
        KRABLL curKRAProfile = new KRABLL();
        curKRAProfile.ProfileId = ProfileId;
        curKRAProfile.KRAID = KRAID;
        KRADAL objprojectMember = new KRADAL();
        return objprojectMember.InsertUpdateKRAProfile(curKRAProfile);
    }


    //public  CheckEmpAppraisal(int empid)
    //{
    //    AppraisalDAL objappraisalDAL = new AppraisalDAL();
    //    return objappraisalDAL.CheckEmpAppraisal(empid);
    //}
}