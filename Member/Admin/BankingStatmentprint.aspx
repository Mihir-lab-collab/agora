<%@ Page Language="VB" Debug="TRUE" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.IO" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Banking Statment Print</title>

    <script language="javascript">
	    function pageprint()
		{
		    window.print()
		}
    </script>

    <script language="VB" runat="server">
        Dim gf As New generalFunction
        Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
            gf.checkEmpLogin()
            Dim Cmd As New SqlCommand()
            Dim dDate As String = Session("sessionDDate")
            Dim ds As DataTable
            Dim netsalTotal As Double
            Dim strmonth As String
            Dim stryear As String
            Dim strmonth1 As String
            Dim stryear1 As String
            netsalTotal = Session("LBLGrandNet")
            
            ds = Session("grid")
            'ds = New DataSet()
            'ds.Tables.Add(Session("grid"))
            
            dgrdPrint.DataSource = ds
            dgrdPrint.DataBind()
            lblaccountno.Text = Session("accountno")
            lblchno.Text = Session("chno")
            lbldate.Text = DateTime.Now.ToString("dd-MMM-yyyy")
            strmonth = MonthName(Month(dDate))
            stryear = Year(dDate)
            strmonth1 = strmonth
            lblmonth.Text = strmonth1
            stryear1 = stryear
            lblyear.Text = stryear1
            lbltotal.Text = Session("NetAmount")
            lblaccountno.Text = Session("Accountno")
            lblchno.Text = Session("Checkno")
            lbltotal.Text = Session("totalsum")
            lbldatemonth.Text = Session("calenderdate")
            Session("grid") = ""
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
            End If
        End Sub

        Dim amount As Single = 0
        Sub amounttotal(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
            If e.Item.ItemType = ListItemType.Item Or _
               e.Item.ItemType = ListItemType.AlternatingItem Then
                amount += Convert.ToSingle(DataBinder.Eval(e.Item.DataItem, "NetSalary"))
            ElseIf e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(3).Text = String.Format("{0:##,###,###}", amount)
            End If
        End Sub
    </script>

</head>
<body leftmargin="10px" topmargin="20px" onload="pageprint()">
    <form id="postform" name="postform" method="post" runat="server">
        <table id="table1" cellspacing="0" cellpadding="0" width="85%" align="left" hight="100%">
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
                    <font style="color: black; font-family: Times New Roman" size="3">To, &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Date :
                        <asp:Label ID="lbldate" runat="server" /><br>
                        The Manager,<br>
                        Axis Bank,<br>
                        Vashi Branch,<br>
                        Navi Mumbai.<br>
                        <br>
                    </font>
                </td>
            </tr>
            <br>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman; font-weight: bolder" size="3">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject : Credit Of
                        Salary A/C No. <b>
                            <asp:Label ID="lblaccountno" runat="server" Width="100px"></asp:Label></b></font>
                <td>
                    <tr>
                    </tr>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br>
                    Sir,
                </td>
                <br>
            </tr>
            <br>
            <tr style="width: 90px">
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3">Please find enclose
                        here with Cheque No . <b>
                            <asp:Label ID="lblchno" runat="server" />
                        </b>of Rs.<b>
                            <asp:Label ID="lbltotal" runat="server" font Style="color: black; font-family: Times New Roman"
                                size="3" />
                        </b>only drawn on&nbsp; <b>
                            <asp:Label ID="lbldatemonth" runat="server" />
                        </b>towards salary of the employee for the month of <b>&nbsp;
                            <asp:Label ID="lblmonth" runat="server" font Style="color: black; font-family: Times New Roman;"
                                size="3" />,
                            <asp:Label ID="lblyear" runat="server" font Style="color: black; font-family: Times New Roman"
                                size="3" />.</b><br>
                        Please credit salary as per following details, </font>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <td width="100%" colspan="2">
                <asp:DataGrid ID="dgrdPrint" runat="server" BorderColor="Black" Font-Size="12pt"
                    font Style="color: black; font-family: Times New Roman" BackColor="White" AutoGenerateColumns="False"
                    FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="11pt" CellPadding="2"
                    ShowFooter="True" OnItemDataBound="DisplayTotal" Width="100%">
                    <ItemStyle VerticalAlign="Top" Font-Size="11pt"></ItemStyle>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn HeaderText="Sr. No.">
                            <ItemTemplate>
                                <%# Container.ItemIndex+1%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn ItemStyle-Wrap="false" >
                            <HeaderStyle HorizontalAlign="left"  Width="45%"></HeaderStyle>
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
                            <HeaderStyle Width="15%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Account Number" ItemStyle-Font-Bold="true" DataField="empAccountNo"
                            HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left">
                            <HeaderStyle Width="15%"></HeaderStyle>
                        </asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Net Salary">
                            <ItemStyle Width="15%" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <input id="lblTotal" style="text-align: right; width: 72px; border-top-style: none;
                                    border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF;
                                    border-bottom-style: none" readonly type="text" size="15" value='<%# cdbl(DataBinder.Eval(Container,"DataItem.netSALARY")).ToString("##,###,###") %>'
                                    dataformatstring="{0:##,###,###}" name="lblTotal">
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            </tr>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3" />Kindly do the
                    needfull and acknowledge.<br>
                    <br>
                    Thanking you,<br>
                    <br>
                </td>
            </tr>
            <tr>
                <td>
                    <font style="color: black; font-family: Times New Roman" size="3">
                        <br>
                        Yours Sincerely,<br>
                        <br>
                        <b>For Intelgain Technologies Pvt. Ltd. </b>
                        <br>
                    </font>
                </td>
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
                    <font style="color: black; font-family: Times New Roman" size="3"><b>Director</b> </font>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
