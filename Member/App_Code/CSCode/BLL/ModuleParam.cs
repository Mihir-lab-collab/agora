using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Customer.DAL;


/// <summary>
/// Summary description for ModuleParam
/// </summary>
public class ModuleParam
{
	public ModuleParam()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public int ModuleID { get; set; }
    public int TypeID { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string Menu { get; set; }
    public string EntryPage { get; set; }
    public string Parameter { get; set; }
    public bool IsMenuVisible { get; set; }
    public bool IsGenric { get; set; }
    public string mode { get; set; }

}