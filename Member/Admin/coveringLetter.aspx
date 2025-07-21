<%@ Page Language="VB" %>

<%@ Import Namespace="System.IO" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Intelgain Technologies  : Project Daily Report</title>

    <script language="VB" runat="server">
        Dim gf As New generalFunction
        Dim conn As New SqlConnection
        Dim cmdCover As SqlCommand
        Public dsCover As DataTable
        
        Dim daCover As SqlDataAdapter
        Dim dtr As SqlDataReader
        Dim dbankdate, dbankmonth, dbankyear As String
        Public strSql As String
        Dim ddate, dyear, dMonth, intmonth, intyear As String
	
	
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            Session("store") = ""
            Session.Add("currentslipdate", DateAdd("m", -1, Now))
            Session("store") = ""
            ddate = Session("currentslipdate")
            dMonth = Month(ddate)
            intmonth = Convert.ToInt32(dMonth)
            dyear = Year(ddate)
            intyear = Convert.ToInt32(dyear)
            Dim conn As New SqlConnection
            conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            If Not IsPostBack Then
                conn.Open()
                strSql = "select  employeemaster.empName ,employeemaster.empAccountNo ,employeemaster.empid,floor(( select Case when EMP.ispf=1 then ((employeepayprocessdetail.paybasic) * 100/40 )-((employeepayprocessdetail.paybasic) * 12/100)-(employeepayprocessdetail.payPT) -(employeepayprocessdetail.payInsurance) else  (employeepayprocessdetail.paybasic+employeepayprocessdetail.payhra+ employeepayprocessdetail.payconveyance+employeepayprocessdetail.paymedical+employeepayprocessdetail.payfood+employeepayprocessdetail.payspecial+employeepayprocessdetail.paylta )-(employeepayprocessdetail.payPT) -(employeepayprocessdetail.payInsurance) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid))as netSALARYOld,floor(( select Case when EMP.ispf=1 then  (employeepayprocessdetail.paybasic) * 100/40 else  (employeepayprocessdetail.paybasic+employeepayprocessdetail.payhra+ employeepayprocessdetail.payconveyance+employeepayprocessdetail.paymedical+employeepayprocessdetail.payfood+employeepayprocessdetail.payspecial+employeepayprocessdetail.paylta ) end from employeepaymaster EMP, employeeMaster EP where EMP.empId = ep.empid and   EP.empid=employeepayprocessdetail.empid) + (((employeepayprocessdetail.payBonus)+(employeepayprocessdetail.payAddition)))-(((employeepayprocessdetail.payPf)+(employeepayprocessdetail.payPT)+(employeepayprocessdetail.payInsurance)+(employeepayprocessdetail.payAT)+(employeepayprocessdetail.payLoanInstl)+(employeepayprocessdetail.payAdvance)+ ((employeepayprocessdetail.payLeave) * (((employeepayprocessdetail.paybasic)+(employeepayprocessdetail.payhra)+(employeepayprocessdetail.payconveyance)+(employeepayprocessdetail.paymedical)+(employeepayprocessdetail.payfood)+(employeepayprocessdetail.payspecial)+(employeepayprocessdetail.paylta))/" & Session.Item("dynoProcesssessiondays") & "))+(employeepayprocessdetail.payDeduction)+(employeepayprocessdetail.payOtherDeduction)))) as netSALARY from employeepayprocessdetail,employeemaster,employeepayprocess where employeepayprocessdetail.empid=employeemaster.empid and  month(paydate) =  '" & intmonth & "'  and year(paydate) =  '" & intyear & "' and  employeepayprocessdetail.payid =" & Session("payid") & ""
                
	strSQL = "select empName ,empAccountNo ,employeemaster.empid,Round(paybasic+ payhra+  payconveyance+ paymedical+ payfood+ payspecial+ paylta+payBonus+payAddition-(payPf+payPT+payInsurance+payAT+payLoanInstl+payAdvance+ (payLeave *(paybasic+payhra+payconveyance+paymedical+payfood+payspecial+paylta)/31)+payDeduction+payOtherDeduction),0) as netSALARY from employeepayprocessdetail,employeemaster,employeepayprocess where employeepayprocessdetail.empid=employeemaster.empid and month(paydate) ='" & intmonth & "' and year(paydate) ='" & intyear & "' and employeepayprocessdetail.payid =" & Session("payid")


                daCover = New SqlDataAdapter(strSql, conn)
                dsCover = New DataTable
                daCover.Fill(dsCover)
                dgrdCLetter.DataSource = dsCover
                dgrdCLetter.DataBind()
                
                ViewState("bankStatement") = dsCover
                conn.Close()
                
            End If
        End Sub
	
	
        Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim sp1 As String
            Dim dgItem As DataGridItem
            Dim cBox As CheckBox
            Dim store As String
            store = ""
            For Each dgItem In dgrdCLetter.Items
                cBox = dgItem.FindControl("chkSelect")
                If cBox.Checked = True Then
                    If store = "" Then
                        store = dgItem.Cells(3).Text
                    Else
                        store = store & "," & dgItem.Cells(3).Text
                    End If
                End If
            Next
            If store = "" Then
                sp1 = "<Script language='JavaScript'>"
                sp1 += "alert('Select at least one Employee.');"
                sp1 += "</" + "script>"
                ClientScript.RegisterStartupScript(Me.GetType, "script123", sp1)
            Else
                Session("store") = store
                Response.Redirect("BankingStatment.aspx")
            End If
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
                e.Item.Cells(2).Text = "<b>Grand Total</b>"
                e.Item.Cells(5).Text = "<b>" & Format(sum, "0,00") & "</b>"
            End If
        End Sub
        
