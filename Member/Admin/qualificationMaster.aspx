<%@ Page Language="VB" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control </title>

    <script language="VB" runat="server">
        Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
        Dim conn As SqlConnection = New SqlConnection(dsn1)
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            If Not IsPostBack Then 'bind for first time page load only
                BindGrid()
            End If
	 
        End Sub
    
        Sub BindGrid()
            Dim strSQL As String
            strSQL = "select * from empQualificationMaster"
            conn.Open()
            Dim cmdQualification As SqlCommand = New SqlCommand(strSQL, conn)
            Dim dtrQualification As SqlDataReader
            dtrQualification = cmdQualification.ExecuteReader()
            dgrdQualification.DataSource = dtrQualification
            dgrdQualification.DataBind()
            conn.Close()
            qualification.Value = ""
            qualificationType.SelectedIndex = 0
        End Sub
	
        Sub addNew_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
            Dim strSQL1 As String
            Try
                strSQL1 = "insert into empQualificationMaster(qualificationDesc,qualificationType) values('" & qualification.Value & "','" & qualificationType.Value & "')"
                conn.Open()
                Dim cmdQual As SqlCommand = New SqlCommand(strSQL1, conn)
                cmdQual.ExecuteNonQuery()
                conn.Close()
                BindGrid()
            Catch ex As Exception
                Dim sp As String
                sp = "<script language='JavaScript'>"
                sp += "alert('Qualification All ready Exist!!');"
                sp += "</" + "script>"
                ClientScript.RegisterStartupScript(Me.GetType, "script1", sp)
            End Try
        End Sub
	
        Sub dgrdEdit(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
            dgrdQualification.EditItemIndex = e.Item.ItemIndex
            BindGrid()
        End Sub
	
        Private Sub dgrdCancel(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
            dgrdQualification.EditItemIndex = -1
            BindGrid()
        End Sub

        Private Sub dgrdUpdate(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
            Dim sp As String
            Dim strQualDesc, strSql As String
            Dim intQual, index As Integer
            index = e.Item.ItemIndex
            Dim str1 As String
            Dim strQualType As DropDownList
            strQualDesc = CType(e.Item.Cells(1).Controls(0), TextBox).Text
            strQualType = CType(e.Item.Cells(2).Controls(1), DropDownList)
            str1 = strQualType.SelectedValue
            'intQual = dgrdQualification.Items(e.Item.ItemIndex).Cells(0).Text
            intQual = dgrdQualification.DataKeys(index)
            If strQualDesc <> "" Then
                strSql = "select * from empQualificationMaster where qualificationDesc='" & strQualDesc & "'"
                Dim cmdQual1 As SqlCommand = New SqlCommand(strSql, conn)
                conn.Open()
                Dim dtrQual As SqlDataReader = cmdQual1.ExecuteReader()
		
                If dtrQual.Read Then
	         
                    sp = "<script language='JavaScript'>"
                    sp += "alert('Qualification All ready Exist!!');"
                    sp += "</" + "script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "script1", sp)

                Else
                    dtrQual.Close()
                    conn.Close()
                    strSql = "UPDATE empQualificationMaster set qualificationDesc='" & strQualDesc & "',qualificationType='" & str1 & "' WHERE qualificationId=" & intQual & ""
                    Dim cmdQual2 As SqlCommand = New SqlCommand(strSql, conn)
                    conn.Open()
                    cmdQual2.Executenonquery()
                    conn.Close()
                    dgrdQualification.EditItemIndex = -1
                    BindGrid()
                End If
            End If
        End Sub
    </script>

</head>
<body>
    <ucl:adminMenu ID="adminMenu" runat="server" />
    <form runat="server">
        <table cellpadding="4" width="60%" border="1" style="border-collapse: collapse" bordercolor="#e8e8e8"
            bordercolorlight="#e8e8e8" bordercolordark="#e8e8e8" align="center">
            <tr>
                <td height="23" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Qualification</b></font></td>
                <td colspan="4" height="23">
                    <input type="text" id="qualification" runat="server" size="20" />&nbsp;</td>
                <td height="23" bgcolor="#edf2e6">
                    <font face="Verdana" color="#a2921e" size="2"><b>Type</b></font></td>
                <td height="23">
                    <select runat="server" id="qualificationType" name="qualificationType">
                        <option value="Under Graduation">Under Graduation</option>
                        <option value="Graduation">Graduation</option>
                        <option value="Post Graduation">Post Graduation</option>
                    </select>
                </td>
                <td height="23">
                    <input type="button" id="addNew" runat="server" value="Add New" align="right" style="font-family: Verdana;
                        font-size: 8pt; color: #A2921E; font-weight: bold; background-color: #C5D5AE"
                        font-bold="true" onserverclick="addNew_OnClick" />
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td align="center">
                    <div id="error" runat="server">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="red"></asp:Label></div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Literal ID="litExc" runat="server" />
                <tr>
                    <td>
                        <tr align="center">
                            <td>
                                <asp:DataGrid ID="dgrdQualification" runat="server" AllowSorting="True" Width="50%"
                                    align="center" BackColor="White" BorderColor="Black" ShowFooter="True" CellPadding="2"
                                    Font-Name="Verdana" Font-Size="10pt" HeaderStyle-BackColor="lightblue" HeaderStyle-Font-Size="11pt"
                                    MaintainState="true" AutoGenerateColumns="False" Font-Names="Verdana" OnEditCommand="dgrdEdit"
                                    OnCancelCommand="dgrdCancel" OnUpdateCommand="dgrdUpdate" DataKeyField="qualificationId">
                                    <HeaderStyle Font-Size="11pt" BackColor="#edf2e6" ForeColor="#A2921E" Font-Bold="True">
                                    </HeaderStyle>
                                    <AlternatingItemStyle BackColor="#edf2e6"></AlternatingItemStyle>
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Sr.">
                                            <ItemTemplate>
                                                <%# Container.ItemIndex+1%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="qualificationDesc" HeaderText="Qualification"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Type">
                                            <ItemTemplate>
                                                <%# container.dataitem("qualificationType") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="drop" runat="server">
                                                    <asp:ListItem Value="Under Graduation"></asp:ListItem>
                                                    <asp:ListItem Value="Graduation"></asp:ListItem>
                                                    <asp:ListItem Value="Post Graduation"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
                                            EditText="Edit" ItemStyle-BackColor="#edf2e6" ItemStyle-ForeColor="#A2921E"></asp:EditCommandColumn>
                                    </Columns>
                                </asp:DataGrid>
                        </tr>
                    </td>
        </table>
    </form>
</body>
</html>
