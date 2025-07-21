Imports System.Data
Imports System.Data.SqlClient
Partial Class emp_candSchedule
    Inherits Authentication
    Dim dsn As String
    Dim strconn As SqlConnection
    Dim dsddlststus As New DataSet
    Dim dadddlststus As SqlDataAdapter
    Dim objcmd As SqlCommand
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        dsn = ConfigurationManager.ConnectionStrings("conString").ToString()
        strconn = New System.Data.SqlClient.SqlConnection(dsn)
        If Not IsPostBack Then
            showstatus()
            showname()
        End If
    End Sub

    Sub showstatus()
        Dim strsqlstatus As String
        strsqlstatus = "select * from candStatusMaster"
        dadddlststus = New SqlDataAdapter(strsqlstatus, strconn)
        dadddlststus.Fill(dsddlststus)
        ddlstatus.DataSource = dsddlststus
        ddlstatus.DataTextField = "statusDesc"
        ddlstatus.DataValueField = "statusId"
        ddlstatus.DataBind()
    End Sub
    Sub showname()
        Dim strsqlname As String
        strsqlname = "select (candFName  + ' ' + candMName + ' ' +  candLName) as candname from candidateMaster where candId= " & Request.QueryString("id")
        strconn.Open()
        objcmd = New SqlCommand(strsqlname, strconn)
        Dim objdatareader As SqlDataReader
        objdatareader = objcmd.ExecuteReader
        If objdatareader.Read() Then
            lblName.Text = objdatareader("candname")
        End If
        strconn.Close()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim candidateid As Integer
        candidateid = Request.QueryString("id")
        Dim strsqlinsertdatail As String
        strsqlinsertdatail = "insert into candSchDetail (candId,statusId,schdate,schComment) values " & _
                            "(" & candidateid & "," & ddlstatus.SelectedValue & "," & txtdate.Text & ",'" & txtcomment.Text & "')"
        strconn.Open()
        objcmd = New SqlCommand(strsqlinsertdatail, strconn)
        objcmd.ExecuteNonQuery()

        Dim sp As String
        sp = "<script language='JavaScript'>"
        sp += "alert('Record has been added successfully.');"
        sp += "</" + "script>"
        Response.Write(sp)
        strconn.Close()
    End Sub
End Class
