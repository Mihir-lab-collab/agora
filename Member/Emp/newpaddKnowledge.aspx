<%@ Page Language="vb" Debug="True" ValidateRequest="false" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%--<%@Import Namespace="DWT_COMMON"%>--%>
<%@ Import Namespace="System.IO" %>
<%@ Register TagPrefix="uc1" TagName="empMenuBar" Src="~/controls/empMenuBar.ascx" %>
<%@ Register TagPrefix="EMPHEADER" TagName="empHeader" Src="~/controls/empHeader.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Add Knowledge </title>

    <script language="JavaScript" src="/includes/calender.js"></script>

    <script language="vb" runat="server">
	
        Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
        Dim conn As SqlConnection = New SqlConnection(dsn1)
        Dim cmd As New SqlCommand
        Dim strSql As String
        Dim strbody As String
        Dim mailTo As String = ConfigurationSettings.AppSettings("commonEmail").ToString()
        Dim mailFrom As String = ConfigurationSettings.AppSettings("fromEmail").ToString()
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            If IsNumeric(Session("dynoEmpIdSession")) Then
			
                txtCommentHistory.BackColor = Drawing.Color.MintCream
                btndelete.Attributes.Add("onclick", "Javascript:return confirm('Are you sure to delete this from knowledge base ?')")
                If Not IsPostBack Then
			
			
                    If Request.QueryString("details") = "true" Then

                        Call bindData()

                    Else

                        viewFile.Visible = "false"

                    End If
                End If
		
            Else

                Response.Redirect("../emp/emplogin.aspx")

            End If
	
        End Sub

        Function Tohtml(ByVal strValue)
            Tohtml = Server.HtmlEncode(strValue)
        End Function

        Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Dim strFlName As String



            If (fileAdd.Value <> "") Then
                Dim strfilepath, strfileext, strFileName, strFName As String
                Dim len As Integer
                strfilepath = fileAdd.PostedFile.FileName
                len = strfilepath.LastIndexOf(".")
                strfileext = strfilepath.Substring(len)
											   
                ' only the attched file name not its path
                strFileName = System.IO.Path.GetFileName(strfilepath)
                strFName = Trim(Replace(strFileName, strfileext, "")) & "_" & Day(Now()) & Month(Now()) & Right(Year(Now()), 2) & Hour(Now()) & Minute(Now()) & Second(Now())
                fileAdd.PostedFile.SaveAs(Server.MapPath("/knowledgeBase/") & strFName & strfileext)
			
                strFlName = Replace(strFName, "'", "''") & strfileext

            ElseIf hlnkFile.Text <> "" Then
			
                strFlName = hlnkFile.Text
		
            Else

                strFlName = ""

            End If

            Dim title, desc, comment As String
            title = Replace(txtTitle.Text, "'", "''")
            desc = Replace(txtDescr.Text, "'", "''")

            If txtComment.Text <> "" Then
                comment = Replace(txtComment.Text, "'", "''")
            Else
                comment = ""
            End If

            Try
			
                If Request.QueryString("details") <> "true" Then
			
                    strSql = "insert into knowledgeBase(empId,kbTitle,kbDescrptn,kbComments,kbFile)values (" & Session("dynoEmpIdSession") & ",'" & title & "','" & desc & "','" & comment & "','" & strFlName & "')"
			
                ElseIf Request.QueryString("details") = "true" Then

                    Dim strComment As String
                    Dim strEmpName As String
                    Dim ds As New DataSet
                    Dim da As New SqlDataAdapter

                    strSql = "SELECT empName FROM employeeMaster where empid=" & Session("dynoEmpIdSession")
                    conn.Open()
                    da = New SqlDataAdapter(strSql, conn)
                    da.Fill(ds, "emp")

                    If ds.Tables(0).Rows.Count > 0 Then
                        strEmpName = ds.Tables(0).Rows(0).Item("empName")
                    End If

                    conn.Close()

					
					

                    If commHis.Value <> "" Then
											
                        strComment = strEmpName & ":" & Left(Now, 15) & ": " & comment & vbCrLf & _
                        "----------------------------------------------" & vbCrLf & commHis.Value & vbCrLf
                    Else
                        strComment = strEmpName & ":" & Left(Now, 15) & ": " & comment & vbCrLf & _
                        "----------------------------------------------"
                    End If

                    If txtComment.Text <> "" Then

                        strSql = "UPDATE knowledgeBase SET kbComments='" & strComment & "',kbTitle='" & title & "',kbDescrptn='" & desc & "',kbFile='" & strFlName & "' Where kbId=" & Request.QueryString("kbId")
					

                    Else
                        strSql = "UPDATE knowledgeBase SET kbTitle='" & title & "',kbDescrptn='" & desc & "',kbFile='" & strFlName & "' Where kbId=" & Request.QueryString("kbId")
					
                    End If


				
					
			
                End If

			

                conn.Open()
                cmd = New SqlCommand(strSql, conn)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                conn.Close()
		
                Response.Redirect("knowledgebase.aspx")

            Catch ex As Exception

                Response.Write(ex.Message)

            End Try


        End Sub

        Public Sub bindData()
	
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet

            Dim kbId As Integer
            kbId = Request.QueryString("kbId")

            strSql = "Select * From knowledgeBase where kbId=" & kbId

            da = New SqlDataAdapter(strSql, conn)
            da.Fill(ds, "kb")

            If ds.Tables(0).Rows.Count > 0 Then
			
                If ds.Tables(0).Rows(0).Item("empid") = Session("dynoEmpIdSession") Then
			
                    btndelete.Visible = True
                    addFile.Visible = "true"
                    txtTitle.ReadOnly = "false"
                    txtDescr.ReadOnly = "false"

                ElseIf Session("dynoAdminSession") = 1 Then

                    btndelete.Visible = True
			

                Else
			
                    btndelete.Visible = False
                    addFile.Visible = "false"
                    txtTitle.ReadOnly = "True"
                    txtDescr.ReadOnly = "True"
                    txtTitle.BackColor = Drawing.Color.MintCream
                    txtDescr.BackColor = Drawing.Color.MintCream
                End If
		
                txtTitle.Text = Replace(ds.Tables(0).Rows(0).Item("kbTitle"), "''", "'")

                txtDescr.Text = Replace(ds.Tables(0).Rows(0).Item("kbDescrptn"), "''", "'")

                txtCommentHistory.Text = Replace(ds.Tables(0).Rows(0).Item("kbComments"), "''", "'")
		
                commHis.Value = txtCommentHistory.Text

                'txtempName.value=ds.tables(0).rows(0).item("empName")

                If ds.Tables(0).Rows(0).Item("kbFile") <> "" Then
					
                    Dim path As String
                    path = Request.ServerVariables("servername") & "/knowledgeBase/"

                    Dim file As String
                    file = ds.Tables(0).Rows(0).Item("kbFile")
                    Dim fileName As String
                    fileName = path & file
                    hlnkFile.NavigateUrl = fileName
                    hlnkFile.Text = Mid(file, InStrRev(file, "\") + 1, Len(file))
                    If hlnkFile.Text <> "" Then
						
                        viewFile.Visible = "true"
                    Else
                        viewFile.Visible = False
                    End If
					
                End If

            End If


        End Sub





        Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

            Response.Redirect("knowledgebase.aspx")

        End Sub

        Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
	
            Dim kbId As Integer
            kbId = Request.QueryString("kbId")
            strSql = "Delete from knowledgeBase where kbId='" & kbId & "'"
            conn.Open()
            cmd = New SqlCommand(strSql, conn)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            conn.Close()
            Response.Redirect("knowledgebase.aspx")

        End Sub

    </script>

</head>
<body>
    <form id="Form1" method="post" runat="server">
    <table valign="top" id="Table3" border="0" cellspacing="0" cellpadding="3"" " width="100%"
        align="center" border="0">
        <tr valign="top">
            <td>
                <EMPHEADER:empHeader ID="Empheader" runat="server"></EMPHEADER:empHeader>
            </td>
        </tr>
        <tr>
            <td>
                <uc1:empMenuBar ID="EmpMenuBar" runat="server"></uc1:empMenuBar>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table border="0" align="center">
                    <tr>
                        <td align="center" bgcolor="#C5D5AE" colspan="3">
                            <a name="Bugs"><font face="Verdana" color="#a2921e"><b>Knowledge Base</b></font></a>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width: 949px">
                            <asp:Label ID="lblMsg" Style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica"
                                runat="server" Font-Bold="True" ForeColor="Red" Visible="False">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table border="1" style="border-collapse: collapse; border-color: black" cellpadding="2"
                                cellspacing="5">
                                <tr>
                                    <td align="left" width="138" bgcolor="#EDF2E6">
                                        <font color="#A2921E" style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica">
                                            <strong>Title*</strong></font>
                                    </td>
                                    <td width="10">
                                        <strong>:</strong>
                                    </td>
                                    <td align="left" width="649">
                                        <asp:TextBox ID="txtTitle" runat="server" Width="300px">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="valrTitle" runat="server" ControlToValidate="txtTitle"
                                            Display="Dynamic" ErrorMessage="Please enter Title">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="138" bgcolor="#EDF2E6">
                                        <p align="left">
                                            <font color="#A2921E" style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica">
                                                <strong>Description*</strong></font>
                                    </td>
                                    <td style="width: 10">
                                        <strong>:</strong>
                                    </td>
                                    <td align="left" style="width: 649">
                                        <asp:TextBox ID="txtDescr" runat="server" TextMode="multiline" Height="200px" Width="400px">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="valrDesc" runat="server" ControlToValidate="txtDescr"
                                            Display="Dynamic" ErrorMessage="Please enter Description">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr id="viewFile" runat="server">
                                    <td align="right" width="138" bgcolor="#EDF2E6">
                                        <p align="left">
                                            <font color="#A2921E" style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica">
                                                <b>View File</b> </font>
                                    </td>
                                    <td style="width: 10">
                                        <strong>:</strong>
                                    </td>
                                    <td align="left" style="width: 649">
                                        <asp:HyperLink ID="hlnkFile" runat="server"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr id="addFile" runat="server">
                                    <td align="right" width="138" bgcolor="#EDF2E6">
                                        <p align="left">
                                            <font color="#A2921E" style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica">
                                                <b>Add File</b> </font>
                                    </td>
                                    <td style="width: 10">
                                        <strong>:</strong>
                                    </td>
                                    <td align="left" style="width: 649">
                                        <input id="fileAdd" type="file" runat="server" size="20">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" width="138" bgcolor="#EDF2E6">
                                        <p align="left">
                                            <font color="#A2921E" style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica">
                                                <b>Comments</b> </font>
                                            <td style="width: 10">
                                                <strong>:</strong>
                                            </td>
                                            <td align="left" style="width: 649">
                                                <input id="txtempName" type="hidden" runat="server">
                                                <asp:TextBox ID="txtComment" runat="server" TextMode="multiline" Height="100px" Width="400px">
                                                </asp:TextBox>
                                            </td>
                                </tr>
                                <td align="right" width="138" bgcolor="#EDF2E6">
                                    <p align="left">
                                        <font color="#A2921E" style="font-size: 10pt; font-family: Arial, Tahoma, Verdana, Helvetica">
                                            <b>Comment History</b></font>
                                        <td style="width: 10">
                                            <strong>:</strong>
                                        </td>
                                        <td align="left" style="width: 649">
                                            <input id="commHis" type="hidden" runat="server">
                                            <asp:TextBox ID="txtCommentHistory" runat="server" TextMode="multiline" Height="100px"
                                                Width="400px" ReadOnly>
                                            </asp:TextBox>
                                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="left" style="width: 649; height: 45px">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3" width="786">
                            <asp:Button ID="btnSubmit" Style="font-family: Verdana; font-size: 8pt; color: #A2921E;
                                font-weight: bold; background-color: #C5D5AE" runat="server" Text="Submit" ToolTip="Click here to submit the Knowledge"
                                OnClick="btnSubmit_Click" />&nbsp;
                            <asp:Button ID="btncancel" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                color: #A2921E; font-weight: bold; background-color: #C5D5AE" CausesValidation="false"
                                OnClick="btncancel_Click" Text="Cancel" />
                            <asp:Button ID="btndelete" runat="server" Style="font-family: Verdana; font-size: 8pt;
                                color: #A2921E; font-weight: bold; background-color: #C5D5AE" CausesValidation="false"
                                OnClick="btndelete_Click" Text="Delete" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </td> </tr> </table>
    </form>
</body>
</html>
