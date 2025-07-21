<%@ Page Language="VB" %>

<%@ Register Src="../controls/adminMenu.ascx" TagName="adminMenu" TagPrefix="uc1" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>

<script language="VB" runat="server">
    Dim sum As Double
    Dim paidSum As Double
    Dim payndingsum As Double
    Dim addCost, totCosts, recvdCost, pendCost As Double
    Dim aInitCost, aAdditionalCost, aCutValue, aCurvalueINr, aTotalINc, aRecPayment, aPendingPayment As Double

    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Redirect("default1.aspx")
        gf.checkEmpLogin()
        Try
            If Session("dynoAdminSession") = 1 Then
                Session("DynoTS") = 1
            End If
            BindGrid()
        Catch ex As Exception

        End Try
    End Sub
    
    Sub BindGrid()
        Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
        Dim conn As SqlConnection = New SqlConnection(dsn1)
        Dim strSQL As String = ""
    
        If Request.QueryString("search") = "all" Then
            'strSQL = "SELECT projId, projName,currSymbol, projCost, projDuration,projStartDate,currExRate, Max(paymentMaster.payDate) as " & _
            '" lastPayDate,projActComp, projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp, " & _
            ' " projCost*currExRate as projTotalCost " & _
            ' " FROM projectMaster  " & _
            '" inner join customerMaster on projectMaster.custId=customerMaster.custId " & _
            '" inner join employeeMaster on projectMaster.projManager=employeeMaster.empid " & _
            '" inner join currencyMaster on currencyMaster.currId=projectMaster.currId " & _
            '" left outer join paymentMaster on paymentMaster.payProjId = projectMaster.projid " & _
            '" group by projId,projName,currSymbol,projStartDate, projDuration,projCost,projActComp,currExRate " & _
            '" ORDER BY projStartDate Desc"
            'Response.Write(strSQL)
            'below one old Qery till 5feb2013
            'strSQL = "SELECT projId,projName,currSymbol,projCost,(select Max(paymentMaster.payDate) as lastPaymentDate from dbo.paymentMaster where payProjID = projectMaster.projId) as lastPayDate, (select isnull(sum(isnull(payAmount,0)),0) from dbo.paymentMaster where payProjID = projectMaster.projId)+ projCost as currentvalue1, (select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId) as TotalInvoiced, (select isnull(sum(isnull(PayExRate,0) * isnull(payAmount,0)),0) + (projCost * currExRate) from dbo.paymentMaster where payProjID = projectMaster.projId) as CurrentValueINR, (select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId) as ReceivedPayment, projDuration,projStartDate,projActComp, projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,projCost*currExRate as projTotalCost ,currExRate,((select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId)-(select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId))as PendingPayment,DateDiff(mm,projStartDate,projActComp )as startenddiff,CONVERT(CHAR(4), projStartDate, 100) + right(CONVERT(CHAR(4),projStartDate, 111),2)+'-' +CONVERT(CHAR(4), projActComp, 100) + right(CONVERT(CHAR(4), projActComp, 111),2)as projectDuration FROM projectMaster inner join customerMaster on projectMaster.custId=customerMaster.custId inner join employeeMaster on projectMaster.projManager=employeeMaster.empid inner join currencyMaster on currencyMaster.currId=projectMaster.currId ORDER BY projectMaster.InsertedOn Desc"
            'below one new Qery from 5feb2013
            strSQL = "SELECT projId,projName,currSymbol,projCost,(select Max(paymentMaster.payDate) as lastPaymentDate from dbo.paymentMaster where payProjID = projectMaster.projId) as lastPayDate, (select case when isnull(sum(isnull(payAmount,0)),0)>projCost then isnull(sum(isnull(payAmount,0)),0) else projCost end from paymentMaster where payprojid = projectMaster.projId)as currentvalue1, (select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId) as TotalInvoiced, (select case when isnull(sum((isnull(PayExRate,0) * isnull(payAmount,0))),0)>(projCost * currExRate)then isnull(sum((isnull(PayExRate,0) * isnull(payAmount,0))),0)else  (projCost * currExRate) end from paymentMaster where payprojid = projectMaster.projId) as CurrentValueINR, (select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId) as ReceivedPayment, projDuration,projStartDate,projActComp, projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,projCost*currExRate as projTotalCost ,currExRate,((select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId AND ( paymentStatus!='Cancel' or paymentStatus is null))-(select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId))as PendingPayment,DateDiff(mm,projStartDate,projActComp )as startenddiff,CONVERT(CHAR(4), projStartDate, 100) + right(CONVERT(CHAR(4),projStartDate, 111),2)+'-' +CONVERT(CHAR(4), projActComp, 100) + right(CONVERT(CHAR(4), projActComp, 111),2)as projectDuration FROM projectMaster inner join customerMaster on projectMaster.custId=customerMaster.custId inner join employeeMaster on projectMaster.projManager=employeeMaster.empid inner join currencyMaster on currencyMaster.currId=projectMaster.currId ORDER BY projectMaster.InsertedOn Desc"
        Else
            'strSQL = "SELECT projId, projName,currSymbol, projCost, projDuration,projStartDate,currExRate, " & _
            '" Max(paymentMaster.payDate) as lastPayDate,projActComp, projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp, " & _
            '" projCost*currExRate as projTotalCost " & _
            '" FROM projectMaster  " & _
            '" inner join customerMaster on projectMaster.custId=customerMaster.custId " & _
            '" inner join employeeMaster on projectMaster.projManager=employeeMaster.empid and customerMaster.custstatus=1 " & _
            '" inner join currencyMaster on currencyMaster.currId=projectMaster.currId " & _
            '" left outer join paymentMaster on paymentMaster.payProjId = projectMaster.projid " & _
			'" WHERE ProjActComp IS NULL" & _
            '" group by projId,projName,currSymbol,projStartDate,projDuration,projCost,projActComp,currExRate " & _
            '" ORDER BY projStartDate Desc"
            'below one old Qery till 5feb2013
            'strSQL = "SELECT projId,projName,currSymbol,projCost,(select Max(paymentMaster.payDate) as lastPaymentDate from dbo.paymentMaster where payProjID = projectMaster.projId) as lastPayDate, (select isnull(sum(isnull(payAmount,0)),0) from dbo.paymentMaster where payProjID = projectMaster.projId)+ projCost as currentvalue1, (select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId) as TotalInvoiced, (select isnull(sum(isnull(PayExRate,0) * isnull(payAmount,0)),0) + (projCost * currExRate) from dbo.paymentMaster where payProjID = projectMaster.projId) as CurrentValueINR, (select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId) as ReceivedPayment, projDuration,projStartDate,projActComp, projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,projCost*currExRate as projTotalCost ,currExRate,((select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId)-(select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId))as PendingPayment,DateDiff(mm,projStartDate,projActComp )as startenddiff,CONVERT(CHAR(4), projStartDate, 100) + right(CONVERT(CHAR(4),projStartDate, 111),2)+'-' +CONVERT(CHAR(4), projActComp, 100) + right(CONVERT(CHAR(4), projActComp, 111),2)as projectDuration FROM projectMaster inner join customerMaster on projectMaster.custId=customerMaster.custId inner join employeeMaster on projectMaster.projManager=employeeMaster.empid inner join currencyMaster on currencyMaster.currId=projectMaster.currId WHERE ProjActComp IS NULL ORDER BY projectMaster.InsertedOn Desc"
            'below one new Qery from 5feb2013
            strSQL = "SELECT projId,projName,currSymbol,projCost,(select Max(paymentMaster.payDate) as lastPaymentDate from dbo.paymentMaster where payProjID = projectMaster.projId) as lastPayDate, (select case when isnull(sum(isnull(payAmount,0)),0)>projCost then isnull(sum(isnull(payAmount,0)),0) else projCost end from paymentMaster where payprojid = projectMaster.projId)as currentvalue1, (select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId) as TotalInvoiced, (select case when isnull(sum((isnull(PayExRate,0) * isnull(payAmount,0))),0)>(projCost * currExRate)then isnull(sum((isnull(PayExRate,0) * isnull(payAmount,0))),0)else  (projCost * currExRate) end from paymentMaster where payprojid = projectMaster.projId) as CurrentValueINR, (select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId) as ReceivedPayment, projDuration,projStartDate,projActComp, projStartDate+[dbo].[funprojectday](projDuration,projStartDate) AS projExpComp,projCost*currExRate as projTotalCost ,currExRate,((select sum(isnull(payAmount,0)) from dbo.paymentMaster where payProjID = projectMaster.projId AND ( paymentStatus!='Cancel' or paymentStatus is null))-(select isnull(sum(case when payconfirmedDate is null or payconfirmedDate='1900-01-01 00:00:00' then 0 else payamount end),0) as PaidInvoiceAmount from dbo.paymentMaster where payProjID = projectMaster.projId))as PendingPayment,DateDiff(mm,projStartDate,projActComp )as startenddiff,CONVERT(CHAR(4), projStartDate, 100) + right(CONVERT(CHAR(4),projStartDate, 111),2)+'-' +CONVERT(CHAR(4), projActComp, 100) + right(CONVERT(CHAR(4), projActComp, 111),2)as projectDuration FROM projectMaster inner join customerMaster on projectMaster.custId=customerMaster.custId inner join employeeMaster on projectMaster.projManager=employeeMaster.empid inner join currencyMaster on currencyMaster.currId=projectMaster.currId WHERE ProjActComp IS NULL ORDER BY projectMaster.InsertedOn Desc"
        End If
    
        conn.Open()
       
        Dim cmd As SqlCommand = New SqlCommand(strSQL, conn)
        Dim Rdr As SqlDataReader
        Rdr = cmd.ExecuteReader()
        MyDataGrid.DataSource = Rdr
        MyDataGrid.DataBind()
    End Sub


     Sub DisplayTotal(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
           
            Try
                         
            Dim DateDiff As String = "Project Duration : " & DataBinder.Eval(e.Item.DataItem, "startenddiff") & " Month(s) "
            e.Item.Cells(11).ToolTip = DateDiff
                
            Dim cellValue As String = DataBinder.Eval(e.Item.DataItem, "projCost") & ""
            If cellValue <> "" Then
                sum += Convert.ToDouble(cellValue)
            End If
                
            Dim paidcellValue As String = DataBinder.Eval(e.Item.DataItem, "ReceivedPayment") & ""
            If paidcellValue <> "" Then
                paidSum += Convert.ToDouble(paidcellValue)
            End If
                
            Dim paindingcellValue As String = DataBinder.Eval(e.Item.DataItem, "PendingPayment") & ""
            If paindingcellValue <> "" Then
                payndingsum += Convert.ToDouble(paindingcellValue)
            End If
              
                
            Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
            Dim conn As SqlConnection = New SqlConnection(dsn1)
            Dim strSQL As String
            Dim chprojId As Object = DataBinder.Eval(e.Item.DataItem, "projId")
            Dim chda As New SqlDataAdapter
            Dim chDs As New DataSet

            conn.Open()

            strSQL = "SELECT chgEstCost FROM changeRequest WHERE chgProjId=" & chprojId
            'Response.Write(strSQL+"changeRequest")
         
            chda = New SqlDataAdapter(strSQL, conn)
            chda.Fill(chDs, "changeRequest")
            Dim chCost As Double
	
            If chDs.Tables(0).Rows.Count > 0 Then
                Dim i As Integer
                For i = 0 To chDs.Tables(0).Rows.Count - 1
                    If Not IsDBNull(chDs.Tables(0).Rows(i).Item("chgEstCost")) Then
                        chCost = chCost + chDs.Tables(0).Rows(i).Item("chgEstCost")
                    End If
                Next
            End If
	
            conn.Close()

            Dim hlnkAddCost As HyperLink
            hlnkAddCost = e.Item.FindControl("hlnkAddCost")
            If chCost <> "0" Then
            
                hlnkAddCost.Text = Format(chCost, "0,0") ' add satya on 11 may 2012
                hlnkAddCost.NavigateUrl = "show_ch_request_to_admin.aspx?projId=" & e.Item.Cells(0).Text
            Else
                hlnkAddCost.Text = "0"
                hlnkAddCost.NavigateUrl = "show_ch_request_to_admin.aspx?projId=" & e.Item.Cells(0).Text
            End If
                
            'Dim curr As Double
            'curr = e.Item.Cells(12).Text
            'Dim totCost As Double = (Convert.ToDouble(cellValue) * curr) + chCost
             
            'conn.Open()

            'Dim dspay As New DataSet
            'strSQL = "SELECT payAmount, payConfirmedDate FROM paymentMaster WHERE  payProjId=" & chprojId
            ''Response.Write(strSQL+"Paymentmaster")
            'chda = New SqlDataAdapter(strSQL, conn)
            'chda.Fill(dspay, "changeRequest")
            'Dim payCost, penCost As Double

            'If dspay.Tables(0).Rows.Count > 0 Then
            '    Dim j As Integer
            '    For j = 0 To dspay.Tables(0).Rows.Count - 1
            '        If Not IsDBNull(dspay.Tables(0).Rows(j).Item("payAmount")) Then
            '            If Not IsDBNull(dspay.Tables(0).Rows(j).Item("payConfirmedDate")) Then
            '                payCost = payCost + dspay.Tables(0).Rows(j).Item("payAmount")
            '            Else
            '                penCost = penCost + dspay.Tables(0).Rows(j).Item("payAmount")
            '            End If
            '        End If
            '    Next
            'End If
                
                
            addCost = addCost + CType(hlnkAddCost.Text, Double)
            aCutValue = aCutValue + CType(e.Item.Cells(5).Text, Double)
            aCurvalueINr = aCurvalueINr + CType(e.Item.Cells(6).Text, Double)
            aTotalINc = aTotalINc + CType(e.Item.Cells(7).Text, Double)
            aRecPayment = aRecPayment + CType(e.Item.Cells(8).Text, Double)
             
           Catch ex As Exception
                            
                
           End Try
                     
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(1).Text = "<b>Total</b>"
           
            e.Item.Cells(3).Text = "<b>" & Format(sum, "0,00") & "</b>"
            e.Item.Cells(4).Text = "<b>" & Format(addCost, "0,00") & "</b>"
            e.Item.Cells(5).Text = "<b>" & Format(aCutValue, "0,00") & "</b>"
            e.Item.Cells(6).Text = "<b>" & Format(aCurvalueINr, "0,00") & "</b>"
            e.Item.Cells(7).Text = "<b>" & Format(aTotalINc, "0,00") & "</b>"
            'e.Item.Cells(8).Text = "<b>" & Format(aRecPayment, "0,00") & "</b>"
            e.Item.Cells(8).Text = "<b>" & Format(paidSum, "0,00") & "</b>"
            e.Item.Cells(9).Text = "<b>" & Format(payndingsum, "0,00") & "</b>"
        End If
    End Sub
   


    Sub DisplayTotal1(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim cellValue As String = DataBinder.Eval(e.Item.DataItem, "projCost") & ""
            If cellValue <> "" Then
                sum += Convert.ToDouble(cellValue)
            End If
		
            Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
            Dim conn As SqlConnection = New SqlConnection(dsn1)
            Dim strSQL As String
            Dim chprojId As Object = DataBinder.Eval(e.Item.DataItem, "projId")
            Dim chda As New SqlDataAdapter
            Dim chDs As New DataSet

            conn.Open()

            strSQL = "SELECT chgEstCost FROM changeRequest WHERE chgProjId=" & chprojId

            chda = New SqlDataAdapter(strSQL, conn)
            chda.Fill(chDs, "changeRequest")
            Dim chCost As Double
	
            If chDs.Tables(0).Rows.Count > 0 Then
                Dim i As Integer
                For i = 0 To chDs.Tables(0).Rows.Count - 1
                    If Not IsDBNull(chDs.Tables(0).Rows(i).Item("chgEstCost")) Then
                        chCost = chCost + chDs.Tables(0).Rows(i).Item("chgEstCost")
                    End If
                Next
            End If
	
            conn.Close()

            Dim hlnkAddCost As HyperLink
            hlnkAddCost = e.Item.FindControl("hlnkAddCost")
            If chCost <> "0" Then
            
                hlnkAddCost.Text = Format(chCost, "0,0") ' add satya on 11 may 2012
                hlnkAddCost.NavigateUrl = "show_ch_request_to_admin.aspx?projId=" & e.Item.Cells(0).Text
            Else
                hlnkAddCost.Text = "0"
                hlnkAddCost.NavigateUrl = "show_ch_request_to_admin.aspx?projId=" & e.Item.Cells(0).Text
            End If

            Dim curr As Double
            curr = e.Item.Cells(12).Text
            Dim totCost As Double = (Convert.ToDouble(cellValue) * curr) + chCost
            If totCost <> 0 Then
                e.Item.Cells(5).Text = Format(totCost, "0,00")
            Else
                e.Item.Cells(5).Text = "0"
            End If
            conn.Open()

            Dim dspay As New DataSet
            strSQL = "SELECT payAmount, payConfirmedDate FROM paymentMaster WHERE  payProjId=" & chprojId
            chda = New SqlDataAdapter(strSQL, conn)
            chda.Fill(dspay, "changeRequest")
            Dim payCost, penCost As Double

            If dspay.Tables(0).Rows.Count > 0 Then
                Dim j As Integer
                For j = 0 To dspay.Tables(0).Rows.Count - 1
                    If Not IsDBNull(dspay.Tables(0).Rows(j).Item("payAmount")) Then
                        If Not IsDBNull(dspay.Tables(0).Rows(j).Item("payConfirmedDate")) Then
                            payCost = payCost + dspay.Tables(0).Rows(j).Item("payAmount")
                        Else
                            penCost = penCost + dspay.Tables(0).Rows(j).Item("payAmount")
                        End If
                    End If
                Next
            End If
	
            If payCost <> 0 Then
                e.Item.Cells(6).Text = Format(payCost, "0,00")
            Else
                e.Item.Cells(6).Text = "0"
            End If
	
            If penCost <> 0 Then
                e.Item.Cells(7).Text = Format(penCost, "0,00")
            Else
                e.Item.Cells(7).Text = "0"
            End If
	
            addCost = addCost + CType(hlnkAddCost.Text, Double)
            totCosts = totCosts + CType(e.Item.Cells(5).Text, Double)
            recvdCost = recvdCost + CType(e.Item.Cells(6).Text, Double)
            pendCost = pendCost + CType(e.Item.Cells(7).Text, Double)
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            e.Item.Cells(0).Text = "<b>Total</b>"
            e.Item.Cells(3).Text = "<b>" & Format(sum, "0,00") & "</b>"
            e.Item.Cells(4).Text = "<b>" & Format(addCost, "0,00") & "</b>"
            e.Item.Cells(5).Text = "<b>" & Format(totCosts, "0,00") & "</b>"
            e.Item.Cells(6).Text = "<b>" & Format(recvdCost, "0,00") & "</b>"
            e.Item.Cells(7).Text = "<b>" & Format(pendCost, "0,00") & "</b>"
        End If
    End Sub
 
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control</title>
</head>
<body>
    <uc1:adminMenu ID="AdminMenu1" runat="server" />
    <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse"
        bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
        <tr>
            <td align="right" colspan="4" bgcolor="#edf2e6" width="50%">
                <b><font face="Verdana" size="2">
                    <p align="left">
                        <a href="default.aspx?search=Current"><font color="#A2921E">Current</font></a> |
                        <a href="default.aspx?search=all"><font color="#A2921E">All </font></a>|
            </td>
            <td width="50%" bgcolor="#edf2e6">
                <b><font face="Verdana" size="2">
                    <p align="right">
                        <a href="projDetail.aspx"><font color="#A2921E"><font color="#A2921E">Add New</font></a><td
                            bgcolor="#edf2e6">
                        </td>
        </tr>
    </table>
    <form id="Form1" runat="server" method="post">
    <asp:DataGrid ID="MyDataGrid" runat="server" AllowSorting="true" Width="100%" BackColor="white"
        BorderColor="black" ShowFooter="True" CellPadding="2" CellSpacing="0" Font-Name="Verdana"
        Font-Size="10pt" HeaderStyle-ForeColor="#A2921E" HeaderStyle-BackColor="#edf2e6"
        HeaderStyle-Font-Size="11pt" HeaderStyle-Font-Bold="True" MaintainState="true"
        FooterStyle-HorizontalAlign="Right" AutoGenerateColumns="false"  OnItemDataBound="DisplayTotal" >
        <AlternatingItemStyle BackColor="#edf2e6" />
        <Columns>
            <asp:BoundColumn Visible="false" DataField="projId" />
            <asp:HyperLinkColumn HeaderText="Project  Title" DataNavigateUrlField="projId" DataNavigateUrlFormatString="projDetail.aspx?projId={0}"
                DataTextField="projName" />
            <asp:BoundColumn HeaderText="Currency" DataField="currSymbol" />
            <asp:HyperLinkColumn HeaderText="Initial Cost" DataNavigateUrlField="projId" DataNavigateUrlFormatString="paymentSummary.aspx?projId={0}"
                ItemStyle-HorizontalAlign="right" DataTextFormatString="{0:##,###,###}" DataTextField="projCost" />
            <asp:TemplateColumn HeaderText="Additional Cost">
                <ItemStyle Width="10%" HorizontalAlign="right" VerticalAlign="middle"></ItemStyle>
                <ItemTemplate>
                    <asp:HyperLink ID="hlnkAddCost" ForeColor="black" runat="server">
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="currentvalue1" HeaderText="Current Value" DataFormatString="{0:##,###,###}"
                ItemStyle-HorizontalAlign="Right" />
            <asp:BoundColumn DataField="CurrentValueINR" HeaderText="Current Value(INR)" ItemStyle-HorizontalAlign="Right"
                DataFormatString="{0:##,###,###}" />
            <asp:BoundColumn DataField="TotalInvoiced" HeaderText="Total Invoiced" DataFormatString="{0:##,###,###}"
                ItemStyle-HorizontalAlign="Right" />
            <asp:HyperLinkColumn HeaderText="Recieved Payment" DataNavigateUrlField="projId"
                DataNavigateUrlFormatString="paymentSummary.aspx?projId={0}" ItemStyle-HorizontalAlign="right"
                DataTextFormatString="{0:##,###,###}" DataTextField="ReceivedPayment" />
            <asp:HyperLinkColumn HeaderText="Pending Payment" DataNavigateUrlField="projId" DataNavigateUrlFormatString="paymentSummary.aspx?projId={0}"
                ItemStyle-HorizontalAlign="right" DataTextFormatString="{0:##,###,###}" DataTextField="PendingPayment" />
            <asp:BoundColumn HeaderText="Last Payment Record" DataField="lastPayDate" DataFormatString="{0:dd-MMM-yyyy}">
                <ItemStyle Wrap="false" />
            </asp:BoundColumn>
            <asp:BoundColumn HeaderText="Project Duration" DataField="projectDuration">
                <ItemStyle Wrap="false" />
            </asp:BoundColumn>
            <asp:BoundColumn HeaderText="CurrentExchangeRate" DataField="currExRate" Visible="false" />
        </Columns>
    </asp:DataGrid>
    </form>
</body>
</html>
