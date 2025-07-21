<%@ Page Language="VB" AutoEventWireup="false" CodeFile="timeSheetReport.aspx.vb"
    EnableViewState="false" Inherits="admin_timeSheetReport" %>

<%@ Register Src="~/controls/empMenuBar.ascx" TagName="empMenuBar" TagPrefix="uc1" %>
<%@ Register Src="~/controls/empHeader.ascx" TagName="empHeader" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Time Sheet Report</title>
    <link rel="stylesheet" href="../css/style.css" type="text/css" />
    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />

    <script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>

    <script type="text/javascript" language="javascript">
    function chkblank()
    {  
        if ((document.getElementById('txtDateFrom').value=="") || (document.getElementById('txtDateto').value==""))
        {
            alert('Enter Date....');
            return false;
        }
    }
    function divhide()
    {
        if (document.getElementById('divtable').style.display=='')
        {
            document.getElementById('imgminus').style.display='none';
            document.getElementById('imgplus').style.display='';
            document.getElementById('divtable').style.display='none';
        }
        
    }
    function divFununhide()
    {      
             if (document.getElementById('divtable').style.display=='none')
             {
                      document.getElementById('imgminus').style.display='';
                      document.getElementById('imgplus').style.display='none';
                      document.getElementById('divtable').style.display='';
             }
    }
    </script>

</head>
<body>
    <form id="Form1" runat="server">
    <table align="center" width="95%" border="1" cellpadding="2" cellspacing="0">
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="2" width="100%" border="0">
                    <tr>
                        <td>
                            <uc2:empHeader ID="EmpHeader1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:empMenuBar ID="EmpMenuBar1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="background: #C5D5AE" align="center">
                <font face="Verdana" size="2" color="#a2921e"><b>Time Sheet Report</b></font>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table3" cellspacing="0" cellpadding="2" width="100%" border="1" bordercolor="#FBF9EC"
                    style="border-collapse: collapse">
                    <tr>
                        <td bgcolor="#C5D5AE" nowrap="nowrap"="true" align="left" style="height: 23px">
                            <font style="color: #A2921E; font-family: Arial" size="2"><b>Project Name</b></font>
                        </td>
                        <td bgcolor="#edf2e6" align="left" valign="top">
                            <font face="Verdana" size="2">
                                <asp:Label ID="lblproject" runat="server" ForeColor="#000000"></asp:Label></font>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td bgcolor="#C5D5AE" nowrap="nowrap" align="left" valign="top">
                            <font style="color: #A2921E; font-family: Arial" size="2"><b>Project Member</b></font>
                        </td>
                        <td valign="top">
                            <asp:DataGrid runat="server" ID="dgrdProjMember" Font-Size="10pt" Font-Name="Verdana"
                                ItemStyle-HorizontalAlign="Left" BackColor="#edf2e6" Font-Names="Verdana" AutoGenerateColumns="False"
                                FooterStyle-HorizontalAlign="Right" AllowSorting="True" HeaderStyle-Font-Size="10pt"
                                ShowHeader="false " CellPadding="2" ShowFooter="false" Width="100%" AllowPaging="false">
                                <ItemStyle ForeColor="#000000" Wrap="false"></ItemStyle>
                                <Columns>
                                    <asp:BoundColumn DataField="empname"></asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="width: 80%">
            <td align="center" width="100%" style="border: 2">
                <table width="100%">
                    <tr align="center">
                        <td align="right" nowrap="nowrap">
                            <font face="Verdana" size="2" color="#a2921e"><b>Date From</b> </font>
                            <asp:TextBox ID="txtDateFrom" runat="server" onclick="popupCalender('txtDateFrom');"></asp:TextBox>
                        </td>
                        <td align="left" nowrap="nowrap">
                            <font face="Verdana" size="2" color="#a2921e"><b>&nbsp;Date To &nbsp;</b></font>
                            <asp:TextBox ID="txtDateTo" runat="server" onclick="popupCalender('txtDateTo');"></asp:TextBox>
                            <input type="submit" runat="server" value="Go" id="btnGO" onclick="return chkblank();" />
                        </td>
                        <td>
                            <div style="display: none">
                                <asp:TextBox ID="txthdnFrom" runat="server"></asp:TextBox></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:Label ID="lblCurrentdate" runat="server" Visible="false"></asp:Label><tr>
            <td width="100%">
                <table width="100%">
                    <tr>
                        <td width="70%" align="left">
                            &nbsp;&nbsp;&nbsp;
                            <img id="imgminus" src="../images/minus.gif" onclick="divhide()" alt="Hide" />
                            <img id="imgplus" src="../images/plus.gif" onclick="divFununhide()" alt="Unhide"
                                style="display: none" />
                            <font face="Verdana" size="2">
                                <asp:Label ID="lbldivhide" Font-Size="Medium" Font-Bold="true" runat="server"></asp:Label></font>
                        </td>
                        <td width="20% align="right">
                            <asp:Label Font-Size="Medium" Font-Bold="true" Font-Names="Verdana" ID="Label1"
                                runat="server" Text="Estimate Hours"></asp:Label>
                        </td>
                        <td width="20%" align="center">
                        
                            <asp:Label Font-Size="Medium" Font-Bold="true" Font-Names="Verdana" ID="lbltotalhours"
                                runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divtable" runat="server">
                    <asp:Table ID="tblReport" runat="server" Width="92%" align="center" Font-Names="Verdana"
                        class="manage2">
                    </asp:Table>
                </div>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript" language="javascript">
