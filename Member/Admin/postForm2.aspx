<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Register TagPrefix="ucl" TagName="adminMenu" Src="~/controls/adminMenu.ascx" %>

<script runat="server">
    Dim conn As New SqlConnection
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Public str As String
    Dim gs As New generalFunction
    Sub Page_Load()
     spanLocation.InnerHtml= Session("DivLocation")
        gs.checkEmpLogin()
        Session.Add("dynoprocessSession", "0")
        PNotes.Text = Session("dynoReportNotessession")
        LBLprocess1.Text = "Salary Generated For The Month " & "" & Session.Item("dynoProcesssessionmonth") & "   " & Session.Item("dynoProcesssessionyear")
        LBLdays1.Text = "Total No Of Days " & Session.Item("dynoProcesssessiondays")
        ctcsum = 0
        grosssum = 0
        netsum = 0
        dgrdReport.DataSource = Session("dynoReportsession")
        dgrdReport.DataBind()
    End Sub


    Sub btnadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim str As String
        Dim strSql As String
        Dim ddate As String
        Dim strpayid As String
        Dim tb As New System.Data.DataTable
        tb = Session("dynoReportsession")
        ddate = DateTime.Now
        str = Session("dynoReportNotessession")
        conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        conn.Open()
        Dim dtsal As String
        Dim dtdesc As String = ""
        Dim dtyear As String
        Dim dtlastmn As DateTime
        ddate = DateAdd("m", -1, ddate)
        dtsal = Month(ddate)
        dtlastmn = Year(ddate) & "-" & dtsal & "-01"
        dtlastmn = CType(dtlastmn, Date)
        dtyear = Year(ddate)

        Select Case dtsal
            Case 1
                dtdesc = "January"
            Case 2
                dtdesc = "February"
            Case 3
                dtdesc = "March"
            Case 4
                dtdesc = "April"
            Case 5
                dtdesc = "May"
            Case 6
                dtdesc = "June"
            Case 7
                dtdesc = "July"
            Case 8
                dtdesc = "August"
            Case 9
                dtdesc = "September"
            Case 10
                dtdesc = "October"
            Case 11
                dtdesc = "November"
            Case 12
                dtdesc = "December"
        End Select

        hdmonth.Value = dtdesc
        hdyear.Value = dtyear

        strSql = "select * from employeePayProcess where month(payDate) ='" & Month(ddate) & "'"
        Dim cmdcheck As SqlCommand = New SqlCommand(strSql, conn)
        Dim drcheck As SqlDataReader
        drcheck = cmdcheck.ExecuteReader
        If (drcheck.Read) Then
            Session.Item("dynoProcesssession") = drcheck("payDate")
		
        End If
        conn.Close()
        Dim a As Integer
        conn.Open()
        strSql = "select * from employeePayProcess where month(payDate) ='" & Month(ddate) & "'"
        Dim cmdcheck1 As SqlCommand = New SqlCommand(strSql, conn)
	
        a = cmdcheck.ExecuteNonQuery
        conn.Close()

        If a = -1 Then
	

            
	
            Dim sql As String
            sql = "insert into employeePayProcess (payComment,payDate) Values('" & Trim(Replace(str, "'", "''")) & _
                "' ,'" & dtlastmn.Date.ToString("yyyy-MMM-dd") & "')"
          
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
            Dim paddcmd As SqlCommand = New SqlCommand(sql, conn)
            paddcmd.ExecuteNonQuery()
		
            sql = "select * from employeePayProcess where month(payDate) ='" & Month(ddate) & "' AND Year(payDate) =" & Year(ddate)
            Dim cmd As SqlCommand = New SqlCommand(sql, conn)
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
		
            If (dr.Read) Then
                Session.Item("dynoflgsession") = dr("payID")
            End If
            strpayid = (Session.Item("dynoflgsession"))
            dr.Close()
            Dim i As Integer
            For i = 0 To tb.Rows.Count - 1
                Dim strEmpName As String
                Dim strEmpId As String
                Dim strBasic As String
                Dim strHra As String
                Dim strConveyance As String
                Dim strMedical As String
                Dim strFood As String
                Dim strSpecial As String
                Dim strLTA As Integer
                Dim strPF As Integer
                Dim strEPF As Integer
                Dim strAT As String
                Dim strPT As String
                Dim strInsurance As String
                Dim strLoan As String
                Dim strAdvance As String
                Dim strDeduction As String
                Dim strLeave As String
                Dim strPresentDays As String
                Dim strTotalDays As String
                Dim strBonus As String
                Dim strAddition As String
                Dim strRemarks As String
                Dim strOtherDiduction As String
                strEmpName = (tb.Rows(i)(0))
                strEmpId = (tb.Rows(i)(1))
                strBasic = (tb.Rows(i)(2))
                strHra = (tb.Rows(i)(3))
                strConveyance = (tb.Rows(i)(4))
                strMedical = (tb.Rows(i)(5))
                strFood = (tb.Rows(i)(6))
                strSpecial = (tb.Rows(i)(7))
                strLTA = (tb.Rows(i)(8))
                strEPF = (tb.Rows(i)(9))
                strPF = (tb.Rows(i)(10))
                strAT = (tb.Rows(i)(11))
                strPT = (tb.Rows(i)(12))
                strInsurance = (tb.Rows(i)(13))
                strLoan = (tb.Rows(i)(14))
                strAdvance = (tb.Rows(i)(15))
                strDeduction = (tb.Rows(i)(16))
                strOtherDiduction = (tb.Rows(i)(17))
                strLeave = (tb.Rows(i)(18))
                strPresentDays = (tb.Rows(i)(19))
                strTotalDays = (tb.Rows(i)(20))
                strBonus = (tb.Rows(i)(21))
                strAddition = (tb.Rows(i)(22))
                strRemarks = (tb.Rows(i)(28))
                If strRemarks <> "" Then
                    strRemarks = Replace(strRemarks, "'", "''")
                End If
                strSql = "insert into employeepayprocessdetail (payid,empid,PayBasic,PayHra,PayConveyance,PayMedical,PayFood,PaySpecial,payLTA,PayPF,PayEPF,PayAT,PayPT,PayInsurance,payLoanInstl,payBonus,payAddition,payAdvance,payLeave,payleaveDays,payDeduction,payOtherDeduction,payremark) Values (" & strpayid & "," & strEmpId & ",'" & strBasic & "','" & strHra & "','" & strConveyance & "','" & strMedical & "','" & strFood & "','" & strSpecial & "','" & strLTA & "','" & strPF & "','" & strEPF & "','" & strAT & "','" & strPT & "','" & strInsurance & "','" & strLoan & "','" & strBonus & "','" & strAddition & "','" & strAdvance & "','" & strLeave & "','" & strPresentDays & "','" & strDeduction & "','" & strOtherDiduction & "','" & strRemarks & "')"
                conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                conn.Open()
                Dim addcmd As SqlCommand = New SqlCommand(strSql, conn)
                addcmd.ExecuteNonQuery()
            Next
            Dim tp As String = ""
            tp += "<Script language=JavaScript>"
            tp += " alert('Record Saved '); "
            tp += "</" + "script>"
            ClientScript.RegisterStartupScript(Me.GetType(), "script123", tp)
        End If

        Response.Redirect("../admin/payProcess.aspx")

    End Sub


    Sub btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Response.Redirect("../admin/payProcess.aspx")
    End Sub

    Dim ctcsum, grosssum, netsum As Double

    Sub dgrdreport_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim tctc As Label
            tctc = e.Item.FindControl("CTC")
            Dim cellValue As String = (tctc.Text) & ""
		
            'DataBinder.Eval(e.Item.DataItem, "CTC")
		
            If cellValue <> "" Then
                ctcsum += Convert.ToDouble(cellValue)
            End If


            Dim tgross As Label
            tgross = e.Item.FindControl("Gross")
            Dim cellValue1 As String = (tgross.Text) & ""


            'DataBinder.Eval(e.Item.DataItem, "GROSS")
            If cellValue1 <> "" Then
                grosssum += Convert.ToDouble(cellValue1)
            End If

            Dim tnetsalary As Label
            tnetsalary = e.Item.FindControl("NetSalary")
            Dim cellValue2 As String = (tnetsalary.Text) & ""
		
            'DataBinder.Eval(e.Item.DataItem, "NETSALARY")
            If cellValue2 <> "" Then
  	   
                netsum += Convert.ToDouble(cellValue2)
		
		
            End If

        End If
 	
        Session("str") = netsum
		
        lblGrandTotal.Text = "Grand Total :"
        LBLGrandCtc.Text = Format(ctcsum, "0,00")
        LBLGrandGross.Text = Format(grosssum, "0,00")
        LBLGrandNet.Text = Format(netsum, "0,00")
        Session("LBLGrandNet") = LBLGrandNet.Text
	
    End Sub
	

    Sub f1()
        Dim l1 As String
        l1 = Format(netsum, "0,00")
	
    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
     <title>Pay Process</title>