</script>

</head>
<body>
    <form id="Form1" runat="server">
        <table height="0" cellspacing="0" cellpadding="4" width="90%" border="0" style="border-collapse: collapse"
            bordercolor="#111111" align="center">
            <tr>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellspacing="1" width="100%">
                        <tr>
                            <td align="center">
                                <font face="Verdana">
                                    <asp:HyperLink ID="lnkForget" runat="server" Width="144px" Font-Bold="True">Bank  Statment</asp:HyperLink></font></td>
                        </tr>
                        <tr>
                            <td style="height: 10px">
                            </td>
                        </tr>
                        <tr id="rowhide" runat="server">
                            <td align="center">
                                <font face="Verdana"></font>
                            </td>
                        </tr>
                        <tr>
                            <td width="25%" colspan="4">
                                <font face="Verdana" size="2"><b>SALARY SLIP FOR THE MONTH :
                                    <asp:Label ID="lbtnpre" runat="server"></asp:Label>
                                    <%= MonthName(dMonth)&" "&Year(dDate) %>
                                </b>
                            </td>
                        </tr>
                    </table>
                    <tr>
                        <td>
                            <asp:DataGrid ID="dgrdCLetter" runat="server" BorderColor="Black" Font-Size="10pt"
                                Font-Name="Verdana" BackColor="White" Font-Names="Verdana" AutoGenerateColumns="False"
                                FooterStyle-HorizontalAlign="Right" HeaderStyle-Font-Size="10pt" ShowFooter="True"
                                OnItemDataBound="DisplayTotal" Width="90%" AllowSorting="True">
                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                <HeaderStyle Font-Bold="True" Width="70px"></HeaderStyle>
                                <FooterStyle></FooterStyle>
                                <Columns>
                                    <asp:TemplateColumn HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server"  Checked="true" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Sr. No.">
                                        <ItemTemplate>
                                            <%# Container.ItemIndex+1%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn ItemStyle-Wrap="false">
                                        <HeaderStyle HorizontalAlign="left"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left"></ItemStyle>
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
                                    <asp:BoundColumn HeaderText="Account Number" DataField="empAccountNo" HeaderStyle-HorizontalAlign="left"
                                        ItemStyle-HorizontalAlign="left">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Net Salary">
                                        <ItemStyle Width="23%" HorizontalAlign="Right"></ItemStyle>
                                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                                        <ItemTemplate>
                                            <input id="lblTotal" style="text-align: right; width: 72px; border-top-style: none;
                                                border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF;
                                                border-bottom-style: none" readonly type="text" size="15" value='<%# cdbl(DataBinder.Eval(Container,"DataItem.netSALARY")).ToString("##,##,###") %>'
                                                dataformatstring="{0:##,###,###}" name="lblTotal">

                                           <%--  <input id="lblTotal" style="text-align: right; width: 72px; border-top-style: none;
                                                border-right-style: none; border-left-style: none; height: 18px; background-color: #FFFFFF;
                                                border-bottom-style: none" readonly type="text" size="15" value='<%# DataBinder.Eval(Container,"DataItem.netSALARY") %>'
                                                dataformatstring="{0:##,###,###}" name="lblTotal">--%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <input name="text" type="text" id="lblGrandTotal" style="text-align: right; font-weight: bold;
                                                width: 78px; border-top-style: none; border-right-style: none; border-left-style: none;
                                                height: 18px; background-color: #FFFFFF; border-bottom-style: none" size="10"
                                                readonly>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>

                        </td>
                    </tr>
            <tr>
                <td>
                </td>
            </tr>
            
            <tr>
                <td style="height: 10px">
                    <asp:Button ID="btnSubmit" Width="70px" runat="server" Text="Proceed" OnClick="btnSubmit_Click">
                    </asp:Button>   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
