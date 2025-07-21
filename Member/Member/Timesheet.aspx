<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Timesheet.aspx.cs" Inherits="Member_Timesheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript">
    </script>
    
    <script language='javascript' type="text/javascript">

 function CheckDescSize() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_txtDiscription').value.trim().length > 500) {
                var CharLength = '<%=txtDiscription.MaxLength %>';
                alert("You can not enter more than " + CharLength + " characters in Description.")
                document.getElementById('ctl00_ContentPlaceHolder1_txtDiscription').value = document.getElementById('ctl00_ContentPlaceHolder1_txtDiscription').value.substring(0, CharLength);
                document.getElementById('ctl00_ContentPlaceHolder1_txtDiscription').focus();
                return false;
            }
            else {
                return true;
            }
        }

        function validate() {
            return true;
        }
        function DoEdit(id, rowIndex) {
           document.getElementById("<%=hdStockTypeMasterID.ClientID %>").value = id;
            document.getElementById("<%=hdIndexID.ClientID%>").value = rowIndex;
            var clickButton = document.getElementById("<%= btnClick.ClientID %>");
            clickButton.click();
        }
    </script>
    <asp:ScriptManager ID="src" runat="server"></asp:ScriptManager>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div id="DivPreview" runat="server" style="padding-top: 20px; display: none;">
                </div>
                <asp:Panel ID="pnlSearchTimesheet" runat="server" Visible="True">
                </asp:Panel>
                <asp:Panel ID="pnlAddTimesheet" runat="server">
                    <table align="left" width="100%" border="1" cellpadding="2" cellspacing="0">
                        <tr>
                            <td style="width: 15%" align="center">
                                <font face="Verdana" size="2"><b>Date </b></font>
                            </td>
                            <td width="20%" align="center">
                                <font face="Verdana" size="2"><b>Module</b></font>
                            </td>
                            <td width="15%" align="center">
                                <font face="Verdana" size="2"><b>Hours</b></font>
                            </td>
                            <td style="width: 35%" align="center">
                                <font face="Verdana" size="2"><b>Description</b></font>
                            </td>
                            <td style="width: 15%" align="center">
                                <font face="verdana" size="2">&nbsp;</font>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:TextBox ID="txtDate" runat="server" size="14"  onkeypress="return false;" autocomplete="off"></asp:TextBox><%-- onclick="popupCalender('txtDate')"--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txtDate" ValidationGroup="T" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                               <ajax:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtDate" CssClass="cal_Theme1" Format="dd-MMM-yyyy" ></ajax:CalendarExtender>

                            </td>
                            <td align="center">
                                <asp:DropDownList ID="ddlModule" CssClass="b_dropdown" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0" Text="Select" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvModule" ControlToValidate="ddlModule" ValidationGroup="T" InitialValue="0" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                            </td>
                            <td align="center">
                                <asp:DropDownList ID="ddlHours" runat="server" Style="margin-bottom: 15px; width: 70px;">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8" Selected="True">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                </asp:DropDownList>

                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtDiscription" Width="275" Height="75" MaxLength="500" runat="server" TextMode="MultiLine"
                                    OnKeyPress="return CheckDescSize();" OnChange="return CheckDescSize();">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDesc" ControlToValidate="txtDiscription" ValidationGroup="T" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <em>
                                    <asp:Label ID="lblMaxSize" Text="500" runat="server"></asp:Label></em>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="btnSubmit" ValidationGroup="T" CommandName="Save" CssClass="small_button white_button open" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
                            
                             <asp:LinkButton ID="btnClear" ValidationGroup="T" CommandName="Clear" CssClass="small_button white_button open" runat="server" OnClick="btnClear_Click" >Clear</asp:LinkButton>
                            </td>
                        </tr>
                    </table>

                </asp:Panel>

                <table id="tblsalaryslip" runat="server" width="100%" class="manage_form">
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
                </table>
                <asp:Panel ID="pnlAdminGridView" runat="server">
                    <asp:GridView ID="gridSearch" DataKeyNames="tsId" PageSize="60" Width="100%" ShowFooter="True" AllowSorting="true" CssClass="manage_grid_a mange_lsttd" OnPageIndexChanging="gridSearch_PageIndexChanging" OnSorting="gridSearch_Sorting" Visible="false" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" runat="server" OnRowDataBound="gridSearch_RowDataBound">
                        <Columns>
                            <asp:BoundField HeaderText="Is Verified" SortExpression="IsApproved" Visible="false" DataField="IsApproved" />
                            <asp:BoundField DataField="moduleId" SortExpression="moduleId" Visible="false" HeaderText="moduleId"></asp:BoundField>
                            <asp:BoundField HeaderText="empid" SortExpression="empid" Visible="false" DataField="empid" />
                            <%--<asp:BoundField HeaderText="Date" SortExpression="tsDate" Visible="true" DataField="tsDate" DataFormatString="{0:dd-MMM-yy}" />--%>
                            <asp:TemplateField HeaderText="Date" SortExpression="tsDate">
                                <ItemTemplate>
                                    <%# Eval("tsDisplayDate") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Module Name" SortExpression="moduleName" Visible="false" DataField="moduleName" />
                            <asp:BoundField HeaderText="Project Name" SortExpression="projName" Visible="True" DataField="projName" />
                            <asp:BoundField DataField="moduleName1" HeaderText="ModuleName" Visible="false"></asp:BoundField>
                            <asp:TemplateField HeaderText="Module Name" SortExpression="moduleName1">
                                <ItemTemplate>
                                    <asp:Label ID="lblTsId" runat="server" Visible="false" Text='<%#Eval("tsId")%>'></asp:Label>
                                    <asp:Label ID="lblUpdateModuleName" runat="server" Text='<%#Eval("moduleName") %>' />
                                    <asp:Label ID="lblModuleId" Visible="false" runat="server" Text='<%#Eval("moduleId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hour" SortExpression="tsHour" HeaderStyle-Width="10px">
                                <%--<ItemTemplate>
                                    <asp:Label ID="lblUpdatehours" Text='<%#Eval("tsHour") %>' runat="server"></asp:Label>
                                </ItemTemplate>--%>
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdatehours" runat="server"
                                        Text='<%# (Eval("tsHour") != DBNull.Value && Convert.ToDecimal(Eval("tsHour")) == 0 ? "" : Eval("tsHour").ToString()) %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblHoursTotal" Text="total Hours" runat="server" Visible="true" />

                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="30%" SortExpression="tsComment">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdateCommentName" runat="server" Text='<%#Eval("tsComment").ToString().Replace("\r\n", "<BR>")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField HeaderText="Entry Date" SortExpression="tsEntryDate" DataFormatString="{0:dd-MMM-yy}" Visible="true" DataField="tsEntryDate" />
                            <asp:TemplateField HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDeleteTask" runat="server" Visible="<%#!IsUdateEnabled()%>" OnClientClick="javascript:return confirm('Are you sure you want to delete this record?')" OnCommand="DeleteTask"  Text="Delete"
                                        CommandArgument='<%# Eval("tsId") %>' CausesValidation="false">
                                        <img src="images/delete.png" border="0"  alt="delete" />  
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoDataFound" runat="server" Text="No Data Found." Font-Bold="true"
                                Font-Size="Medium" />
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdntaskId" runat="server" />
    <asp:HiddenField ID="hdStockTypeMasterID" runat="server" />
    <asp:HiddenField ID="hdIndexID" runat="server" />
    <asp:HiddenField ID="hdProjID" runat="server" />
    <asp:HiddenField ID="hdnIsSendMail" runat="server" />

    <asp:Button ID="btnClick" runat="server" CommandName="edit" OnClick="btnClick_Click" Style="display: none;" CausesValidation="false"></asp:Button>
</asp:Content>

