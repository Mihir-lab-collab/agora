<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addKnowledge.aspx.vb" Inherits="emp_addKnowledge" ValidateRequest="false" %>
<%@Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title> Add Knowledge </title>
<script language="JavaScript" src="/includes/calender.js" type="text/javascript"></script>
</head>
<body>
<form id="Form1" method="post" runat="server">
    <table valign="top" id="Table3"  cellspacing="0" cellpadding="3" width="100%"  align="center" border="0">
        <tr valign="top">
            <td><EMPHEADER:empHeader id="Empheader" runat="server"></EMPHEADER:empHeader></td>
        </tr>
        <tr>
            <td>
                <uc1:empMenuBar id="EmpMenuBar" runat="server">
                </uc1:empMenuBar>
            </td>
         </tr>
         <tr>
                <td valign="top">
                    <table   border="0" align="center">
                        <tr>
                            <td align="center" bgcolor="#C5D5AE" colspan="3"><a name="Bugs">
                                <font face="Verdana" color="#a2921e"><b>Knowledge Base</b></font></a>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 949px">
                            <asp:Label ID="lblMsg" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica" runat="server"  Font-Bold="True" ForeColor="Red" Visible="False" >
                            </asp:Label></td>
                        </tr>      
                        <tr>
                            <td align="center"  >
                                <table  border="1" style="border-collapse: collapse;border-color:black" cellpadding="2" cellspacing="5" >
                                    <tr>
                                        <td align="left" width="138" bgcolor="#EDF2E6" >
                                            <font color="#A2921E" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica">
                                            <strong>Title*</strong></font>
                                        </td>
                                        <td width="10" ><strong>:</strong></td>
                                        <td align="left" width="649" >
                                            <asp:TextBox ID="txtTitle" runat="server"  Width="300px"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="valrTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" ErrorMessage="Please enter Title">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr  >
                                        <td align="right" width="138" bgcolor="#EDF2E6" >
                                            <p align="left"><font color="#A2921E" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica"><strong> Description*</strong></font></td>
                                        <td style="width: 10"><strong>:</strong></td>
                                        <td align="left" style="width: 649">
                                            <asp:TextBox ID="txtDescr" runat="server" TextMode="multiline" height="200px" Width="400px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="valrDesc" runat="server" ControlToValidate="txtDescr" Display="Dynamic" ErrorMessage="Please enter Description">*</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr id="viewFile" runat="server" >
                                        <td align="right" width="138" bgcolor="#EDF2E6"  ><p align="left"><font color="#A2921E" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica"><b>View File</b> </font>
                                        </td>
                                        <td style="width: 10"><strong>:</strong></td>
                                        <td align="left" style="width: 649"> 
                                            <asp:HyperLink ID="hlnkFile" runat="server"></asp:HyperLink>					
                                        </td>
                                    </tr>
                                    <tr id="addFile" runat="server">
                                        <td align="right" width="138" bgcolor="#EDF2E6"  >
                                            <p align="left"><font color="#A2921E" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica">
                                            <b>Add File</b> </font>
                                        </td>
                                        <td style="width: 10"><strong>:</strong></td>
                                        <td align="left" style="width: 649"> 
                                            <input id="fileAdd" type="file" runat="server"  size="20" onkeypress="return false;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="138" bgcolor="#EDF2E6"  >
                                            <p align="left"><font color="#A2921E" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica"><b>Comments</b> </font>
                                        </p>
                                        </td>    
                                        <td style="width: 10"><strong>:</strong></td>
                                        <td align="left" style="width: 649"> 
                                            <input id="txtempName" type="hidden" runat="server">
                                            <asp:TextBox ID="txtComment" runat="server" TextMode="multiline" height="100px" Width="400px" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" width="138" bgcolor="#EDF2E6"  >
                                            <p align="left"><font color="#A2921E" style="font-size: 10pt;font-family: Arial, Tahoma, Verdana, Helvetica">
                                                <b>Comment History</b></font>
                                            </p>
                                        </td>        
                                        <td style="width: 10"><strong>:</strong></td>
                                        <td align="left" style="width: 649"> 
                                            <input id="commHis" type="hidden" runat="server">
                                            <asp:TextBox ID="txtCommentHistory" runat="server" TextMode="multiline" height="100px" Width="400px" READONLY></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colSpan="3" align="left" style="width: 649; height: 45px">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3" width="786">
                                            <asp:Button ID="btnSubmit" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" runat="server"  Text="Submit" ToolTip="Click here to submit the Knowledge" onclick="btnSubmit_Click" />&nbsp;
                                            <asp:button id="btncancel" runat="server" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" causesValidation="false" onclick="btncancel_Click"   Text="Cancel"  />
                                            <asp:button id="btndelete" runat="server" style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE" causesValidation="false" onclick="btndelete_Click"   Text="Delete" visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>    
                </td>
        </tr>
    </table>
</form>
</body>
</html>
