using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class ProjectModule
{
    public int ID { get; set; }
    public int ParentID { get; set; }
    public int ProjID { get; set; }
    public int TypeID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Estimate { get; set; }
    //To Be deleted
    public int ModuleID { get; set; }

    public ProjectModule()
    {
            
    }
    public ProjectModule(int _ID, int _ParentID, int _ProjID, int _TypeID, string _Name, string _Description, int _Estimate)
    {
        this.ID = _ID;
        this.ParentID = _ParentID;
        this.ProjID = _ProjID;
        this.TypeID = _TypeID;
        this.Name = _Name;
        this.Description = _Description;
        this.Estimate = _Estimate;
    }

    public ProjectModule(int _ID, string _Name)
    {
        this.ID = _ID;
        this.Name = _Name;
        this.ModuleID = _ID;
    }

    public static List<ProjectModule> GetModules(int ProjID)
    {
        ProjectModuleDAL objProjectModule = new ProjectModuleDAL();
        return objProjectModule.GetModules(ProjID);
    }

    //To be deleted
    public static void DeleteProjectModuleByprojid(int ProjID)
    {
    }
    public static void InsertProjectModule(int ProjID, int ModuleID)
    {
    }
    public static List<ProjectModule> GetProjectModulesByProjId(int ProjID)
    {
        ProjectModuleDAL objProjectModule = new ProjectModuleDAL();
        return objProjectModule.GetProjectModulesByProjId(ProjID);
    }
}
