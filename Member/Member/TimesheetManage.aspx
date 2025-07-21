<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="TimesheetManage.aspx.cs" Inherits="Member_TimesheetManage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css" />
    <link href="../styles/kendo.common.min.css" rel="stylesheet" />
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

        function EditTS(id, rowIndex) {

            document.getElementById("<%=hdStockTypeMasterID.ClientID %>").value = id;

            document.getElementById("<%=hdIndexID.ClientID%>").value = rowIndex;
            var clickButton = document.getElementById("<%= btnClick.ClientID %>");
            clickButton.click();
        }
        function CheckClicked() {
            document.getElementById("<%=hdnCheckMonthClick.ClientID %>").value = "Click"

        }
        function CheckValidation() {
            document.getElementById("<%=hdnCheckConsolidateTS.ClientID %>").value = "Click"
        }
        function CheckDate() {
            var startDate = document.getElementById('<%= txtStartDate.ClientID %>').value;
            var endDate = document.getElementById('<%= txtEndDate.ClientID %>').value;

            if (startDate === '' || endDate === '') {
                alert("Please enter both Start Date and End Date.");
                return false;
            }

            return true;
        }
    </script>
    <%-- <style type="text/css">
        .FixedHeader {
            position: sticky;
            top: 0;
            background-color: #f2f2f2
        }
    </style>--%>
    <asp:ScriptManager ID="src" runat="server"></asp:ScriptManager>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <div style="float: left">
                        <asp:Label ID="lblModuleDetails" Text="Timesheet" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    </div>
                    <div id="divTotlalhrs" runat="server" style="float: right">
                        <b>Total hours : </b>
                        <asp:Label ID="LblTotalHrs" Text="Total Hours" runat="server"></asp:Label>
                        hrs
                    </div>
                </div>

                <asp:Panel ID="pnlSearchTimesheet" runat="server" Visible="True">
                    <asp:Table ID="tbAddModule" runat="server" Width="100%" class="manage_form mrg_bot">

                        <asp:TableRow>
                            <asp:TableHeaderCell HorizontalAlign="Right" ID="AdminEmpMember" runat="server" VerticalAlign="Middle">Members</asp:TableHeaderCell>
                            <asp:TableCell ID="AdminEmpMember1" runat="server">
                                <asp:DropDownList ID="ddlMember" AppendDataBoundItems="true" runat="server" Width="250"></asp:DropDownList>
                            </asp:TableCell>

                            <asp:TableHeaderCell HorizontalAlign="Right" ID="empStatus" runat="server" VerticalAlign="Middle">Status</asp:TableHeaderCell><asp:TableCell>
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="250">
                                    <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                    <asp:ListItem Value="1">Approved</asp:ListItem>
                                    <asp:ListItem Value="0">UnApproved</asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow>
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Module</asp:TableHeaderCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="ddlselectModule" AppendDataBoundItems="true" runat="server" Width="250">
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableHeaderCell HorizontalAlign="Right" VerticalAlign="Middle">Search</asp:TableHeaderCell><asp:TableCell>
                                <asp:LinkButton ID="btnSearch" autopostback="true" CausesValidation="false" CssClass="small_button white_button open" OnClientClick="return validate()" runat="server"
                                    OnClick="Search_Click">Search</asp:LinkButton>
                                &nbsp; &nbsp;
                                <asp:Button ID="btnConsolidateTimesheet" runat="server" CssClass="small_button white_button open" OnClientClick="CheckValidation()" OnClick="btnConsolidateTimesheet_Click" Text="View Consolidate Timesheet" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
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
                                <asp:TextBox ID="txtDate" runat="server" size="14" onclick="popupCalender('txtDate')" autocomplete="off"
                                    onkeypress="return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="MT" ControlToValidate="txtDate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                                <ajax:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtDate" CssClass="cal_Theme1" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                            </td>
                            <td align="center">
                                <asp:DropDownList ID="ddlAddModule" CssClass="b_dropdown" runat="server">
                                    <asp:ListItem Value="0" Text="Select" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvModule" ValidationGroup="MT" ControlToValidate="ddlAddModule" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>

                            </td>
                            <td align="center" id="tdmember" runat="server" visible="true">
                                <asp:DropDownList ID="ddlAddMember" CssClass="b_dropdown" runat="server" Width="130px">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAdminMember" ValidationGroup="MT" ControlToValidate="ddlAddMember" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
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
                                    runat="server" OnClick="PagerButtonClick" OnClientClick="CheckClicked()" CausesValidation="False" />
                                <asp:Label ID="lblMonth" runat="server"></asp:Label>
                                <asp:LinkButton ID="nextMonth" Text=">>" CommandArgument="next" OnClientClick="CheckClicked()" runat="server" OnClick="PagerButtonClick"
                                    CausesValidation="False" />
                            </b></font>
                        </td>
                        <td id="td1" align="right" runat="server">
                            <div Style="float: right;">
                                 <asp:LinkButton ID="btnApproved" CausesValidation="false" CssClass="small_button white_button open" runat="server" Text="Approve" OnClick="btnChecked_Click"></asp:LinkButton>
                                 <asp:Button runat="server" ID="Button1" Text="Export" OnClick="btnExcelSaved_Click" CssClass="small_button white_button open" Style="float: right; margin-bottom: 10px; margin-left: 15px" />
                            </div>
                           
                        </td>
                    </tr>

                </table>
                <asp:Panel ID="pnlAdminGridView" runat="server">
                    <asp:GridView ID="gridSearch" DataKeyNames="tsId" Width="100%" ShowFooter="True" AllowSorting="true" CssClass="manage_grid_a mange_lsttd" OnSorting="gridSearch_Sorting" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" runat="server" OnRowDataBound="gridSearch_RowDataBound" Style="border: 1px solid red">
                        <Columns>
                            <asp:BoundField HeaderText="tsVerified" SortExpression="IsApproved" Visible="false" DataField="IsApproved" />
                            <asp:BoundField DataField="moduleId" SortExpression="moduleId" Visible="false" HeaderText="moduleId"></asp:BoundField>
                            <asp:BoundField HeaderText="empid" SortExpression="empid" Visible="false" DataField="empid" />
                            <asp:BoundField HeaderText="Date" SortExpression="tsDate" Visible="true" DataField="tsDate" DataFormatString="{0:dd-MMM-yy}" />
                            <asp:BoundField HeaderText="Module Name" SortExpression="moduleName" Visible="false" DataField="moduleName" />
                            <asp:BoundField HeaderText="Project Name" SortExpression="projName" Visible="True" DataField="projName" />
                            <asp:BoundField DataField="moduleName" HeaderText="ModuleName" Visible="false"></asp:BoundField>
                            <asp:TemplateField HeaderText="Module Name" SortExpression="moduleName">
                                <ItemTemplate>
                                    <asp:Label ID="lblTsId" runat="server" Visible="false" Text='<%#Eval("tsId")%>'></asp:Label>
                                    <asp:Label ID="lblUpdateModuleName" runat="server" Text='<%#Eval("moduleName") %>' />
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
                            <asp:TemplateField HeaderText="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="false" HeaderStyle-Width="400" SortExpression="tsComment">
                                <ItemTemplate>
                                    <asp:Label ID="lblUpdateCommentName" runat="server" Text='<%#Eval("tsComment").ToString().Replace("\r\n", "<BR>") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                           <asp:TemplateField HeaderText="Entry Date" SortExpression="tsEntryDate">
    <ItemTemplate>
        <asp:Label ID="lblEntryDate" runat="server" Text='<%# Eval("tsEntryDate", "{0:dd-MMM-yy}") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="20px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDeleteTask" runat="server" OnClientClick="javascript:return confirm('Are you sure you want to delete this record?')" OnCommand="DeleteTask" Text="Delete"
                                        CommandArgument='<%# Eval("tsId") %>' CausesValidation="false">
                                        <img src="images/delete.png" border="0"  alt="delete" />  
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-CssClass="chkheaderstyle">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkHeadChecked" onclick="javascript: return select_deselectAll (this.checked, this.id);" runat="server" AutoPostBack="true" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItemChecked" onclick="javascript: return select_deselectAll (this.checked, this.id);" Checked='<%# Convert.ToBoolean(Eval("IsApproved")) %>' runat="server" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <%--<EmptyDataTemplate>
                            <asp:Label ID="lblNoDataFound" runat="server" Text="No Data Found." Font-Bold="true"
                                Font-Size="Medium" />
                        </EmptyDataTemplate>--%>
                    </asp:GridView>
                </asp:Panel>

            </div>
        </div>
    </div>
    <input type="hidden" id="tsId" runat="server" />
    <input type="hidden" id="lId" runat="server" />
    <input type="hidden" id="monId" runat="server" />
    <asp:HiddenField ID="hdntaskId" runat="server" />
    <asp:HiddenField ID="hdStockTypeMasterID" runat="server" />
    <asp:HiddenField ID="hdIndexID" runat="server" />
    <asp:HiddenField ID="hdnCheckMonthClick" Value="NotClick" runat="server" />
    <asp:HiddenField ID="hdnCheckConsolidateTS" Value="NotClick" runat="server" />
    <asp:Button ID="btnClick" runat="server" CommandName="edit" OnClick="btnClick_Click" Style="display: none;"
        CausesValidation="false"></asp:Button>
    <%-- Add for US 968-Consolidate_timesheet --%>
    <div id="divAddPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 370px; min-height: 50px; top: 8%; left: 250px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div runat="server" id="divCT" style="width: 100%; height: 400px; padding-top: 15px; overflow: auto;overflow-x:hidden;" visible="false">
            <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()" alt="Close" />
            <asp:Label ID="lblHours" runat="server" Visible="false" Style="font-size: Medium; font-weight: bold;">Total Hours</asp:Label>         
   <table>
      <tr>
          <td>
           <strong> From Date :</strong>
           <asp:TextBox ID="txtStartDate" runat="server" CssClass="date-picker" placeholder="Start Date" />
           <ajax:CalendarExtender ID="CalendarExtenderStart" runat="server" TargetControlID="txtStartDate" Format="dd-MMM-yyyy" />
           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <strong> To Date :</strong>
           <asp:TextBox ID="txtEndDate" runat="server" CssClass="date-picker" placeholder="End Date" />
           <ajax:CalendarExtender ID="CalendarExtenderEnd" runat="server" TargetControlID="txtEndDate" Format="dd-MMM-yyyy" />
           &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
           <asp:Button ID="Button2" runat="server" Text="Search" CssClass="small_button white_button open" OnClientClick="return CheckDate()" OnClick="btnSearch_Click"/>
       </td>
     </tr>
   </table>
                <br />
                <br />
            <strong>Project Start Date :</strong>
            <asp:Label runat="server" ID="lblProjectStartDate"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <strong>Project Status :</strong>
            <asp:Label runat="server" ID="lblProjectStausDate"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button runat="server" ID="btnExcelSave" Text="Export" OnClick="btnExcelSave_Click" CssClass="small_button white_button open" Style="float: right; margin-bottom: 10px" />
            <br />

            <br />
            <asp:GridView ID="gridConsolidateTimesheet" Width="100%" Height="50%" runat="server" HeaderStyle-CssClass="FixedHeader" ShowFooter="True" AllowSorting="true" CssClass="manage_grid_a mange_lsttd" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Style="border: 1px solid red;"
                DataKeyNames="EmpName,Designation,TotalHours,Module" OnSorting="gridConsolidateTimesheet_Sorting" AllowPaging="true" PageSize="11" OnPageIndexChanging="gridConsolidateTimesheet_PageIndexChanging">
                <Columns>

                    <asp:TemplateField HeaderText="Name" SortExpression="empName">
                        <ItemTemplate>
                            <asp:Label ID="lblempName" Text='<%#Eval("EmpName") %>' runat="server"></asp:Label>

                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotalHours" runat="server" Text="Total Hours" Font-Bold="true"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Hours" SortExpression="Hour" HeaderStyle-Width="10px">
                        <ItemTemplate>
                            <asp:Label ID="lblUpdatehours" Text='<%#Eval("TotalHours") %>' runat="server"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblProjectTotalHrs" Text='<%#Eval("ProjectTotalHrs") %>' runat="server" Font-Bold="true" Visible="true" />
                             <%-- <asp:Label ID="lblProjectTotalHrs" runat="server" Text="" Visible="true" />--%>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Designation" SortExpression="designation">
                        <ItemTemplate>
                            <asp:Label ID="lblDesignation" Text='<%#Eval("Designation")%>' runat="server"></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Module" SortExpression="module">
                        <ItemTemplate>
                            <asp:Label ID="lblModule" Text='<%#Eval("Module")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Designation" DataField="Designation" />--%>
                </Columns>
              <%--  <EmptyDataTemplate>
                    <asp:Label ID="lblNoDataFound" Style="display: flex; justify-content: space-around;" runat="server" Text="No Data Found." Font-Bold="true"
                        Font-Size="Medium" />
                </EmptyDataTemplate>--%>

            </asp:GridView>
        </div>
        <br />
        <table id="tblCTMonth" runat="server" width="100%" class="manage_form" visible="false">
            <tr>
                <td>
                    <font face="Verdana" size="2"><b>
                        <asp:LinkButton ID="prev" Text="<<" CommandArgument="prev" OnClick="prev_next_Click"
                            runat="server" CausesValidation="False" />
                        <asp:Label ID="lblCurrentMonth" runat="server"></asp:Label>
                        <asp:LinkButton ID="next" Text=">>" CommandArgument="next" runat="server" OnClick="prev_next_Click"
                            CausesValidation="False" />
                    </b></font>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblNotDataFound" runat="server" Visible="false" Style="font-size: Medium; font-weight: bold;"></asp:Label>
     <%-- <%--  <asp:GridView ID="gridConsolidateTimesheetForOneMonth" Width="100%" runat="server" HeaderStyle-CssClass="FixedHeader" ShowFooter="True"
            Visible="false" AllowSorting="true" CssClass="manage_grid_a mange_lsttd" AutoGenerateColumns="false" Style="border: 1px solid red"
            DataKeyNames="EmpName,TotalHours">
            <Columns>
                <%-- <asp:BoundField HeaderText="Name" DataField="EmpName" />--%>
                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("EmpName") %>'></asp:Label>
                    </ItemTemplate>
                     <%--<FooterTemplate>
                        <asp:Label ID="lblTotalHours" runat="server" Text="Total Hours" Font-Bold="true"></asp:Label>
                    </FooterTemplate>--%>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hours" SortExpression="Hours">
                    <ItemTemplate>
                        <asp:Label ID="lbHours" runat="server" Text='<%#Eval("Totalhours") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblMonthTotalHours" Text='<%#Eval("MonthTotalHours") %>' runat="server" Font-Bold="true"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <%-- <asp:BoundField HeaderText="Hours" DataField="TotalHours" />--%>
            </Columns>
           <%-- <EmptyDataTemplate>
                <asp:Label ID="lblNoDataFound" Style="display: flex; justify-content: space-around;" runat="server" Text="No Data Found." Font-Bold="true"
                    Font-Size="Medium" />
            </EmptyDataTemplate>--%>

        </asp:GridView>
    </div>
    <script type="text/javascript">     
        function closeAddPopUP() {
            $("#divAddPopup").css('display', 'none');
            $("#gridConsolidateTimesheet").css('display', 'none');
            $("#divAddPopupOverlay").css('display', 'none');

        }

        function ShowPopUpInActive() {
            $('#divAddPopup').css('display', '');
            $('#divAddPopupOverlay').addClass('k-overlay');
            $('#divdoInactiveUsers').css('display', '');
            $('#divdoInactiveUsers').addClass('k-overlay');
        }
    </script>
    <%-- End --%>
</asp:Content>
