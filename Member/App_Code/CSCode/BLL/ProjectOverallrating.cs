
using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;

/// <summary>
/// Summary description for ProjectOverallrating
/// </summary>
public class ProjectOverallrating
{
    private string p;

	
    public string Overallrating { get; set; }
    public int projId { get; set; }

    public ProjectOverallrating()
    {
    }


    public ProjectOverallrating(int projId, string Overallrating)
    {
        this.projId = projId;
        this.Overallrating = Overallrating;
    }

    public ProjectOverallrating(string Overallrating)
    {
        // TODO: Complete member initialization
        this.Overallrating = Overallrating;
    }

 
    public static ProjectOverallrating GetOverallratingByPRojId(int projId)
    {
        ProjectOverallratingDAL objBugsByProjId = new ProjectOverallratingDAL();
        return objBugsByProjId.GetOverallratingByPRojId(projId);
    }


}