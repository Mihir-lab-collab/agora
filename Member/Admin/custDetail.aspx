<%@ Page Language="VB" %>

<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.NET" %>
<%@ Import Namespace="System.IO.FileStream" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Customer Details</title>

    <script language="VB" runat="server">
        Dim SortField As String
        Dim custCode As String
        Dim sql As String
        Dim Conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim strCustid, strCustname, strCustpassword, strcustemail As String
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            Dim intCustid As Integer = Request.QueryString("custId")
            Dim strSQL As String = "select custname,custemail,custid from customermaster  where custId =" & intCustid
            Dim strdsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
            Dim conn As SqlConnection = New SqlConnection(strdsn1)
            conn.Open()
            Dim objcmd As SqlCommand = New SqlCommand(strSQL, conn)
            Dim objdatareader As SqlDataReader

            objdatareader = objcmd.ExecuteReader

            If objdatareader.Read() Then
                strCustid = objdatareader("custid")
                strCustname = objdatareader("Custname").ToString()
                strcustemail = objdatareader("Custemail").ToString()
            End If
            objdatareader.Close()
        End Sub

        Function boolean_to_int(ByVal flag As Boolean) As Integer
            If (flag = True) Then
                boolean_to_int = 1
            Else
                boolean_to_int = 0
            End If
        End Function


        Sub addBT_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
            If custId.Value = "" Then
                sql = "INSERT INTO customerMaster(custName,custCompany," & _
                "custEmail,custNotes,custAddress,allowTSCust,allowInvoice," & _
                "allowTaskManager,allowComplaints,allowFeedback,allowProjectReport,allowChangeRequest) VALUES('" & _
                custName.Value & "','" & custCompany.Value & "','" & custEmail.Value & "','" & _
                custNotes.Value & "','" & _
                Replace(custAddress.Value, "'", "''") & "'," & boolean_to_int(chkTimeSheet.Checked) & "," & _
                boolean_to_int(chkInvoices.Checked) & "," & boolean_to_int(chkTask.Checked) & "," & _
                boolean_to_int(chkComplaint.Checked) & "," & boolean_to_int(chkFeedback.Checked) & "," & _
                boolean_to_int(chkProjReport.Checked) & "," & boolean_to_int(chkChangeReq.Checked) & ")"
            Else
                sql = "UPDATE customerMaster SET custName='" & custName.Value & _
                "',custCompany='" & custCompany.Value & "',custEmail='" & _
                custEmail.Value & "',custNotes='" & custNotes.Value & "',custAddress='" & _
                Replace(custAddress.Value, "'", "''") & "', allowTSCust=" & _
                boolean_to_int(chkTimeSheet.Checked) & ",allowInvoice=" & boolean_to_int(chkInvoices.Checked) & _
                ",allowTaskManager=" & boolean_to_int(chkTask.Checked) & ",allowComplaints=" & _
                boolean_to_int(chkComplaint.Checked) & ",allowFeedback=" & boolean_to_int(chkFeedback.Checked) & _
                ",allowProjectReport=" & boolean_to_int(chkProjReport.Checked) & ",allowChangeRequest=" & _
                boolean_to_int(chkChangeReq.Checked) & " WHERE custid=" & custId.Value
            End If
            Response.Write(sql)
            ' Response.End()
            Conn.open()
            Dim Cmd As New SqlCommand(sql, Conn)
            Cmd.ExecuteNonQuery()
            Response.Redirect("customerView.aspx")
        End Sub

        '===============================================================================

        Sub loadData()

            Dim strdsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
            Dim conn As SqlConnection = New SqlConnection(strdsn1)
            Dim Rdr As SqlDataReader
            custCode = Request.QueryString("custId")
            conn.Open()
            If custCode <> "" Then
                sql = "select allowTSCust,allowInvoice,allowTaskManager,allowComplaints,allowFeedback,allowProjectReport,allowChangeRequest from customerMaster where custId=" & custCode
                Dim cmdchecked As New SqlCommand(sql, conn)
                Rdr = cmdchecked.ExecuteReader
                Dim a, b, c, d, e, f, g As Integer
                If Rdr.Read() Then
                    a = Rdr("allowTSCust")
                    b = Rdr("allowInvoice")
                    c = Rdr("allowTaskManager")
                    d = Rdr("allowComplaints")
                    e = Rdr("allowFeedback")
                    f = Rdr("allowProjectReport")
                    g = Rdr("allowChangeRequest")
					
                    If a = -1 Then
                        chkTimeSheet.Checked = True
                    Else
                        chkTimeSheet.Checked = False
                    End If
					
                    If b = -1 Then
                        chkInvoices.Checked = True
                    Else
                        chkInvoices.Checked = False
                    End If
					
                    If c = -1 Then
                        chkTask.Checked = True
                    Else
                        chkTask.Checked = False
                    End If
					
                    If d = -1 Then
                        chkComplaint.Checked = True
                    Else
                        chkComplaint.Checked = False
                    End If
					
                    If e = -1 Then
                        chkFeedback.Checked = True
                    Else
                        chkFeedback.Checked = False
                    End If
					 
                    If f = -1 Then
                        chkProjReport.Checked = True
                    Else
                        chkProjReport.Checked = False
                    End If
					
                    If g = -1 Then
                        chkChangeReq.Checked = True
                    Else
                        chkChangeReq.Checked = False
                    End If
                End If
                Rdr.Close()
	 
                sql = "select * from customerMaster where custId=" & custCode
                Dim Cmd As New SqlCommand(sql, conn)
                Rdr = Cmd.ExecuteReader()
                If Rdr.Read() Then
                    custId.Value = Rdr("custId")
                    custIdLbl.Text = custId.Value
                    custName.Value = Rdr("custName").ToString()
                    If Not IsDBNull(Rdr("custCompany")) Then
                        custCompany.Value = Rdr("custCompany")
                    End If
                    custEmail.Value = Rdr("custEmail").ToString()
                    custNotes.Value = Rdr("custNotes") & ""
                    custAddress.Value = Rdr("custAddress") & ""
                End If
                Rdr.Close()
            End If
        End Sub
    </script>

    <% loadData()%>