function call_Ajax(moduleId,module)
{
    var strimg = document.getElementById("plus" + moduleId).src;

    //  if (strimg == "/images/plus.gif")
    if(strimg.match("/images/plus.gif"))
    {
        document.getElementById("plus"+moduleId).src="../images/minus.gif";
    }
    else
    {
        document.getElementById("plus"+moduleId).src="../images/plus.gif";
    }
  
    var  str ;
    var Condiction ;
    Condiction =  document.getElementById("txthdnFrom").value;
    if (module == 1 )
    {
        str= document.getElementById("module_"+moduleId).innerHTML;
    }
    else
    {
        str =  document.getElementById("submodule_"+moduleId).innerHTML ;
    }
    if( str  == "" )
    {
        var xmlHttp = ajaxFunction() ;
	    xmlHttp.open("GET","ajaxFill.aspx?&no="+Math.random()+"&prjID=<%=Request.QueryString("id")%>&ID="+moduleId+"&module="+module+"&condition="+Condiction,true);
        xmlHttp.onreadystatechange=function()
        {
            if(xmlHttp.readyState==4)
            {
                var str =  xmlHttp.responseText;
                if (str != "") 
			    {		
		            if ( module == 1 )
				    {
				        document.getElementById("module_"+moduleId).innerHTML = str ;
				    } 
			        else
			        {
			            document.getElementById("submodule_"+moduleId).innerHTML = str ;
			        }
			    }
		    }
        }
        xmlHttp.send(null);  
    }
    else
    {
        if ( module == 1 )
            document.getElementById("module_"+moduleId).innerHTML = "" ;
        else
	        document.getElementById("submodule_"+moduleId).innerHTML = "" ;
    }
}
function ajaxFunction()
{  
    var xmlHttp;
    try
    {  
        xmlHttp=new XMLHttpRequest();
	}
    catch (e)
    {    // Internet Explorer
	    try
      	{   
		    xmlHttp=new ActiveXObject("Msxml2.XMLHTTP"); 
		}
    	catch (e)
      	{    
		    try
        	{ 
			    xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
		    }
     		catch (e)
        	{   
			    alert("Your browser does not support AJAX!");   
				return false;
			}
	 	}  
	}
    return xmlHttp;
}
    </script>

</body>
</html>
