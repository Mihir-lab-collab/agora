<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control</title>

    <script language="VB" runat="server">
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            BindGrid()
        End Sub
    
       Sub BindGrid()
            Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
            Dim conn As SqlConnection = New SqlConnection(dsn1)
            Dim strSQL As String = ""
			
            If Request.QueryString("search") = "all" Then
                strSQL = "SELECT employeeMaster.empid,employeeMaster.empPassword,skillMaster.skillid,skillMaster.skillDesc," & _
                " employeeMaster.empName, employeeMaster.empAddress,employeeMaster.empContact, " & _
                "employeeMaster.empJoiningDate, employeeMaster.empLeavingDate, employeeMaster.empAccountNo, " & _
                "employeeMaster.empBDate,employeeMaster.empADate,employeeMaster.empPrevEmployer,employeeMaster.empTester, " & _
                "datediff(month,empJoiningdate,getdate())+ empExperince as empExperince, " & _
                "DateAdd(m,empprobationPeriod,empJoiningDate) as empProbationTill, " & _
                " ((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+ " & _
                "(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical) " & _
                "+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta) " & _
                " + (employeepaymaster.emppaybasic)*12/100 )as ctc,(employeepayMaster.emppaymonth) as emppayMonth, " & _
                " (employeepayMaster.emppayyear) as emppayYear,(SELECT TOP (1) empQualificationMaster.qualificationDesc FROM empQualification INNER JOIN empQualificationMaster ON empQualification.qualificationId = empQualificationMaster.qualificationId AND  empQualification.qualificationId = empQualificationMaster.qualificationId WHERE (empQualification.empId = employeepayMaster.empId)) as Qualification " & _
                "FROM employeeMaster inner join skillMaster on (employeeMaster.skillId=skillMaster.skillId)  left outer join " & _
                " employeepayMaster on (employeeMaster.empId=employeepayMaster.empId) "
				
            Else
                If Request.QueryString("search") = "" Or Request.QueryString("search") = "Active" Then
                    strSQL = "SELECT employeeMaster.empid,employeeMaster.empPassword,skillMaster.skillid,skillMaster.skillDesc," & _
                 " employeeMaster.empName, employeeMaster.empAddress,employeeMaster.empContact, " & _
                 "employeeMaster.empJoiningDate, employeeMaster.empLeavingDate, employeeMaster.empAccountNo, " & _
                 "employeeMaster.empBDate,employeeMaster.empADate,employeeMaster.empPrevEmployer,employeeMaster.empTester, " & _
                 "datediff(month,empJoiningdate,getdate())+ empExperince as empExperince, " & _
                 "DateAdd(m,empprobationPeriod,empJoiningDate) as empProbationTill, " & _
                 "((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+ " & _
                 "(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical) " & _
                 "+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta) " & _
                 " + (employeepaymaster.emppaybasic)*12/100 )as ctc,(employeepayMaster.emppaymonth) as emppayMonth, " & _
                  " (employeepayMaster.emppayyear) as emppayYear ,(SELECT TOP (1) empQualificationMaster.qualificationDesc FROM empQualification INNER JOIN empQualificationMaster ON empQualification.qualificationId = empQualificationMaster.qualificationId AND  empQualification.qualificationId = empQualificationMaster.qualificationId WHERE (empQualification.empId = employeeMaster.empId)) as Qualification " & _
                 "FROM employeeMaster inner join skillMaster on (employeeMaster.skillId=skillMaster.skillId) and  (employeeMaster.empLeavingdate is null) left outer join " & _
                 " employeepayMaster on (employeeMaster.empId=employeepayMaster.empId) "
                Else
				
                    If Request.QueryString("search") = "Inactive" Then
				   
                        strSQL = "SELECT employeeMaster.empid,employeeMaster.empPassword,skillMaster.skillid,skillMaster.skillDesc," & _
                     " employeeMaster.empName, employeeMaster.empAddress,employeeMaster.empContact, " & _
                     "employeeMaster.empJoiningDate, employeeMaster.empLeavingDate, employeeMaster.empAccountNo, " & _
                     "employeeMaster.empBDate,employeeMaster.empADate,employeeMaster.empPrevEmployer,employeeMaster.empTester, " & _
                     "datediff(month,empJoiningdate,getdate())+ empExperince as empExperince, " & _
                     "DateAdd(m,empprobationPeriod,empJoiningDate) as empProbationTill, " & _
                     "((employeepaymaster.emppaybasic)+(employeepaymaster.emppayhra)+ " & _
                     "(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical) " & _
                     "+(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta) " & _
                     " + (employeepaymaster.emppaybasic)*12/100 )as ctc,(employeepayMaster.emppaymonth) as emppayMonth, " & _
                       "(employeepayMaster.emppayyear) as emppayYear,(SELECT TOP (1) empQualificationMaster.qualificationDesc FROM empQualification INNER JOIN empQualificationMaster ON empQualification.qualificationId = empQualificationMaster.qualificationId AND  empQualification.qualificationId = empQualificationMaster.qualificationId WHERE (empQualification.empId = employeepayMaster.empId)) as Qualification " & _
                     "FROM employeeMaster inner join skillMaster on (employeeMaster.skillId=skillMaster.skillId) and  (employeeMaster.empLeavingdate is not null) left outer join " & _
                     " employeepayMaster on (employeeMaster.empId=employeepayMaster.empId)"
				   
			   
                    End If
                End If
            End If
            conn.Open()
           ' Response.Write(strSQL)
            'Response.End()
            
            
            Dim cmd As SqlCommand = New SqlCommand(strSQL, conn)
            Dim Rdr As SqlDataReader
            Rdr = cmd.ExecuteReader()
            MyDataGrid.DataSource = Rdr
            MyDataGrid.DataBind()
            Dim a As Integer
            Dim s As Integer
            Dim sr As Integer
            Dim item As DataGridItem
            For Each item In MyDataGrid.Items
                a = item.Cells(8).Text
                s = Math.Floor(a / 12)
                sr = a Mod 12
                item.Cells(8).Text = s & " years " & sr & " months "
            Next
        End Sub
        Private Sub MyDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
			
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim cellValue As Integer = DataBinder.Eval(e.Item.DataItem, "empId") & ""
                Dim str As String
                str = "select projectmaster.projname from projectmember,projectmaster where projectmember.empid=" & cellValue & " and projectmember.projid=projectmaster.projid"
                'response.write(str)
                'response.end
                Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
                Dim conn As SqlConnection = New SqlConnection(dsn1)
                conn.Open()
                Dim st As String
                Dim Rdr As SqlDataReader
                Dim cmd As SqlCommand = New SqlCommand(str, conn)
                Rdr = cmd.ExecuteReader()
                Dim i As Integer
                st = ""
                While Rdr.Read
                    If st = "" Then
                        st = "- " & Rdr(0)
                    Else
                        st = st & "<BR>- " & Rdr(0)
                    End If
                    i = i + 1
                End While
                e.Item.Cells(13).Text = st
                Rdr.Close()
                Dim strcodereview As String
                Dim sk = e.Item.Cells(0)
                strcodereview = "select distinct projname from projectmaster where codeRevteam like '%" & cellValue & "' or codeRevteam like  '%" & cellValue & "%' or codeRevteam like '" & cellValue & "%' "
                Dim stcodereview As String
                Dim rdeCDT As SqlDataReader
                cmd = New SqlCommand(strcodereview, conn)
                rdeCDT = cmd.ExecuteReader()
                Dim j As Integer
                stcodereview = ""
                While rdeCDT.Read
                    If stcodereview = "" Then
                        stcodereview = "- " & rdeCDT(0)
                    Else
                        stcodereview = stcodereview & "<BR>- " & rdeCDT(0)
                    End If
                    j = j + 1
                End While
                e.Item.Cells(14).Text = stcodereview
            End If
        End Sub
	 
        Function getRevDate(ByVal monthStr, ByVal yearStr) As String
            Dim strDate As String = ""
            If IsNumeric(monthStr) And IsNumeric(yearStr) Then
                strDate = Left(MonthName(monthStr), 3) & " " & yearStr
            End If
            Return strDate
        End Function
    </script>

