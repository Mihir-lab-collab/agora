<%@ Page Language="VB" AutoEventWireup="false" CodeFile="adminAddchangeRequest.aspx.vb"
	Inherits="admin_adminAddchangeRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>

<html xmlns="http://www.w3.org/1999/xhtml">

<script language="javascript" type="text/javascript">
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
</script>

<script language="javascript" type="text/javascript">
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

<head runat="server">
	<title>Untitled Page</title>
		<link rel="stylesheet" href="/includes/CalendarControl.css"  type="text/css"/>
</head>
<body>
	<form id="form1" runat="server">
		<table align="center" width="95%" style="height: 90%">
			<tr>
				<td align="center">
					<font face="verdana" size="2"><b>Change Request</b></font></td>
			</tr>
			<tr>
				<td valign="top">
					<table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;
						background-color: #EAEAEA; border-color: #111111" id="AutoNumber1" width="100%">
						<tr>
							<td face="Verdana" color="black" size="2" bgcolor="#EAEAEA" nowrap="nowrap"="nowrap="nowrap"" width="25%">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Project</font></td>
							<td width="75%">
								<font face="Verdana" size="2"><b>
									<asp:Label ID="lblprojectname" runat="server" face="Verdana" color="black" size="2"></asp:Label></b></font>
							</td>
						</tr>
						<tr>
							<td bgcolor="#EAEAEA">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Company Name: </font>
							</td>
							<td>
								<font face="Verdana" size="2">
									<asp:Label ID="lblCompName" face="Verdana" size="2" color="black" runat="server"
										columns="62" maxlength="254"></asp:Label></font>
							</td>
						</tr>
						<tr>
							<td bgcolor="#EAEAEA">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Customer Company : </font>
							</td>
							<td>
								<font face="Verdana" size="2">
									<asp:Label ID="lblCustname" face="Verdana" color="black" runat="server" columns="62"
										maxlength="254"></asp:Label></font>
							</td>
						</tr>
						<tr style="display: none">
							<td bgcolor="#EAEAEA">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Currency: </font>
							</td>
							<td>
								<font face="Verdana" size="2">
									<asp:Label ID="txtCurrency" runat="server" columns="62" maxlength="254"></asp:Label></font>
							</td>
						</tr>
						<tr>
							<td bgcolor="#EAEAEA">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Change Request Title: </font>
							</td>
							<td>
								<asp:TextBox ID="txtchangetitle" runat="server" Columns="62" MaxLength="254"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap"="nowrap="nowrap"">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Change Request Description:</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:TextBox Rows="3" TextMode="MultiLine" ID="chgdescription" Columns="48" row="5"
									runat="server"></asp:TextBox></td>
						</tr>
						<tr style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Change Approval:</font></td>
							<td width="25%" valign="middle" bgcolor="#EAEAEA">
								<asp:Label ID="lblApproved" runat="server" Font-Bold="True" Font-Size="10pt" Font-Names="Verdana"></asp:Label>
							</td>
						</tr>
						<tr id="trECT" runat="server">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Estimated Completion Time :</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:TextBox ID="estComTime" size="3" runat="server" type="text" onkeypress=" return numericCharacters(event)"
									MaxLength="3"></asp:TextBox><font face="verdana" size="2" color="black"><b>&nbsp;&nbsp;Days</b></font></td>
						</tr>
						<tr id="trECT1" runat="server">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Estimated Completion Cost :</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:TextBox ID="estComCost" runat="server" type="text" onkeypress=" return numericCharacters(event)"></asp:TextBox></td>
						</tr>
						<tr runat="server" style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Exchange Rate :</font></td>
							<td valign="middle" bgcolor="#EAEAEA">
								<input id="txtExchangerate" runat="server" disabled="true" type="text" onkeyup="if(isNaN(this.value)){alert('Invalid number');return false;}" /></td>
						</tr>
						<tr runat="server" style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Approved By :</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:DropDownList ID="ddlemp" runat="server" Enabled="False" CssClass="cssData">
								</asp:DropDownList></td>
						</tr>
						<tr runat="server">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Change Request Date :</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:TextBox runat="server" ID="txtchange" onclick="popupCalender('txtchange');"></asp:TextBox>
							</td>
						</tr>
						<tr style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Approved Date :</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:TextBox runat="server" ID="txtdateapp" onclick="popupCalender('txtdateapp');"></asp:TextBox>
							</td>
						</tr>
						<tr id="trCD" runat="server" style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Completion Date :</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<asp:TextBox ID="txtDate" runat="server" size="14" onclick="popupCalender('txtDate');"></asp:TextBox></td>
						</tr>
						<tr style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Comments:</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<textarea rows="3" name="comment" cols="48"></textarea></td>
						</tr>
						<tr style="display: none">
							<td width="25%" bgcolor="#EAEAEA" nowrap="nowrap">
								<font face="verdana" size="2" color="#0000FF">&nbsp;Comment History:</font></td>
							<td width="75%" valign="middle" bgcolor="#EAEAEA">
								<textarea rows="5" name="comment_history" cols="48" readonly="readonly" style="color: #C0C0C0;
									border: 1px solid #F4F4F4"></textarea></td>
						</tr>
						<tr style="display: none">
							<td>
								<input id="fileAdd" type="file" runat="server" style="height: 25; width: 300;" size="20" />
							</td>
						</tr>
						<tr>
							<td width="100%" bgcolor="#EAEAEA" colspan="2">
								<p align="center">
									<asp:Button ID="btnSave" Style="font-family: Verdana; font-size: 8pt;" runat="server"
										Width="55px" Text="Submit" onclick="btnSave_Click1"></asp:Button>&nbsp;
									<input id="btnclose" type="button" onclick="javascript:window.close();" value="Close"
										width="110px" style="font-family: Verdana; font-size: 8pt;"/></p>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</form>
</body>
</html>
