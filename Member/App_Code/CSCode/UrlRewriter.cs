using System;
using System.Web;
using System.Web.UI;
using System.Web.Routing;
using System.Web.Compilation;
using System.Web.SessionState;

/// <summary>
/// Summary description for UrlRewriter
/// </summary>
/// 
namespace Proposal.UrlRewriting
{
    public class UrlRewriter : IRouteHandler
    {
        public UrlRewriter()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            using (RouteTable.Routes.GetReadLock())
            {
                String virtualPath = String.Empty;
                String rewritePath = String.Empty;
                String queryString = String.Empty;
                Boolean isValidProject = false;
                string projectName = requestContext.RouteData.Values["projectName"] as string;
                string pageName = requestContext.RouteData.Values["pageName"] as string;
                //string entryPage = requestContext.RouteData.Values["entry"] as string;    //commented for Login.aspx
                string LoginPage = requestContext.RouteData.Values["entry"] as string;      //added later for Login.aspx
                //string ManageProjectsPage = requestContext.RouteData.Values["ManageProjects"] as string;

                if (!String.IsNullOrEmpty(projectName))
                {
                    Constants.projectNameSessionToken = projectName;
                }


                if (LoginPage != null)
                {
                    if (!LoginPage.ToLower().Contains("aspx"))
                    {
                        if (LoginPage.ToUpper() == "ADMIN")
                        {
                            virtualPath = "~/Admin/Default.aspx";
                            rewritePath = "~/Admin/Default.aspx";
                        }
                        else if (LoginPage.ToUpper() == "CUST")
                        {
                            virtualPath = "~/Cust/default.aspx";
                            rewritePath = "~/Cust/default.aspx";
                        }
                        else if (LoginPage.ToUpper() == "EMP")
                        {
                            virtualPath = "~/Emp/Default.aspx";
                            rewritePath = "~/Emp/Default.aspx";
                        }
                        else
                        {
                            virtualPath = "~/" + LoginPage;
                            rewritePath = "~/" + LoginPage;
                        }
                        isValidProject = true;
                    }
                    else
                    {
                        //if (LoginPage == "default.aspx" || LoginPage == "index.aspx" || LoginPage == "Default.htm" || LoginPage == "Default.asp" || LoginPage == "index.htm" || LoginPage == "index.html" || LoginPage == "iisstart.htm")
                        if (LoginPage.ToLower() != "entry.aspx")
                        {
                            isValidProject = true;
                            virtualPath = "~/" + LoginPage;
                            rewritePath = "~/" + LoginPage;
                        }
                        else
                        {
                            isValidProject = true;
                            virtualPath = "~/" + Constants.LoginPageAspxName;
                            rewritePath = "~/" + LoginPage;
                        }
                        //if (LoginPage == "default.aspx")
                        //{
                        //    isValidProject = true;
                        //    virtualPath = "~/default.aspx";
                        //    rewritePath = "~/default.aspx";
                        //}
                        //else
                        //{
                        //    isValidProject = true;
                        //    virtualPath = "~/" + Constants.LoginPageAspxName;
                        //    rewritePath = "~/" + LoginPage;
                        //}
                    }
                }

                else if (pageName != null)
                {
                    isValidProject = true;
                    switch (pageName.ToLower())
                    {
                        case Constants.briefPageName:
                            //virtualPath = "~/" + Constants.briefPageAspxName;
                            virtualPath = "~/Proposals/" + Constants.briefPageAspxName;
                            break;
                        case Constants.aboutPageName:
                            virtualPath = "~/Proposals/" + Constants.aboutPageAspxName;
                            break;
                        case Constants.WarrantyPageName:
                            virtualPath = "~/Proposals/" + Constants.WarrantyPageAspxName;
                            break;
                        case Constants.UiInterfacePageName:
                            virtualPath = "~/Proposals/" + Constants.UiInterfacePageAspxName;
                            break;
                        case Constants.ProjectLifeCyclePageName:
                            virtualPath = "~/Proposals/" + Constants.ProjectLifeCyclePageAspxName;
                            break;
                        case Constants.ProjectEstimatePageName:
                            virtualPath = "~/Proposals/" + Constants.ProjectEstimatePageAspxName;
                            break;
                        case Constants.ManageProjectsPageName:                  //added later after Login page
                            virtualPath = "~/" + Constants.ManageProjectsPageAspxName;
                            break;
                        default:
                            virtualPath = "../../" + projectName + "/" + pageName;
                            break;
                    }
                    if (pageName.ToLower() != Constants.ManageProjectsPageName)
                    {
                        rewritePath = "~/~/" + projectName + "/" + pageName;
                    }
                    else if (pageName.ToLower() == Constants.ManageProjectsPageName)
                    {
                        rewritePath = "~/~/" + pageName;
                    }
                }
                Page p = new Page();
                if (!String.IsNullOrEmpty(virtualPath))
                {

                    if (isValidProject)
                    {
                        try
                        {
                            p = (Page)BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(Page));
                            HttpContext.Current.RewritePath(rewritePath);
                        }
                        catch (Exception ex)
                        {
                            p = (Page)BuildManager.CreateInstanceFromVirtualPath("~/NotFound.aspx", typeof(Page));
                        }
                    }
                    else
                    {
                        p = (Page)BuildManager.CreateInstanceFromVirtualPath("~/invalidProject.aspx", typeof(Page));
                    }
                }
                return p;
            }
        }

    }
}