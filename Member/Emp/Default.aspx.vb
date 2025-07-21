
Partial Class emp_Default
    Inherits Authentication

    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load
        Response.Redirect("empHome.aspx")
    End Sub
End Class
