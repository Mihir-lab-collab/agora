<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="EmpWFH.aspx.cs" Inherits="Member_EmpWFH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script src="js/EmpLeave.js"></script>
    <script type="text/javascript">
        function showDays() {

            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            $('[id$="hdnTo"]').val(ToDate);
            $('[id$="hdnFrom"]').val(FromDate);

            if (FromDate == "" && ToDate == "")
                return;

            $.ajax(
                {
                    type: "POST",
                    url: "EmpWFH.aspx/CalculateWFH",
                    contentType: "application/json;charset=utf-8",
                    data: "{'FromDate':'" + FromDate + "','ToDate':'" + ToDate + "' }",
                    cache: false,
                    async: false,
                    dataType: "json",
                    success: function (msg) {
                        var message = jQuery.parseJSON(msg.d);
                        $('[id$="lblWFHCount"]').text(message);

                    },
                    error: function (msg) {

                    }
                }
            );
        }
        function GetDataOnInsert() {

            var fromDate = $("#txtFromDate").val();
            var toDate = $("#txtToDate").val();
            var reason = $("#txtReason").val();
            var dateSpan = $("#lblDateError");
            var reasonSpan = $("#lblReason");
            var wfhCount = $("#" + '<%= lblWFHCount.ClientID %>');
            var wfhCountText = wfhCount.text();
            document.getElementById("<%=hdnWFHCount.ClientID%>").value = wfhCountText;
            if (fromDate == "" || toDate == "") {
                dateSpan.html("Date field cannot be blank");
                return false;
            }
            if (wfhCountText == "0") {
                alert('You cannot apply WFH on weekly off and public holiday.')
                return false;
            }
            if (reason == "") {
                reasonSpan.html("Please enter reason of Work From Home");
                return false;
            }
            else {
                document.getElementById("<%=hdnReason.ClientID%>").value = reason;
                reasonSpan.html("");
               
            }
            
            if (fromDate != '' && toDate != '') {
                dateFirst = fromDate.split('/');
                dateSecond = toDate.split('/');
                var startDate = new Date(dateFirst[2], dateFirst[1] - 1, dateFirst[0]); //Year, Month, Date
                var endDate = new Date(dateSecond[2], dateSecond[1] - 1, dateSecond[0]);

                if (startDate > endDate) {
                    dateSpan.html("From date should be less than or equal to To date.");
                    $('[id$="lblWFHCount"]').text('');
                    return false;
                }
                else {
                    dateSpan.html("");
                }
            }

            if (fromDate != "") {
                document.getElementById("<%=hdnFrom.ClientID%>").value = fromDate;
                if (toDate != "") {
                    document.getElementById("<%=hdnTo.ClientID%>").value = toDate;

                    return true;
                }
            }
        }
        function CheckDate() {

            var dateSpan = $("#lblDateError");
            var offSt = $("#txtFromDate").val();
            var offEn = $("#txtToDate").val();

            if (offSt != '' && offEn != '') {

                dateFirst = offSt.split('/');
                dateSecond = offEn.split('/');
                var startDate = new Date(dateFirst[2], dateFirst[1] - 1, dateFirst[0]); //Year, Month, Date
                var endDate = new Date(dateSecond[2], dateSecond[1] - 1, dateSecond[0]);
                if (startDate > endDate) {
                    dateSpan.html("From date should be less than or equal to To date.");
                    $('[id$="lblWFHCount"]').text('');
                    return false;
                }
                else {
                    showDays();
                    dateSpan.html("");
                    return true;
                }
            }
        }
        function setPopUPData() {
            var FromDate = $("#txtFromDate").val();
            var ToDate = $("#txtToDate").val();
            var EmpId = $('[id$="hdnEmpId"]').val();
            var flag = true;

            $.ajax(
                {
                    type: "POST",
                    url: "EmpWFH.aspx/IfExistsWFH",
                    contentType: "application/json;charset=utf-8",
                    data: "{'EmpID':'" + EmpId + "','FromDate':'" + FromDate + "','ToDate':'" + ToDate + "' }",
                    cache: false,
                    async: false,
                    dataType: "json",
                    success: function (msg) {
                        var message = jQuery.parseJSON(msg.d);
                        if (message == true) {
                            alert('Work From Home for same date already exists!');
                            flag = false;
                            return flag;
                        }
                        else {
                            flag = true;
                            return flag;
                        }

                    },
                    error: function (x, e) {
                        flag = false;
                        alert('error' + x.responseText);

                    }
                }
            );
            return flag;
        }
        function CheckInTime() {
            var EmpID = $('[id$="hdnEmpId"]').val();
            $.ajax(
                {
                    type: "POST",
                    url: "EmpWFH.aspx/CheckInTime",
                    contentType: "application/json;charset=utf-8",
                    data: "{'EmpId':'" + EmpID + "'}",
                    cache: false,
                    async: false,
                    dataType: "json",
                    success: function (msg) {
                        var message = jQuery.parseJSON(msg.d);
                        if (message == true) {
                            alert('you have already fill punch in time');
                            flag = false;
                            return flag;
                        }
                        else {
                            flag = true;
                            return flag;
                        }

                    },
                    error: function (x, e) {
                        flag = false;
                        alert('error' + x.responseText);

                    }
                }
            );
            return flag;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblWFH" Text="Work From Home" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button">Apply For WFH</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div>
                    <table id="tdLDetails" runat="server" width="100%" class="manage_grid_a" style="border-top: none; border-bottom: none;">
                        <tr>
                            <td colspan="4" style="text-align: left">
                                <b>
                                    <asp:Label ForeColor="Red" ID="lblCurYear" runat="server"></asp:Label></b>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="4">
                                <asp:GridView ID="gridWFH" PageSize="10" Width="50%" CssClass="manage_grid_a aligncenter" Visible="true"
                                     AutoGenerateColumns="false" runat="server" Style="margin-left: 250px !important;" 
                                    >
                                    <Columns>
                                        <asp:BoundField DataField="Type" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red" Visible="true" HeaderStyle-Width="20%"></asp:BoundField>
                                        <asp:BoundField HeaderText="Total <br />(Annual)" ItemStyle-Font-Bold="true" Visible="true" DataField="Total" HeaderStyle-Width="20%" HtmlEncode="False" />
                                        <%--<asp:BoundField HeaderText="Total <br />(Till Current Date)" ItemStyle-Font-Bold="true" Visible="true" DataField="Total_Accrual" HeaderStyle-Width="20%" HtmlEncode="False"/>--%>
                                        <asp:TemplateField HeaderText="Total <br />(Till Current Date)" ItemStyle-Font-Bold="true" HeaderStyle-Width="20%" >
                                            <ItemTemplate>
                                                   <asp:Label ID="lbleTotalcnt" Text='<%#Eval("Total_Accrual") %>' runat="server"> </asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Consumed" Visible="true" ItemStyle-Font-Bold="true" DataField="Consumed" HeaderStyle-Width="20%" />
                                        <asp:BoundField HeaderText="Balance" Visible="true" ItemStyle-Font-Bold="true" DataField="Balance" HeaderStyle-Width="20%" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <div style="float: left">
                                    <b>
                                        <asp:LinkButton ID="lbtnpre" runat="server" OnClick="lbtnpre_Click"><b><< </b></asp:LinkButton>
                                        <%-- <%= Microsoft.VisualBasic.DateAndTime.MonthName(Convert.ToInt32(dMonth)).ToString() + " " + Microsoft.VisualBasic.DateAndTime.Year(DateTime.Parse(ddate)) %>--%>
                                        <%="Apr-" + hdnYear.Value + " To " + "Mar-" +  Convert.ToInt32(Convert.ToInt32(hdnYear.Value)+1) %>
                                        <asp:LinkButton ID="lbtnnext" runat="server" OnClick="lbtnnext_Click"><b>>> </b></asp:LinkButton>
                                    </b>&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;
                                    <label>Work From Home Status</label>
                                    <asp:DropDownList ID="ddlStatus" AppendDataBoundItems="true" runat="server" Width="250">
                                        <asp:ListItem Value="0" Text="All" Selected="True" />
                                        <asp:ListItem Value="a" Text="Approved" />
                                        <asp:ListItem Value="r" Text="Rejected" />
                                        <asp:ListItem Value="p" Text="Pending" />
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSearch" CssClass="small_button white_button open" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                </div>
                                <div>
                                    <br />
                                    &nbsp;&nbsp;&nbsp;
                                </div>
                                <div>
                                    <asp:GridView ID="gridWFHDetails" OnRowDataBound="gridWFHDetails_RowDataBound" DataKeyNames="empWFHId,WFHStatus,WFHFrom,WFHTo,empId,WFHEntryDate" PageSize="10" Width="100%" CssClass="manage_grid_a headerleft" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" runat="server">
                                        <Columns>
                                             <asp:BoundField HeaderText="Apply" Visible="true" DataField="WFHFrom" DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                            <asp:BoundField HeaderText="From" Visible="true" DataField="WFHFrom" DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                            <asp:BoundField HeaderText="To" Visible="true" DataField="WFHTo" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField HeaderText="Reason" Visible="true" DataField="WFHDescription" />
                                            <asp:BoundField HeaderText="Status" Visible="true" DataField="WFHStatus" HeaderStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="Admin Comments" Visible="true" DataField="WFHComment" />
                                            <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Action">
                                                <ItemTemplate>
                                                    <%-- <span style='width: 31px; display: <%# Convert.ToInt32(Eval("lstatus"))== 0 ?"" : "none"  %>'>--%>
                                                    <asp:LinkButton ID="lnkDeleteTask" runat="server" Text="Delete" OnCommand="DeleteWFH" OnClientClick="return confirm('Are you sure, you want to delete?')" CommandArgument='<%# Eval("empWFHId") %>' CausesValidation="false">
                                                         <img src="images/delete.png" border="0"  alt="delete" />  
                                                    </asp:LinkButton></span> 
                                                    <asp:LinkButton ID="lnkEditTask" Visible="false" runat="server" OnCommand="lnkEditTask_Command" Text="Edit" CommandName="EditWFH" CommandArgument='<%# Eval("empWFHId")+","+Eval("WFHStatus")+","+Eval("WFHFrom")+","+Eval("WFHTo") %>' CausesValidation="false">
                                                         <img src="images/edit.png" border="0"  alt="edit" />  
                                                    </asp:LinkButton></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoDataFound" style="display: flex;justify-content: space-around;" runat="server" Text="No Data Found." Font-Bold="true"
                                                Font-Size="Medium" />
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </td>

                        </tr>
                    </table>

                </div>
                <br />
                <div>
                </div>
            </div>
        </div>
    </div>
    <div id="divAddPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 2; transform: scale(1); border: solid;" data-role="draggable">
        <%--height:428px;--%>
        <div style="margin-left: 9px;">
            <div class="popup_head">
                <h3>Employee WFH Details/Management</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <br />
            <div>
                &nbsp;&nbsp;<b style="color: red">Current WFH Balance:&nbsp;&nbsp;</b>
                <asp:Label ID="lblbalance" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;<table cellpadding="0" cellspacing="0" border="1" width="50%" class="manage_form" style="margin-left: 135px;">
                    <tr>
                        <td width="25%">Work From Home</td>
                        <td width="25%"><b><%= hdnBalance.Value %></b></td>
                    </tr>
                </table>
                <br />
                <hr />
            </div>

            <div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <center>
                    <h2>Add Work From Home Request</h2>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>WFH From</th>
                    <td align="center">
                        <input id="txtFromDate" name="txtFromDate" autocomplete="off" onkeyup="return false" style="border-radius: 0;" onkeypress="return false" /><%--onfocusout="showDays();"--%>
                        &nbsp;&nbsp;&nbsp;&nbsp; To &nbsp;&nbsp;&nbsp;&nbsp;
                      <input id="txtToDate" name="txtFromDate" onkeyup="return false" autocomplete="off"  style="border-radius: 0;" onkeypress="return false" /><%--onfocusout="showDays();"--%>
                        <span style="color: Red;">*</span><br />
                        <span id="lblDateError" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>No. Of WFH Applied</th>
                    <td align="center">
                        <asp:Label ID="lblWFHCount" runat="server" Text=""></asp:Label>

                    </td>
                </tr>
                <tr>
                    <th>Reason Of WFH</th>
                    <td align="center">
                        <input id="txtReason" name="txtReason" type="text" style="width: 300px;" class="k-textbox" />
                        <span style="color: Red;">*</span><br />
                        <span id="lblReason" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSubmit" runat="server"  Text="Submit" AutoPostback = "false" CssClass="small_button red_button open" OnClientClick="if (GetDataOnInsert() == true)  { javascript: return setPopUPData(); } else{ return false;}  " OnClick="lnkSubmit_Click" />
                        <%--javascript:return GetDataOnInsert();--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div id="divAddPopupOverlayForFillAttendance" runat="server"></div>
    <div runat="server" class="k-widget k-windowAdd" id="divAddPopupForFillAttendance" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 2; transform: scale(1); border: solid;" data-role="draggable">
        <%--height:428px;--%>
        <div style="margin-left: 9px;">
            <div class="popup_head">
                <h3>Employee WFH Attendance Management</h3>
                <%--   <img src="Images/delete_ic.png" class="close-button" runat="server" onclick="closeAttendancePopUp()"--%>
                <%-- alt="Close" />--%>
                <asp:ImageButton ID="Imgbtn" src="Images/delete_ic.png" runat="server" OnClick="Imgbtn_Click" class="close-button" />
                <div class="clear">
                </div>
            </div>
            <br />
            <div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <center>
                    <h2 runat="server" id="WFHAttendance">Fill Work From Home Attendance</h2>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <div  runat="server" id="divWFH">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Work From Home Date:</th>
                    <td align="center">
                        <asp:Label runat="server" ID="lblShowDT"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnStartDay" style="text-transform:capitalize;" OnClick="btnStartDay_Click"  OnClientClick="if (CheckInTime() == true) { javascript: return true; } else { return false; }" runat="server" Enabled="true" Text="Punch-In" CssClass="small_button red_button open" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                       <asp:Button ID="btnEndDay"  runat="server" style="text-transform: capitalize;" Text="Punch-Out" Enabled="true"  OnClick="btnEndDay_Click" CssClass="small_button red_button open" />
                        <%--<span style="color: Red;">*</span><br />
                        <span id="lblDateError" style="color: Red;"></span>--%>
                    </td>
                </tr>
            </table>
            </div>
        </div>
        <br />
        <br />
         <asp:GridView ID="gridwfhattendancedetails" DataKeyNames="attDate,attInTime,attOutTime,Day" runat="server"
            PageSize="10" Width="100%" CssClass="manage_grid_a headerleft" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false">
             <RowStyle HorizontalAlign="Center" />
            <Columns>
                <asp:BoundField  HeaderText="Punch-In" DataField="attInTime" ShowHeader="true" Visible="true" DataFormatString="{0:t}" HtmlEncode="false" HeaderStyle-Font-Bold="true"></asp:BoundField>
                <asp:BoundField HeaderText="Punch-Out" DataField="attOutTime" Visible="true" DataFormatString="{0:t}" HeaderStyle-Font-Bold="true"></asp:BoundField>
                <asp:BoundField HeaderText="Day" DataField="Day" Visible="true" HeaderStyle-Font-Bold="true"></asp:BoundField>
                <asp:BoundField HeaderText="Date" DataField="attDate" Visible="true" DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-Font-Bold="true"></asp:BoundField>
            </Columns>

            <EmptyDataTemplate>
                <asp:Label ID="lblNoDataFound" style="display: flex;justify-content: space-around;" runat="server" Text="No Data Found." Font-Bold="true"
                    Font-Size="Medium" />
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    <asp:HiddenField ID="hdnFrom" runat="server" />
    <asp:HiddenField ID="hdnTo" runat="server" />
    <asp:HiddenField ID="hdnReason" runat="server" />
    <asp:HiddenField ID="hdnCurrent" runat="server" />
    <asp:HiddenField ID="hdnLast" runat="server" />
    <asp:HiddenField ID="hdnEmpId" runat="server" />
    <asp:HiddenField ID="hdnYear" runat="server" Value="0" />
    <asp:HiddenField ID="hdnWFHCount" runat="server"/>
    <asp:HiddenField ID="hdnBalance" runat="server"/>
</asp:Content>
