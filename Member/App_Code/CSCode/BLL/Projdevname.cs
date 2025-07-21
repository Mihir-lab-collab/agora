using Customer.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.BLL;
/// <summary>
/// Summary description for Projdevname
/// </summary>
public class Projdevname
{

    public string codedevteam;
    public string codedevname;
	public Projdevname()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public Projdevname(string codedevteam, string codedevname)
    {
        this.codedevteam = codedevteam;
        this.codedevname = codedevname;
    }

    public Projdevname(string codedevname)
    {
        // TODO: Complete member initialization
        this.codedevname = codedevname;
    }


    public static Projdevname GetProjdevname(string codedevteam)
    {
        ProjdevnameDAL objProjEmpname = new ProjdevnameDAL();
        return objProjEmpname.GetProjempname(codedevteam);
    }
}