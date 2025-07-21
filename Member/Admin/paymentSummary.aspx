<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %> 
<%@ Import Namespace="System" %>
<%@ Page Language="VB" Debug="TRUE" %>
<%@ Register src="../Controls/adminMenu.ascx" tagname="adminMenu" tagprefix="uc1" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <script language="JavaScript" type="text/javascript" src="../includes/CalendarControl.js"> </script>
<link href="../Css/style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Dyno Admin Control</title>
    <link rel="stylesheet" href="/includes/CalendarControl.css"  type="text/css"/>
    <script language="VB" runat="server">
        Dim SortField As String
        Dim gf As New generalFunction
       
        Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            gf.checkEmpLogin()
            If Not IsPostBack Then
                BindGrid()
            End If
        End Sub
    
        Sub MyDataGrid_Sort(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
            SortField = e.SortExpression
            BindGrid()
        End Sub

        Sub BindGrid()
            Dim Conn As SqlConnection
            Dim Rdr As SqlDataReader
            Dim strSQL As String
            Conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            Conn.Open()
            Dim Cmd As New SqlCommand()
            Cmd.Connection = Conn

            strSQL = "SELECT * FROM projectMaster,customerMaster " & _
            "WHERE projectMaster.custId=customerMaster.custId AND " & _
            "projId=" & Request.QueryString("projid")
            Cmd.CommandText = strSQL
            Rdr = Cmd.ExecuteReader()
            If Rdr.Read() Then
                projName.Text = Rdr("projName")
                CustName.Text = Rdr("custCompany")
            End If
            Rdr.Close()

            strSQL = "SELECT payid,payDate,currSymbol,payAmount,payExRate,payComment, payExRate*payAmount as payTotalAmount, 'EDIT' as edit,InvoiceSendOn,crId," & _
            " case when Isnull(paymentMaster.paymentStatus,'') = 'Cancel' then '' else 'DELETE' end as deleteRec,case when Isnull(paymentMaster.paymentStatus,'') = 'Cancel' then '' else 'Send Mail' end as SendAlert," & _
            " case when paymentMaster.payConfirmedDate = '1900-01-01' then null  else paymentMaster.payConfirmedDate end as ConfirmedDate " & _
            ",  case when Isnull(paymentMaster.paymentStatus,'') = 'Cancel' then 'Invoice Cancelled' when paymentMaster.payConfirmedDate = '1/1/1900 12:00:00 AM' then 'Pending' else 'Paid'  end as payStatus FROM paymentMaster, projectMaster, " & _
            "currencyMaster WHERE projectMaster.projid=paymentMaster.payProjId AND " & _
            "currencyMaster.currId=paymentMaster.payCurrency AND " & _
            "payProjId=" & Request.QueryString("projid") & " ORDER BY payDate DESC"
           
            Cmd.CommandText = strSQL
            Rdr = Cmd.ExecuteReader()
            MyDataGrid.DataSource = Rdr
            MyDataGrid.DataBind()
    
        End Sub
        Dim sum As Double
        Dim sum_price As Double

    
        Sub DisplayTotal(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim cellValue As String = DataBinder.Eval(e.Item.DataItem, "payAmount") & ""
                If cellValue <> "" Then
                    sum += Convert.ToDouble(cellValue)
                End If
                cellValue = DataBinder.Eval(e.Item.DataItem, "payTotalAmount") & ""
                If cellValue <> "" Then
                    sum_price += Convert.ToDouble(cellValue)
                End If
               ' Dim paydt As String
                'paydt = DataBinder.Eval(e.Item.DataItem, "payConfirmedDate") & ""
                'If paydt <> "" Then
                 '   e.Item.Cells(8).Text = "Paid"
               ' Else
                '    e.Item.Cells(8).Text = "Pending"
                'End If
            ElseIf e.Item.ItemType = ListItemType.Footer Then
                e.Item.Cells(0).Text = "<b>Total</b>"
                e.Item.Cells(2).Text = "<b>" & Format(sum, "0,00.0") & "</b>"
                e.Item.Cells(4).Text = "<b>" & Format(sum_price, "0,00.0") & "</b>"
            End If
	

            'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            ' Dim dgItem As DataGridItem
            ' For Each dgItem In dgrdrating.Items
            'Dim detail As Button = CType(e.Item.Cells(9).FindControl("btninvoice"), Button)
            '  If e.Item.Cells(10).Text <> "" Then
            'detail.Attributes.Add("onclick", "popupProjectDetailFeedback(" & e.Item.Cells(10).Text & "); return false;")
            'Else
            ' e.Item.Cells(10).Text = ""
            ' End If
            ' Next
            '  End If

        End Sub
        Sub addPaymentFun(ByVal sender As Object, ByVal e As System.EventArgs)
            Server.Transfer("Invoice_Details.aspx?Mode=PrID&projid=" & Request.QueryString("projid"))
        End Sub


        Sub ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)


            '******************************************************************
            'check is it view click or delete click event
            '******************************************************************
            If e.CommandName = "delete" Then
                ' dim payid as integer
                'payid = Int32.Parse(e.Item.Cells(0).Text)
                Dim key As Integer = MyDataGrid.DataKeys(e.Item.ItemIndex)

                'response.Write(key)
                'response.End()

                Dim Conn As SqlConnection
                Dim Rdr As SqlDataReader
                Dim strSQL As String
                Conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
                Conn.Open()
                Dim Cmd As New SqlCommand()
                Cmd.Connection = Conn
	
                strSQL = "SELECT * FROM paymentMaster WHERE payId=" & key
                Cmd.CommandText = strSQL
                Rdr = Cmd.ExecuteReader()
                If Rdr.Read() Then
                    Dim projId As String = Rdr("payProjId")
                    strSQL = "DELETE FROM paymentMaster WHERE payId=" & Rdr("payId")
                    Rdr.Close()
                    Cmd.CommandText = strSQL
                    Response.Write(strSQL)
                    Cmd.ExecuteNonQuery()
                    Response.Redirect("paymentSummary.aspx?projid=" & projId)
                End If
                Rdr.Close()
                Response.End()
                
            ElseIf e.CommandName = "Status" And e.CommandArgument <> "Invoice Cancelled" Then
                Dim key As Integer = MyDataGrid.DataKeys(e.Item.ItemIndex)
                ' Response.Write(key)
                Session("PayID") = key
                pnlPaymentDate.Visible = True
                
            End If
        End Sub
        Function GetPdfFile(ByVal strPayID As String) As String
            Dim strProjID As String
            Dim strFileName As String
            strProjID = Request.QueryString("projid")
            strFileName = strProjID & "_" + strPayID & ".pdf"
            Dim onjFinfo As FileInfo
            onjFinfo = New FileInfo(Server.MapPath("\admin\invoice\") & strFileName)
            If onjFinfo.Exists = False Then
                Return ""
            Else
                Return "<a href='#' onclick='javascript:window.open(""http://" & HttpContext.Current.Request.ServerVariables("HTTP_HOST").ToString() & "/admin/Invoice/" & strFileName & """)'><img src='../images/icon-pdf.gif' border='0'></a>"
            End If

            Return ""
            
        End Function
       
        
        Protected Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            pnlPaymentDate.Visible = False
            txtPaymentDate.Text = ""
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            pnlPaymentDate.Visible = False
            txtPaymentDate.Text = ""
            
        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim payid As Integer
            
            Dim dd As String = Convert.ToDateTime(txtPaymentDate.Text)
            Response.Write(dd)
            payid = Convert.ToInt32(Session("PayID"))
            Dim Conn As SqlConnection
            Dim Cmd As New SqlCommand()
            Conn = New SqlConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
            Conn.Open()
            Cmd.Connection = Conn
            Dim strupdatesql As String = "update paymentMaster set payConfirmedDate='" & Convert.ToDateTime(txtPaymentDate.Text) & "' where payid= " & payid & ""
            Response.Write(strupdatesql)
            Cmd.CommandText = strupdatesql
            Cmd.ExecuteNonQuery()
            Conn.Close()
            Conn.Dispose()
            Response.Redirect("paymentSummary.aspx?projid=" & Request.QueryString("projid"))
        End Sub
         
        Public Function GetView() As Boolean
            Dim value As Boolean
            Dim status As String = Eval("payStatus").ToString()
            If status = "Paid" Then
                value = False
            Else
                value = True
            End If
            Return value
        End Function
        Public Function CreateStyle(strValue As String) As String
            Dim value As String
            If strValue = "Invoice Cancelled" Then
                value = "font-family: Verdana;font-size: 8pt; color: Red; font-weight: 200; Font-Bold=true"
            Else
                value = "font-family: Verdana;font-size: 8pt; color: Blue; font-weight: 200; Font-Bold=true"
            End If
            Return value
        End Function
        
        Public Function CreateEnable(strValue As String) As Boolean
            Dim value As Boolean
            If strValue = "Invoice Cancelled" Then
                value = False
            Else
                value = True 
            End If
            Return value
        End Function
        Public Function GetViewLbl() As Boolean
            Dim value As Boolean
            Dim status As String = Eval("payStatus").ToString()
            If status = "Paid" Then
                value = True
            Else
                value = false
            End If
            Return value
        End Function
        Public Function convertDate(ByVal strDate As String) As String
            Dim strAry As String() = strDate.Split("-")
            Dim strRtn As String = ""
            Try
                strRtn = strAry(1) & "/" & strAry(0) & "/" & strAry(2)
            Catch
            End Try
            Return strRtn
        End Function
</script>

</head>
<body>
    <form runat="server" id="Form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <table cellpadding="4" width="100%" border="1">
        <tr>
            <td>
                <uc1:adminMenu ID="adminMenu1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
               <asp:Button ID="Button1" runat="server" Text="Generate Invoice" OnClick="addPaymentFun">
                </asp:Button>
            </td>
        </tr>
    </table>
    <asp:Label runat="server" ID="projName" BackColor="LightBlue" ForeColor="Black" Font-Name="Verdana"
        Font-Size="14pt" BorderColor="Black" />
    <br/>
    <br/>
    <asp:Label runat="server" ID="CustName" BackColor="LightBlue" ForeColor="Black" Font-Name="Verdana"
        Font-Size="14pt" BorderColor="Black" />
    <br/>
    <br/>

    <asp:DataGrid ID="MyDataGrid" runat="server" AllowSorting="True" Width="100%" BackColor="White"
        BorderColor="Black" ShowFooter="True" CellPadding="2" DataKeyField="payId" Font-Name="Verdana"
        Font-Size="10pt" HeaderStyle-BackColor="lightblue" HeaderStyle-Font-Size="11pt"
        FooterStyle-HorizontalAlign="Right" AutoGenerateColumns="False" OnItemDataBound="DisplayTotal"
        OnItemCommand="ItemCommand" OnSortCommand="MyDataGrid_Sort" Font-Names="Verdana">
        <FooterStyle HorizontalAlign="Right"></FooterStyle>
        <HeaderStyle Font-Size="11pt" BackColor="LightBlue"></HeaderStyle>
        <Columns>
            <asp:BoundColumn DataField="payDate" ItemStyle-Width="8%" HeaderText="Date of Invoice"
                DataFormatString="{0:dd-MMM-yy}"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Invoice No.">
                <ItemStyle Wrap="false" VerticalAlign="Middle"></ItemStyle>
                <ItemTemplate>
                DWT/<%#Eval("payId")%>
		
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="currSymbol" HeaderText="Currency"></asp:BoundColumn>
            <asp:BoundColumn DataField="payAmount" HeaderText="Amount" DataFormatString="{0:##,###,###}">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="payExRate" HeaderText="Ex Rate" DataFormatString="{0:0.0}">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="payTotalAmount" ItemStyle-Width="7%" HeaderText="Amount (INR)"
                DataFormatString="{0:##,###,###}">
                <ItemStyle HorizontalAlign="Right"></ItemStyle>
            </asp:BoundColumn>
            <asp:BoundColumn DataField="ConfirmedDate" ItemStyle-Width="8%" HeaderText="Pay Date"
                DataFormatString="{0:dd-MMM-yy}"></asp:BoundColumn>
            <asp:BoundColumn DataField="payComment" HeaderText="Comment"></asp:BoundColumn>
             <asp:BoundColumn DataField="crId" HeaderText="Change Request Id" Visible="false"></asp:BoundColumn>
              <asp:BoundColumn DataField="InvoiceSendOn" HeaderText="Last Invoice sent" DataFormatString="{0:dd-MMM-yy}" ItemStyle-Width="8%"></asp:BoundColumn>
           <%-- <asp:BoundColumn DataField="payTransCharge" HeaderText="Other Charges(%)" DataFormatString="{0:0.0}">
            </asp:BoundColumn>--%>
           <%-- <asp:BoundColumn HeaderText="payStatus" DataField="payStatus"></asp:BoundColumn>--%>
              <asp:TemplateColumn HeaderText="Payment Status" >
                    <ItemStyle Width="10%" HorizontalAlign="left" VerticalAlign="middle"></ItemStyle>
                    <ItemTemplate>
                        <asp:LinkButton ID="Status" runat="server"   Enabled =<%# CreateEnable(Eval("payStatus"))%>
                        CommandName="Status" CommandArgument ='<%# Eval("payStatus")%>'  align="right" Style='<%# CreateStyle(Eval("payStatus"))%>' Visible='<%# GetView() %>' > <%# Eval("payStatus")%>
                    </asp:LinkButton> 
                        <asp:Label ID="lblstatus" runat="server" Visible='<%# GetViewLbl() %>'  ><%# Eval("payStatus")%></asp:Label> 
                    </ItemTemplate>
                </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemStyle Wrap="false" VerticalAlign="Middle"></ItemStyle>
                <ItemTemplate>
                   <%#GetPdfFile(Eval("payId").ToString())%>
		
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="payId" Visible="False"></asp:BoundColumn>
            <asp:HyperLinkColumn DataNavigateUrlField="payId" DataNavigateUrlFormatString="Invoice_Details.aspx?Mode=PID&payId={0}"
                DataTextField="Edit">
                <ItemStyle HorizontalAlign="Right" Font-Bold="true" Font-Names="Verdana" Font-Size="8pt"
                    ForeColor="#A2921E"></ItemStyle>
            </asp:HyperLinkColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemStyle Wrap="false" HorizontalAlign="center" VerticalAlign="middle"></ItemStyle>
                <ItemTemplate>
                    <asp:LinkButton ID="delete" runat="server" OnClientClick="return Confirm_Delete();"
                        CommandName="delete" Text=<%# Eval("deleteRec")%> align="right" Style="font-family: Verdana;
                        font-size: 8pt; color: Blue; font-weight: 200;" Font-Bold="true">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
             <asp:TemplateColumn HeaderText="">
                <ItemStyle Wrap="false" HorizontalAlign="center" VerticalAlign="middle"></ItemStyle>
                <ItemTemplate>
                    <a  href="#" onclick='javascript:showMails(<%# Eval("payId")%>)'><%# Eval("SendAlert")%></a> 
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
     <asp:Panel ID="pnlPaymentDate" runat="server" Visible="false" Width="100%" BorderColor="Red" >
    <div style="height: 160px; min-width: 300px; position: absolute; z-index: 100001; left: 35%; top: 42%;border:3px solid #d0d0d0; width:480px; background:#fff;" ><%----%>
    <asp:ImageButton ID="ibtnClose" runat="server" ImageUrl="~/Images/delete.png" OnClick="cmdCancel_Click"
                    align="right" CausesValidation="false" />

                 <div style="margin-left:12px;"><h2> Update  Payment date</h2>
                 <%--<br />--%>
                 <hr />
                 </div>
                <table width="100%" border="0" cellpadding="0" cellspacing="0" class="manage-form">
                    <tr>
                        <th>
                           Confirm Payment Date:
                        </th>
                       <td>
                       <asp:TextBox runat="server" ID="txtPaymentDate" onclick="popupCalender('txtPaymentDate');" oncopy="return false" onpaste="return false" oncut="return false" onkeypress="return false"  ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvStateID" runat="server" ValidationGroup="PaymentDate"
                                Display="Dynamic" ControlToValidate="txtPaymentDate" Text="Required" ForeColor="Red"></asp:RequiredFieldValidator>

                       </td>
                    </tr> <%--<tr><td colspan="2">&nbsp;</td></tr>--%>
                    <tr >
                    <td>
                    </td>
                    <td style="padding-top:12px;">
                    
                     <asp:Button ID="btnUpdate" runat="server" Text="Submit" 
                                OnClick="btnUpdate_Click" ValidationGroup="PaymentDate" /> &nbsp;&nbsp
                                 <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                OnClick="btnCancel_Click" />
                    </td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                   <tr><td colspan="2">&nbsp;</td></tr>
                </table>
              
       </div>
    </asp:Panel>


    </form>
</body>
</html>

<script language="javascript" type="text/javascript">
    function popupProjectDetailFeedback(Id) {
        window.open("paymentConfirm.aspx?payId=" + Id, null, "height=420,width=850,left=100,top=50,status=yes,toolbar=no,menubar=no,location=no");
    }
    function showMails(payID) {
        window.open('Invoice_Mail.aspx?projectID=<%=Request.QueryString("projid")%>&invID=' + payID, null, 'height=450,width=580,left=100,top=50,status=yes,toolbar=no,menubar=no,location=no');
	 }
	 function Confirm_Delete() {
	     var i = confirm("Are you sure,You Want To Delete Record? ");
	     if (i == true)
	         return true;
	     else
	         return false;
	 }
</script>

