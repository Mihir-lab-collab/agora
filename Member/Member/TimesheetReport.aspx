<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="TimesheetReport.aspx.cs" Inherits="Member_TimesheetReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" cellpadding="0">
        <tr>
            <td>
                <font face="Verdana" size="2"><b>
                    <asp:LinkButton ID="prevMonth" Text="<<" CommandArgument="prev"
                        runat="server" OnClick="PagerButtonClick" CausesValidation="False" />
                    <asp:Label ID="lblMonth" runat="server"></asp:Label>
                    <asp:LinkButton ID="nextMonth" Text=">>" CommandArgument="next" runat="server" OnClick="PagerButtonClick"
                        CausesValidation="False" />
                </b></font>
            </td>
        </tr>
        <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
    </table>
    <asp:GridView ID="grdTimesheet" AllowSorting="true" OnSorting="grdTimesheet_Sorting" runat="server" ShowHeaderWhenEmpty="true" OnPageIndexChanging="grdTimesheet_OnPageIndexChanging"
        CssClass="manage_grid_a mange_lsttd" DataKeyNames="EmpID" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" PageSize="100" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" Text='<%#Eval("EmpName")%>' NavigateUrl='<%# "MonthlyTimesheet.aspx?EmpID="+Eval("EmpID") %>' onclick="window.open(this.href, 'Popupwindow','toolbar=yes,location=no,menubar=no,width=1000,height=600,resizable=no,scrollbars=yes,top=200,left=250');return false;"></asp:HyperLink>
                    <asp:HiddenField ID="hdnEmpID" Value='<%#Eval("EmpID")%>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Role" SortExpression="Role" HeaderText="Role" />
            <asp:BoundField DataField="TSHour" SortExpression="Hours" HeaderText="Hours" />
        </Columns>
        <PagerStyle />
        <EmptyDataTemplate>
            <asp:Label ID="lblNoDataFound" runat="server" Text="No Data Found." Font-Bold="true"
                Font-Size="Medium" />
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:HiddenField ID="hdnmonth" runat="server" />
    <asp:HiddenField ID="hdnYear" runat="server" />
    <%--<asp:HiddenField ID="hdnEmpID" runat="server" />--%>
</asp:Content>

