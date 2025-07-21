<%@ Page Language="VB" Debug="TRUE" EnableSessionState="True" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Pay Details</title>

    <script language="VB" runat="server">
        Dim conn As SqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
        Dim SortField As String
        Dim empPayMonth, empPayYear, Months,isper As String
        Dim empId As Integer
        Dim empname As String
        Dim strdate As String
        Dim strnextyear As String
        Dim strprevyear As String
        Dim ctcsum, grosssum, netsum As Double
        Dim gf As New generalFunction
			
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            gf.checkEmpLogin()
            'Format Date
            strdate = Request.QueryString("strdate")
            empId = Request.QueryString("empid")
            empname = Request.QueryString("empname")

            If strdate = "" Then
                strdate = Format(Now(), "dd-MMM-yyyy")
            End If
            
            strnextyear = Format(DateAdd(DateInterval.Year, 1, DateTime.Parse(strdate)), "dd-MMM-yyyy")
            strprevyear = Format(DateAdd(DateInterval.Year, -1, DateTime.Parse(strdate)), "dd-MMM-yyyy")

            lnkPrev.NavigateUrl = "payrolldetails.aspx?empId=" & empId & "&strdate=" & strprevyear & "&empname=" & empname
            lnkNext.NavigateUrl = "payrolldetails.aspx?empId=" & empId & "&strdate=" & strnextyear & "&empname=" & empname
            lblyear.Text = Format(DateAdd(DateInterval.Month, -11, DateTime.Parse(strdate)), "MMM-yyyy") & " to  " & Format(DateTime.Parse(strdate), "MMM-yyyy")
            '=========End Procedure======================================================================			
            If Not IsPostBack Then
			
                'Session.Add("dynoempIdsession", "0")
                'Session.Add("dynoempNamesession", "0")
	   
                empId = Request.QueryString("empid")
                empname = Request.QueryString("empname")


                lblempName.Text = empname
                lblempid.Text = empId

           
                'Session.Item("dynoempIdsession") = empId
                'Session.Item("dynoempNamesession") = empname

                hdnid.Value = empId
                hdname.Value = empname

                Dim Cmd As New SqlCommand
                Dim ds As New DataSet
   
                Dim strcn As String = ""

                conn.Open()
                Cmd.CommandText = CommandType.Text
                '07092012 Dim da As SqlDataAdapter = New SqlDataAdapter("select empPayId,empId,empPayMonth,empPayYear,empPayBasic,empPayHra,empPayConveyance,empPayMedical,empPayFood,empPaySpecial,empPayBasic*12/100 as empPayPF ,empPayBasic*12/100 as empPayEPF,empPayLTA,empPT from employeepaymaster where empid ='" & empId & "' order by emppayyear desc ", conn)
                Dim da As SqlDataAdapter = New SqlDataAdapter("select empPayId,empId,empPayMonth,empPayYear,(employeepaymaster.emppaybasic)+" +
                "(employeepaymaster.emppayhra)+(employeepaymaster.emppayconveyance)+(employeepaymaster.emppaymedical)+" +
                "(employeepaymaster.emppayfood)+(employeepaymaster.emppayspecial)+(employeepaymaster.emppaylta)+" +
                "(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster " +
                "where empid=employeepaymaster.empid) as totalctc,empPayBasic,empPayHra,empPayConveyance,empPayMedical,empPayFood," +
                "empPaySpecial,    (select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 end  from employeemaster " +
                "where empid=employeepaymaster.empid) empPayPF ,(select Case when ispf=1 then employeepaymaster.empPayBasic*12/100 else 0 " +
                "end  from employeemaster where empid=employeepaymaster.empid) empPayEPF,empPayLTA,empPT,empPayBonus,IsPerformanceBased, empInsurance " +
                "from employeepaymaster where empid ='" & empId & "' order by emppayyear desc ", conn)
                da.Fill(ds)
                DGrdpayrolldetail.DataSource = ds
                DGrdpayrolldetail.DataBind()

                isper = "False"
                  If (ds.Tables.Count > 0 And ds.Tables(0).Rows.Count > 0)
                    isper = ds.Tables(0).Rows(0)("IsPerformanceBased").ToString()
                End If
  
                If isper = "True" Then
                    isper = "(Performance Based)"
                Else
                    isper = "(Fixed)"

                End If

                conn.Close()
                Cmd.Dispose()
                Dim ds1 As New DataSet
                Dim str1 As String = "select month(paydate) as months,year(paydate) as years, (employeemaster.empname) as empname," +
                "(employeemaster.empAccountNo) as empAccountNo ,(employeepayprocessdetail.empid) as empid, " +
                "DateAdd(month, empProbationperiod, empJoiningDate) as empConfirmDate , (employeepayprocessdetail.paybasic) as basic," +
                "(employeepayprocessdetail.PayHra) as Hra,(employeepayprocessdetail.payConveyance) as Conveyance," +
                "(employeepayprocessdetail.PayMedical) as Medical,(employeepayprocessdetail.PayFood) as Food," +
                "(employeepayprocessdetail.PaySpecial) as Special,(employeepayprocessdetail.PayPF) as PF," +
                "(employeepayprocessdetail.PayInsurance) as PI," +
                "(employeepayprocessdetail.PayEPF) as EPF, (employeePayprocessdetail.payLeaveDays) as PresentDays," +
                "(employeepayprocessdetail.PayLTA) as LTA, (employeepayprocessdetail.PayPT) as PT,(employeepayprocessdetail.PayAT) as AT," +
                "(employeepayprocessdetail.Paydeduction) as deduction,(employeepayprocessdetail.PayLeave) as Leave, " +
                "(employeepayprocessdetail.PayAdvance) as Advance,(employeepayprocessdetail.Paybonus) as Bonus, " +
                "(employeepayprocessdetail.Payaddition) as Addition,(employeepayprocessdetail.PayloanInstl) as Loan, " +
                "(employeepayprocessdetail.PayOthers) as Others,(employeepayprocessdetail.payOtherDeduction) as PayOtherDedu," +
                "(employeepayprocessdetail.Payremark) as RemarkS,((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+ " +
                "(employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+ (employeepayprocessdetail.payfood)+ " +
                "(employeepayprocessdetail.payspecial)+ (employeepayprocessdetail.paylta)+(employeepayprocessdetail.paypf))  as Ctc, " +
                "((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+" +
                "(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+ " +
                "(employeepayprocessdetail.paylta)) as gross,((employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)) " +
                "as TotalAddition, ((employeepayprocessdetail.payPf)+(employeepayprocessdetail.payPT)+(employeepayprocessdetail.payAT)+" +
                "(employeepayprocessdetail.payLoanInstl)+(employeepayprocessdetail.payAdvance)+ " +
                "((employeepayprocessdetail.payLeave) * (((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+" +
                "(employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+" +
                "(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta))/Day(DateAdd(d,-1,dateadd(m,1,paydate)))))+" +
                "(employeepayprocessdetail.payDeduction)+ (employeepayprocessdetail.payOtherDeduction)+ " +
                "(employeepayprocessdetail.payInsurance)) as TotalDeduction,  " +
                "((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+" +
                "(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+" +
                "(employeepayprocessdetail.paylta)- (employeepayprocessdetail.payEpf)-(employeepayprocessdetail.payPT)-(employeepayprocessdetail.payInsurance)-" +
                "(employeepayprocessdetail.payAT)-(employeepayprocessdetail.payLoanInstl)-(employeepayprocessdetail.payAdvance)- " +
                "(employeepayprocessdetail.payOtherDeduction)- ((employeepayprocessdetail.payLeave) * " +
                "(((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+" +
                "(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+" +
                "(employeepayprocessdetail.paylta))/Day(DateAdd(d,-1,dateadd(m,1,paydate)))))-(employeepayprocessdetail.payDeduction)+" +
                "(employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)) as netSALARY from employeepayprocess," +
                "employeepayprocessdetail,employeemaster where employeepayprocessdetail.empid=employeemaster.empid and  paydate between '" +
                Format(DateAdd(DateInterval.Month, -12, DateTime.Parse(strdate)), "dd-MMM-yyyy") & "' AND '" & Format(DateTime.Parse(strdate), "dd-MMM-yyyy") &
                "' and employeepayprocess.payid=employeepayprocessdetail.payid  and " +
                "employeemaster.empid = '" & empId & "' order by paydate desc"
             
                
                'response.write(str1)
		  
                Dim da1 As SqlDataAdapter = New SqlDataAdapter(str1, conn)
                da1.Fill(ds1, "salary")
                DGrdpayrollalldetail.DataSource = ds1
                DGrdpayrollalldetail.DataBind()
            End If
            '============End procedure
        End Sub

	  
        Function getRevDate(ByVal monthStr As String, ByVal yearStr As String)
            Dim strDate As String = ""
            If IsNumeric(monthStr) Then
                strDate = Left(MonthName(monthStr), 3) & " " & yearStr
            End If
            Return strDate
        End Function

        Sub dgrdPayroll_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
	   
            Dim strY As String
            Dim dt As String
	   
            If e.Item.Cells(2).Text = Trim(Str(1)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "JAN - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(2)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "FEB - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(3)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "MAR - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(4)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "APR - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(5)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "MAY - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(6)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "JUN - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(7)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "JUL - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(8)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "AUG - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(9)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "SEP - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(10)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "OCT - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(11)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "NOV - " & strY
                dt = e.Item.Cells(2).Text
            End If
            If e.Item.Cells(2).Text = Trim(Str(12)) Then
                strY = (e.Item.Cells(3).Text)
                e.Item.Cells(2).Text = "DEC - " & strY
                dt = e.Item.Cells(2).Text
            End If
        End Sub
	
     
        Sub btnsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Response.Redirect("addpayroll.aspx?empId=" & hdnid.Value & "&empName=" & hdname.Value)
        End Sub
        Dim sum As Double
        Dim sum_price As Double
        Sub DisplayTotal(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
    
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

            ' LBLGrandTotal.text="Total:" 
            LBLGrandCtc.Text = Format(ctcsum, "0,00")
            LBLGrandGross.Text = Format(grosssum, "0,00")
            LBLGrandNet.Text = Format(netsum, "0,00")
            Session("LBLGrandNet") = LBLGrandNet.Text
	
            If e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(0).Text = "<b>Grand Total</b>"
            
			
            End If
        End Sub
        Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
            If e.CommandName = "delete" Then
                Dim id As Integer
                Dim emp As Integer
                Dim sql As String
			
                id = e.Item.Cells(16).Text
                emp = e.Item.Cells(0).Text
                sql = "delete from  employeePayMaster where  empPayId = " & id
                conn.Open()
                Dim sqldeletecommad As New SqlCommand(sql, conn)
                sqldeletecommad.ExecuteNonQuery()
                conn.Close()
                Dim sp As String
                sp = "<Script language=JavaScript>"
                sp += " alert('Delete Record s deleted ');"
                sp += "window.self.location = window.self.location; "
                sp += "</" + "script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "script123", sp)
                'dgrdPayroll_ItemDataBound
                ' DGrdpayrolldetail.bindGrid()
				
            End If
        End Sub
 

    </script>

</head>
<body ms_positioning="GridLayout">
    <table cellpadding="4" width="100%" border="1" style="border-collapse: collapse"
        bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
        <tr>
            <td align="center" bgcolor="#C5D5AE" colspan="4">
                <font face="Verdana" color="#a2921e"><b></b></font>
            </td>
            <tr>
                <td bgcolor="#C5D5AE" nowrap="nowrap" width="150" valign="top">
                    <font style="color: #A2921E; font-family: Arial" size="4">Salary Details :</font>
                </td>
                <td bgcolor="#edf2e6" width="60%" valign="top">
                    <font face="verdana" size="2"><b>
                        <asp:Label ID="lblempName" runat="server"></asp:Label>(<asp:Label ID="lblempid" runat="server"></asp:Label>)
                    </b>
                </td>
                <td bgcolor="#edf2e6" valign="top">
                    <font face="verdana" size="2"><b></b></font>
                </td>
    </table>
    <form id="Form1" method="post" runat="server">
        <asp:DataGrid ID="DGrdpayrolldetail" runat="server" AllowSorting="true" Width="100%"
            BackColor="white" BorderColor="black" ShowFooter="True" CellPadding="2" CellSpacing="0"
            Font-Name="Verdana" Font-Size="10pt" HeaderStyle-ForeColor="#A2921E" HeaderStyle-BackColor="#edf2e6"
            HeaderStyle-Font-Size="11pt" HeaderStyle-Font-Bold="True" MaintainState="true"
            FooterStyle-HorizontalAlign="Right" OnItemCommand="ItemCommand" AutoGenerateColumns="False"
            OnItemDataBound="dgrdPayroll_ItemDataBound">
            <AlternatingItemStyle BackColor="#edf2e6" />
            <Columns>
                <asp:BoundColumn HeaderText="Employee ID" Visible="false" DataField="empid" />
                <asp:TemplateColumn Visible="false">
                    <ItemTemplate>
                        <% If empPayMonth >= 1 And empPayMonth <= 12 Then%>
                        empPaymonth = Session("dynoMonthSession")
                        <% End If%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn HeaderText="Revision Date" DataField="empPayMonth" />
                <asp:BoundColumn Visible="false" DataField="empPayYear" />

                 <asp:BoundColumn HeaderText="CTC" DataField="totalCTC" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />

                <asp:BoundColumn HeaderText="Basic " DataField="empPayBasic" />
                <asp:BoundColumn HeaderText="HRA" DataField="empPayHra" />
                <asp:BoundColumn HeaderText="Conveyance" DataField="empPayConveyance" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="Medical" DataField="empPayMedical" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="Food " DataField="empPayFood" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="SP" DataField="empPaySpecial" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="PF" DataField="empPayPF" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="EPF" DataField="empPayEPF" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="LTA" DataField="empPayLTA" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="PT" DataField="empPT" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:BoundColumn HeaderText="Ins" DataField="empInsurance" ItemStyle-HorizontalAlign="right"
                    DataFormatString="{0:##,###,###}" />
                <asp:TemplateColumn HeaderText="A Bonus" ItemStyle-HorizontalAlign="right">
                    <ItemTemplate>
                         <%#Container.DataItem("empPayBonus")%> 
                          <%=isper %>
                   </ItemTemplate>
                </asp:TemplateColumn>

                <asp:BoundColumn Visible="false" HeaderText="Emp Pay ID" DataField="empPayid" />
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" CommandName="delete" runat="server" Width="80px" Text="Delete"
                            Style="font-family: Verdana; font-size: 8pt; color: #A2921E; font-weight: bold;
                            background-color: #C5D5AE"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <tr>
            <td colspan="2" height="40" bgcolor="#C5D5AE" color="#A2921E">
                <font face="Arial" color="#A2921E" bgcolor="#edf2e6"></font>
                <asp:Button ID="btnsave" OnClick="btnsave_Click" runat="server" Width="134px" Text="Add New Revision"
                    Style="font-family: Verdana; font-size: 10pt; color: #A2921E; font-weight: bold;
                    background-color: #C5D5AE"></asp:Button>
            </td>
        </tr>
        <p>
        </p>
        <p>
        </p>
        <table cellpadding="4" width="100%" style="border-collapse: collapse" bordercolor="#E8E8E8"
            bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
            <tr>
                <td align="left" bgcolor="#edf2e6" colspan="4">
                    <asp:HyperLink ID="lnkPrev" runat="server" Font-Overline="false" Font-Names="Verdana"
                        Font-Size="X-Small" ForeColor="#A2921E"><b><<</b> </asp:HyperLink>
                    <asp:Label ID="lblyear" runat="server" Font-Names="Arial" Font-Size="X-Small" ForeColor="#A2921E"></asp:Label>
                    <asp:HyperLink ID="lnkNext" runat="server" Font-Overline="false" Font-Names="verdana"
                        Font-Size="X-Small" ForeColor="#A2921E"><b>>></b></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:DataGrid ID="DGrdpayrollalldetail" runat="server" AllowSorting="true" Width="100%"
                        BackColor="white" BorderColor="black" ShowFooter="True" CellPadding="2" CellSpacing="0"
                        Font-Name="Verdana" Font-Size="10pt" HeaderStyle-ForeColor="#A2921E" HeaderStyle-BackColor="#edf2e6"
                        HeaderStyle-Font-Size="11pt" HeaderStyle-Font-Bold="True" MaintainState="true"
                        FooterStyle-HorizontalAlign="Right" AutoGenerateColumns="False" OnItemDataBound="DisplayTotal">
                        <AlternatingItemStyle BackColor="#edf2e6" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="Month">
                                <ItemTemplate>
                                    <%# getRevDate(Container.dataitem("Months"), Container.dataitem("Years"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Basic" DataField="Basic" DataFormatString="{0:##,###,###}" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="HRA" DataField="Hra" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Conv" DataField="Conveyance"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Medi" DataField="Medical" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Food" DataField="Food" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Spec" DataField="Special" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="LTA" DataField="Lta" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="EPF" DataField="Epf" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="PF" DataField="Pf" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="AT" DataField="AT" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="PT" DataField="PT" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Ins" DataField="PI" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Loan" DataField="Loan" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Adva" DataField="Advance" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Dedu" DataField="Deduction"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />

                            <asp:BoundColumn HeaderStyle-Width="67px" HeaderText="OD" DataField="PayOtherDedu"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />

                            
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Leav" DataField="Leave" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Pres" DataField="PresentDays"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Bonu" DataField="Bonus" DataFormatString="{0:##,###,###}"
                                HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="Addi" DataField="Addition"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="TotA" DataField="TotalAddition"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:BoundColumn HeaderStyle-Width="47px" HeaderText="TotD" DataField="TotalDeduction"
                                DataFormatString="{0:##,###,###}" HeaderStyle-HorizontalAlign="Center" />
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <HeaderTemplate>
                                    CTC</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="CTC" readonly="true" runat="server" Width="70px" Text='<%# DataBinder.Eval(container.dataitem,"CTC","{0:##,###,###}")  %>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%=LBLGrandCtc.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <HeaderTemplate>
                                    Gross</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Gross" readonly="true" runat="server" Width="60px" Text='<%# DataBinder.eval(container.dataitem,"Gross","{0:##,###,###}") %>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%= LBLGrandGross.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <HeaderTemplate>
                                    NetSalary</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="NetSalary" readonly="true" runat="server" Width="70px" Text='<%# databinder.eval(container.dataitem,"NetSalary","{0:##,###,###}") %>'
                                        textmode="singleline">
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Center" Font-Bold="True"></FooterStyle>
                                <FooterTemplate>
                                    <%=LBLGrandNet.text%>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    Remark</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="remark" runat="server" Width="10px" Text=' <%#container.dataitem("Remarks") %>'>   </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
        <table id="tblTotal" cellspacing="0" cellpadding="0" width="1550px" border="0" runat="server">
            <tr>
                <td valign="middle" align="left" width="1018px" colspan="1" rowspan="1">
                    <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" font-Name="Verdana"
                        Font-Size="10" Visible="false"></asp:Label></td>
                <td valign="middle" align="right" width="58.4px" colspan="1" rowspan="1">
                    <asp:Label ID="LBLGrandCtc" runat="server" Font-Bold="True" font-Name="Verdana" Font-Size="10"
                        Visible="false"></asp:Label></td>
                <td valign="middle" align="left" width="33.2px" colspan="1" rowspan="1">
                    <asp:Label ID="LBLGrandGross" runat="server" Font-Bold="True" font-Name="Verdana"
                        Font-Size="10" Visible="false"></asp:Label></td>
                <td valign="middle" align="left" width="98.4px" colspan="1" rowspan="1">
                    <asp:Label ID="LBLGrandNet" runat="server" Font-Bold="True" font-Name="Verdana" Font-Size="10"
                        Visible="false"></asp:Label></td>
            </tr>
        </table>
        <input type="hidden" id="detail" name="detail">
        <input type="hidden" id="hdnid" runat="server">
        <input type="hidden" id="hdname" runat="server">
        <input type="hidden" id="hdbasic" runat="server">
    </form>
</body>
</html>
