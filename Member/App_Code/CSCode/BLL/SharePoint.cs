using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

public class SharePoint
{
    string SSServer = "http://10.0.0.71:3333/";
    public SharePoint()
    {
    }

    public string AddSite(string SiteName)
    {

        return "";
    }

    public string AddDirectory(string DirName)
    {

        return "";
    }

    public string AddUser(string User, string SiteName="", string DirName ="")
    {

        return "";
    }

    public string Read()
    {
        ClientContext clientContext = new ClientContext(SSServer);
        Web site = clientContext.Web;
        clientContext.Load(site);
        clientContext.ExecuteQuery();
        return site.Title;
    }
}