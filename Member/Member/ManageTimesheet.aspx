<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ManageTimesheet.aspx.cs" Inherits="Member_ManageTimesheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript">
    </script>
    <script language='javascript' type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequest);
        function BeginRequest(sender, e) {
            e.get_postBackElement().disabled = true;

        }

        function DescSize() {
            //if (document.forms[0].txtDesc.value.length > 254) {
            //    return false;
            //}
            //else {
            //    return true;
            //}
            if (CheckDescSize()) {
                return true;
            }
            else {
                return false;
            }
        }
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

        function OnEditProductSupplyMaster(id, rowIndex) {

            document.getElementById("<%=hdStockTypeMasterID.ClientID %>").value = id;

            document.getElementById("<%=hdIndexID.ClientID%>").value = rowIndex;
            var clickButton = document.getElementById("<%= btnClick.ClientID %>");
            clickButton.click();
        }

        function select_deselectAll(chkVal, idVal) {
            if (!chkVal)
                document.getElementById('ctl00_ContentPlaceHolder1_gridSearch_ctl01_chkHeadChecked').checked = false;
            var frm = document.forms[0];
            // Loop through all elements
            for (i = 0; i < frm.length; i++) {

                // Look for our Header Template's Checkbox
                if (idVal.indexOf('chkHeadChecked') != -1) {
                    // Check if main checkbox is checked, then select or deselect datagrid checkboxes 
                    if (chkVal == true) {
                        frm.elements[i].checked = true;
                    }
                    else {
                        frm.elements[i].checked = false;
                    }
                    // Work here with the Item Template's multiple checkboxes
                }
                else if (idVal.indexOf('chkItemChecked') != -1) {
                    // Check if any of the checkboxes are not checked, and then uncheck top select all checkbox
                    if (frm.elements[i].checked == false) {

                        frm.elements[1].checked = false; //Uncheck main select all checkbox
                    }
                }
            }
        }

        function validate() {

            if (document.getElementById("<%=ddlProj.ClientID%>").selectedIndex == 0) {
                var grp = document.getElementById('grp');
                grp.innerHTML = "Please select Project.";

            }
            else {
                var grp = document.getElementById('grp');
                grp.innerHTML = "";
            }


            if (document.getElementById("<%=ddlProj.ClientID%>").selectedIndex == 0) {
                return false;
            }
            else {
                return true;

            }
        }

        function validateEmp() {

            if (document.getElementById("<%=ddlempProjMember.ClientID%>").selectedIndex == 0) {
                var grp = document.getElementById('grpEmp');
                grp.innerHTML = "Please select Project Member.";

            }
            else {
                var grp = document.getElementById('grpEmp');
                grp.innerHTML = "";
            }


            if (document.getElementById("<%=ddlempProjMember.ClientID%>").selectedIndex == 0) {
                return false;
            }
            else {
                return true;

            }
        }



    </script>
    <asp:ScriptManager ID="src" runat="server"></asp:ScriptManager>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">



                    <asp:Label ID="lblModuleDetails" Text="Timesheet" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right" id="divLocation" runat="server" visible="false">
                        <font face="Verdana" size="2" color="#F60"><b>Location:</b></font>
                        <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                            OnSelectedIndexChanged="dlLocation_SelectedIndexChanged" Visible="false" AppendDataBoundItems="true" Width="200px">
                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <font face="Verdana" size="2">
                            <asp:Label ID="lblLocationId" runat="server" Visible="false" /></font>
                        <asp:Button ID="btnAdd" Text="Monthly Timesheet" runat="server" OnClick="btnAdd_Click" CssClass="small_button white_button open" CausesValidation="false" />
                    </div>
                </div>

                <div id="DivPreview" runat="server" style="padding-top: 20px; display: none;">
                </div>
                <%--  <div style="float: right;" width="200px;">
                   
                </div>--%>
                <asp:Panel ID="pnlSearchTimesheet" runat="server" Visible="True">
                    <asp:Table ID="tbAddModule" runat="server" Width="100%" class="manage_form mrg_bot">

                        <asp:TableRow>
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Project</asp:TableHeaderCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddlProj" OnSelectedIndexChanged="ddlProj_SelectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true" runat="server" Width="250">
                                </asp:DropDownList>

                            </asp:TableCell>

                            <asp:TableHeaderCell HorizontalAlign="Right" ID="AdminEmpMember" runat="server" VerticalAlign="Middle">Project Member</asp:TableHeaderCell>
                            <asp:TableCell ID="AdminEmpMember1" runat="server">
                                <asp:DropDownList ID="ddlempProjMember" AutoPostBack="true" OnSelectedIndexChanged="ddlProjMember_SelectedIndexChanged" AppendDataBoundItems="true" runat="server" Width="250"></asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow ID="Admin2" runat="server">
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Module</asp:TableHeaderCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddlselectModule" OnSelectedIndexChanged="ddlselectModule_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" runat="server" Width="250">
                                </asp:DropDownList>


                            </asp:TableCell>

                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Project</asp:TableHeaderCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddlEmpProj" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEmpProj_SelectedIndexChanged" runat="server" AutoPostBack="true" Width="250">
                                </asp:DropDownList>

                            </asp:TableCell>

                        </asp:TableRow>

                        <asp:TableRow ID="Admin3" runat="server">
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Project Member</asp:TableHeaderCell><asp:TableCell>
                                <asp:DropDownList ID="ddlProjMember" AutoPostBack="true" OnSelectedIndexChanged="ddlProjMember_SelectedIndexChanged1" AppendDataBoundItems="true" runat="server" Width="250">
                                </asp:DropDownList>
                            </asp:TableCell>

                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Module</asp:TableHeaderCell><asp:TableCell>
                                <asp:DropDownList ID="ddlEmpModule" runat="server" AppendDataBoundItems="true" Width="250">
                                </asp:DropDownList>
                            </asp:TableCell>

                        </asp:TableRow>


                        <asp:TableRow>
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Status</asp:TableHeaderCell><asp:TableCell>
                                <asp:DropDownList ID="ddlStatus" runat="server" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" Width="250">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                    <asp:ListItem Value="0">UnApproved</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>

                            <asp:TableHeaderCell HorizontalAlign="Right" ID="empStatus" runat="server" VerticalAlign="Middle">Status</asp:TableHeaderCell><asp:TableCell>
                                <asp:DropDownList ID="ddlEmpStatus" runat="server" AutoPostBack="true" Width="250">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                    <asp:ListItem Value="0">UnApproved</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>

                        </asp:TableRow>
                        <asp:TableRow ID="adminSearch" runat="server">
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Search</asp:TableHeaderCell><asp:TableCell>
                                <asp:LinkButton ID="btnSearch" autopostback="true" CausesValidation="false" CssClass="small_button white_button open" OnClientClick="return validate()" runat="server"
                                    OnClick="Search_Click">Search</asp:LinkButton>
                            </asp:TableCell>

                            <asp:TableHeaderCell ID="TableHeaderCell1" runat="server" HorizontalAlign="Right" VerticalAlign="Middle">Search</asp:TableHeaderCell><asp:TableCell>
                                <asp:Button ID="btnempsearch" OnClick="btnempsearch_Click" runat="server" CssClass="small_button white_button open" Text="Search" OnClientClick="return validateEmp()"></asp:Button>

                            </asp:TableCell>

                        </asp:TableRow>
                    </asp:Table>
                    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
                </asp:Panel>
                <asp:Panel ID="pnlAddTimesheet" runat="server">
                    <%--<td colspan="2">--%>
                    <table align="left" width="100%" border="1" cellpadding="2" cellspacing="0">
                        <tr>
                            <td style="width: 15%" align="center">
                                <font face="Verdana" size="2"><b>Date </b></font>
                            </td>
                            <td width="20%" align="center">
                                <font face="Verdana" size="2"><b>Module</b></font>
                            </td>
                            <td style="width: 15%" align="center" id="trprojmember" visible="true"
                                runat="server">
                                <font face="verdana" size="2"><b>Project Member</b></font>
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
                                <asp:TextBox ID="txtDate" runat="server" size="14" onclick="popupCalender('txtDate')"
                                    onkeypress="return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="MT" ControlToValidate="txtDate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                <ajax:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtDate" CssClass="cal_Theme1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                            </td>
                            <td align="center">
                                <asp:DropDownList ID="ddlModule" AutoPostBack="true" CssClass="b_dropdown" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Value="0" Text="Select" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvModule" ValidationGroup="MT" ControlToValidate="ddlModule" InitialValue="0" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                            </td>
                            <td align="center" id="tdmember" runat="server" visible="true">
                                <asp:DropDownList ID="ddlAdminSelectMember" CssClass="b_dropdown" runat="server" Width="130px">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAdminMember" ControlToValidate="ddlAdminSelectMember" InitialValue="0" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
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
                                <asp:RequiredFieldValidator ID="rfvDesc" ValidationGroup="MT" ControlToValidate="txtDiscription" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <em>
                                    <asp:Label ID="lblMaxSize" Text="500" runat="server"></asp:Label></em>
                            </td>
                            <td align="center">
                                <asp:LinkButton ID="btnSubmit" CommandName="Save" ValidationGroup="MT" CssClass="small_button white_button open" runat="server" OnClick="btnSubmit_Click">Save</asp:LinkButton>
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
                    <tr>
                        <td id="tdApprove" align="right" runat="server">
                            <asp:LinkButton ID="btnApproved" CausesValidation="false" CssClass="small_button white_button open" runat="server" Text="Approved/UnApproved" OnClick="btnChecked_Click"></asp:LinkButton>
                        </td>
                    </tr>

                </table>

                <asp:Panel ID="pnlEmployeeGridView" runat="server" Visible="true">
                    <asp:GridView ID="grdTimesheet" DataKeyNames="tsId" AllowSorting="true" OnSorting="grdTimesheet_Sorting" OnRowDataBound="grdTimesheet_RowDataBound" runat="server" ShowHeaderWhenEmpty="true" OnPageIndexChanging="grdTimesheet_OnPageIndexChanging"
                        CssClass="manage_grid_a mange_lsttd" AutoGenerateColumns="False" ShowFooter="true" AllowPaging="true" PageSize="60" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="tsDate" SortExpression="tsDate" DataFormatString="{0:dd-MMM-yy}" HeaderText="Date" />
                            <asp:BoundField DataField="projName" SortExpression="projName" HeaderText="Project Name" />

                            <asp:TemplateField HeaderText="Module Name" SortExpression="moduleName1">
                                <ItemTemplate>
                                    <asp:Label ID="lblTsId" runat="server" Visible="false" Text='<%#Eval("tsId") %>' />
                                    <asp:Label ID="lblModuleName" runat="server" Text='<%#Eval("moduleName1") %>' />
                                    <asp:Label ID="prjmoduleId" runat="server" Visible="false" Text='<%#Eval("moduleId") %>'></asp:Label>
                                </ItemTemplate>
                                <%--column inside label need to display footer template will come--%>
                                <FooterTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Text="Total Hours"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hours" SortExpression="tsHour">
                                <ItemTemplate>
                                    <asp:Label ID="lblHours" runat="server" Text='<%#Eval("tsHour") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblHoursTotal1" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment" HeaderStyle-Width="230px" ItemStyle-HorizontalAlign="Center" SortExpression="tsComment">
                                <ItemTemplate>
                                    <asp:Label ID="lblCommentName" runat="server" Text='<%#Eval("tsComment") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="tsEntryDate" SortExpression="tsEntryDate" DataFormatString="{0:dd-MMM-yy hh:mm tt}"
                                HeaderText="Entry Date" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" Visible="false" runat="server" Text='<%#Eval("tsVerified") %>' />

                                    <%--  <asp:LinkButton ID="btnEdit"  Enabled="true" CommandArgument='<%#Eval("tsId") %>'
                                                        runat="server" OnClick="EditData" CommandName="View" CausesValidation="False"><img src="images/edit.png" border="0"  alt="Edit" /></asp:LinkButton>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle />
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoDataFound" runat="server" Text="No Data Found." Font-Bold="true"
                                Font-Size="Medium" />
                        </EmptyDataTemplate>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlAdminGridView" runat="server">
                    <asp:GridView ID="gridSearch" DataKeyNames="tsId" PageSize="60" Width="100%" ShowFooter="True" AllowSorting="true" CssClass="manage_grid_a mange_lsttd" OnPageIndexChanging="gridSearch_PageIndexChanging" OnSorting="gridSearch_Sorting" Visible="false" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" runat="server" OnRowUpdating="gridSearch_RowUpdating" OnRowDataBound="gridSearch_RowDataBound">
                        <Columns>

                            <%--     <asp:BoundField HeaderText="tsId" SortExpression="tsId" Visible="true"  DataField="tsId"  />--%>
                            <asp:BoundField HeaderText="tsVerified" SortExpression="tsVerified" Visible="false" DataField="tsVerified" />
                            <asp:BoundField DataField="moduleId" SortExpression="moduleId" Visible="false" HeaderText="moduleId"></asp:BoundField>
                            <asp:BoundField HeaderText="empid" SortExpression="empid" Visible="false" DataField="empid" />
                            <asp:BoundField HeaderText="Date" SortExpression="tsDate" Visible="true" DataField="tsDate" DataFormatString="{0:dd-MMM-yy}" />
                            <asp:BoundField HeaderText="Module Name" SortExpression="moduleName1" Visible="false" DataField="moduleName1" />
                            <asp:BoundField HeaderText="Project Name" SortExpression="projName" Visible="True" DataField="projName" />
                            <asp:BoundField DataField="moduleName1" HeaderText="ModuleName" Visible="false"></asp:BoundField>
                            <asp:TemplateField HeaderText="Module Name" SortExpression="moduleName1">
                                <ItemTemplate>
                                    <asp:Label ID="lblTsId" runat="server" Visible="false" Text='<%#Eval("tsId")%>'></asp:Label>
                                    <asp:Label ID="lblUpdateModuleName" runat="server" Text='<%#Eval("moduleName1") %>' />
                                    <asp:Label ID="lblModuleId" Visible="false" runat="server" Text='<%#Eval("moduleId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Project Member" SortExpression="empName">
                                <ItemTemplate>
                                    <asp:Label ID="lbldropProjectMember" Text='<%#Eval("empName") %>' runat="server"></asp:Label>
                                    <asp:Label ID="lblmemId" Visible="false" runat="server" Text='<%#Eval("empid") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Text="Total Hours"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Hour" SortExpression="tsHour" HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdatehours" Text='<%#Eval("tsHour") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblHoursTotal" Text="total Hours" runat="server" Visible="true" />

                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="30%" SortExpression="tsComment">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdateCommentName" runat="server" Text='<%#Eval("tsComment") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField HeaderText="Entry Date" SortExpression="tsEntryDate" DataFormatString="{0:dd-MMM-yy}" Visible="true" DataField="tsEntryDate" />
                            <asp:TemplateField HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDeleteTask" runat="server" Visible="<%#!IsUdateEnabled()%>" OnClientClick="javascript:return confirm('Are you sure you want to delete this record?')" OnCommand="DeleteTask" Text="Delete"
                                        CommandArgument='<%# Eval("tsId") %>' CausesValidation="false">
                                        <img src="images/delete.png" border="0"  alt="delete" />  
                                    </asp:LinkButton>
                                    <asp:Image ID="Image1" alt="delete" src="images/delete.png" Visible="<%#IsUdateEnabled()%>" runat="server" />

                                </ItemTemplate>
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderStyle-Width="10px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%#Eval("tsId") %>' Text="Update" CausesValidation="false" CommandName="Update" OnCommand="UpdateTask"><img src="images/edit.png" border="0"  alt="edit" /></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-CssClass="chkheaderstyle">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeadChecked" onclick="javascript: return select_deselectAll (this.checked, this.id);" runat="server" AutoPostBack="true" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItemChecked" onclick="javascript: return select_deselectAll (this.checked, this.id);" Checked='<%# Convert.ToBoolean(Eval("tsVerified")) %>' runat="server" AutoPostBack="true" />
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
    <input type="hidden" id="tsId" runat="server" />
    <input type="hidden" id="lId" runat="server" />
    <input type="hidden" id="monId" runat="server" />
    <input type="hidden" id="dateval" runat="server" />
    <input type="hidden" id="curdate" runat="server" />

    <asp:HiddenField ID="hdntaskId" runat="server" />
    <asp:HiddenField ID="hdnMonthNo" runat="server" />
    <asp:HiddenField ID="hdnYear" runat="server" />
    <asp:HiddenField ID="hdStockTypeMasterID" runat="server" />
    <asp:HiddenField ID="hdIndexID" runat="server" />
    <asp:Button ID="btnClick" runat="server" CommandName="edit" OnClick="btnClick_Click" Style="display: none;"
        CausesValidation="false"></asp:Button>

</asp:Content>
