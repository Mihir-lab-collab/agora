using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MailInfo
/// </summary>
public class MailInfo
{
	public MailInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string ccName { get; set; }
    public string subject { get; set; }
    public string fileName { get; set; }
    public string description { get; set; }
    public string filePath { get; set; }

}