</head>
<body>
<ucl:adminMenu ID="adminMenu" runat="server" />
    <form name="form" id="form" runat="server">
    <table id="Table4" cellspacing="0" cellpadding="0" width="100%" align="left" >
        <tr valign="top" height="10%" bgcolor="#EDF2E6">
            <td colspan="2" height="30">
                <p align="left">
                    <font face="Verdana" color="#A2921E"><b><span style="font-size: 14pt">Pay Comment</span></b></font><br>
            </td>
        </tr>
        <tr valign="top" height="10%">
            <td width="100%" colspan="2" align="left" bgcolor="#edf2e6">
                <asp:Label ID="PNotes" readonly="true" runat="server" rows="2" cols="70" BgColor="#edf2e6"
                    face="Verdana" Font-Bold Font-Size="11" Font-Name="verdana"></asp:Label>
            </td>
        </tr>
        <tr width="100%" height="10%" colspan="2" align="left" bgcolor="#EDF2E6">
            <td colspan="2" height="40">
                <font face="Verdana" color="#A2921E"></font>
                <asp:Button ID="btnadd" OnClick="btnadd_Click" runat="server" Width="90px" Text="Submit"
                    Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                    background-color: #C5D5AE"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                <asp:Button ID="btncancel" OnClick="btncancel_Click" runat="server" Width="90px"
                    Text="Cancel" Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                    background-color: #C5D5AE"></asp:Button>

                 <div style="text-align:right  ; font-family:Verdana ;"   > <b><span id="spanLocation" runat ="server">Mumbai</span> </b></div>
            </td>
        </tr>
        <tr>
            <td height="10%" align="left" bgcolor="#c5d5ae" colspan="0">
                <b><font face="Verdana" color="#a2921e" size="2"></font></b>
                <asp:Label font-Name="Verdana" Font-Size="12" Font-Bold="True" ID="LBLprocess1" runat="server"
                    Text=" ">
                </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label Font-Size="12" Font-Bold="True" ID="LBLdays1" runat="server" Text=" "
                    font-Name="Verdana">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
        <tr height="60%" valign="top">
            <td height="100%">
                <asp:DataGrid ID="dgrdReport" CssClass="text" runat="server" Font-Size="10pt" Font-Name="Verdana"
                    BackColor="White" Font-Names="Verdana" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right"
                    Width="1300px" autopostback="true" OnItemDataBound="dgrdReport_ItemDataBound"
                    HeaderStyle-Font-Size="10pt" HeaderStyle-ForeColor="#A2921E" HeaderStyle-Font-Bold="True"
                    AllowSorting="True" HeaderStyle-BackColor="#C5D5AE" CellPadding="0" BorderColor="#808080"
                    ShowFooter="True">
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                            <HeaderTemplate>
                                EmpName</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="txtempname" readonly="true" runat="server" Width="90px" Text='<%# container.dataitem("empname")%>'
                                    textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="left" Font-Bold="True"></FooterStyle>
                            <FooterTemplate>
                                <%= LBLGrandTotal.text%>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9"></ItemStyle>
                            <HeaderTemplate>
                                EmpId</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="txtEmpId" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("empId")%>'
                                    textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                BasIc</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="basic" runat="server" Width="50px" textmode="singleline">
                             <%# DataBinder.Eval(container.dataitem,"Basic","{0:##,###,###}") %> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                HRA</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="HRA" readonly="true" runat="server" Width="70px" Text='<%#        DataBinder.Eval(container.dataitem,"HRA","{0:##,###,###}") %>'
                                    textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Conveya</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Conveyance" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Conveyance")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Medical</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Medical" readonly="true" runat="server" Width="30px" Text='<%# container.dataitem("Medical")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Food</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Food" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Food")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Speical</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Specail" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Special")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                LTA</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="LTA" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("LTA")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                EPF</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="EPF" readonly="true" runat="server" Width="50px" Text='<%# DataBinder.Eval(container.dataitem,"EPF")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                PF</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="PF" readonly="true" runat="server" Width="50px" Text='<%# DataBinder.Eval(container.dataitem,"PF")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                AdvTax</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="AT" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("AT")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                PT</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="PT" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("PT")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Insurance</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Insurance" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Insurance")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Loan</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Loan" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Loan")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Advance</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Advance" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Advance")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                IncomeTax Deductions</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="DOthers" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("IncomeTax Deduction")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Other Deduction</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="DOthersdeduction" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("other deduction")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#C6E2FF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#C6E2FF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Leave</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="leave" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Leave")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Present</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Presentdays" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Presentdays")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn Visible="false">
                            <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Total days</HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox Width="40px" ID="tataldays" runat="server" Text='<%# container.dataitem("TotalDays") %>'
                                    TextMode="singleline">
                                </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Bonus</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Bonus" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Bonus")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#FFDFDF" HorizontalAlign="Center"></HeaderStyle>
                            <ItemStyle BackColor="#FFDFDF" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Additions</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="AOthers" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("Addition")%>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline">
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Total-Addition</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="TotalAddition" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("TotalAddition") %>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline" Font-Bold>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Total-Deduction</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="TotalDeduction" readonly="true" runat="server" Width="50px" Text='<%# container.dataitem("TotalDeduction") %>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline" Font-Bold>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                CTC</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="CTC" readonly="true" runat="server" Width="70px" Text='<%# container.dataitem("CTC") %>'
                                    DataFormatString="{0:##,###,###}" textmode="singleline" Font-Bold>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                            <FooterTemplate>
                                <%=LBLGrandCtc.text%>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                Gross</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Gross" readonly="true" runat="server" Width="60px" Text='<%# container.dataitem("Gross")%>'
                                    textmode="singleline" Font-Bold>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                            <FooterTemplate>
                                <%= LBLGrandGross.text%>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                            <ItemStyle BackColor="#E9E9E9" HorizontalAlign="Center"></ItemStyle>
                            <HeaderTemplate>
                                NetSalary</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="NetSalary" readonly="true" runat="server" Width="70px" Text='<%# DataBinder.Eval(container.dataitem,"NetSalary","{0:##,###,###}") %>'
                                    textmode="singleline" Font-Bold>
                                </asp:Label>
                            </ItemTemplate>
                            <FooterStyle BackColor="#E9E9E9" HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                            <FooterTemplate>
                                <%=LBLGrandNet.text%>
                            </FooterTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                Remark</HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="remark" runat="server" Width="30px" Text=' <%#container.dataitem("Remarks") %>'>   </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <table id="tblTotal" cellspacing="0" cellpadding="0" width="1550px" border="0" runat="server">
                    <tr>
                        <td valign="middle" align="left" width="1018px" colspan="1" rowspan="1">
                            <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" font-Name="Verdana"
                                Font-Size="10" Visible="false"></asp:Label>
                        </td>
                        <td valign="middle" align="right" width="58.4px" colspan="1" rowspan="1">
                            <asp:Label ID="LBLGrandCtc" runat="server" Font-Bold="True" font-Name="Verdana" Font-Size="10"
                                Visible="false"></asp:Label>
                        </td>
                        <td valign="middle" align="left" width="33.2px" colspan="1" rowspan="1">
                            <asp:Label ID="LBLGrandGross" runat="server" Font-Bold="True" font-Name="Verdana"
                                Font-Size="10" Visible="false"></asp:Label>
                        </td>
                        <td valign="middle" align="left" width="98.4px" colspan="1" rowspan="1">
                            <asp:Label ID="LBLGrandNet" runat="server" Font-Bold="True" font-Name="Verdana" Font-Size="10"
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdmonth" runat="server">
    <input type="hidden" id="hdyear" runat="server">
    </form>
</body>
</html>
