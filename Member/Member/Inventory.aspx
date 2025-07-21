<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="~/Member/Inventory.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <div class="content_wrap">
        <div class="gride_table">

            <div class="grid_head">
                <asp:Label ID="lblInventoryDetails" Text="Inventory" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                <div style="float: right">
                    <asp:Button ID="BtnInvoice" runat="server" Text="Create Invoice" OnClick="BtnInvoice_Click" CssClass="small_button white_button open" />
                </div>
            </div>
            <div class="accord">
                <ajax:Accordion runat="server" ID="accordionInvenotory"
                    HeaderCssClass="accord_head "
                    HeaderSelectedCssClass="accord_head_a"
                    ContentCssClass="accord_cont" FadeTransitions="false" RequireOpenedPane="false" SelectedIndex="-1">
                    <Panes>
                        <ajax:AccordionPane runat="server" ID="Panel1">
                            <Header>Inventory Item Details</Header>
                            <Content>
                                <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
                                <asp:UpdatePanel runat="server" ID="panelIID">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvItemList" DataKeyNames="CategoryID" runat="server" Width="100%" CssClass="manage_gridb" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" ShowHeader="false" PageSize="25"
                                            OnPageIndexChanging="gvItemList_PageIndexChanging"
                                            OnRowDataBound="gvItemList_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Inventory Items">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCategoryID" runat="server" Value='<%#Eval("CategoryID") %>' />
                                                        <asp:LinkButton ID="lnkcatId" CommandArgument='<%#Eval("CategoryID") %>' runat="server" OnClick="lnkcatId_Click"><%#Eval("Name") %></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PQty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurchasedQuantity" runat="server" Text='<%#Eval("PurchasedQuantity") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EQty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpiredQuantity" runat="server" Text='<%#Eval("ExpiredQuantity") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BQty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalanceQuantity" runat="server" Text='<%#Eval("BalanceQuantity") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Purchased Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEditLastPurchasedDate" runat="server" Text='<%#Eval("LastPurchasedDate") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Purchased Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastPurchasedPrice" runat="server" Text='<%#Eval("LastPurchasedPrice") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle />
                                            <EmptyDataTemplate>
                                                No Records
                                            </EmptyDataTemplate>
                                            <PagerTemplate>
                                                <table id="pagerInnerTable1" runat="server" class="pageing">
                                                    <tr align="center">
                                                        <td>
                                                            <asp:LinkButton ID="lnkPrevPage1" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Prev" runat="server">Prev</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <div id="divListBottons1" runat="server"></div>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkNextPage1" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Next" runat="server">Next</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </PagerTemplate>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </Content>
                        </ajax:AccordionPane>
                        <ajax:AccordionPane runat="server" ID="Panel2">
                            <Header>Invoice Details</Header>
                            <Content>
                                <asp:ScriptManagerProxy ID="smp1" runat="server"></asp:ScriptManagerProxy>
                                <asp:UpdatePanel runat="server" ID="panelInoviceDetails">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvInvoiceList" DataKeyNames="ItemInvoiceID" runat="server" Width="100%" CssClass="manage_gridb" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" ShowHeader="false" PageSize="25"
                                            OnPageIndexChanging="gvInvoiceList_PageIndexChanging"
                                            OnRowDataBound="gvInvoiceList_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Invoice Details">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnItemInvoiceID" runat="server" Value='<%#Eval("ItemInvoiceID") %>' />
                                                          <asp:LinkButton ID="lnkInvoiceItem" CommandArgument='<%#Eval("ItemInvoiceID") %>' runat="server" OnClick="lnkInvoiceItem_Click"><%#Eval("InvoiceNo") %></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalPrice" runat="server" Text='<%#Eval("Price") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PurchaseDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurchaseDate" runat="server" Text='<%#((DateTime)Eval("PurchaseDate")).ToString ("dd/MM/yyyy") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Supplier">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplier" runat="server" Text='<%#Eval("Supplier") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Note">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNote" runat="server" Text='<%#Eval("Note") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle />
                                            <PagerTemplate>
                                                <table id="pagerInnerTable2" runat="server" class="pageing">
                                                    <tr align="center">
                                                        <td>
                                                            <asp:LinkButton ID="lnkPrevPage2" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Prev" runat="server">Prev</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <div id="divListBottons2" runat="server"></div>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkNextPage2" CssClass="small_button white_button pagerLink" CommandName="Page" CommandArgument="Next" runat="server">Next</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </PagerTemplate>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </Content>
                        </ajax:AccordionPane>
                        
                    </Panes>
                </ajax:Accordion>
            </div>
        </div>
    </div>
    <div>
        <asp:Label ID="lblResult" runat="server"></asp:Label>
    </div>
</asp:Content>

