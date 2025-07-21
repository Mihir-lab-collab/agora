Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections

Partial Class admin_empattdetails1
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim strConn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim objgeneralFunction As New generalFunction
    Dim cmdEmp As SqlCommand
    Dim dtrEmp As SqlDataReader
    Dim cmd As SqlCommand
    Dim empId As String
    Dim strSQL As String
    Dim innersql As String
    Dim totalemp As Integer
    Dim totaldays As Integer
    Dim year As Integer = DateTime.Now.Year
    Dim Month As Integer = DateTime.Now.Month
    Dim i As Integer
    Dim j As Integer
    Dim strouttime As String
    Dim da As New SqlDataAdapter
    Dim attdate As String = ""
    Dim months As String
    Dim years As String
    Dim grdtotaldays As Integer
    Dim cnt As Integer = 0
    Dim gf As New generalFunction
    Dim dateList As New ArrayList



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        If Not IsPostBack Then
            bindgrid()
            findholidayofmonth()
        End If
    End Sub
    Sub bindgrid()
        attdate = Request.QueryString("strDate")

        If (attdate = "") Then
            months = DateTime.Now.Month.ToString()
            years = DateTime.Now.Year.ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        Else
            ' months = attdate.Substring(0, 1).ToString()
            'years = attdate.Substring(4, 4).ToString()
            Dim myArray(3)

            myArray = attdate.Split("/")
            months = myArray(0).ToString()
            years = myArray(2).ToString()
            totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))
        End If

        cmd = New SqlCommand()
        cmd.CommandText = "new_fillattendance"
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@days", totaldays)
        cmd.Parameters.AddWithValue("@months", months)
        cmd.Parameters.AddWithValue("@years", years)
        cmd.Connection = strConn
        Dim ds As New DataSet
        da = New SqlDataAdapter(cmd)
        da.Fill(ds)
        grdtotaldays = totaldays
        totaldays = totaldays + 1
        If (totaldays <= 31) Then
            For i = totaldays To 31
                'Response.Write("</br>aa" & i)
                ds.Tables(0).Columns.Add("aa" & i.ToString())
            Next
        End If
        'grdatt.Columns(3).HeaderText = "Test"
        '        ds.Tables(0).Columns(1).ColumnName = " Employee  Name  "
        grdatt.DataSource = ds.Tables(0).DefaultView
        grdatt.DataBind()

        ' Response.Write(grdtotaldays)
        grdtotaldays = grdtotaldays + 1
        If (grdtotaldays <= 31) Then
            For i = grdtotaldays To 31
                grdatt.Columns(i + 1).Visible = False
            Next
        End If
        grdatt.Columns(0).Visible = False

    End Sub


    Protected Sub grdatt_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdatt.RowDataBound
        Dim gvr As GridViewRow = e.Row
        Dim j As Integer = 0
        If (gvr.RowType = DataControlRowType.DataRow) Then
            Dim l1 As Label = DirectCast(e.Row.Cells(2).FindControl("Label1"), Label)

            'Dim chk3 As CheckBox = DirectCast(e.Row.Cells(2).FindControl("chk3"), CheckBox)
            'Dim lbl3 As Label = DirectCast(e.Row.Cells(2).FindControl("Label3"), Label)
            'If (lbl3.Text.ToString() <> "A") Then
            '    chk3.Visible = False
            'Else
            '    chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
            '    lbl3.Visible = False
            'End If
            For j = 1 To 31

                Dim strdate As String = grdatt.Columns(j + 1).HeaderText.ToString() & "-" & months & "-" & CStr(year)
                Dim strUpdate As String = "strUpdate"
                Dim chk3 As CheckBox = DirectCast(e.Row.Cells(j + 1).FindControl("chk" & CStr(j + 2)), CheckBox)
                Dim lbl3 As Label = DirectCast(e.Row.Cells(j + 1).FindControl("Label" & CStr(j + 2)), Label)
                If (lbl3.Text.ToString() <> "A") Then
                    chk3.Visible = False
                    If (lbl3.Text.EndsWith("L") = True) Then
                        lbl3.BackColor = Color.FromArgb(134, 198, 124)
                        chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
                        chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "','" & strUpdate & "')")
                        chk3.Visible = True
                        'lbl3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
                        'lbl3.Font.Underline = True
                        'e.Row.Cells(j + 1).BackColor = Color.Red
                    End If
                Else
                    chk3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "')")
                    lbl3.Visible = False
                End If
                '  Dim strUpdate As String = "strUpdate"
                lbl3.Attributes.Add("style", "cursor:pointer;")
                lbl3.Attributes.Add("onClick", "doAttandance('" & l1.Text & "','" & strdate & "','" & strUpdate & "')")
            Next
        End If

    End Sub


    Protected Sub grdatt_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdatt.SelectedIndexChanged

    End Sub
    Sub findholidayofmonth()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim t As Integer = 0
        Dim arrholiday As New ArrayList
        Dim bccolor As Color = Color.FromArgb(197, 213, 174)
        totaldays = DateTime.DaysInMonth(CInt(years), CInt(months))


        For x = 1 To totaldays
            Dim d As New Date(CInt(years), CInt(months), x)
            If (d.ToString("dddd").ToUpper() = "SATURDAY") Then
                If (y <> 1) Then

                    x = x + 1
                    If x <= totaldays Then
                        arrholiday.Add(x.ToString())
                    End If

                    y = 1
                Else
                    t = x + 1
                    arrholiday.Add(t.ToString())
                    arrholiday.Add(x.ToString())
                    y = 0
                End If

            Else
                If (d.ToString("dddd").ToUpper() = "SUNDAY") Then
                    arrholiday.Add(x.ToString())
                End If
            End If

        Next

        strSQL = "select day(holidaydate) as holidate from holidaymaster where month(holidaydate)=" & months & " and year(holidaydate)=" & years
        dtrEmp = objgeneralFunction.funcSelect(strSQL)
        If dtrEmp.HasRows Then
            While (dtrEmp.Read())
                arrholiday.Add(CStr(dtrEmp(0)))
            End While
        End If
        objgeneralFunction.funcConClose()

        For Each t In arrholiday
            grdatt.Columns(CInt(t + 1)).ItemStyle.BackColor = bccolor
        Next


    End Sub
End Class
