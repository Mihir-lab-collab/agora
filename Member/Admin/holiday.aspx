<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Holiday</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css" />
</head>

<script language="VB" runat="server">
 
    Dim objCommon as New clsCommon()
    Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As SqlConnection = New SqlConnection(dsn1)
    Dim flg As Integer = 0
    Dim lflg As Integer = 0
    Dim gf As New generalFunction
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        gf.checkEmpLogin()
        If Not IsPostBack Then
            If Session("dynoAdminSession") = 1 Then
                txtholidaydate.Attributes.Add("onClick", "popupCalender('txtholidaydate')")
		        BindLocation()
            End If
        End If

    End Sub

  

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim strsql1 As String = ""
      
        strsql1 = "select holidaydate from holidaymaster where holidaydate ='" & txtholidaydate.Text & "'" 
        strsql1 = "select holidaydate from holidaymaster where holidaydate ='" & txtholidaydate.Text & "' and  LocationID ='" & dlLocation.SelectedValue & "'"

        Dim sqlcmd1 As SqlCommand = New SqlCommand(strsql1, conn)
        conn.Open()

          
        Dim test As String
        test = sqlcmd1.ExecuteScalar()

        If test <> "" Then

            If Trim(CType(test, Date)) = Trim(CType(txtholidaydate.Text, Date))   Then
  
                lflg = 1
            End If

        Else

            lflg = 0
        End If


   	          
        Dim DR As SqlDataReader
        DR = sqlcmd1.ExecuteReader()
        While DR.Read()
            If DR("holidayDate").ToString() <> "" Then
                txtholidaydate.Text = CDate(DR("holidayDate")).ToString("dd-MMM-yyyy")
            Else
                txtholidaydate.Text = DR("holiday").ToString()
            End If
        End While

        DR.Close()
        conn.Close()

        Dim strsql As String = ""
	              
        If lflg = 1 Then
	
            If txtholidaydesc.Text <> "" Then
                strsql = "update holidaymaster set holidaydesc='" & Trim(Replace(txtholidaydesc.Text, "'", "''")) & "'     where holidaydate='" & txtholidaydate.Text & "'  and  LocationID ='" & dlLocation.SelectedValue & "'" 
		
                LBLholidaydate.Text = "RECORD UPDATED "
            End If
        Else
            If txtholidaydesc.Text <> "" Then
                strsql = "insert into holidaymaster (holidaydate,holidaydesc,LocationID) Values('" & Trim(Replace(txtholidaydate.Text, "'", "''")) & "' ,'" & Trim(Replace(txtholidaydesc.Text, "'", "''")) & "','" & Trim(Replace( dlLocation.SelectedValue, "'", "''")) & "')"

                LBLholidaydate.Text = "RECORD INSERTED "
            End If
        End If


        If txtholidaydesc.Text <> "" Then
            Dim sqlcmd As SqlCommand = New SqlCommand(strsql, conn)
            conn.Open()
            sqlcmd.ExecuteNonQuery()
            conn.Close()
        End If

    End Sub

    Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect("../admin/holiday.aspx")
	
    End Sub



    Sub btndelete_Click(ByVal source As Object, ByVal e As System.EventArgs)
        Dim deletesql As String
        deletesql = "delete from holidaymaster where  holidaydate = '" & txtholidaydate.Text & "' and  LocationID ='" & dlLocation.SelectedValue & "'"  

        Dim deletecmd As SqlCommand = New SqlCommand(deletesql, conn)
        conn.Open()
        deletecmd.Connection = conn
        deletecmd.CommandText = deletesql
        deletecmd.ExecuteNonQuery()
        If (txtholidaydate.Text.ToString() <> "") Then
            LBLholidaydate.Text = "RECORD DELETED "
        End If
    
        
    End Sub



    Sub BindLocation()
      
      Dim dtEmployeeLocation As DataTable = New DataTable()
      dtEmployeeLocation = objCommon.EmployeeLocationList()
      dlLocation.DataSource = dtEmployeeLocation
      dlLocation.DataTextField = "Name"
      dlLocation.DataValueField = "LocationID"
      dlLocation.DataBind()
      Dim lstLocation As New ListItem("--Select--", "0")
      dlLocation.Items.Insert(0, lstLocation)

    End Sub
      
     

   

</script>

<body>

    <script language="javascript" src="/includes/CalendarControl.js" type="text/javascript">
    </script>

    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form id="Form1" method="post" runat="server">
        <table id="Table2" style="height: 1" cellspacing="0" cellpadding="2" width="100%"
            border="0">
            <tr>
                <td align="center">
                    <table id="Table1" bordercolor="#c5d5ae" cellspacing="0" cellpadding="4" border="1">
                        <tr>
                            <td bgcolor="#c5d5ae" colspan="4">
                                <b><font face="Verdana" color="#a2921e" size="2">Holiday Details</font></b></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td nowrap="nowrap"="nowrap="nowrap"" colspan="3">
                                <asp:TextBox ID="txtholidayid" runat="server" MaxLength="50" Visible="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap"="nowrap="nowrap"" bgcolor="#edf2e6">
                                <b><font face="Verdana" color="#a2921e" size="2">Location</font></b></td>
                            <td nowrap="nowrap"="nowrap="nowrap"" colspan="3">
                               
                                
                                  <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass ="b_dropdown" >
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="dlLocation"  InitialValue ="0"  Display ="Dynamic" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                             




                            </td>
                        </tr>

                        <tr>
                            <td nowrap="nowrap"="nowrap="nowrap"" bgcolor="#edf2e6">
                                <b><font face="Verdana" color="#a2921e" size="2">Holiday Date</font></b></td>
                            <td nowrap="nowrap"="nowrap="nowrap"" colspan="3">
                                <asp:TextBox ID="txtholidaydate" runat="server" MaxLength="55" onclick="popupCalender('txtholidaydate')"
                                    onkeypress="return false;"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtholidaydate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>

                            </td>
                        </tr>

                         

                        <tr>
                            <td nowrap="nowrap"="nowrap="nowrap"" bgcolor="#edf2e6">
                                <b><font face="Verdana" color="#a2921e" size="2">Narration </font></b>
                            </td>
                            <td nowrap="nowrap"="nowrap="nowrap"" colspan="3">
                                <asp:TextBox ID="txtholidaydesc" runat="server" MaxLength="355" Width="95%"></asp:TextBox>

                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtholidaydesc" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap="nowrap"="nowrap="nowrap"" align="center" width="75%" colspan="4" height="30" rowspan="2">
                                <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" Width="90px" BorderWidth="1px" 
                                    BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E" Text="Save"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" OnClick="btncancel_Click" runat="server" Width="90px"
                                    BorderWidth="1px" BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E"
                                    Text="Cancel"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btndelete" OnClick="btndelete_Click" runat="server" Width="90px"
                                    BorderWidth="1px" BackColor="#EDF2E6" BorderStyle="Groove" BorderColor="#A2921E"
                                    Text="Delete"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table align="center" id="Table4" cellspacing="0" cellpadding="4" border="0">
            <tr>
                <td align="center" colspan="4">
                    <b><font face="Verdana" color="#a2921e" size="2"></font></b>
                    <asp:Label ID="LBLholidaydate" runat="server" Width="100%" MaxLength="50">
                    </asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
