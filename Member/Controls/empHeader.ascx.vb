
Partial Class controls_empHeader
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        compName.Text = ConfigurationManager.AppSettings("compName")
    End Sub
End Class
