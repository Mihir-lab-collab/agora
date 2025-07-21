<%@ Page Language="C#" AutoEventWireup="true" CodeFile="traineeRegistration.aspx.cs"
    Inherits="Admin_traineeRegistration" %>

<%@ OutputCache Duration="1" Location="None" NoStore="true" VaryByParam="none" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration Form </title>
    <link rel="stylesheet" href="Includes/calendar.css" type="text/css" />

    <script type="text/javascript" language="javascript" src="Includes/dhtmlgoodies_calendar.js"></script>

    <script type="text/javascript" language="javascript" src="Includes/DateValidation.js"></script>

    </script>

    <script language="javascript" type="text/javascript">
 function checkOther(objID)
 {
    if(document.getElementById(objID) !=null)
    {
         var drpEducationType=document.getElementById(objID); 					
         if(drpEducationType.options[drpEducationType.selectedIndex].text == "Other") 
         {
           document.getElementById("txtOtherEducation").value='';
           document.getElementById("trOther").style.display='';
           
         }
         else
         {
          document.getElementById("trOther").style.display='none';
         }   
    }
 } 
 
 function ClearAllControls()
 {
   document.getElementById("txtFirstName").value="";
   document.getElementById("txtMiddleName").value="";
   document.getElementById("txtLastName").value="";
   document.getElementById("txtDOB").value="";
   document.getElementById("txtAddress").value="";
   document.getElementById("txtCity").value="";
   document.getElementById("txtEmailID").value="";
   document.getElementById("txtPhoneNo").value="";
   document.getElementById("txtMobNo").value="";
   document.getElementById("txtOtherEducation").value="";
   document.getElementById("txtInterstedBcoz").value="";
  }
        
    </script>

    <style type="text/css">
        .text
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: normal;
            color: #141313;
        }
        .textbox
        {
            border: 1px #CCCCCC solid;
            font-size: 12px;
            font-family: Helvetica, Arial, sans-serif;
            color: #777272;
            background-color: #FFFFFF;
        }
        .error
        {
            font-size: 12px;
            font-family: Helvetica, Arial, sans-serif;
            margin-left: 5px;
            color: #eb2929;
        }
        html, body, #wrapper
        {
            min-height: 100%; /*Sets the min height to the
                       height of the viewport.*/
            width: 100%;
            height: 100%; /*Effectively, this is min height
                   for IE5+/Win, since IE wrongly expands
                   an element to enclose its content.
                   This mis-behavior screws up modern
                   browsers*/
        }
        html > body, html > body #wrapper
        {
            height: auto; /*this undoes the IE hack, hiding it
                   from IE using the child selector*/
        }
        body
        {
            margin: 0 auto;
        }
        #wrapper
        {
            position: absolute;
            top: 0;
            left: 0;
            padding-bottom: 70px;
        }
        #footer
        {
            position: absolute;
            bottom: 0;
            height: 60px;
        }
        .style3
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            font-weight: normal;
            color: #141313;
            width: 246px;
        }
        .heading
        {
            color: #333333;
            font-weight: bold;
            height: 18px;
        }
    </style>