</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form runat="server">
    <table cellpadding="4" ="100%" border="1" style="border-collapse: collapse" bordercolor="#e8e8e8">
        <tr>
            <td colspan="4" align="right" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2">
                    <p align="left">
                        <a href="empView.aspx?search=Active"><font color="#A2921E">Active</font></a>|<a href="empView.aspx?search=Inactive"><font
                            color="#A2921E">Inactive</font></a>|<a href="empView.aspx?search=all"><font color="#A2921E">All</font></a>
                        | <a href="empDetail.aspx"><font color="#A2921E">Add New</font></a>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="litExc" runat="server" />
                <asp:DataGrid ID="MyDataGrid" runat="server" AllowSorting="True" BackColor="White"
                    BorderColor="Black" ShowFooter="True" CellPadding="2" Font-Name="Verdana" Font-Size="10pt"
                    HeaderStyle-BackColor="lightblue" HeaderStyle-Font-Size="11pt" MaintainState="true"
                    AutoGenerateColumns="False" Font-Names="Verdana" OnItemDataBound="MyDataGrid_ItemDataBound">
                    <HeaderStyle Font-Size="11pt" BackColor="#edf2e6" ForeColor="#A2921E" Font-Bold="True">
                    </HeaderStyle>
                    <AlternatingItemStyle BackColor="#edf2e6"></AlternatingItemStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="Sr.">
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="empId" HeaderText="Emp Number">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:HyperLinkColumn DataNavigateUrlField="empId" DataNavigateUrlFormatString="empDetail.aspx?empid={0}"
                            DataTextField="empName" HeaderText="Emp Name"></asp:HyperLinkColumn>
                        <asp:BoundColumn DataField="skillDesc" HeaderText="Designation">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empTester" HeaderText="Tester" Visible="false" FooterStyle-Wrap="true">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empJoiningDate" HeaderText="Joining Date" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empProbationTill" HeaderText="Probation Till" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empLeavingDate" HeaderText="Leaving Date" DataFormatString="{0:dd-MMM-yyyy}"
                            Visible="false">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empExperince" HeaderText="Experience">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empBDate" HeaderText="Birth Date" DataFormatString="{0:dd-MMM-yyyy}">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="empADate" HeaderText="Anniversary Date" DataFormatString="{0:dd-MMM-yyyy}"
                            Visible="false">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="ctc" HeaderText="CTC (INR)" DataFormatString="{0:##,###,###}">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Revision Month">
                            <ItemTemplate>
                                <%# getRevDate(Container.dataitem("emppayMonth"), Container.dataitem("emppayYear"))%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Developer">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Code Review">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                                <asp:BoundColumn DataField="Qualification" HeaderText="Qualification">
                            <HeaderStyle Font-Bold="True" ForeColor="#a2921e"></HeaderStyle>
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
