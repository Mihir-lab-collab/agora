<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Banking Statment</title>
    <script language="javascript" type="text/javascript">
	function Chknumber() 
	{		
	    
		var msg = "Do not leave the :\n\n";
		var key = "";
		if (document.getElementById("accountno").value.length == 0) key+=" -- Account Number\n";
		if (document.getElementById("chno").value.length == 0) key+=" -- Check Number\n";
		if (key != "") 
		{
			alert(msg + key + "\nUnable to submit !!")
			return false
		} 
	}
	
	function chkdate() 
	{
		var msg = "Do not leave the :\n\n";
		var key = "";
		if (document.getElementById("accountno").value.length == 0) key+=" -- Account Number\n";
		if (document.getElementById("txtcalender").value.length == 0) key+=" -- Date\n";
		if (key != "") 
		{
			alert(msg + key + "\nUnable to submit !!")
			return false
		} 
	}
    </script>

    <script language="VB" runat="server">
        Dim gf As New generalFunction
        Public dsCover As DataTable
        
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            gf.checkEmpLogin()
            Dim Cmd As New SqlCommand()
            
            Dim strSQL As String
            Dim ddate, dyear, dMonth, intmonth, intyear As String
            Session.Add("currentslipdate", DateAdd("m", -1, Now))
            ddate = Session("currentslipdate")
            dMonth = Month(ddate)
            intmonth = Convert.ToInt32(dMonth)
            dyear = Year(ddate)
            intyear = Convert.ToInt32(dyear)
            lbldate.Text = DateTime.Now.ToString("dd-MMM-yyyy")
            Dim strmonth As String
            Dim stryear As String
            strmonth = MonthName(Month(ddate))
            stryear = Year(ddate)
            Dim strmonth1 As String
            Dim stryear1 As String
            '' to display current date
            strmonth1 = strmonth
            lblmonth.Text = strmonth1
            stryear1 = stryear
            lblyear.Text = stryear1
            Dim conn As New SqlConnection
            Dim daCover As SqlDataAdapter
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            conn.Open()
		
            strSQL = "select employeemaster.empName ,employeemaster.empAccountNo ,employeemaster.empid," & _
            "(employeepayprocessdetail.paybasic + employeepayprocessdetail.payhra + " & _
            "employeepayprocessdetail.payconveyance + employeepayprocessdetail.paymedical + " & _
            "employeepayprocessdetail.payfood + employeepayprocessdetail.payspecial + " & _
            "employeepayprocessdetail.paylta) as gross,(employeepayprocessdetail.payBonus + " & _
            "employeepayprocessdetail.payAddition) as TotalAddition,((employeepayprocessdetail.paybasic)+" & _
            "(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+" & _
            "(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+" & _
            "(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta)- " & _
            "(employeepayprocessdetail.payEpf)-(employeepayprocessdetail.payPT)-(employeepayprocessdetail.payInsurance)-(employeepayprocessdetail.payAT)-" & _
            "(employeepayprocessdetail.payLoanInstl)-(employeepayprocessdetail.payAdvance)-" & _
            "((employeepayprocessdetail.payLeave) * (((employeepayprocessdetail.paybasic)+" & _
            "(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+" & _
            "(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+" & _
            "(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta))/" & _
            Session.Item("dynoProcesssessiondays") & "))-(employeepayprocessdetail.payDeduction)+" & _
             "(employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)-(employeepayprocessdetail.payOtherDeduction)) as oldnetSALARY,floor(( select Case when EMP.ispf=1 then ((employeepayprocessdetail.paybasic) * 100/40 )-((employeepayprocessdetail.paybasic) * 12/100)-(employeepayprocessdetail.payPT)-(employeepayprocessdetail.payInsurance) else ((employeepayprocessdetail.paybasic) * 100/55 )-(employeepayprocessdetail.payPT) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid))as netSALARYOld,floor(( select Case when EMP.ispf=1 then  (employeepayprocessdetail.paybasic) * 100/40 else  (employeepayprocessdetail.paybasic+employeepayprocessdetail.payhra+ employeepayprocessdetail.payconveyance+employeepayprocessdetail.paymedical+employeepayprocessdetail.payfood+employeepayprocessdetail.payspecial+employeepayprocessdetail.paylta ) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid) + (((employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)))-(((employeepayprocessdetail.payPf)+(employeepayprocessdetail.payPT)+(employeepayprocessdetail.payInsurance)+(employeepayprocessdetail.payAT)+(employeepayprocessdetail.payLoanInstl)+(employeepayprocessdetail.payAdvance)+ ((employeepayprocessdetail.payLeave) * (((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta))/" & Session.Item("dynoProcesssessiondays") & "))+(employeepayprocessdetail.payDeduction)+(employeepayprocessdetail.payOtherDeduction)))) as netSALARY from " & _
            "employeepayprocessdetail,employeemaster,employeepayprocess where employeepayprocessdetail.empid=" & _
            "employeemaster.empid and  month(paydate) =  '" & intmonth & "'  and year(paydate) =  '" & intyear & _
            "' and employeepayprocess.payid=employeepayprocessdetail.payid  and employeemaster.empid in(" & _
            Session("store") & ")"

