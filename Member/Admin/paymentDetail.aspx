<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>

<%@ Page Language="VB" %>

<%@ Register Src="../Controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc2" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control</title>

    <script language="javascript" src="/includes/CalendarControl.js" type="text/javascript"></script>

    <link rel="stylesheet" href="/includes/CalendarControl.css" type="text/css" />
    <link rel="stylesheet" href="../css/style.css" type="text/css" />

    <script language="VB" runat="server">
        Dim SortField As String
        Dim empCode As String
        Dim sql As String
        Dim gf As New generalFunction
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
     
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            conn.Open()
            If Not IsPostBack() Then
                loadData()
            End If
        End Sub

        Sub addBT_OnClick(ByVal objSource As Object, ByVal objArgs As EventArgs)
            If payCode.Value = "" Then
                If Not IsNumeric(payTransCharge.Value) Then
                    payTransCharge.Value = 0
                End If

                If IsDate(payConfirmDate.Value) Then
                    sql = "INSERT INTO paymentMaster(payProjId,payDate,payAmount," & _
                    "payExRate,payComment,payConfirmedDate,payTransCharge) VALUES(" & _
                    projCode.Value & ",'" & Trim(payDate.Value) & "'," & payAmount.Value & _
                    "," & payExRate.Value & ",'" & payDesc.Value & "','" & _
                    Trim(payConfirmDate.Value) & "'," & payTransCharge.Value & ")"
                Else
                    sql = "INSERT INTO paymentMaster(payProjId,payDate,payAmount," & _
                    "payExRate,payComment,payTransCharge) VALUES(" & projCode.Value & _
                    ",'" & Trim(payDate.Value) & "'," & payAmount.Value & "," & _
                    payExRate.Value & ",'" & payDesc.Value & "'," & payTransCharge.Value & ")"
                End If
            Else
                If IsDate(payConfirmDate.Value) Then
                    sql = "UPDATE paymentMaster SET payDate='" & Trim(payDate.Value) & "',payAmount=" & _
                    payAmount.Value & ",payExRate=" & payExRate.Value & ",payComment='" & _
                    payDesc.Value & "',payConfirmedDate='" & Trim(payConfirmDate.Value) & _
                    "',payTransCharge=" & payTransCharge.Value & " WHERE payId=" & payCode.Value
                Else
                    sql = "UPDATE paymentMaster SET payDate='" & Trim(payDate.Value) & "',payAmount=" & _
                    payAmount.Value & ",payExRate=" & payExRate.Value & ",payComment='" & _
                    payDesc.Value & "',payTransCharge=" & payTransCharge.Value & _
                    " WHERE payId=" & payCode.Value
                End If
            End If
            Dim cmd As SqlCommand = New SqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            Server.Transfer("paymentSummary.aspx?projid=" & projCode.Value)
        End Sub

        Sub loadData()
            Dim Rdr As SqlDataReader
            Dim projId As String = Request.QueryString("projId")
            Dim payId As String = Request.QueryString("payId")

            If payId <> "" Then
                sql = "SELECT * FROM projectMaster,customerMaster,currencyMaster,paymentMaster " & _
                "WHERE projectMaster.custId=customerMaster.custId AND projectMaster." & _
                "currId=currencyMaster.currId AND paymentMaster.payprojid=" & _
                "projectMaster.projId AND payId=" & payId & _
                " ORDER BY projName"
                Dim Cmd As New SqlCommand(sql, conn)
                Rdr = Cmd.ExecuteReader()
                If Rdr.Read() Then
                    payCode.Value = Rdr("payId")
                    projNameLbl.Text = Rdr("projName")
                    projCode.Value = Rdr("projId")
                    custCompany.Text = Rdr("custCompany")
                    currency.Text = Rdr("currName")
                    payAmount.Value = Rdr("payAmount")
                    payDate.Value = Day(Rdr("payDate")) & "-" & Left(MonthName(Month(Rdr("payDate"))), 3) & "-" & Right(Year(Rdr("payDate")), 2)
                    payExRate.Value = Rdr("payExRate")
                    payDesc.Value = Rdr("payComment") & ""
            
                    If Rdr("payConfirmedDate").ToString() <> "" Then
                        payConfirmDate.Value = Day(Rdr("payConfirmedDate")) & "-" & Left(MonthName(Month(Rdr("payConfirmedDate"))), 3) & "-" & Right(Year(Rdr("payConfirmedDate")), 2)
                    End If
                    payTransCharge.Value = Rdr("payTransCharge")
                End If
                Rdr.Close()
            ElseIf projId <> "" Then
                sql = "SELECT * FROM projectMaster,customerMaster,currencyMaster " & _
                "WHERE projectMaster.custId=customerMaster.custId AND projectMaster." & _
                "currId=currencyMaster.currId AND projId=" & projId & _
                " ORDER BY projName"
                Dim Cmd As New SqlCommand(sql, conn)
                Rdr = Cmd.ExecuteReader()
                If Rdr.Read() Then
                    projNameLbl.Text = Rdr("projName")
                    projCode.Value = Rdr("projId")
                    custCompany.Text = Rdr("custCompany")
                    currency.Text = Rdr("currName")
                End If
                Rdr.Close()
            Else
                sql = "SELECT * FROM projectMaster ORDER BY projName"
                Dim Cmd As New SqlCommand(sql, conn)
                Rdr = Cmd.ExecuteReader()
                projMaster.DataSource = Rdr
                projMaster.DataTextField = "projName"
                projMaster.DataValueField = "projId"
                projMaster.DataBind()
                Rdr.Close()
            End If
        End Sub

        Sub getProject(ByVal objSource As Object, ByVal objArgs As EventArgs)
            Response.Redirect("paymentDetail.aspx?projid=" & Request("projMaster"))
        End Sub
    </script>

