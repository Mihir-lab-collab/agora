using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TechnologykeywordBLL
/// </summary>
public class TechnologykeywordBLL
{

    public int techId;
    public string techName;
    public string subTechName;

    public TechnologykeywordBLL()
    {
    }

    public TechnologykeywordBLL(int techId, string techName, string subTechName)
    {
        this.techId = techId;
        this.techName = techName;
        this.subTechName = subTechName;
    }

    public static List<TechnologykeywordBLL> getall(string mode,string techname)
    {
        TechnologykeywordDAL _techdal = new TechnologykeywordDAL();
        return _techdal.GetAllTechnologyKeyword(mode,techname);
    }


    public void AddNewTechnology(int TechID, string techName)
    {
        TechnologykeywordBLL curtech = new TechnologykeywordBLL();
        curtech.techId = TechID;
        curtech.techName = techName;
        //curtech.subTechName = subTechName;
        TechnologykeywordDAL objtech = new TechnologykeywordDAL();
        objtech.AddNewTechnology(curtech);     
    }

    public void UpdateTechnology(int TechID, string techName, string subTechName)
    {
        TechnologykeywordBLL curtech = new TechnologykeywordBLL();
        curtech.techId = TechID;
        curtech.techName = techName;
        curtech.subTechName = subTechName;
        TechnologykeywordDAL objtech = new TechnologykeywordDAL();
        objtech.UpdateTechnology(curtech);     
    }

    public void DeleteTechnology(int TechID)
    {
        TechnologykeywordBLL curtech = new TechnologykeywordBLL();
        curtech.techId = TechID;
        TechnologykeywordDAL objtech = new TechnologykeywordDAL();
        objtech.DeleteTechnology(curtech);     
    }


}