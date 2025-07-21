<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="~/Member/InventoryBrand.aspx.cs" Inherits="InventoryBrand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="scriptmanageBrand" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel runat="server" ID="updatepanelBrand">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="box_border">

                        <div class="grid_head">
                            <asp:Label ID="lblBrandDetails" Text="Brands" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </div>
                        <asp:GridView ID="gvDetails" DataKeyNames="BrandID" runat="server" CssClass="manage_grid_a mange_lsttd"
                            AutoGenerateColumns="false"
                            ShowFooter="true" HeaderStyle-Font-Bold="true"
                            OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                            OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                            OnRowUpdating="gvDetails_RowUpdating"
                            OnRowCommand="gvDetails_RowCommand"
                            OnRowDataBound="gvDetails_RowDataBound"
                            OnPageIndexChanging="gvDetails_PageIndexChanging"
                            Font-Names="Arial" Font-Size="Small" AllowPaging="false" PageSize="2" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="50">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Member/images/edit.png" ToolTip="Update" CausesValidation="false"/>
                                        <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Member/images/cancel.png" ToolTip="Cancel"  CausesValidation="false"/>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Member/images/edit.png" ToolTip="Edit" CausesValidation="false"/>
                                        <asp:ImageButton ID="imgbtnDelete" CommandName="Delete" runat="server" ImageUrl="~/Member/images/delete.png" ToolTip="Delete"  CausesValidation="false"/>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Member/images/addButton.png" CommandName="AddNew" ToolTip="Add Attribute" ValidationGroup="B"/>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name">
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hdnEditBrandID" runat="server" Value='<%#Eval("BrandID") %>' />
                                        <asp:TextBox ID="txtEditName" runat="server" Text='<%#Eval("Name") %>' Width="90%" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtftrname" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvname" runat="server" ValidationGroup="B" ControlToValidate="txtftrname" Text="*" />
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Description">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescEdit" runat="server" TextMode="MultiLine" Height="90%" Text='<%#Eval("Description") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Description") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFtDesc" runat="server" TextMode="MultiLine" Height="90%" />
                                        <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ValidationGroup="B" ControlToValidate="txtFtDesc" Text="*" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle />
                            <PagerTemplate>
                                <table id="pagerInnerTable" runat="server" class="pageing">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkPrevPage" CssClass="small_button white_button pagerLink" ValidationGroup="B" CommandName="Page" CommandArgument="Prev" runat="server" CausesValidation="false">Prev</asp:LinkButton>
                                        </td>
                                        <td>
                                            <div id="divListBottons" runat="server"></div>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnkNextPage" CssClass="small_button white_button pagerLink" ValidationGroup="B" CommandName="Page" CommandArgument="Next" runat="server" CausesValidation="false">Next</asp:LinkButton> 
                                        </td>
                                    </tr>
                                </table>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div>
                <asp:Label ID="lblResult" runat="server"> </asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

