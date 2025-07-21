Imports System.Data
Imports System.Data.SqlClient
Imports dwtDAL
Partial Class admin_EditAttendance
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objgeneralFunction As New generalFunction
    Dim cmdEmp As SqlCommand
    Dim dtrEmp As SqlDataReader
    Dim empId As String
    Dim strSQL As String
    Dim strempid As String
    Dim strUpdate As String
    Dim sp1 As String
    Dim gf As New generalFunction
    Dim str As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        gf.checkEmpLogin()
        Dim arrdate As New ArrayList
        str = Request.QueryString("currDate")
        strempid = Request.QueryString("id")
        strUpdate = Request.QueryString("strUpdate")

        strSQL = "SELECT empname  FROM employeemaster WHERE empid=" & strempid
        dtrEmp = SqlHelper.ExecuteReader(dsn, CommandType.Text, strSQL)
        If (dtrEmp.HasRows) Then
            If (dtrEmp.Read()) Then
                lblempname.Text = CStr(dtrEmp("empName"))
            End If
        End If

        If Not IsPostBack Then
            Bind_dropattstatus()
            txtdateFrom.Text = Request.QueryString("currDate")
            txtdate.Text = Request.QueryString("currDate")

        End If
    End Sub

    Sub Bind_dropattstatus()

        Try
            strSQL = "SELECT statusid,statusDesc from empstatus where statusid<>'A'"
            dtrEmp = objgeneralFunction.funcSelect(strSQL)
            If dtrEmp.HasRows() Then
                dropattstatus.DataSource = dtrEmp
                dropattstatus.DataTextField = "statusDesc"
                dropattstatus.DataValueField = "statusid"
                dropattstatus.DataBind()
            End If
            objgeneralFunction.funcConClose()
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        ' Dim days As Long
        'days = DateDiff(DateInterval.Day, Convert.ToDateTime(txtdateFrom.Text), Convert.ToDateTime(txtdate.Text))
        Dim todate As DateTime
        todate = Convert.ToDateTime(txtdate.Text)
       
        While (Convert.ToDateTime(txtdateFrom.Text) <= todate)
            txtdate.Text = txtdateFrom.Text



            If strUpdate = "" Then
                Try
                    txtdate.Text = txtdate.Text + " " + hdnTime.Value + "AM"

                    strSQL = "insert into empatt values(" & strempid & ",'" & CDate(txtdate.Text) & "','" & dropattstatus.SelectedValue.ToString() & "','" & CDate(txtdate.Text) & "','" & CDate(txtdate.Text).AddHours(9) & "'," & "null" & ",'0.0.0.0','" & Session("dynoEmpIdSession") & "',getdate())" & ""
                    'Response.Write(strSQL)
                    ' Response.End()

                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strSQL)
                    'sp1 = "<Script language=JavaScript>"
                    'sp1 += "window.opener.location=window.opener.location;"
                    'sp1 += "window.closess();"
                    'sp1 += "</" + "script>"
                    'ClientScript.RegisterStartupScript(Me.GetType(), "1", sp1)
                Catch ex As Exception
                    Response.Write(ex.Message.ToString())
                End Try
            Else
                Try



                    Dim strDate As String = String.Empty
                    Dim arrChar() As Char
                    Dim reverseStr As String = String.Empty
                    Dim strYear As String = String.Empty
                    strDate = str
                    Dim tdate As String = CDate(txtdate.Text).ToString("dd-MMM-yyyy")
                    arrChar = str.ToCharArray()
                    Array.Reverse(arrChar)
                    reverseStr = arrChar
                    str = str.Replace("-", "/")
                    str = str.Length.ToString()
                    ' strYear = reverseStr.Substring(4, 7)
                    'Response.Write(strDate & "<br/>")

                    txtdate.Text = txtdate.Text + " " + hdnTime.Value + "AM"

                    strYear = txtdate.Text



                    'strSQL = "UPDATE  empatt SET attInTime='" & txtdate.Text & "',attOutTime='" & CDate(txtdate.Text).AddHours(9) & "', attStatus='" & dropattstatus.SelectedValue.ToString() & "' where  Replace(Convert(varchar,attdate,106),' ','-') Like '%" & tdate & "%'and empid='" & strempid & "'"
                    strSQL = "UPDATE  empatt SET attInTime='" & txtdate.Text & "',attOutTime='" & CDate(txtdate.Text).AddHours(9) & "', attStatus='" & dropattstatus.SelectedValue.ToString() & "' where REPLACE(CONVERT(VARCHAR(11),attdate,106),' ','-')='" & tdate & "' and empid='" & strempid & "'"

                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, strSQL)
                    'sp1 = "<Script language=JavaScript>"
                    'sp1 += "window.opener.location=window.opener.location;"
                    'sp1 += "window.close();"
                    'sp1 += "</" + "script>"
                    'ClientScript.RegisterStartupScript(Me.GetType(), "1", sp1)




                Catch ex As Exception
                    Response.Write(ex.Message.ToString())
                End Try
            End If


            txtdateFrom.Text = Convert.ToDateTime(txtdateFrom.Text).AddDays(1)
        End While


        sp1 = "<Script language=JavaScript>"
        sp1 += "window.opener.location=window.opener.location;"
        sp1 += "window.close();"
        sp1 += "</" + "script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "1", sp1)
    End Sub
End Class
