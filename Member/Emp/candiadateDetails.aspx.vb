Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.File
Partial Class emp_candiadateDetails
    Inherits Authentication
    Dim dsn As String
    Dim strFileContains As String
    Dim Cmdlist As New SqlCommand
    Dim rdrlista As SqlDataReader
    Dim strConn As SqlConnection
    Dim sqlAdap As SqlDataAdapter
    Dim ds As DataSet
    '  Dim ID As Integer
    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        dsn = ConfigurationManager.ConnectionStrings("conString").ToString()
        ddlcity.Attributes.Add("onchange", "funDisplay()")
        strConn = New System.Data.SqlClient.SqlConnection(dsn)

        If Not IsPostBack Then
            loadData()
            fillcity()
            fillSkill()
            fillStatus()
        End If

    End Sub
    Public Sub saveFileContaimns(ByVal filePath As String)
        '--
        ' ''  Dim oFile As File
        ' ''  Dim strFileReader As StreamReader
  


        ' ''  Dim strPath As String = ""
        ' ''  Dim fleDReader As New FileStream("C:\\sandy.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
        ''Dim stmDReader As New StreamReader("C:\\sandy.txt")
        ' '' Using stmDReader As StreamReader = New StreamReader("C:\\sandy.txt")

        ''Dim strNotePadData As String = stmDReader.ReadToEnd()


        ' '' FileStream fleDReader = new FileStream("filename", FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.ReadWrite);
        ' ''StreamReader stmDReader = new StreamReader(fleDReader);
        ' '' strNotePadData = stmDReader.ReadToEnd();
        ''stmDReader.Close()
        ''Response.Write(strNotePadData + "sa")
        ' ''  End Using

        ' ''strFileReader = oFile.OpenText("C:\Documents and Settings\Sandeep\Desktop\resume.doc")
        ' ''strFileContains = strFileReader.ReadToEnd()
        ' ''strFileReader.Close()

        ' '' Response.Write(strFileContains + "sA")

        '--
    End Sub
    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim strsqlemail As String
        ' strsqlemail = "select count(*) as coun from candidateMaster where candemail='" & txtemail.Text & " '"
        strsqlemail = "select count(*) as coun from candidateMaster where candfname='" & txtFname.Text & "' and candMName='" & txtMname.Text & "' and candLname='" & txtLname.Text & "' "
        strsqlemail = strsqlemail + " and DATEDIFF(D,candbirthdate, '" + txtdob.Text + "') = 0 "
        sqlAdap = New SqlDataAdapter(strsqlemail, strConn)
        ds = New DataSet
        sqlAdap.Fill(ds)
      
        If ds.Tables(0).Rows(0).Item("coun") > 0 Then
            Dim spemail As String
            spemail = "<script language='JavaScript'>"
            spemail += "alert('This user already registered.');"
            spemail += "</" + "script>"
            Response.Write(spemail)
            Exit Sub
        End If



        Dim cityname As String
        Dim strFullFileName As String = fleUploadResumeDetails.PostedFile.FileName
        ' saveFileContaimns(strFullFileName)
        Dim imgdatastream As Stream = fleUploadResumeDetails.PostedFile.InputStream
        Dim imgdatalen As Integer = fleUploadResumeDetails.PostedFile.ContentLength
        Dim imgtype As String = fleUploadResumeDetails.PostedFile.ContentType
        'Dim imgtitle As String = TextBox1.Text
        Dim imgdata() As Byte = New Byte((imgdatalen) - 1) {}
        Dim n As Integer = imgdatastream.Read(imgdata, 0, imgdatalen)
        Dim strSqlInsert As String
        Dim Experience As Integer
        Experience = (12 * dropempExpyears.Value) + dropempExpmonths.Value

        If ddlcity.SelectedValue = "Other" Then
            cityname = txtCity.Text
            Dim strcityins As String
            Dim cmdcity As SqlCommand
            strcityins = "insert into cityMaster(CityName) values('" & txtCity.Text & "')"
            strConn.Open()
            cmdcity = New SqlCommand(strcityins, strConn)
            cmdcity.ExecuteNonQuery()
            strConn.Close()
        Else
            cityname = ddlcity.SelectedItem.Text
        End If

        Dim dob As String
        Dim telno As String

        If txtdob.Text = "" Then
            dob = ""
        Else
            dob = txtdob.Text
        End If

        If txtTelno.Text = "" Then
            telno = ""
        Else
            telno = txtTelno.Text
        End If

        Dim mobileno As String

        If txtmobileno.Text = "" Then
            mobileno = ""
        Else
            mobileno = txtmobileno.Text
        End If

        strSqlInsert = "insert into candidateMaster (candFName,candMName,candLName,candbirthdate,candtelno,candmobileno,candemail, " & _
                       " candpermaddress,candrelativeno,candpost,candexp,candCurrSalary,candExpSalary,candPreviEmp,candReasonChange,candCity, " & _
                       "resumes,resumetype) values('" & txtFname.Text & "','" & txtMname.Text & "','" & txtLname.Text & "',  " & _
                       " '" & dob & "' ,'" & telno & "', '" & mobileno & "','" & txtemail.Text & "','" & txtPermAddress.Text & "'," & _
                       " '" & txtRelativeno.Text & "','" & empSkill.Value & "','" & Experience & "','" & txtCSalary.Text & "','" & txtESalary.Text & "' , " & _
                       " '" & txtPemployer.Text & "','" & txtreason.Text & "','" & cityname & "',@imagedata,@imagetype ) select @candid=@@identity from employeemaster"

        Cmdlist = New SqlCommand(strSqlInsert, strConn)
        Cmdlist.Parameters.Add("@imagedata", SqlDbType.Image)
        Cmdlist.Parameters.Add("@imagetype", SqlDbType.VarChar, 50)
        Cmdlist.Parameters("@imagedata").Direction = ParameterDirection.Input
        Cmdlist.Parameters("@imagetype").Direction = ParameterDirection.Input
        Cmdlist.Parameters("@imagedata").Value = imgdata
        Cmdlist.Parameters("@imagetype").Value = imgtype
        strConn.Open()


        '-- for candiate id
        Cmdlist.Parameters.Add("@candId", SqlDbType.Int)
        Cmdlist.Parameters("@candId").Direction = ParameterDirection.Output
        '-- end 
        Cmdlist.ExecuteNonQuery()
        ID = Cmdlist.Parameters("@candId").Value

        Dim strsqlid As String
        Dim arr(20) As String
        Dim item As ListItem
        Dim i As Integer
        For Each item In lstempQual.Items
            If item.Selected Then
                arr(i) = item.Value
                strsqlid = "insert into candQualification(candId,candqualificationId) values('" & ID & "','" & arr(i) & "')"
                Dim cimid As New SqlCommand(strsqlid, strConn)
                cimid.ExecuteNonQuery()
            End If
            i = i + 1
        Next
        strConn.Close()

        '-- fill skills candskill



        Dim strsqlskill As String
        Dim arrskill(20) As String
        Dim itemskill As ListItem
        Dim j As Integer
        strConn.Open()
        For Each itemskill In lstSkill.Items
            If itemskill.Selected Then
                arrskill(j) = itemskill.Value
                strsqlskill = "insert into candskill(candId,skillid) values('" & ID & "','" & arrskill(j) & "')"
                Dim cimskill As New SqlCommand(strsqlskill, strConn)
                cimskill.ExecuteNonQuery()
            End If
            j = j + 1
        Next

        strConn.Close()
        '--



        Dim strsqlDetails As String
        'Response.End()
        Dim cid As Integer
        cid = Request.QueryString("id")
        strsqlDetails = "insert into candSchDetail (candId,statusId,schDate,schComment) values(" & ID & "," & candStatus.Value & ",'" & txtschdate.Text & "','" & txtAddComment.Text & "')"
        strConn.Open()
        Dim cmdinsert As New SqlCommand(strsqlDetails, strConn)
        cmdinsert.ExecuteNonQuery()
        strConn.Close()

        Dim sp As String
        sp = "<script language='JavaScript'>"
        sp += "alert('Record has been added successfully.');"
        sp += "</" + "script>"
        Response.Write(sp)
        Response.Write("<script language='javascript'>{window.location.href='/emp/showCandiate.aspx' ; } </" & "script>")
    End Sub
    Sub loadData()
        Dim strsqllist As String

        strsqllist = "SELECT * FROM empQualificationMaster"
        Cmdlist = New SqlCommand(strsqllist, strConn)
        strConn.Open()
        Cmdlist.CommandText = strsqllist
        rdrlista = Cmdlist.ExecuteReader()
        lstempQual.DataSource = rdrlista
        lstempQual.DataTextField = "qualificationDesc"
        lstempQual.DataValueField = "qualificationId"
        lstempQual.DataBind()
        rdrlista.Close()

        Dim strsqlpost As String
        'Dim strsqllist As SqlDataReader
        strsqlpost = "SELECT * FROM skillMaster"
        Dim Cmd1 As New SqlCommand(strsqlpost, strConn)
        rdrlista = Cmd1.ExecuteReader()
        empSkill.DataSource = rdrlista
        empSkill.DataTextField = "skillDesc"
        empSkill.DataValueField = "skillId"
        empSkill.DataBind()
        rdrlista.Close()
        strConn.Close()


    End Sub
    Sub fillcity()
        Dim strProject As String
        'strsqlcity = "select cityid,cityname from cityMaster"

        'sqlAdap = New SqlDataAdapter(strsqlcity, strConn)
        'ds = New DataSet
        'sqlAdap.Fill(ds)

        'ddlcity.DataSource = ds
        'ddlcity.DataBind()
        Dim dtrProject As SqlDataReader
        Dim cmdProject As SqlCommand
        strProject = "select cityid,cityname from cityMaster"
        strConn.Open()
        cmdProject = New SqlCommand(strProject, strConn)
        dtrProject = cmdProject.ExecuteReader()
        If ddlcity.Items.Count = 0 Then
            Do While (dtrProject.Read())
                ddlcity.Items.Add(New ListItem(dtrProject("cityname").ToString(), dtrProject("cityid").ToString()))

            Loop
            ddlcity.Items.Add(New ListItem("Other"))
        End If
        dtrProject.close()
        cmdProject.Dispose()
        strConn.Close()
       
    End Sub

    Protected Sub btnback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnback.Click
        Response.Redirect("showCandiate.aspx")
    End Sub

    Sub fillStatus()
        Dim strsqlpost As String
        Dim rdrlista As SqlDataReader
        strsqlpost = "select * from  dbo.candStatusMaster"
        Dim Cmdststus As New SqlCommand(strsqlpost, strConn)
        strConn.Open()
        rdrlista = Cmdststus.ExecuteReader()
        candStatus.DataSource = rdrlista
        candStatus.DataTextField = "statusDesc"
        candStatus.DataValueField = "statusId"
        candStatus.DataBind()
        rdrlista.Close()
        strConn.Close()
    End Sub
    Sub fillSkill()

        Dim strsqlskill As String
        'Dim strsqllist As SqlDataReader
        strsqlskill = "select * from techskills"
        Dim Cmdskill As New SqlCommand(strsqlskill, strConn)
        strConn.Open()
        rdrlista = Cmdskill.ExecuteReader()

        lstSkill.DataSource = rdrlista
        lstSkill.DataTextField = "skillDesc"
        lstSkill.DataValueField = "techId"
        lstSkill.DataBind()
        rdrlista.Close()
        strConn.Close()
    End Sub
End Class
