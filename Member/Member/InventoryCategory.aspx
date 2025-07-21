<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="~/Member/InventoryCategory.aspx.cs" Inherits="InventoryCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function ConfirmDelete() {
            if (confirm("Do you want to delete this record.")) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManagerCategory" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanelCategory" runat="server">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="box_border">
                        <div class="grid_head">
                            <asp:Label ID="lblMasterDetails" Text="Product Master" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </div>
                        <asp:GridView ID="gvItemList" DataKeyNames="CategoryID" runat="server" Width="100%" CssClass="manage_grid_a mange_lsttd"
                            CellPadding="0" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" PageSize="25"
                            OnRowCancelingEdit="gvItemList_RowCancelingEdit"
                            OnRowEditing="gvItemList_RowEditing"
                            OnRowUpdating="gvItemList_RowUpdating"
                            OnRowDeleting="gvItemList_RowDeleting" OnRowDataBound="gvItemList_RowDataBound">
                            <Columns>
                                <%-- Buttons Add Update Delete Cancel --%>
                                <asp:TemplateField HeaderStyle-Width="50">
                                    <EditItemTemplate>
                                        <%--<asp:ImageButton  />--%>

                                        <asp:LinkButton ID="imgbtnUpdate" CommandName="Update" runat="server" ToolTip="Update">
                                             <img src="images/edit.png" alt="Update" /></asp:LinkButton>

                                        <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Member/images/cancel.png" ToolTip="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgbtnEdit" CommandName="Edit" runat="server" ToolTip="Edit">
                                             <img src="images/edit.png" alt="ADD" /></asp:LinkButton>

                                        <asp:LinkButton ID="imgbtnDelete" CommandName="Delete" runat="server" ToolTip="Delete" OnClientClick="return ConfirmDelete();">
                                             <img src="images/delete.png" alt="ADD" /></asp:LinkButton>

                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="imgbtnAdd" runat="server" CausesValidation="true"
                                            CommandName="AddNew" ToolTip="Add Attribute" ValidationGroup="validaiton" OnClick="imgbtnAdd_Click1" Width="20px"
                                            Height="20px"><img src="Images/addButton.png" alt="ADD" /></asp:LinkButton>

                                    </FooterTemplate>
                                </asp:TemplateField>
                                <%-- Category Details --%>
                                <asp:TemplateField HeaderText="Inventory Items" ItemStyle-HorizontalAlign="Left">
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hdnEditCategoryID" runat="server" Value='<%#Eval("CategoryID") %>' />
                                        <asp:TextBox ID="txtftrCategoryNameEdit" runat="server" Text='<%#Eval("Name") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnCategoryID" runat="server" Value='<%#Eval("CategoryID") %>' />
                                        <asp:Label ID="Name" runat="server" Text='<%#Eval("Name") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtftrCategoryName" runat="server" Text='<%#Eval("Name") %>' />
                                        <asp:RequiredFieldValidator ID="rfvattname" runat="server" ControlToValidate="txtftrCategoryName" Text="*" ValidationGroup="validaiton" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Label ID="Name" runat="server" Text="" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCatID" runat="server" CommandArgument='<%#Eval("Name")%>' OnClick="lnkCatID_Click">Add Attribute</asp:LinkButton>
                                        <%--<a href="./InventoryAttribute.aspx?id=<%#Eval("CategoryID")%>&Name=<%#Eval("Name")%>">Add Attribute</a>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="Name" runat="server" Text="" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
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
                </div>
            </div>
            <div>
                <asp:Label ID="lblResult" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

