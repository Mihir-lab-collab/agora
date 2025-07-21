Imports System.Data
Imports System.Data.SqlClient
Partial Class emp_getcustomer
    Inherits Authentication

    Dim gf As New generalFunction
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString
        Dim conn As SqlConnection = New SqlConnection(dsn1)
        Dim sqlstr As String
        Dim dtrShow As SqlDataReader
        sqlstr = "select * from dbo.employeeMaster where empid=" & Request.QueryString("q")
        Dim cmdshow As New SqlCommand(sqlstr, conn)
        conn.Open()
        dtrShow = cmdshow.ExecuteReader()
        If dtrShow.Read Then

            Response.Write("<tr><td><B>Emp ID --  </b>" & dtrShow("empid") & "</td>")
            Response.Write("<br>")
            Response.Write("<tr><td><b>Emp Name --  </b>" & dtrShow("empname") & "</td>")
            Response.Write("<br>")
            Response.Write("<tr><td>Emp Address --  </b>" & dtrShow("empaddress") & " </td>")
            Response.Write("<br>")
            Response.Write("<tr><td><b>Emp contact --  </b>" & dtrShow("empcontact") & " </td>")
            Response.Write("<br>")
            Response.Write("<tr><td><b>Emp Email --  </b>" & dtrShow("empemail") & " </td>")
            Response.Write("<br>")
            Response.Write("<tr><td><b>Emp Acc --  </b>" & dtrShow("empAccountNo") & " </td>")
            Response.Write("<br>")
            Response.Write("<tr><td><b>Emp joining date --  </b>" & dtrShow("empjoiningdate") & "</b></td>")
            Response.Write("<br>")
            Response.Write("<tr><td><b>Emp Previous Employer --  </b>" & dtrShow("empPrevemployer") & "</b></td>")

            ' Response.Write("<td>" & dtrShow("empid") & "</td></tr>")

        End If
        conn.Close()
        Response.Write("</table>")
    End Sub
End Class
