<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="EmpLeave.aspx.cs" Inherits="Member_EmpLeave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script src="js/EmpLeave.js"></script>

    <style type="text/css">
        .style3 {
            height: 25px;
            width: 4%;
        }

        .style1 {
            height: 25px;
        }

        .style4 {
            width: 4%;
        }

        .popup_head {
            background: url("images/table-title.png") repeat scroll left -1px rgba(0, 0, 0, 0) !important;
            margin-bottom: 0;
            border: 1px solid #ccc;
            border-top: none;
            border-bottom: none;
            padding-left: 12px;
        }

        .aligncenter td {
            text-align: center !important;
        }

        .headerleft th {
            text-align: left !important;
            padding-left: 10px;
        }

        tbody tr:hover {
            background: none !important;
        }
    </style>
    <script type="text/javascript">
        function showDays() {

            var FromDate = $("#txtStartDate").val();
            var ToDate = $("#txtEndDate").val();
            $('[id$="hdnTo"]').val(ToDate);
            $('[id$="hdnFrom"]').val(FromDate);

            if (FromDate == "" && ToDate == "")
                return;

            $.ajax(
          {
              type: "POST",
              url: "EmpLeave.aspx/CalulateLeave",
              contentType: "application/json;charset=utf-8",
              data: "{'FromDate':'" + FromDate + "','ToDate':'" + ToDate + "' }",
              cache: false,
              async: false,
              dataType: "json",
              success: function (msg) {
                  var message = jQuery.parseJSON(msg.d);
                  $('[id$="lblLeaveCount"]').text(message);

              },
              error: function (msg) {

              }
          }
       );
        }
        function GetDataOnInsert() {

            var fromDate = $("#txtStartDate").val();
            var toDate = $("#txtEndDate").val();
            var reason = $("#txtReason").val();
            var dateSpan = $("#lblDateError");
            var reasonSpan = $("#lblReason");


            if (fromDate == "" || toDate == "") {
                dateSpan.html("Date field cannot be blank");
                return false;
            }
            if (reason == "") {
                reasonSpan.html("Please enter reason of leave");
                return false;
            }
            else {
                document.getElementById("<%=hdnReason.ClientID%>").value = reason;
                reasonSpan.html("");
                //return true;
            }

            if (fromDate != '' && toDate != '') {
                dateFirst = fromDate.split('/');
                dateSecond = toDate.split('/');
                var startDate = new Date(dateFirst[2], dateFirst[1] - 1, dateFirst[0]); //Year, Month, Date
                var endDate = new Date(dateSecond[2], dateSecond[1] - 1, dateSecond[0]);

                if (startDate > endDate) {
                    dateSpan.html("From date should be less than or equal to To date.");
                    $('[id$="lblLeaveCount"]').text('');
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
        function openApplyForLeave() {
            var url = document.getElementById('<%= hfMSTeamURL.ClientID %>').value;
            window.open(url, '_blank');
        }

    </script>
    <script type="text/javascript">
        function CheckDate() {

            var dateSpan = $("#lblDateError");
            var offSt = $("#txtStartDate").val();
            var offEn = $("#txtEndDate").val();

            if (offSt != '' && offEn != '') {

                dateFirst = offSt.split('/');
                dateSecond = offEn.split('/');
                var startDate = new Date(dateFirst[2], dateFirst[1] - 1, dateFirst[0]); //Year, Month, Date
                var endDate = new Date(dateSecond[2], dateSecond[1] - 1, dateSecond[0]);
                if (startDate > endDate) {
                    dateSpan.html("From date should be less than or equal to To date.");
                    $('[id$="lblLeaveCount"]').text('');
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
            var FromDate = $("#txtStartDate").val();
            var ToDate = $("#txtEndDate").val();
            var EmpId = $('[id$="hdnEmpId"]').val();
            var leaveexistsSpan = $("#lblleaveexists");
            var flag = true;

            $.ajax(
        {
            type: "POST",
            url: "EmpLeave.aspx/IfExistsLeave",
            contentType: "application/json;charset=utf-8",
            data: "{'EmpID':'" + EmpId + "','FromDate':'" + FromDate + "','ToDate':'" + ToDate + "' }",
            cache: false,
            async: false,
            dataType: "json",
            success: function (msg) {
                var message = jQuery.parseJSON(msg.d);
                if (message == true) {
                    alert('Leave for same date already exists!');
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
                    <asp:Label ID="lblLeaves" Text="Leaves" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td>
                                                <span id="spn" runat="server" onclick="openApplyForLeave();" class="small_button white_button">Apply For Leave</span>
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
                            <td width="25%"><b>
                                <asp:Label ForeColor="Red" ID="Label1" runat="server"> Joining Date</asp:Label></b> </td>
                            <td width="15%">
                                <asp:Label ID="lblJDate" runat="server"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                            </td>
                            <td width="15%">
                                <b>
                                    <asp:Label ForeColor="Red" ID="Label2" runat="server">Confirmation Date</asp:Label></b>
                            </td>
                            <td width="25%">
                                <asp:Label ID="lblCDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:GridView ID="gridLeave" PageSize="10" Width="50%" CssClass="manage_grid_a aligncenter" Visible="true"
                                     AutoGenerateColumns="false" runat="server" Style="margin-left: 250px !important;" 
                                    OnRowDataBound="gridLeave_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="Type" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red" Visible="true" HeaderStyle-Width="20%"></asp:BoundField>
                                        <asp:BoundField HeaderText="Total <br />(Annual)" ItemStyle-Font-Bold="true" Visible="true" DataField="Total" HeaderStyle-Width="20%" HtmlEncode="False" />
                                        <%--<asp:BoundField HeaderText="Total <br />(Till Current Date)" ItemStyle-Font-Bold="true" Visible="true" DataField="Total_Accrual" HeaderStyle-Width="20%" HtmlEncode="False"/>--%>
                                        <asp:TemplateField HeaderText="Total <br />(Till Current Date)" ItemStyle-Font-Bold="true" HeaderStyle-Width="20%">
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
                                <br />
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
                                    <label>Leave Type</label>
                                    <asp:DropDownList ID="ddlleaveType" AppendDataBoundItems="true" runat="server" Width="250">
                                        <asp:ListItem Value="0" Text="Select" Selected="True" />                                        
                                        <asp:ListItem Value="CL" Text="CL" />
                                        <asp:ListItem Value="PL" Text="PL" />
                                        <asp:ListItem Value="CO" Text="CO" />
                                        <asp:ListItem Value="SL" Text="SL" />
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;&nbsp;
                                    <label>Leave Status</label>
                                    <asp:DropDownList ID="ddlStatus" AppendDataBoundItems="true" runat="server" Width="250">
                                        <asp:ListItem Value="0" Text="Select" Selected="True" />
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
                                    <asp:GridView ID="gridLeaveDetails" DataKeyNames="ID" PageSize="10" Width="100%" CssClass="manage_grid_a headerleft" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" runat="server">
                                        <Columns>
                                            <asp:BoundField HeaderText="From" Visible="true" DataField="leaveFrom" DataFormatString="{0:dd-MMM-yyyy}"></asp:BoundField>
                                            <asp:BoundField HeaderText="To" Visible="true" DataField="leaveTo" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField HeaderText="Type" Visible="true" DataField="statusDesc" />
                                            <asp:BoundField HeaderText="Reason" Visible="true" DataField="leaveDesc" />
                                            <asp:BoundField HeaderText="Status" Visible="true" DataField="leaveStatus" HeaderStyle-Font-Bold="true" />
                                            <asp:BoundField HeaderText="Admin Comments" Visible="true" DataField="AdComments" />
                                            <asp:TemplateField HeaderStyle-Width="10px">
                                                <ItemTemplate>
                                                    <span style='width: 31px; display: <%# Convert.ToInt32(Eval("lstatus"))== 0 ?"" : "none"  %>'>
                                                        <asp:LinkButton ID="lnkDeleteTask" runat="server" Text="Delete" OnCommand="DeleteLeave" OnClientClick="return confirm('Are you sure, you want to delete?')" CommandArgument='<%# Eval("ID") %>' CausesValidation="false">
                                                         <img src="images/delete.png" border="0"  alt="delete" />  
                                                        </asp:LinkButton></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoDataFound" runat="server" Text="No Data Found." Font-Bold="true"
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
                <h3>Employee Leave Details/Management</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <br />
            <div>
                &nbsp;&nbsp;<b style="color: red">Current Leave Balance:&nbsp;&nbsp;</b>
                <asp:Label ID="lblbalance" runat="server" Text=""></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;<table cellpadding="0" cellspacing="0" border="1" width="50%" class="manage_form" style="margin-left: 135px;">
                    <tr>
                        <td width="25%">CL</td>
                        <td width="25%"><b><%= hdnCL_Bal.Value %></b></td>
                        <td width="25%">SL</td>
                        <td width="25%"><b><%= hdnSL_Bal.Value%></b></td>
                    </tr>
                    <tr>
                        <td>PL</td>
                        <td><b><%= hdnPL_Bal.Value %></b></td>
                        <td>CO</td>
                        <td><b><%= hdnCO_Bal.Value %></b></td>
                    </tr>
                </table>
                <br />
                <hr />
            </div>
            <div>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <center>
                    <h2>Add Leave</h2>
                </center>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>Leave Type</th>
                    <td align="center">
                        <asp:DropDownList ID="ddlAddLeaveType" AppendDataBoundItems="true" runat="server" Width="250">
                        </asp:DropDownList>
                        <span style="color: Red;">*</span>
                        <span id="lbl3" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Leave From</th>
                    <td align="center">
                        <input id="txtStartDate" name="txtFromDate" onkeyup="return false" style="border-radius: 0;" onkeypress="return false" /><%--onfocusout="showDays();"--%>
                        &nbsp;&nbsp;&nbsp;&nbsp; To &nbsp;&nbsp;&nbsp;&nbsp;
                      <input id="txtEndDate" name="txtFromDate" onkeyup="return false" style="border-radius: 0;" onkeypress="return false" /><%--onfocusout="showDays();"--%>
                        <span style="color: Red;">*</span><br />
                        <span id="lblDateError" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>No. Of Leaves Applied</th>
                    <td align="center">
                        <asp:Label ID="lblLeaveCount" runat="server" Text=""></asp:Label>

                    </td>
                </tr>
                <tr>
                    <th>Reason Of leave</th>
                    <td align="center">
                        <input id="txtReason" name="txtReason" type="text" style="width: 300px;" class="k-textbox" />
                        <span style="color: Red;">*</span><br />
                        <span id="lblReason" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSubmit" runat="server" Text="Submit" CssClass="small_button red_button open" OnClientClick="if (GetDataOnInsert() == true)  { javascript: return setPopUPData(); } else{ return false;}  " OnClick="lnkSubmit_Click" />
                        <%--javascript:return GetDataOnInsert();--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hfMSTeamURL" runat="server" />
    <asp:HiddenField ID="hdnFrom" runat="server" />
    <asp:HiddenField ID="hdnTo" runat="server" />
    <asp:HiddenField ID="hdnReason" runat="server" />
    <asp:HiddenField ID="hdnCurrent" runat="server" />
    <asp:HiddenField ID="hdnLast" runat="server" />
    <asp:HiddenField ID="hdnEmpId" runat="server" />
    <asp:HiddenField ID="hdnYear" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCL_Bal" runat="server" Value="0" />
    <asp:HiddenField ID="hdnSL_Bal" runat="server" Value="0" />
    <asp:HiddenField ID="hdnPL_Bal" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCO_Bal" runat="server" Value="0" />
</asp:Content>
