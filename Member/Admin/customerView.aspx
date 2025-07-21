<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<%@ Page Language="VB" %>

<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>

<%
    If Request("logout") = "true" Then
        Session.Abandon()
        Response.Redirect("/emp/empHome.aspx")
    End If
%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control</title>

    <script runat="server">
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            If Not IsPostBack Then
                BindGrid()
            End If
        End Sub
    
        Sub BindGrid()
            Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            Dim strSQL As String

            strSQL = "SELECT * FROM customerMaster ORDER BY custId"
            
            conn.Open()
            Dim cmd As SqlCommand = New SqlCommand(strSQL, conn)
            Dim Rdr As SqlDataReader
            Rdr = cmd.ExecuteReader()
            MyDataGrid.DataSource = Rdr
            MyDataGrid.DataBind()
        End Sub

    </script>

</head>
<body>
    <table cellpadding="4" width="100%">
        <tr>
            <td colspan="5">
                <uc1:adminMenu ID="AdminMenu1" runat="server" />
            </td>
        </tr>
        <tr bgcolor="#edf2e6">
            <td align="right" colspan="5" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2"><a href="/admin/custDetail.aspx"><font color="#A2921E">
                    Add Customer</font></a></b>
            </td>
        </tr>
    </table>
    <form runat="server">
        <asp:Literal ID="litExc" runat="server"></asp:Literal>
        <asp:DataGrid ID="MyDataGrid" runat="server" AutoGenerateColumns="False" HeaderStyle-Font-Size="11pt"
            HeaderStyle-BackColor="#edf2e6" Font-Size="10pt" Font-Name="Verdana" HeaderStyle-ForeColor="#A2921E"
            CellPadding="2" ShowFooter="True" HeaderStyle-Font-Bold="True" BorderColor="Black"
            BackColor="White" Width="100%" AllowSorting="True" Font-Names="Verdana">
            <HeaderStyle Font-Size="11pt" BackColor="#edf2e6"></HeaderStyle>
            <AlternatingItemStyle BackColor="#edf2e6"></AlternatingItemStyle>
            <Columns>
                <asp:HyperLinkColumn DataNavigateUrlField="custId" DataNavigateUrlFormatString="custDetail.aspx?custid={0}"
                    DataTextField="custName" HeaderText="Name"></asp:HyperLinkColumn>
                <asp:BoundColumn DataField="CustCompany" HeaderText="Organisation"></asp:BoundColumn>
                <asp:BoundColumn DataField="custEmail" HeaderText="Contact Email"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
    </form>
</body>
</html>
