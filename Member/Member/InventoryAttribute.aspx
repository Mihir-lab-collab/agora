<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="~/Member/InventoryAttribute.aspx.cs" Inherits="Member_InventoryAttribute" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManagerCategoryDetail" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanelCategoryDetail" runat="server">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="box_border">
                        <div class="grid_head">
                            <asp:Label ID="Label1" Text="Category :  " runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="lblCategoryName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                        </div>
                        <asp:GridView ID="gvDetails" runat="server"
                            AutoGenerateColumns="false" DataKeyNames="SrNo" CssClass="manage_grid_a"
                            ShowFooter="true"
                            OnRowCancelingEdit="gvDetails_RowCancelingEdit"
                            OnRowDeleting="gvDetails_RowDeleting" OnRowEditing="gvDetails_RowEditing"
                            OnRowUpdating="gvDetails_RowUpdating"
                            OnRowCommand="gvDetails_RowCommand"
                            OnRowDataBound="gvDetails_RowDataBound"
                            OnPageIndexChanging="gvDetails_PageIndexChanging"
                            Font-Names="Arial" Font-Size="Small" AllowPaging="True" PageSize="10" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="50">
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="imgbtnUpdate" CommandName="Update" runat="server" ImageUrl="~/Member/images/edit.png" ToolTip="Update" Height="20" Width="20" />
                                        <asp:ImageButton ID="imgbtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/Member/images/cancel.png" ToolTip="Cancel" Height="20" Width="20" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CommandName="Edit" runat="server" ImageUrl="~/Member/images/edit.png" ToolTip="Edit" Height="20" Width="20" />
                                        <%--<asp:ImageButton ID="imgbtnDelete" CommandName="Delete" runat="server" ImageUrl="~/images/delete.png" ToolTip="Delete" Height="20" Width="20" />--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/Member/images/addButton.png" CommandName="AddNew" ToolTip="Add Attribute" ValidationGroup="validaiton" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SrNo" Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="SrEditNo" runat="server" Text='<%#Eval("SrNo") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="SrNo" runat="server" Text='<%#Eval("SrNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Attribute Name">
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hndAttributeID" runat="server" Value='<%#Eval("AttributeID") %>' />
                                        <asp:TextBox ID="txtEditattname" runat="server" Text='<%#Eval("Name") %>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblitemName" runat="server" Text='<%#Eval("Name") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtftrattname" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvattname" runat="server" ControlToValidate="txtftrattname" Text="*" ValidationGroup="validaiton" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlTypeEdit" runat="server"  CssClass="b_dropdown">
                                            <asp:ListItem>Checkbox</asp:ListItem>
                                            <asp:ListItem>Textbox</asp:ListItem>
                                            <asp:ListItem>Dropdown</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlAttributeTypeDataEntry" runat="server" CssClass="b_dropdown">
                                            <asp:ListItem>Checkbox</asp:ListItem>
                                            <asp:ListItem>Textbox</asp:ListItem>
                                            <asp:ListItem>Dropdown</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvdesignation" runat="server" ControlToValidate="ddlAttributeTypeDataEntry" Text="*" ValidationGroup="validaiton" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtValueEdit" runat="server" width="110"/>
                                        <asp:Button ID="btnValueEdit" runat="server" Text="Add" OnClick="btnValueEdit_Click" CssClass="small_button white_button open" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlValueShow" runat="server" Visible="false" CssClass="b_dropdown">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtftrValue" runat="server" width="110"/>

                                         <asp:Button ID="btnValues" runat="server" Text="Add" OnClick="btnValues_Click" CssClass="small_button white_button open" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Default Value">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlDefaultValueEdit" runat="server"  CssClass="b_dropdown"></asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDefaultValue" runat="server" Text='<%#Eval("DefaultValue") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="dldefaultValue" runat="server" CssClass="b_dropdown">
                                            <asp:ListItem Selected="True" Text="None" Value="None"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvDefaultValue" runat="server" ControlToValidate="dldefaultValue" Text="*" ValidationGroup="validaiton" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle />
                            <PagerTemplate>
                            </PagerTemplate>
                        </asp:GridView>
                        <div class="tdfoot "> 
                            <asp:Label ID="lblresult" runat="server" ></asp:Label>
                            <asp:Button ID="btnCancel" runat="server" Text="BACK" OnClick="btnCancel_Click" CssClass="small_button white_button open" />
                            </div>
                  
                     
                        </table>
                    </div>
                </div>
            </div>

            <!--End form-->
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
