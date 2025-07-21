using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TechicalSkills
/// </summary>
public class TechicalSkills
{
    public int techId { get; set; }
    public string skillDesc { get; set; }

	public TechicalSkills()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public TechicalSkills(int techId,string skillDesc )
    {
        this.techId = techId;
        this.skillDesc = skillDesc;
    }

    public static List<TechicalSkills> GetTechSkills()
    {
        return GetSecondarySkills();
    }

    public static List<TechicalSkills> GetSecondarySkills()
    {
        TechnicalSkillsDAL objTechSkill = new TechnicalSkillsDAL();
        return objTechSkill.GetSecondarySkills();
    }
}