</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <div id="d" runat="server" />
    <form runat="server" id="custForm">
        <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse;
            border-color: #e8e8e8">
            <tr>
                                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Contact Name</b></font></td>
                <td width="25%">
                    <input type="text" id="custName" runat="server" size="20" />&nbsp;</td>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>ID</b></font></td>
                <td width="25%">
                    <asp:Label ID="custIdLbl" runat="server" Text=""></asp:Label>
                    <input id="custId" type="text" size="20" name="custId" runat="server" readonly="readonly" style="visibility:hidden"/></td>
            </tr>
            <tr>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Organisation</b></font></td>
                <td width="25%">
                    <input type="text" id="custCompany" runat="server" size="30" />&nbsp;</td>
                                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b></b></font>
                </td>
                <td width="25%">
                </td>
            <tr>
                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Address</b></font></td>
                <td width="25%">
                    <textarea id="custAddress" runat="server" rows="3" cols="40" name="custNotes"></textarea>
                </td>

                <td width="25%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Contact Email <br />(comma separated)</b></font></td>
                <td width="25%">
                    <textarea id="custEmail" runat="server" rows="3" cols="40" name="custEmail"></textarea>
                    </td>
            </tr>
            <tr>
                <td colspan="4" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b><BR />Rights Management</b></font></td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>TimeSheet</b></font></td>
                <td>
                    <asp:CheckBox ID="chkTimeSheet" runat="server" /></td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Feedback</b></font></td>
                <td>
                    <asp:CheckBox ID="chkFeedback" runat="server" Checked="true" /></td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Invoices</b></font></td>
                <td>
                    <asp:CheckBox ID="chkInvoices" runat="server" /></td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Project Reports</b></font></td>
                <td>
                    <asp:CheckBox ID="chkProjReport" runat="server" Checked="true" /></td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Complaints</b></font></td>
                <td>
                    <asp:CheckBox ID="chkComplaint" runat="server" Checked="true" /></td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Change Request</b></font></td>
                <td>
                    <asp:CheckBox ID="chkChangeReq" runat="server" Checked="true" /></td>
            </tr>
            <tr>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Assign Task</b></font></td>
                <td>
                    <asp:CheckBox ID="chkTask" runat="server" Checked="true" /></td>
                <td width="17%" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"></font>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td width="100%" colspan="4" align="center" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Notes </b></font>
                    <br />
                    <textarea id="custNotes" runat="server" rows="5" cols="70" name="Textarea1"></textarea>
                </td>
            </tr>
            <tr>
                <%		If custId.Value <> "" Then%>
                <td align="center" colspan="4">
                    <input type="button" id="addBT2" runat="server" value="Save" align="right" style="font-family: Verdana;
                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                        font-bold="true" width="55" onserverclick="addBT_OnClick">
                </td>
                <% 		Else%>
                <td align="center" colspan="4">
                    <input type="button" id="addBT1" runat="server" value="Save" align="right" style="font-family: Verdana;
                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                        font-bold="true" width="55" onserverclick="addBT_OnClick">
                    <%		End If%>
            </tr>
        </table>
    </form>
</body>
</html>