</head>
<body>
    <form runat="server" id="projForm">
    <table cellpadding="4" width="100%" border="1" style="border-color: #e8e8e8">
        <tr>
            <td colspan="4">
                <uc2:adminMenu ID="adminMenu1" runat="server" />
            </td>
        </tr>
        <tr>
            <td width="25%">
                Project
            </td>
            <td width="25%">
                <input type="hidden" id="payCode" runat="server" width="0" size="20" />
                <input type="hidden" id="projCode" runat="server" width="0" size="20" />
                <%
                    If Request.QueryString("projId") = "" And Request.QueryString("payId") = "" Then
                %>
                <asp:DropDownList runat="server" ID="projMaster" OnSelectedIndexChanged="getProject" AutoPostBack="true"/>
                <%
                Else
                %>
                <asp:Label runat="server" ID="projNameLbl"/>
                <%
                End If
                %>
                &nbsp;
            </td>
            <td width="25%">
                Payment Due Date
            </td>
            <td width="25%">
                <input type="text" id="payDate" runat="server" size="10" name="payDate" onclick="popupCalender('payDate')"
                    readonly />
                <font face="verdana" size="2" color="black"><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ControlToValidate="payDate" ErrorMessage="Required"
                    Font-Names="Verdana" Font-Size="Xx-Small"></asp:RequiredFieldValidator></b></font>
            </td>
        </tr>
        <tr>
            <td width="25%">
                Customer
            </td>
            <td width="25%">
                <asp:Label runat="server" ID="custCompany" />
            </td>
            <td width="25%">
                Amount
            </td>
            <td width="25%">
                <input type="text" id="payAmount" runat="server" size="10" name="payAmount" />
            </td>
        </tr>
        <tr>
            <td width="25%">
                Currency
            </td>
            <td width="25%">
                <asp:Label runat="server" ID="currency"  />
            </td>
            <td width="25%">
                Exchange Rate
            </td>
            <td width="25%">
                <input type="text" id="payExRate" runat="server" size="3" name="payExRate" />
            </td>
        </tr>
        <tr>
            <td align="left">
                Trans Charges(%)
            </td>
            <td align="left">
                <input id="payTransCharge" type="text" size="3" name="payTransCharge" runat="server" />
            </td>
            <td align="left">
                Confirmation Date
            </td>
            <td align="left">
                <input id="payConfirmDate" onclick="popupCalender('payConfirmDate')" type="text"
                    size="10" name="payConfirmDate" runat="server" readonly />
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="4" align="center">
                Payment Description<p>
                    <textarea id="payDesc" runat="server" rows="5" cols="70" name="payDesc"></textarea>
                </p>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <input type="button" id="addBT" runat="server" value="Save" onserverclick="addBT_OnClick"
                    name="addBt" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
