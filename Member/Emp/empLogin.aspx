<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>

<%@ Page Language="vb" AutoEventWireup="false" %>

<%@ Import Namespace="dwtDAL" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Intelgain Technologies Pvt Ltd</title>

    <script language="javascript" type="text/javascript">
		  function loginValidation()
		  {
		    if ((document.forms[0].empId.value=="")||(document.forms[0].empPassword.value==""))
		    {
		        alert("Please Enter Valid EmpId/Password");
		        document.forms[0].empId.focus(); 
		        return false;
		    }
		    else
		    {
		        return false;
		    }
		  }
    </script>

    <script runat="server">
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader
        Dim sqlStr As String

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                empId.Text = Request.QueryString("empId")

                If Request("logout") = "true" Then
                    Session.Abandon()
                    Response.Redirect("/Member")
                End If
                
                If Session("EmpID") <> Nothing Then
                    Login(Session("EmpID"))
                End If
                
                If Session("DynoEmpSession") = Nothing Then
                    Response.Redirect("/Member/", False)
                    Response.End()
                End If
                    
            End If
        End Sub
 
        Sub doLogin(ByVal obj As Object, ByVal eArg As EventArgs)
            'Login(empId.Text)
        End Sub

        Sub Login(EmpID As String)
            Dim comppwd As String
            Dim haskey As String = ConfigurationManager.AppSettings("hashKey").ToString()
            Dim tempbyte As Byte = Byte.Parse("123")
            Dim bytearr(1) As Byte
            bytearr(0) = tempbyte

            sqlStr = "SELECT * FROM employeeMasterView WHERE  empLeavingDate is null and  empId=" & EmpID
            conn.Open()
            cmd = New SqlCommand(sqlStr, conn)
            dr = cmd.ExecuteReader
            If dr.HasRows = True Then
                If dr.Read() Then
                    'comppwd = SessionHelper.ComputeHash(empPassword.Text, haskey, bytearr)
                    'check = SessionHelper.VerifyHash(empPassword.Text, haskey, CStr(dr("empPassword")))
                    ' If (check = True) Then
                    Dim EmpData As New Collection
                    Dim i As Integer
                    EmpData.Add(True, "Valid")

                    For i = 0 To dr.FieldCount - 1
                        If IsDBNull(dr(i)) Then
                            EmpData.Add("", dr.GetName(i))
                        Else
                            EmpData.Add(dr(i), dr.GetName(i))
                        End If
                    Next
                    Session("DynoEmpSession") = EmpData
                    Session("dynoEmpIdSession") = dr("empId")
                    Session("dynoEmpNameSessionHeader") = dr("empName")
                    Session("dynoEmpNameSession") = dr("empName")
                    Session("dynoBugAdminSession") = 0
                            		
                    If dr("IsSuperAdmin") <> 0 Then
                        Session("dynoAdminSession") = 1
                    End If
                    If dr("IsAccountAdmin") <> 0 Then
                        Session("IsAccountAdminSession") = 1
                    End If
                    If dr("IsPM") <> 0 Then
                        Session("projectmanager") = 1
                        Session("dynoMgrSession") = 1
                    End If
                    If dr("IsTester") <> 0 Then
                        Session("dynoBugAdminSession") = 1
                    End If
                    If dr("IsProjectReport") <> 0 Then
                        Session("dynoProjectReport") = 1
                    End If
                    If dr("IsProjectStatus") <> 0 Then
                        Session("dynoProjectStatus") = 1
                    End If
                    If dr("IsLeaveAdmin") <> 0 Then
                        Session("dynoLeaveAdmin") = 1
                    End If
                    If dr("IsPayrollAdmin") <> 0 Then
                        Session("IsPayrollAdminSession") = 1
                    End If
					
                    If Request.QueryString("bugsid") <> "" Then
                        Session("Displaybugid") = Request.QueryString("bugsid")
                        Response.Redirect("taskManager.aspx")
                    Else
                        Response.Redirect("empHome.aspx")
                    End If
                Else
                    txtStatus.Text = "You have Entered Wrong Employee Id"
                End If
                'Else
                '  txtStatus.Text = "You have Entered Wrong Password"
                ' End If
            Else
                txtStatus.Text = "You are not an authorized user "
            End If
           
            dr.Close()
            conn.Close()
        End Sub
    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table id="table3" style="height: 100%" cellspacing="0" cellpadding="2" width="90%"
        align="center" border="0">
        <tr>
            <td>
                <EMPHEADER:empHeader ID="Empheader" runat="server"></EMPHEADER:empHeader>
            </td>
        </tr>
        <tr>
            <td align="center" height="90%">
                <table id="table1" cellspacing="0" cellpadding="3" width="300" border="1">
                    <tr>
                        <td align="center" bgcolor="#edf2e6" colspan="3">
                            <asp:Label ID="txtLogin" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                ForeColor="#804040" Font-Bold="true">Login Here</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#edf2e6" colspan="2">
                            <asp:Label ID="txtStatus" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#edf2e6" width="20%">
                            <asp:Label ID="txtEmpId" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                ForeColor="#804040" Font-Bold="true">Emp Id</asp:Label>
                        </td>
                        <td width="20%">
                            <asp:TextBox ID="empId" runat="server" TextMode="SingleLine" Width="120px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valrId" runat="server" ErrorMessage="Required" Font-Names="Verdana"
                                Font-Size="8pt" ControlToValidate="empId" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valeId" runat="server" ErrorMessage="Enter 4 Digits"
                                Font-Names="Verdana" Font-Size="8pt" ControlToValidate="empId" ValidationExpression="[0-9]{4}"
                                Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="20%" bgcolor="#edf2e6">
                            <asp:Label ID="txtEmpPassword" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                ForeColor="#804040" Font-Bold="true">Password</asp:Label>
                        </td>
                        <td width="80%">
                            <asp:TextBox ID="empPassword" runat="server" TextMode="Password" Width="120px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valrpwd" runat="server" ErrorMessage="Required" Font-Names="Verdana"
                                Font-Size="8pt" ControlToValidate="empPassword"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" bgcolor="#edf2e6" colspan="3">
                            <asp:Button ID="submitBtn" OnClick="doLogin" runat="server" Text="Login" Font-Names="Verdana"
                                Font-Size="Smaller" ForeColor="#804040" Font-Bold="true" BackColor="#EDF2E6">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
