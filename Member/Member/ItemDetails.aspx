<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="~/Member/ItemDetails.aspx.cs" Inherits="_Default" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="http://cdn.webrupee.com/font" />
    <script language="javascript" type="text/javascript">
        function GetBrandID(source, eventArgs) {
            var _strCoCode = eval(eventArgs.get_value());
            document.getElementById('ctl00_ContentPlaceHolder1_ctl02_hdnAutoBrandID').value = _strCoCode;
        }

        function GetSupplierID(source, eventArgs) {
            var strCoCode = eval(eventArgs.get_value());
            document.getElementById('ctl00_ContentPlaceHolder1_hdnAutoSupplierID').value = strCoCode;
        }
    </script>
    <asp:ScriptManager runat="server" ID="scriptManageInvoice"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="updatepanelInvoice">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="box_border">
                        <div class="grid_head">
                            <asp:Label ID="lblItemDetails" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </div>
                        <asp:GridView ID="gvItemList" runat="server" Width="100%"
                            CellPadding="0" AutoGenerateColumns="False" ShowFooter="true" CssClass="manage_grid_a" AllowSorting="true" PageSize="25" AllowPaging="true" OnPageIndexChanging="gvItemList_PageIndexChanging" OnSorting="gvItemList_Sorting" OnRowDataBound="gvItemList_RowDataBound">
                            <Columns>
                                <%-- Buttons Add Update Delete Cancel --%>
                                <%-- <asp:TemplateField>--%>
                                <%-- <EditItemTemplate>
                                <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/images/edit.png" ToolTip="Update" Height="20" Width="20" />
                                <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/cancel.png" ToolTip="Cancel" Height="20" Width="20" />
                            </EditItemTemplate>--%>
                                <%-- <FooterTemplate>
                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Images/addButton.png" CommandName="AddNew" Width="30px" Height="30px" ToolTip="Add Attribute" ValidationGroup="validaiton" OnClick="imgbtnAdd_Click" />
                            </FooterTemplate>--%>
                                <%-- </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Invoice No" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnItemInvoiceID" runat="server" Value='<%#Eval("ItemInvoiceID") %>' />
                                        <asp:LinkButton ID="lnkInvoiceItem" runat="server" CommandArgument='<%#Eval("ItemInvoiceID") %>' OnClick="lnkInvoiceItem_Click"><%#Eval("InvoiceNo") %></asp:LinkButton>
                                        <%--<a href="./InvoiceDetails.aspx?id=<%#Eval("ItemInvoiceID")%>&Name=<%#Eval("InvoiceNo")%>&pr=1<%= "&ItemId=" + Request.QueryString["id"] + "&ItemName=" + Request.QueryString["Name"] %>"><%#Eval("InvoiceNo") %></a>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- InvoiceNo --%>
                                <%--<asp:TemplateField HeaderText="Invoice No" ItemStyle-HorizontalAlign="Center" SortExpression="InvoiceNo">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- PurchaseDate --%>
                                <asp:TemplateField HeaderText="Purchase Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseDate" runat="server" Text='<%#((DateTime)Eval("PurchaseDate")).ToString ("dd/MM/yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Note --%>
                                <asp:TemplateField HeaderText="Note" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNote" runat="server" Text='<%#Eval("Note") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Supplier --%><asp:TemplateField HeaderText="Supplier" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("Supplier") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Brand --%>
                                <asp:TemplateField HeaderText="Brand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("BrandName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%-- Price --%><asp:TemplateField HeaderText="Price" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# CSCode.Global.GetCurrencyFormat(Convert.ToDouble(Eval("Price"))) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Quantity --%><asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Exp Date --%><asp:TemplateField HeaderText="Expire Date" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="Label7" runat="server" Text='<%#((DateTime)Eval("ExpiryDate")).ToString ("dd/MM/yyyy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Description --%><asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
         
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lbl" runat="server" Text="No records found."></asp:Label>
                            </EmptyDataTemplate>
                            <PagerStyle />
                            <PagerTemplate>
                                <table id="pagerInnerTable" runat="server" class="pageing">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkPrevPage" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Prev" runat="server">Prev</asp:LinkButton>
                                        </td>
                                        <td>
                                            <div id="divListBottons" runat="server"></div>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkNextPage" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Next" runat="server">Next</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <div style="float: right">
                        <asp:Button ID="BtnBack" runat="server" Text="Back" OnClick="BtnBack_Click" CssClass="small_button white_button open" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