strSQL = "select empName ,empAccountNo ,employeemaster.empid,Round(paybasic+ payhra+ payconveyance+ paymedical+ payfood+ payspecial+ paylta+payBonus+payAddition-(payPf+payPT+payInsurance+payAT+payLoanInstl+payAdvance+ (payLeave *(paybasic+payhra+payconveyance+paymedical+payfood+payspecial+paylta)/31)+payDeduction+payOtherDeduction),0) as netSALARY from employeepayprocessdetail,employeemaster,employeepayprocess where employeepayprocessdetail.empid=employeemaster.empid and month(paydate) =  '" & intmonth & "'  and year(paydate) =  '" & intyear & "' and employeepayprocess.payid=employeepayprocessdetail.payid  and employeemaster.empid in(" & Session("store") & ")"

	'Response.Write (strSQL)
	
            daCover = New SqlDataAdapter(strSQL, conn)
            dsCover = New DataTable
            
            daCover.Fill(dsCover)
            dgrdCLetter1.DataSource = dsCover
            dgrdCLetter1.DataBind()
            Session("grid") = dsCover
            
            ViewState("bankStatement") = dsCover
            conn.Close()
            Session("payid") = ""
        End Sub

        Dim sum As Double
        Dim sum_price As Double
        
        Sub DisplayTotal(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim cellValue As String = DataBinder.Eval(e.Item.DataItem, "netSALARY") & ""
                If cellValue <> "" Then
                    sum += Convert.ToDouble(cellValue)
                End If
            ElseIf e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(1).Text = "<b>Grand Total</b>"
                e.Item.Cells(4).Text = "<b>" & Format(sum, "0,00") & "</b>"
                lblSum.Text = Format(sum, "0,00.00")
            End If
        End Sub

        Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If (Page.IsValid) Then
                Session("Accountno") = accountno.Value
                Session("Checkno") = chno.Text
                Session("totalsum") = lblSum.Text
                Session("calenderdate") = txtcalender.Text
                Response.Redirect("../admin/BankingStatmentprint.aspx")
            End If
        End Sub

        Protected Sub btnExport_Click(sender As Object, e As EventArgs)
            Dim StrTo As String
            Dim StrLast As String
            StrTo = "To,                              Date : " + lbldate.Text + " " + System.Environment.NewLine +
                    "The Manager," + System.Environment.NewLine +
                    "Axis Bank," + System.Environment.NewLine +
                    "Vashi Branch," + System.Environment.NewLine +
                    "Navi Mumbai." + System.Environment.NewLine + System.Environment.NewLine +
                    "Subject: Credit Of Salary A/C No." + System.Environment.NewLine + System.Environment.NewLine +
                    "Sir," + System.Environment.NewLine + System.Environment.NewLine +
                    "Please find enclose here with Cheque No. " + chno.Text + "   of Rs. " + lblSum.Text + "  only drawn on " + txtcalender.Text + "   towards salary of the employee for the month of " + lblmonth.Text + " " + lblyear.Text + System.Environment.NewLine +
                    "Please credit salary as per following details," + System.Environment.NewLine
            
            StrLast = "Kindly do the needfull and acknowledge." + System.Environment.NewLine + System.Environment.NewLine +
                      "Thanking you," + System.Environment.NewLine + System.Environment.NewLine +
                      "Yours Sincerely," + System.Environment.NewLine + System.Environment.NewLine +
                      "For Intelgain Technologies Pvt. Ltd.." + System.Environment.NewLine +System.Environment.NewLine +
                      "Director " + System.Environment.NewLine


           
            dsCover = ViewState("bankStatement")
'            dsCover.Columns.Remove("netSALARYOld")
'            dsCover.Columns.Remove("oldnetSALARY")
'            dsCover.Columns.Remove("gross")
'            dsCover.Columns.Remove("TotalAddition")
            Dim attachment As String = "attachment; filename=" + "BankStatment_" + DateTime.Now.ToString("MMMM") + DateTime.Now.Year.ToString() + ".xls"
            Response.ClearContent()
            Response.AddHeader("content-disposition", attachment)
            Response.ContentType = "application/vnd.ms-excel"
            Dim tab As String = ""
             
            Response.Write(StrTo)   '.Replace(",", " ")
            Response.Write(System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine)
            For Each dc As DataColumn In dsCover.Columns
                Response.Write(tab + dc.ColumnName)
                tab = vbTab
            Next
            Response.Write(vbLf)
            Dim i As Integer
            For Each dr As DataRow In dsCover.Rows
                tab = ""
                For i = 0 To dsCover.Columns.Count - 1
                    Response.Write(tab & dr(i).ToString())
                    tab = vbTab
                Next
                Response.Write(vbLf)
            Next
            Response.Write(System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine)
            Response.Write(StrLast)
            Response.[End]()
        End Sub
        
    </script>

    <script language="JavaScript" src="/includes/calender.js"></script>

</head>
<body topmargin="20px">
    <form id="Form1" runat="server">
        <p>
            &nbsp;</p>
        <table id="table1" cellspacing="0" cellpadding="0" width="100%" align="left" height="100%">
            <tr>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</tr>
            <tr>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</tr>
            <tr>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</tr>
            <tr>
            </tr>
            <tr>
                <td width="100%">
                    <font style="color: black; font-family: Times New Roman" size="3">To, &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Date
                        :
                        <asp:Label ID="lbldate" runat="server" /><br>
                        The Manager,<br>
                        Axis Bank,<br>
                        Vashi Branch,<br>
                        Navi Mumbai.<br>
                        <br>
                    </font>
                </td>
            </tr>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman; font-weight: bolder" size="3">
                        <b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject: Credit Of Salary
                            A/C No.
                            <input type="text" id="accountno" runat="server" size="20" value="72010200002639" />
                            <asp:RegularExpressionValidator runat="server" Display="dynamic" ControlToValidate="accountno"
                                ErrorMessage="Enter Valid A/C No." ValidationExpression="[0-9]{9,15}" />
                        </b></font>
                    <td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br>
                    Sir,
                </td>
            </tr>
            <br>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3">Please find enclose
                        here with Cheque No.
                        <asp:TextBox ID="chno" runat="server" Width="90px" Font-Name="Times New Roman" Font-Size="12" />
                        of Rs.<b>
                            <asp:TextBox ID="lblSum" runat="server" align="right" DataFormatString="{#.00}" ReadOnly="true"
                                Style="text-align: right; font-weight: bold; width: 70px; border-top-style: none;
                                border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF;
                                border-bottom-style: none" size="10"></asp:TextBox>
                        </b>only drawn on
                        <asp:TextBox ID="txtcalender" runat="server" Width="70px" />
                        towards salary of the employee for the month of &nbsp
                        <asp:Label ID="lblmonth" runat="server" font Style="color: black; font-family: Times New Roman"
                            size="12" />,
                        <asp:Label ID="lblyear" runat="server" font Style="color: black; font-family: Times New Roman"
                            size="12" />.<br>
                        Please credit salary as per following details, </font>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;
                    <asp:DataGrid ID="dgrdCLetter1" runat="server" BorderColor="Black" Font-Size="12pt"
                        font Style="color: black; font-family: Times New Roman" BackColor="White" AutoGenerateColumns="False"
                        FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="11pt" CellPadding="2"
                        ShowFooter="True" OnItemDataBound="DisplayTotal" Width="90%">
                        <ItemStyle VerticalAlign="Top" Font-Size="11pt"></ItemStyle>
                        <HeaderStyle Font-Bold="True" Width="70px"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn HeaderText="Sr. No.">
                                <ItemTemplate>
                                    <%# Container.ItemIndex+1%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn ItemStyle-Wrap="false">
                                <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Font-Bold="true"></ItemStyle>
                                <HeaderTemplate>
                                    Employee Name</HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="txtempname" readonly="true" runat="server" Width="90px" Text='<%# container.dataitem("empName")%>'
                                        textmode="singleline"> </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn HeaderText="Empolyee Code" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left"
                                DataField="empid">
                                <HeaderStyle Width="20%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="Account Number" ItemStyle-Font-Bold="true" DataField="empAccountNo"
                                HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                                <HeaderStyle Width="25%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="Net Salary">
                                <ItemStyle Width="23%" HorizontalAlign="Right"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                <ItemTemplate>
                                    <input id="lblTotal" style="text-align: right; width: 72px; border-top-style: none;
                                        border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF;
                                        border-bottom-style: none" readonly type="text" size="15" value='<%# cdbl(DataBinder.Eval(Container,"DataItem.netSALARY")).ToString("##,###,###") %>'
                                        dataformatstring="{0:##,###,###}" name="lblTotal">

                                   <%--  <input id="lblTotal" style="text-align: right; width: 72px; border-top-style: none;
                                        border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF;
                                        border-bottom-style: none" readonly type="text" size="15" value='<%# DataBinder.Eval(Container, "DataItem.netSALARY")%>'
                                        dataformatstring="{0:##,###,###}" name="lblTotal">--%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>&nbsp;&nbsp;&nbsp;
                </td>
                <tr>
                    <td>
                        <font style="color: black; font-family: Times New Roman" size="3" />Kindly do the
                        needfull and acknowledge.<br>
                        <br>
                        Thanking you,</font><br>
                    </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3" />Yours Sincerely,<br>
                    <br>
                    </font></td>
            </tr>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3"><b>For  Intelgain 
                        Technologies Pvt. Ltd..</b></font></td>
            </tr>
            <br>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3"><b>Director</b></font></td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnPrint" OnClick="btnprint_click" Width="80px" Text="Print" runat="server">
                    </asp:Button>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Button ID="btnExport" Width="120px" runat="server" Text="Export to Excel" OnClick="btnExport_Click">
                    </asp:Button>
                </td>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
