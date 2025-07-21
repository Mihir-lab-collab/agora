<%@ Page Language="vb" AutoEventWireup="false" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="dwtDAL" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD html 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Intelgain Technologies Pvt Ltd</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />

    <script language="javascript" src="/includes/CalendarControl.js" type="text/javascript"></script>

    <script runat="server">
        Dim haskey As String = ConfigurationManager.AppSettings("hashKey").ToString()
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim sql As String
        Dim gf As New generalFunction
		    
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            gf.checkEmpLogin()
            If Not IsPostBack Then
                sql = "SELECT * FROM EmployeeMasterView WHERE empId=" & gf.EmpSession("empID")
                Dim SqlCmd As New SqlCommand(sql, conn)
                conn.Open()
                Dim DRead As SqlDataReader
                DRead = SqlCmd.ExecuteReader
                While DRead.Read()
                    lblEmpId.Text = DRead("empid").ToString()
                    txtEmpPwd.Text = ""
                    lblEmpName.Text = DRead("empName").ToString()
                    txtEmpAddress.Text = DRead("empAddress").ToString()
                    txtEmpContactNo.Text = DRead("empContact").ToString()
                    lblEmpEMail.Text = DRead("empEmail").ToString()
                    lblEmpAccNo.Text = DRead("empAccountNo")
		                
                    If IsDate(DRead("empBdate")) Then
                        txtEmpBdate.Text = Format(DRead("empBdate"), "dd-MMM-yyyy")
                    End If
                    
                        Dim empAdate As Boolean
                        Boolean.TryParse(Convert.ToString(DRead("empAdate")), empAdate)
                    
                        If (IsDBNull(DRead("empAdate")) = False And IsDate(empAdate) And Format(empAdate, "dd-MMM-yyyy") <> "01-Jan-1900") Then
                            txtEmpADate.Text = Format(DRead("empAdate"), "dd-MMM-yyyy")
                        Else
                            txtEmpADate.Text = ""
                        End If
                    
                    lblEmpSkills.Text = DRead("SkillDesc").ToString()
                    If DRead("EmpJoiningDate").ToString() <> "" Then
                        lblEmpJoinDate.Text = CDate(DRead("EmpJoiningDate")).ToString("dd-MMM-yyyy")
                    Else
                        lblEmpJoinDate.Text = DRead("EmpJoiningDate").ToString()
                    End If
                       
                        
                    If DRead("empProbationPeriod").ToString() <> "" Then
                        lblEmpProbPeriod.Text = DRead("empProbationPeriod").ToString() & " months"
                    Else
                        lblEmpProbPeriod.Text = DRead("empProbationPeriod").ToString()
                    End If
                End While
                DRead.Close()
            End If
        End Sub
    
        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim tempbyte As Byte = Byte.Parse("123")
            Dim bytearr(1) As Byte
            Dim strPassword As String
            bytearr(0) = tempbyte
            If txtEmpPwd.Text <> "" Then
                strPassword = SessionHelper.ComputeHash(txtEmpPwd.Text, haskey, bytearr)
            Else
                strPassword = SessionHelper.ComputeHash("dynamic", haskey, bytearr)
            End If
            sql = "UPDATE employeeMaster SET empPassword='" & strPassword & _
                  "',empAddress='" & txtEmpAddress.Text & "', empContact='" & txtEmpContactNo.Text & _
                  "', empBDate ='" & txtEmpBdate.Text & "',empADate ='" & txtEmpADate.Text & "' where empid=" & _
                  gf.EmpSession("empId")
            Dim SqlCmd As New SqlCommand(sql, conn)
            conn.Open()
            SqlCmd.ExecuteNonQuery()
            conn.Close()
           
            
        End Sub
    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="Table2" cellspacing="0" cellpadding="2" width="100%" border="0">
        <tr>
            <td>
                <table id="Table3" cellspacing="0" cellpadding="2" width="100%" border="0" height="1">
                    <tr>
                        <td>
                            <EMPHEADER:empHeader ID="Empheader" runat="server"></EMPHEADER:empHeader>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <uc1:empMenuBar ID="EmpMenuBar" runat="server"></uc1:empMenuBar>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1">
                    <tr>
                        <td bgcolor="#c5d5ae" colspan="4">
                            <b><font face="Verdana" color="#a2921e" size="2">Employee Profile</font></b>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Emp Id</font></b>
                        </td>
                        <td nowrap width="75%" colspan="3">
                            <asp:Label ID="lblEmpId" runat="server" Font-Bold="True" Font-Size="Smaller" Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Name</font></b>
                        </td>
                        <td nowrap colspan="3">
                            <asp:Label ID="lblEmpName" runat="server" Font-Bold="True" Font-Size="Smaller" Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Address</font></b>
                        </td>
                        <td nowrap colspan="3" height="57">
                            <asp:TextBox ID="txtEmpAddress" runat="server" MaxLength="255" TextMode="MultiLine"
                                Height="51px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Contact Number</font></b>
                        </td>
                        <td nowrap colspan="3">
                            <asp:TextBox ID="txtEmpContactNo" runat="server" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">E-Mail</font></b>
                        </td>
                        <td nowrap colspan="3">
                            <asp:Label ID="lblEmpEMail" runat="server" Font-Bold="True" Font-Size="Smaller" Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2"><b><font face="Verdana" color="#a2921e"
                                size="2">Designation</font></b></font></b>
                        </td>
                        <td nowrap width="75%" colspan="3" height="30">
                            <asp:Label ID="lblEmpSkills" runat="server" Font-Bold="True" Font-Size="Smaller"
                                Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Join Date</font></b>
                        </td>
                        <td nowrap width="75%" colspan="3" height="30">
                            <asp:Label ID="lblEmpJoinDate" runat="server" Font-Bold="True" Font-Size="Smaller"
                                Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Probation Period</font></b>
                        </td>
                        <td nowrap width="75%" colspan="3" height="30">
                            <asp:Label ID="lblEmpProbPeriod" runat="server" Font-Bold="True" Font-Size="Smaller"
                                Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Password</font></b>
                        </td>
                        <td nowrap width="75%" colspan="3" height="30">
                            <asp:TextBox ID="txtEmpPwd" runat="server"></asp:TextBox>
                            <asp:requiredfieldvalidator runat="server" ControlToValidate="txtEmpPwd" errormessage="enter password"></asp:requiredfieldvalidator>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Birth Date</font></b>
                        </td>
                        <td nowrap width="75%" colspan="3" height="30">
                            <asp:TextBox ID="txtEmpBdate" runat="server" onclick="popupCalender('txtEmpBdate')"
                                onkeypress="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Anniversary Date</font></b>
                        </td>
                        <td nowrap width="75%" colspan="3" height="30">
                            <asp:TextBox ID="txtEmpADate" runat="server" onclick="popupCalender('txtEmpADate')"
                                onkeypress="return false;"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap bgcolor="#edf2e6">
                            <b><font face="Verdana" color="#a2921e" size="2">Account No.</font></b>
                        </td>
                        <td>
                            <asp:Label ID="lblEmpAccNo" runat="server" Font-Bold="True" Font-Size="Smaller" Font-Names="Verdana"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap align="center" width="75%" colspan="4" height="30" rowspan="2">
                            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="90px" BorderWidth="1px"
                                BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E" Text="Save"></asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
