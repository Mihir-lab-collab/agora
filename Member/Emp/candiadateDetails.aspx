<%@ Page Language="VB" AutoEventWireup="false" CodeFile="candiadateDetails.aspx.vb" Inherits="emp_candiadateDetails" %>

<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>

<html xmlns="http://www.w3.org/1999/xhtml">

<script language="javascript" type="text/javascript">
    function goback()
    {
        window.history.back ;
    }
    function funDisplay()
    {
    var val=document.getElementById('ddlcity');
      if(val[val.selectedIndex].text  == "Other")
        {
        document.getElementById('divtxt').style.display='';  
           
        }
        else
        {
        document.getElementById('divtxt').style.display='none';    
        }
    
    }
function chkblank()
{  
    if ((document.getElementById('txtchangetitle').value == "")) 
    {
        alert('Enter Title....');
        return false;
    }
	    if ((document.getElementById('estComTime').value == "")) 
    {
        alert('Enter days....');
        return false;
    }
   if ((document.getElementById('txtchange').value == "")) 
    {
        alert('Change Request Date...');
        return false;
    }

 
	
}
	function numericCharacters(event)
		{
		/*********
		//IF IE
		**********/
		if(document.all)
		{
		if(event.keyCode>=48 && event.keyCode<=57)
		{
		return true;
		}
		else
		{
			
	
		return false;
		}
		}
		/************************************
		//OTHER BROWSER LIKE FIREFOX,NETSCAPE
		*************************************/
		if ((!document.all )&& (document.getElementById))
		{
		if(event.which>=48 && event.which<=57)
		{
		return true;
		}
		else if(event.which==0 || event.which==8)
		{
		return true;
		}
		else
		{
		
		return false;
		
		}
		}
		}
</script>

<head>
    <title>Candiate Details</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css"  type="text/css"/>