</head>
<body onload="javascript:ClearAllControls();">
    <div id="wrapper">
        <form id="form1" runat="server">
        <table cellspacing="0" cellpadding="0" width="70%" align="center" border="0">
            <tr>
                <td>
                    <a href="../emp/empHome.aspx" border="0">
                        <img src="../images/dynamic_logo1.gif" border="0"></a>
                </td>
                <td colspan="2">
                    <table cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
                        <tr>
                            <td width="100%" align="center" colspan="2">
                                <font size="4" face="Verdana"><b>
                                    <asp:Label ID="compName" runat="server" Text="Dynamic Web Technologies Pvt Ltd"></asp:Label>
                                </b></font>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 100%;">
                                <font size="2" face="Verdana"><b>Trainee Registration </b></font>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="20%">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table cellpadding="4" cellspacing="0" width="70%" border="0" align="center" >
            <tr>
                <td colspan="5" align="center" class="heading">
                    Registration Form
                </td>
            </tr>
            <tr>
                <td colspan="5" align="center">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" class="style3">
                    &nbsp;
                </td>
                <td align="left" class="text">
                    First Name
                </td>
                <td align="left" class="text">
                    Middle Name
                </td>
                <td align="left" class="text">
                    Last Name
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Name :
                </td>
                <td>
                    <asp:TextBox ID="txtFirstName" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvalFirstName" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtFirstName">
                    </asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="txtMiddleName" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtLastName" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvalLastName" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtLastName">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Date Of Birth :
                </td>
                <td colspan="3">
                    <input id="txtDOB" type="text" name="txtDOB" runat="server" style="width: 130px"
                        readonly />&nbsp;
                    <img style="width: 18px; cursor: pointer; height: 15px; padding-top: 9px" onclick="displayCalendar(document.getElementById('txtDOB'),'dd/mm/yyyy',this)"
                        src="Images/callend.gif" />
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtDOB">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Gender :
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="drpGender" runat="server" CssClass="textbox">
                        <asp:ListItem Value="Select" Selected="True">-- Select --</asp:ListItem>
                        <asp:ListItem Value="male">Male</asp:ListItem>
                        <asp:ListItem Value="female">Female</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rvalGender" CssClass="error" Display="Dynamic" runat="server"
                        ErrorMessage="Please select gender" ForeColor="Red" ControlToValidate="drpGender"
                        InitialValue="Select">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3" valign="top">
                    Address :
                </td>
                <td colspan="3" valign="top">
                    <asp:TextBox ID="txtAddress" CssClass="textbox" runat="server" MaxLength="100" TextMode="MultiLine"
                        Width="240px" Height="83px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtAddress">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    State :
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="drpState" runat="server" CssClass="textbox">
                        <asp:ListItem Value="Select" Selected="True">-- Select --</asp:ListItem>
                        <asp:ListItem Value="0">Andhra Pradesh</asp:ListItem>
                        <asp:ListItem Value="1">Arunachal Pradesh</asp:ListItem>
                        <asp:ListItem Value="2">Assam</asp:ListItem>
                        <asp:ListItem Value="3">Bihar</asp:ListItem>
                        <asp:ListItem Value="4">Chhattisgarh</asp:ListItem>
                        <asp:ListItem Value="5">Goa</asp:ListItem>
                        <asp:ListItem Value="6">Gujarat</asp:ListItem>
                        <asp:ListItem Value="7">Haryana</asp:ListItem>
                        <asp:ListItem Value="8">Himachal Pradesh</asp:ListItem>
                        <asp:ListItem Value="9">Jammu and Kashmir</asp:ListItem>
                        <asp:ListItem Value="10">Jharkhand</asp:ListItem>
                        <asp:ListItem Value="11">Karnataka</asp:ListItem>
                        <asp:ListItem Value="12">Kerala</asp:ListItem>
                        <asp:ListItem Value="13">Madhya Pradesh</asp:ListItem>
                        <asp:ListItem Value="14">Maharashtra</asp:ListItem>
                        <asp:ListItem Value="15">Manipur</asp:ListItem>
                        <asp:ListItem Value="16">Meghalaya</asp:ListItem>
                        <asp:ListItem Value="17">Mizoram</asp:ListItem>
                        <asp:ListItem Value="18">Nagaland</asp:ListItem>
                        <asp:ListItem Value="19">Orissa</asp:ListItem>
                        <asp:ListItem Value="20">Punjab</asp:ListItem>
                        <asp:ListItem Value="21">Rajasthan</asp:ListItem>
                        <asp:ListItem Value="22">Sikkim</asp:ListItem>
                        <asp:ListItem Value="23">Tamil Nadu</asp:ListItem>
                        <asp:ListItem Value="24">Tripura</asp:ListItem>
                        <asp:ListItem Value="25">Uttar Pradesh</asp:ListItem>
                        <asp:ListItem Value="26">Uttarakhand</asp:ListItem>
                        <asp:ListItem Value="27">West Bengal</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rvalState" CssClass="error" Display="Dynamic" runat="server"
                        ErrorMessage="Please select state" ForeColor="Red" ControlToValidate="drpState"
                        InitialValue="Select">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    City :
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCity" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtCity">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Phone No :
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtPhoneNo" MaxLength="10" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>    
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="error" runat="server" ErrorMessage="Enter valid number"
                        ControlToValidate="txtPhoneNo" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>                             
                </td>
            </tr>
             <tr>
                <td class="style3">
                    Mobile No :
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtMobNo" MaxLength="10" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>   
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtMobNo">
                    </asp:RequiredFieldValidator> &nbsp;
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="error" runat="server" ErrorMessage="Enter valid number"
                        ControlToValidate="txtMobNo" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>             
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Email ID :
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmailID" CssClass="textbox" runat="server" Width="200px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="error" Display="Dynamic"
                        runat="server" ErrorMessage="Required" ForeColor="Red" ControlToValidate="txtEmailID">
                    </asp:RequiredFieldValidator>&nbsp;
                    <asp:RegularExpressionValidator ID="rEmail" CssClass="error" runat="server" ErrorMessage="Enter valid emailID"
                        ControlToValidate="txtEmailID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Education :
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="drpEducation" runat="server" CssClass="textbox" onChange="javascript:checkOther(this.id);">
                        <asp:ListItem Value="Select" Selected="True">-- Select --</asp:ListItem>
                        <asp:ListItem Value="0">B.Sc (CS)</asp:ListItem>
                        <asp:ListItem Value="1">B.Sc (IT)</asp:ListItem>
                        <asp:ListItem Value="2">B.E (IT)</asp:ListItem>
                        <asp:ListItem Value="3">B.E (CS)</asp:ListItem>
                        <asp:ListItem Value="4">B.Tech</asp:ListItem>
                        <asp:ListItem Value="5">M.Tech</asp:ListItem>
                        <asp:ListItem Value="6">B.C.A</asp:ListItem>
                        <asp:ListItem Value="7">M.C.A</asp:ListItem>
                        <asp:ListItem Value="8">M.Sc (CS)</asp:ListItem>
                        <asp:ListItem Value="9">M.Sc (IT)</asp:ListItem>
                        <asp:ListItem Value="10">Other</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rvalQualification" Display="Dynamic" runat="server"
                        ErrorMessage="Please select education" CssClass="error" ForeColor="Red" ControlToValidate="drpEducation"
                        InitialValue="Select">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr style="display: none;" id="trOther">
                <td class="style3">
                    Other Details :
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOtherEducation" CssClass="textbox" runat="server" Width="130px"></asp:TextBox>
                    <asp:Label ID="oher" runat="server" CssClass="error">(Please specify other education details)</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style3" valign="top">
                    I am interested in training because :
                </td>
                <td colspan="3" valign="top">
                    <asp:TextBox ID="txtInterstedBcoz" CssClass="textbox" MaxLength="800" runat="server"
                        TextMode="MultiLine" Width="350px" Height="83px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;
                </td>
                <td colspan="5" align="left">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="style3">
                    &nbsp;
                </td>
                <td colspan="4" align="left">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" ToolTip="click here to save the record"
                        OnClick="btnSubmit_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnReset" runat="server" Text="Reset" ToolTip="cleck here to reset"
                        CausesValidation="false" OnClick="btnReset_Click" />&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
        </table>      
        </form>
        <div id="footer">
            <table width="70%" align="center" style="margin: 0 auto;">
                <tr>
                    <td align="center" class="text">
                        Dynamic Web Technologies Pvt. Ltd.
                    </td>
                </tr>
                <tr>
                    <td align="center" class="text">
                        B-203, Sanpada Station Complex, Navi Mumbai-400705, India
                    </td>
                </tr>
                <tr>
                    <td align="center" class="text">
                        Tel: +91(22) 41516100 Fax: 022-41516101 Email: <a style="text-decoration: none;"
                            href="mailto:Corp.training@dynamicwebtech.com">Corp.training@dynamicwebtech.com</a>
                        Web: <a href="http://www.dynamicwebtech.com" style="text-decoration: none;" target="_blank"
                            title="website link">www.dynamicwebtech.com</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>
