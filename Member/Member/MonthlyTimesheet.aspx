<%@ Page Title="Monthly Timesheet" Language="C#"
    AutoEventWireup="true" CodeFile="MonthlyTimesheet.aspx.cs" Inherits="Member_MonthlyTimesheet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
     <link href="css/layout.css" rel="stylesheet" type="text/css" />
    <title>
        Monthly Timesheet
    </title>
</head>
<body>
    <form id="frmLogin" runat="server">
          <asp:Button ID="btnExport" runat="server" Width="80" Style="margin:6px" CssClass="small_button white_button open" Text="Export"  OnClick="btnExport_Click"/>
            <br />
        <asp:GridView ID="grdTimesheet" AllowSorting="true" OnSorting="grdTimesheet_Sorting" runat="server" ShowHeaderWhenEmpty="true" OnPageIndexChanging="grdTimesheet_OnPageIndexChanging"
            OnRowDataBound="grdTimesheet_RowDataBound" CssClass="manage_grid_a mange_lsttd" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" PageSize="25" Width="100%">
            <Columns>
                <asp:BoundField DataField="tsDisplayDate" SortExpression="tsDate" HeaderText="Date" />
                <asp:BoundField DataField="projName" SortExpression="projName" HeaderText="Project Name" />
                <asp:TemplateField HeaderText="Module Name" SortExpression="moduleName">
                    <ItemTemplate>
                        <%#Eval("moduleName")%>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblhours" runat="server">Total Hours</asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hours" SortExpression="tsHour">
                    <ItemTemplate>
                        <asp:Label ID="lblCountHours" runat="server" Text='<%#Eval("tsHour") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalHours" runat="server"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>              
                <asp:BoundField DataField="tsComment" SortExpression="tsComment" HeaderText="Comment" />
                <asp:BoundField DataField="tsEntrydate" SortExpression="tsEntrydate" HeaderText="Entry Date" DataFormatString="{0:dd-MMM-yy hh:mm tt}" />
            </Columns>

            <PagerStyle />
            <EmptyDataTemplate>
                <asp:Label ID="lblNoDataFound" runat="server" Text="No Data Found." Font-Bold="true"
                    Font-Size="Medium" />
            </EmptyDataTemplate>
        </asp:GridView>
    </form>
</body>
</html>