</head>
<body>
    <form id="Form1" runat="server" onsubmit="">
        <table cellspacing="1" cellpadding="4" align="center" border="2" bordercolor="#F1F4EC"
            width="100%">
            <tr>
                <td>
                    <table id="Table3" cellspacing="0" cellpadding="2" width="100%" border="0" height="1">
                        <tr>
                            <td>
                                <EMPHEADER:empHeader id="Empheader" runat="server">
                                </EMPHEADER:empHeader></td>
                        </tr>
                        <tr>
                            <td style="height: 65px">
                                <uc1:empMenuBar ID="EmpMenuBar" runat="server"></uc1:empMenuBar>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center" style="background: #C5D5AE">
                    <font face="Verdana" color="#a2921e"><b>Candiate Details</b></font></td>
            </tr>
            <tr>
                <td align="center">
                    <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1"
                        width="90%">
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">First Name </font><font color="red">
                                    *</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtFname" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Middle Name</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtMname" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Last Name</font></b><font color="red">
                                    *</font></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtLname" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Date of Birth</font></b><font color="red">
                                    *</font></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtdob" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28" onclick="popupCalender('txtdob');"> </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Present Address </font></b>
                            </td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtPresentAddress" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="27" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Tel. No. </font></b>
                            </td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtTelno" runat="server" Font-Size="Smaller" MaxLength="11" onkeypress=" return numericCharacters(event)"
                                    Font-Names="Verdana" Columns="28"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Mobile No.</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtmobileno" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    MaxLength="20" Columns="28" onkeypress=" return numericCharacters(event)"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">E-Mail </font></b><font color="red">
                                    *</font></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtemail" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Permanent Address</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtPermAddress" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="27" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Refrence(Contact no)</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtRelativeno" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28" onkeypress=" return numericCharacters(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Post Applied For</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <select size="1" id="empSkill" name="combo_skill" runat="server" font-size="Smaller"
                                    font-names="Verdana">
                                </select>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Working Experience </font></b>
                            </td>
                            <td nowrap="nowrap" width="25%">
                                <select runat="server" id="dropempExpyears" name="empExpyears">
                                    <option value="0" selected>yy</option>
                                    <option value="0">0</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                </select>
                                <select runat="server" id="dropempExpmonths" name="empExpmonths">
                                    <option value="0" selected>mm</option>
                                    <option value="0">0</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                                    <option value="4">4</option>
                                    <option value="5">5</option>
                                    <option value="6">6</option>
                                    <option value="7">7</option>
                                    <option value="8">8</option>
                                    <option value="9">9</option>
                                    <option value="10">10</option>
                                    <option value="11">11</option>
                                    <option value="12">12</option>
                                </select>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Current Salary (Gr.)</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtCSalary" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28" onkeypress=" return numericCharacters(event)"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Expected Salary (Gr.)</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtESalary" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28" onkeypress=" return numericCharacters(event)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Previous Employer</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtPemployer" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="28"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Reason For Change the Job</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:TextBox ID="txtreason" runat="server" Font-Size="Smaller" Font-Names="Verdana"
                                    Columns="27" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Educational Qualification</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:ListBox ID="lstempQual" runat="server" Rows="4" cols="40" Font-Size="Smaller"
                                    Font-Names="Verdana" Name="empQualification" SelectionMode="Multiple" EnableViewState="true">
                                </asp:ListBox>
                            </td>
                            <td bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">City</font></b></td>
                            <td>
                                <asp:DropDownList ID="ddlcity" runat="server" Font-Size="Smaller" Font-Names="Verdana">
                                </asp:DropDownList><br>
                                <br>
                                <div style="display: none" id="divtxt">
                                    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox></div>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Technical Skills</font></b></td>
                            <td nowrap="nowrap" width="25%">
                                <asp:ListBox ID="lstSkill" runat="server" Rows="4" cols="40" Font-Size="Smaller"
                                    Font-Names="Verdana" SelectionMode="Multiple" EnableViewState="true"></asp:ListBox>
                            </td>
                            <td bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2"></font></b>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Upload Resume</font></b></td>
                            <td nowrap="nowrap" colspan="3" align="left">
                                <input id="fleUploadResumeDetails" type="file" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="background: #C5D5AE">
                                <font face="Verdana" color="#a2921e"><b>Schedule Candiate</b></font></td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Candiate Status</font></b></td>
                            <td nowrap="nowrap" align="left">
                                <font face="Verdana" size="2">
                                    <select size="1" id="candStatus" runat="server">
                                    </select>
                                </font>
                            </td>
                            <td nowrap="nowrap" bgcolor="#edf2e6" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Date</font></b></td>
                            <td nowrap="nowrap" align="left">
                                <font face="Verdana" size="2">
                                    <asp:TextBox ID="txtschdate" runat="server" onclick="popupCalender('txtschdate');"
                                        Rows="3" Columns="40"></asp:TextBox></font>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" bgcolor="#edf2e6" colspan="1" align="left">
                                <b><font face="Verdana" color="#a2921e" size="2">Add Comment</font></b></td>
                            <td colspan="3" align="left">
                                <font face="Verdana" size="2">
                                    <asp:TextBox ID="txtAddComment" runat="server" TextMode="MultiLine" Rows="3" Columns="50"></asp:TextBox></font></td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap" align="center" width="75%" colspan="4" rowspan="2">
                                <asp:Button ID="btnSave" runat="server" Width="90px" BorderWidth="1px" BackColor="#EDF2E6"
                                    BorderStyle="Groove" BorderColor="#A2921E" Text="Save" OnClientClick="return validation();"></asp:Button>
                                <asp:Button ID="btnback" runat="server" Width="90px" BorderWidth="1px" BackColor="#EDF2E6"
                                    BorderStyle="Groove" BorderColor="#A2921E" Text="Back"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

<script language="javascript" type="text/javascript">
function validation()  
{
    if (document.getElementById('txtFname').value == '')
    {
        alert('Enter First name');
        return false;
    }
     if (document.getElementById('txtLname').value == '')
     {
        alert('Enter last Name');
        return false;
     }
    
      if (document.getElementById('txtemail').value == '')
     {
        alert('Enter Email');
        return false;
     }
     
    var id=document.getElementById("txtEmail");
	var at=id.value.indexOf('@');
	var lastat=id.value.lastIndexOf('@');
	var dot=id.value.indexOf('.');
	lastdot=id.value.lastIndexOf('.')
	
	if ( !( (0 < at) && (at < (lastdot-1)) && (lastdot < (id.value.length-1)) && (at == lastat) ) ) 
	{
		error = 1;
		alert("Email address is not formatted properly.");
		return false
	}
	//return false;
     
     
   
}


</script>

