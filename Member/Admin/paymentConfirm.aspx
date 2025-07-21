<%@ Page Language="VB" AutoEventWireup="false" CodeFile="paymentConfirm.aspx.vb" Inherits="admin_paymentConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD Xhtml 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
<body>
 <form > 
        <input type="hidden" value="dwtindia2557" name="Merchant_Id" />
        <input type="hidden" value="USD" name="Currency" />
        <input type="hidden" value="A" name="TxnType" />
        <input type="hidden" value="TXN" name="actionID" />
        <table height="0" cellspacing="0" width="90%"  border="1" align="center">
            <tbody>
                 <tr>
                    <td>
                        </td>
                    <td>
                 
                        <table id="invoiceTBL" align="center" style="BORDER-COLLAPSE: collapse" bordercolor="whitesmoke" cellspacing="0" cellpadding="4"  border="0">
                            <tbody>
                                <tr>
                    <td>
                        &nbsp;&nbsp;</td>
                    <td>
                        <input type="hidden" size="20" value="DWT/<%=session("payId")%>" name="Order_Id" />
                        <input type="hidden" size="20" value="<%=session("payAmount") + session("payTransAmount")%>" name="Amount" />
                        <input type="hidden" size="20" value="<%=session("custName")%>" name="billing_cust_name" />
                        <input type="hidden" size="20" value="<%=session("custAddress")%>" name="billing_cust_address" />
                        <input type="hidden" size="20" value="<%=session("custEmail")%>" name="billing_cust_email" />
                        <input type="hidden" size="20" value="<%=session("payConfirmedDate")%>" name="pay_confirm_date" />
                        <table id="invoiceTBL" align="center" style="BORDER-COLLAPSE: collapse" bordercolor="whitesmoke" cellspacing="0" cellpadding="4" width="700" border="0">
                            <tbody>
                                <tr>
                                    <td width="100%" bgcolor="#f4f4f4" colspan="2">
                                        <p align="center">
                                            <b><font face="Arial">I N V O I C E</font></b>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" width="428" height="100">
                                        <br />
                                        <font face="Arial" size="2"><%=session("custCompany")%>
                                        <br />
                                        <%=session("custAddress")%></font></td>
                                    <td valign="top" nowrap="nowrap"="nowrap="nowrap"" width="45%">
                                        <br />
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td nowrap="nowrap"="nowrap="nowrap"" width="100%">
                                                        <font face="Arial" size="2">Invoice No: DWT/<%=session("payId")%> </font></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap="nowrap"="nowrap="nowrap"" width="100%">
                                                        <font face="Arial" size="2">Dated: <%=Day(session("payDate")) & "-" & Left(MonthName(Month(session("payDate"))),3) & "-" & Year(session("payDate"))%></font></td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="428" bgcolor="#f4f4f4" >
                                        <b><font face="Arial" size="2">
                                       Description</font></b></td>
                                    <td width="45%" bgcolor="#f4f4f4">
                                        <p align="right" >
                                            <b><font face="Arial" size="2">
                                           Amount</font></b>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="75%">
                                        <font face="Arial" size="2"><%=session("payComment")%>
                                        <br />
                                        <br />
                                        </font></td>
                                    <td align="right" width="200">
                                        <font face="Arial" size="2"><%=session("currName")%><%=FormatNumber(session("payAmount"),0)%></font></td>
                                </tr>
                                <tr>
                                    <td width="428">
                                        <font face="Arial" size="2">Transaction Charges (<%=session("payTransCharge")%>%)</font></td>
                                    <td align="right" width="45%">
                                        <font face="Arial" size="2"><%=session("currName")%><%=FormatNumber(session("payTransAmount"),0)%></font></td>
                                </tr>
                                <tr>
                                    <td width="428" bgcolor="#f4f4f4">
                                        <b><font face="Arial" size="2">Gross Total</font></b></td>
                                    <td width="45%" bgcolor="#f4f4f4">
                                        <p align="right">
                                            <b><font face="Arial" size="2"><%=session("currName")%><%=FormatNumber(session("payTransAmount") + session("payAmount"),0)%></font></b>
                                        </p>
                                    </td>
                                </tr>
								<tr><td>&nbsp;</td></tr>
								  <tr>
                                    <td width="428" bgcolor="#f4f4f4">
                                        <b><font face="Arial" size="2">Invoice Status</font></b></td>
                                    <td width="45%" bgcolor="#f4f4f4">
                                    <p align="right">
                                     <b>
									 <font face="Arial" size="2"> 
                                 	 <%
								      If (session("payConfirmedDate") <> "") then
									 %>
                                       <p align="right" style="COLOR: green">Paid</p>  
										<% Else %>
									   <p align="right" style="COLOR: red">Pending  </p>
										<% End If %>
								    </font>
								   </b>
									 </p>
                                    </td>
                                </tr>
        <TR align="center" > <TD colspan="4">
<INPUT type="button" value="Close" onclick="window.close()">
          </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></FORM></BODY></html>