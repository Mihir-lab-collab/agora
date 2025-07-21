Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Partial Class admin_paymentConfirm
    Inherits Authentication
    Dim dsn As String = ConfigurationManager.ConnectionStrings("conString").ToString()
    Dim conn As SqlConnection = New System.Data.SqlClient.SqlConnection(dsn)
    Dim gf As New generalFunction

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gf.checkEmpLogin()
        Dim payTransAmount As String = ""
        Dim Rdrcust As SqlDataReader
        Dim Cmdcust As SqlCommand
        Dim strsqlcust As String
        strsqlcust = "SELECT * FROM paymentMaster,customerMaster,projectMaster, currencyMaster " & _
         "WHERE projectMaster.currId=currencyMaster.currId AND customerMaster.custId=" & _
         "projectMaster.custId AND projectMaster.projId=paymentMaster.payProjId AND payId=" & CStr(Request.QueryString("payId"))

        conn.Open()
        Cmdcust = New SqlCommand(strsqlcust, conn)
        Rdrcust = Cmdcust.ExecuteReader

        Session("payId") = ""
        Session("payAmount") = ""
        Session("payComment") = ""
        Session("payTransCharge") = ""
        Session("custName") = ""
        Session("custCompany") = ""
        Session("custAddress") = ""
        Session("custEmail") = ""
        Session("payDate") = ""
        Session("payConfirmedDate") = ""
        Session("payAmount") = ""
        Session("currName") = ""
        Session("payTransAmount") = ""

        If Rdrcust.Read() Then

            If Trim(Rdrcust("payId")) <> "" Then
                Session("payId") = Trim(Rdrcust("payId"))
            Else
                Session("payId") = ""
            End If

            If IsDBNull(Trim(Rdrcust("payAmount"))) Or Trim(Rdrcust("payAmount")) <> "" Then
                Session("payAmount") = Trim(Rdrcust("payAmount"))
            Else
                Session("payAmount") = "0"
            End If


            If Not IsDBNull(Rdrcust("payComment")) Then
                If Rdrcust("payComment") <> "" Then
                    Session("payComment") = Trim(Rdrcust("payComment"))
                Else
                    Session("payComment") = ""
                End If

            Else
                Session("payComment") = ""
            End If

            If IsDBNull(Rdrcust("payTransCharge")) Or Trim(Rdrcust("payTransCharge")) <> "" Then
                Session("payTransCharge") = Trim(Rdrcust("payTransCharge"))
            Else
                Session("payTransCharge") = ""
            End If

            If IsDBNull(Rdrcust("custName")) Or Trim(Rdrcust("custName")) <> "" Then
                Session("custName") = Trim(Rdrcust("custName"))
            Else
                Session("custName") = ""
            End If

            If IsDBNull(Rdrcust("custCompany")) Or Trim(Rdrcust("custCompany")) <> "" Then
                Session("custCompany") = Trim(Rdrcust("custCompany"))
            Else
                Session("custCompany") = ""
            End If


            If Not IsDBNull(Rdrcust("custAddress")) Then
                If Rdrcust("custAddress") <> "" Then
                    Session("custAddress") = Trim(Rdrcust("custAddress"))
                Else
                    Session("custAddress") = ""
                End If

            Else
                Session("custAddress") = ""
            End If

            If IsDBNull(Rdrcust("custEmail")) Or Trim(Rdrcust("custEmail")) <> "" Then
                Session("custEmail") = Trim(Rdrcust("custEmail"))
            Else
                Session("custEmail") = ""
            End If

            If IsDBNull(Rdrcust("payDate")) Or Trim(Rdrcust("payDate")) <> "" Then
                Session("payDate") = Trim(Rdrcust("payDate"))
            Else
                Session("payDate") = ""
            End If

            If IsDate(Rdrcust("payConfirmedDate")) Then
                Session("payConfirmedDate") = Trim(Rdrcust("payConfirmedDate"))
            Else
                Session("payConfirmedDate") = ""
            End If

            If IsDBNull(Trim(Rdrcust("payAmount"))) Or Trim(Rdrcust("payAmount")) <> "" Then
                Session("payAmount") = Trim(Rdrcust("payAmount"))
            Else
                Session("payAmount") = ""
            End If

            If IsDBNull(Rdrcust("currSymbol")) Or Trim(Rdrcust("currSymbol")) <> "" Then
                Session("currName") = Trim(Rdrcust("currSymbol")) & " "
            Else
                Session("currName") = ""
            End If
        End If
        Rdrcust.Close()
        conn.Close()

        Session("payTransAmount") = 0
        If IsNumeric(Session("payTransCharge")) Then
            Session("payTransAmount") = (Session("payAmount") * Session("payTransCharge")) / 100
            If InStr(payTransAmount, ".") > 0 Then
                Session("payTransAmount") = FormatNumber(Session("payTransAmount") + 1, 0)
            End If
        End If

    End Sub
End Class
