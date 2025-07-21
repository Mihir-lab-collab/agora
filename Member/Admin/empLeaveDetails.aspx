<%@ Page Language="VB" ContentType="text/html" ResponseEncoding="iso-8859-1" %>
<%@Import Namespace="System.Data.SqlClient"%>
<%@Import Namespace="System.Data"%>


<script language="VB" runat="server">
Dim sql  as String
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As SqlConnection = New SqlConnection(dsn)
    Dim dr1 As SqlDataReader
    Dim cmd As SqlCommand
    Dim gf As New generalFunction

    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        gf.checkEmpLogin()
        If Request.QueryString("empLeaveId") <> "" Then
            If Me.IsPostBack = False Then
                cmd = New SqlCommand("select leave.*,convert(varchar(20),leave.leaveFrom,106) as fromDate,convert(varchar(20),leave.leaveEntryDate,106) as applydate,convert(varchar(20),leave.leaveTo,106) as todate, empStatus.statusDesc as desc1,case when leavestatus='p' then 'pending' when leavestatus='r' then 'rejected' when leavestatus='a' then 'approved' end as ls, emp.empId,emp.empName  from employeeMaster emp, empLeaveDetails leave,empStatus where LeaveId = statusID AND emp.empid=leave.empid AND leave.empLeaveId=" & Request.QueryString("empLeaveId") & " order by leave.leaveEntrydate", conn)
                conn.Open()
                dr1 = cmd.ExecuteReader()
                While dr1.Read()
                    lblempid.Text = dr1("empId")
                    lblempName.Text = dr1("empName")
                    lblLeaveAppliedOn.Text = dr1("applydate")
                    lblLeaveFrom.Text = dr1("fromDate")
                    lblLeaveTo.Text = dr1("todate")
                    lblLeaveType.Text = dr1("desc1")
                    lblReason.Text = dr1("leaveDesc")
                    lblstatus.Text = dr1("ls")

                    If IsDBNull(dr1.Item("leaveSenctionedDate")) Then
                    Else
                        lblLeaveSanctionedDate.Text = dr1("leaveSenctionedDate")
                    End If

                    'If dr1.Item(leaveSanctionBy) IsNot DBNull.Value Then
                    'lblLeaveSanctionedBy.text=dr1("leaveSanctionBy")
                    'end if
                    'If dr1.item(leaveComment) IsNot DBNull.Value Then
                    'lblAdminComments.text=dr1("leaveComment")
                    'end if
                End While
                conn.Close()
            End If
        End If
    End Sub

</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD html 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Applied Leave Details</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
</head>
<body>
<form runat="server">
<table width="100%"  border="0" cellspacing="0" height="100%" cellpadding="0">
  <tr>
    <td><div align="center">Leave Applied Details </div></td>
  </tr>
  <tr>
    <td align="center"><font face="Verdana" size="2" color="#A2921E" ><b>
	<table width="50%"  border="0" cellspacing="0" cellpadding="0">
      <tr bgcolor="#FFFFEE">
        <td width="100%" colspan="2" align="center"><table width="60%"  border="0" cellspacing="0" cellpadding="2">
          <tr bgcolor="#C5D5AE">
            <td>
			 <font face="Verdana" size="3" color="#A2921E" ><b>
			 <div align="left">Employee ID :</div></td>
            <td nowrap="nowrap"="true"><font face="Verdana" size="3" color="#A2921E"><b>
              <div align="left">
                <asp:Label ID="lblempid" runat="server" />                          
            </div></td>
          </tr>
          <tr>
            <td nowrap="nowrap"="true">
			<font face="Verdana" size="2" color="#A2921E"><b>
			<div align="left">Employee Name: </div></td>
            <td nowrap="nowrap"="true">
			<font face="Verdana" size="2" color="#A2921E"><b>
              <div align="left">
                <asp:Label ID="lblempName" runat="server" />                          
            </div></td>
          </tr>
          
          <tr bgcolor="#C5D5AE">
            <td><div align="left">
			<font face="Verdana" size="2" color="#A2921E"><b>
			Leave Applied On: </div></td>
            <td><font face="Verdana" size="2" color="#A2921E"><b>
              <div align="left">
                <asp:Label ID="lblLeaveAppliedOn" runat="server" />                          
           </div></td>
          </tr>
		  <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
        </table></td>
        
      </tr>
      <tr bgcolor="#FFFFEE">
        
        <td  align="center" colspan="2"><table width="60%"  border="1" cellspacing="0" cellpadding="0">
          <tr align="center" bgcolor="#C5D5AE">
            
            <td colspan="2">
			<font face="Verdana" size="2" color="#A2921E"><b>
			Leave</td>
          </tr>
          <tr>
            <td><font face="Verdana" size="2" color="#A2921E"><b><div align="center">From</div></td>
            <td><div align="center"> <font face="Verdana" size="2" color="#A2921E"><b>To</div></td>
          </tr>
          
          <tr>
            <td><font face="Verdana" size="2" color="#A2921E"><b>
              <asp:Label ID="lblLeaveFrom" runat="server"/>            
            </td>
            <td><font face="Verdana" size="2" color="#A2921E"><b>
              <asp:Label ID="lblLeaveTo" runat="server" />            
           </td>
          </tr>
		  <tr  bgcolor="#C5D5AE">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
        </table></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td><div align="center">
      <table width="50%"  border="1" cellspacing="0" cellpadding="0">
        <tr>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  Leave Type</td>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">
            <asp:Label ID="lblLeaveType" runat="server" />           
          </div></td>
        </tr>
        <tr>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  Leave Reason</td>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">
            <asp:Label ID="lblReason" runat="server" />          
          </div></td>
        </tr>
        
        <tr>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  Leave Status</td>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">
            <asp:Label ID="lblstatus" runat="server" />          
          </div></td>
        </tr>
		<tr>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  Leave Sanctioned Date </td>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">
            <asp:Label ID="lblLeaveSanctionedDate" runat="server" />          
          </div></td>
        </tr>
		<tr>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  Leave Sanction By </td>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">
            <asp:Label ID="lblLeaveSanctionedBy" runat="server" />          
          </div></td>
        </tr>
		<tr>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  Admin Comments </td>
          <td>
		  <font face="Verdana" size="2" color="#A2921E"><b>
		  <div align="left">
            <asp:Label ID="lblAdminComments" runat="server" />          
          </div></td>
        </tr>
      </table>
    </div></td>
  </tr>
</table>
</form>
</body>
</html>