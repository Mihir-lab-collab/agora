using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;

/// <summary>
/// Summary description for ProjEmpname
/// </summary>
public class ProjEmpname
{

    public string coderevteam;
    public string coderevname;
	public ProjEmpname()
	{

	}
    public ProjEmpname(string coderevteam, string coderevname)
    {
        this.coderevteam = coderevteam;
        this.coderevname = coderevname;
    }

    public ProjEmpname(string coderevname)
    {
        // TODO: Complete member initialization
        this.coderevname = coderevname;
    }


    public static ProjEmpname GetProjempname(string coderevteam)
    {
        ProjEmpnameDAL objProjEmpname = new ProjEmpnameDAL();
        return objProjEmpname.GetProjempname(coderevteam);
    }

}