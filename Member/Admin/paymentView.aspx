<%@ Page Language="VB" %>
<%@Import Namespace="System.Data"%>
<%@Import Namespace="System.Data.SqlClient"%>
<%
%>
<script language="VB" runat="server">
Dim SortField As String
    Dim gf As New generalFunction
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        gf.checkEmpLogin()
        If Not IsPostBack Then
            If SortField = "" Then
                SortField = "custCode"
            End If
            BindGrid()
        End If
    End Sub
    
Sub MyDataGrid_Sort(sender As Object, e As DataGridSortCommandEventArgs)
	SortField = e.SortExpression
    BindGrid
End Sub

Sub BindGrid()
        Dim dsn1 As String = ConfigurationManager.ConnectionStrings("conString").ToString()
	Dim conn As SqlConnection = New SqlConnection(dsn1)
       
    Dim strSQL as string

    strSQL="SELECT payProjId,projName,AVG(projCost) as projCost,AVG(projCost*currExRate) as projTotalCost," & _
    "custCompany,SUM(payAmount) AS " & _
    "payAmount,Max(payDate) as lastPayDate, currName,SUM(payAmount*payExRate) as totalAmount " & _
    "FROM projectMaster,customerMaster,paymentMaster,currencyMaster WHERE " & _
    "projectMaster.custId=customerMaster.custId AND projectMaster.projId= " & _
    "paymentMaster.payProjId AND currencyMaster.currId=projectMaster.currId " & _
    " GROUP BY payProjId,projName,custCompany,currName " & _
    "ORDER BY projName"
    
    'Conn=New OLEDBConnection(ConfigurationManager.ConnectionStrings("conString").ToString())
    Conn.Open()
    'Dim Cmd as New OLEDBCommand(strSQL,Conn)
    Dim cmd As SqlCommand = New SqlCommand(strSQL,conn)
	Dim Rdr As SqlDataReader
    Rdr=Cmd.ExecuteReader()
    myDataGrid.DataSource = Rdr
    myDataGrid.DataBind()
End Sub

dim sum as double
dim sum_price as double

Sub DisplayTotal(sender As Object, e As DataGridItemEventArgs)
   	If e.Item.ItemType = ListItemType.Item OR e.Item.ItemType = ListItemType.AlternatingItem then
		Dim cellValue as string = DataBinder.Eval(e.Item.DataItem, "projTotalCost") & ""
		if cellValue<>"" then
			sum += Convert.ToDouble(cellValue) 
		end if
		cellValue = DataBinder.Eval(e.Item.DataItem, "totalAmount") & ""
		if cellValue<>"" then
			sum_price += Convert.ToDouble(cellValue) 
		end if
   	ElseIf e.Item.ItemType = ListItemType.Footer then
		e.Item.Cells(0).Text = "<b>Total</b>" 
    	e.Item.Cells(4).Text =  "<b>" & format(Sum,"0,00") & "</b>"
		e.Item.Cells(6).Text =  "<b>" & format(Sum_Price,"0,00") & "</b>"
	End If
 End Sub
</script>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>Dyno Admin Control</title>
</head>
<body>

<Table cellpadding="4" width="100%" border="1" style="border-collapse: collapse" bordercolor="#E8E8E8" bordercolorlight="#E8E8E8" bordercolordark="#E8E8E8">
<tr>
  <td colspan="4" align="right" bgcolor="#edf2e6">
  <a href="paymentDetail.aspx"><font color="#A2921E">Add Payment</font></a></td>
</tr>
</table>

<form runat="server">
<asp:literal id="litExc" runat="server" />
<ASP:DataGrid id="MyDataGrid" runat="server"
	AllowSorting="true"
    Width="100%"
   	BackColor="white"
    BorderColor="black"
    ShowFooter="True" 
    CellPadding=2 
    CellSpacing="0"
    Font-Name="Verdana"
    Font-Size="10pt"
	Headerstyle-Forecolor="#A2921E" 
    Headerstyle-BackColor="#edf2e6"
    Headerstyle-Font-Size="11pt"
	Headerstyle-Font-Bold="True"
    MaintainState="true"
    Footerstyle-HorizontalAlign="Right"
	AutoGenerateColumns="False"
	OnItemDataBound="DisplayTotal"
	OnSortCommand="MyDataGrid_Sort">
	<AlternatingItemStyle BackColor="#edf2e6" />
    <Columns>
		<asp:HyperLinkColumn
                HeaderText="Project Name"
                DataNavigateUrlField="payProjId"
                DataNavigateUrlFormatString="projDetail.aspx?projId={0}"
                DataTextField="projName"
            />

       <asp:BoundColumn HeaderText="Customer" DataField="custCompany" />
       <asp:BoundColumn HeaderText="Currency" DataField="currName"/>
       <asp:BoundColumn HeaderText="Project Cost" DataField="projCost"
       ItemStyle-HorizontalAlign="right" DataFormatString="{0:##,###,###}"/>
       <asp:BoundColumn HeaderText="Project Cost(INR)" DataField="projTotalCost"
       ItemStyle-HorizontalAlign="right" DataFormatString="{0:##,###,###}"/>
		<asp:HyperLinkColumn
                HeaderText="Payment Recieved"
                DataNavigateUrlField="payProjId"
                DataNavigateUrlFormatString="paymentSummary.aspx?projId={0}"
                DataTextField="payAmount"
                ItemStyle-HorizontalAlign="right" 
				DataTextFormatString="{0:##,###,###}"
            />
		<asp:HyperLinkColumn
                HeaderText="Payment Recieved (INR)"
                DataNavigateUrlField="payProjId"
                DataNavigateUrlFormatString="paymentSummary.aspx?projId={0}"
                DataTextField="totalAmount"
                ItemStyle-HorizontalAlign="right" 
				DataTextFormatString="{0:##,###,###}"
            />
       <asp:BoundColumn HeaderText="Last Payment Date" DataField="lastPayDate" DataFormatString="{0:dd-MMM-yy}" />
  </Columns>
 </ASP:DataGrid>
</form>
</body>
</html>