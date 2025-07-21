Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net.Mail
Imports System.Web.Mail

Partial Class emp_addKnowledge
    Inherits Authentication
    Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As New SqlConnection(dsn1)
    Dim cmd As New SqlCommand
    Dim strSql As String
    Dim strbody As String
    Dim fileContent As String
    Dim documentPath As String
    Dim DynamicFileReader As System.IO.StreamReader
    Dim mailTo As String = ConfigurationSettings.AppSettings("commonEmail").ToString()
    Dim mailFrom As String = ConfigurationSettings.AppSettings("fromEmail").ToString()
    Dim gf As New generalFunction
    Dim TargetPath As String = ConfigurationSettings.AppSettings("DataBank").ToString() + "/KB/"

    Public Function repalceHTML(ByVal str As String) As String
        Dim tmpStr As String
        tmpStr = str.Replace(">", "&gt;")
        tmpStr = tmpStr.Replace("<", "&lt;")
        repalceHTML = tmpStr
    End Function
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim strFlName As String = ""

        If Directory.Exists(TargetPath) = False Then
            Directory.CreateDirectory(TargetPath)
        End If

        If (fileAdd.Value <> "") Then
            Dim strfilepath, strfileext, strFileName, strFName As String
            Dim len As Integer
            strfilepath = fileAdd.PostedFile.FileName
            len = strfilepath.LastIndexOf(".")
            strfileext = strfilepath.Substring(len)
            ' only the attched file name not its path
            strFileName = System.IO.Path.GetFileName(strfilepath)
            strFName = Trim(Replace(strFileName, strfileext, "")) & "_" & Day(Now()) & Month(Now()) & Right(Year(Now()), 2) & Hour(Now()) & Minute(Now()) & Second(Now())
            fileAdd.PostedFile.SaveAs(TargetPath & strFName & strfileext)
            strFlName = Replace(strFName, "'", "''") & strfileext
            documentPath = TargetPath & strFlName
        ElseIf hlnkFile.Text <> "" Then
            strFlName = hlnkFile.Text
            documentPath = TargetPath & strFlName
        Else
            strFlName = ""
            documentPath = ""
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

            DynamicFileReader = IO.File.OpenText(Server.MapPath("..\\mailTemplates\\kbInsertTemplate.htm"))
            fileContent = DynamicFileReader.ReadToEnd()
            fileContent = fileContent.Replace("{title}", repalceHTML(txtTitle.Text))
            fileContent = fileContent.Replace("{description}", repalceHTML(txtDescr.Text))
            fileContent = fileContent.Replace("{comment}", repalceHTML(txtComment.Text))
            gf.SendEmail(fileContent, "From DWT KB.", mailTo, mailFrom, documentPath)

            Response.Redirect("knowledgebase.aspx")
        Catch ex As Exception
            Response.Write(ex.Message)
            Response.End()
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        If IsNumeric(Session("dynoEmpIdSession")) Then
            txtCommentHistory.BackColor = Drawing.Color.MintCream
            'btndelete.Attributes.Add("onclick", "Javascript:return confirm('Are you sure to delete this from knowledge base ?')")
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
                hlnkFile.NavigateUrl = "/Common/Download.aspx?m=KB&f=" + ds.Tables(0).Rows(0).Item("kbFile")
                hlnkFile.Text = ds.Tables(0).Rows(0).Item("kbFile")
                If hlnkFile.Text <> "" Then
                    viewFile.Visible = "true"
                Else
                    viewFile.Visible = False
                End If
            End If
        End If
    End Sub


    Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.Click
        Response.Redirect("knowledgebase.aspx")
    End Sub
    Sub btndelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndelete.Click
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




End Class
