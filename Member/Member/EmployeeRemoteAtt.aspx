<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="EmployeeRemoteAtt.aspx.cs" Inherits="Member_EmployeeRemoteAtt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../includes/CalendarControl.css" type="text/css" />
    <script language="JavaScript" src="../includes/CalendarControl.js" type="text/javascript"></script>

    <asp:ScriptManager ID="src" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <table id="tblWFHAttendance" runat="server" width="100%" class="manage_form">
                </table>
                <br />
                <div align="center">
                    <asp:Label ID="lblWFH" Text="Remote Attendance" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </div>
                <br />
                <div align="center">                  
                    <asp:Label ID="WFHAttendance" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                    <asp:Button ID="btnStartDay" style="text-transform:capitalize;" OnClick="btnStartDay_Click" OnClientClick="if (CheckInTime() == true) { javascript: return true; } else { return false; }" runat="server" Enabled="true" Text="Punch-In" CssClass="small_button red_button open" />
                    <asp:Button ID="btnEndDay" runat="server" style="text-transform: capitalize;" Text="Punch-Out" Enabled="true" OnClick="btnEndDay_Click" CssClass="small_button red_button open" />
                </div>
                <br /><br />
                <!-- Label to display the selected month -->
                <div align="center">
                    <asp:Label ID="lblSelectedMonth" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                </div>
                <br />
                <div align="center">
                <tr>
                        <td align="center">
                            <font face="Verdana" size="2"><b>
                                <asp:LinkButton ID="LinkButton1" Text="<<" CommandArgument="prev" runat="server" OnClick="PagerButtonClick" CausesValidation="False" />
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                <asp:LinkButton ID="LinkButton2" Text=">>" CommandArgument="next" runat="server" OnClick="PagerButtonClick" CausesValidation="False" />
                            </b></font>
                        </td>
                    </tr>
              </div>


                <div>
                    <asp:GridView ID="gridwfhattendancedetails" DataKeyNames="attDate,attInTime,attOutTime,Day" runat="server"
                        PageSize="10" Width="100%" CssClass="manage_grid_a headerleft" Visible="true" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                        Style="font-family: Verdana; font-size: small;">
                        <RowStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField HeaderText="Punch-In" DataField="attInTime" ShowHeader="true" Visible="true" DataFormatString="{0:t}" HtmlEncode="false" HeaderStyle-Font-Bold="true"></asp:BoundField>
                            <asp:BoundField HeaderText="Punch-Out" DataField="attOutTime" Visible="true" DataFormatString="{0:t}" HeaderStyle-Font-Bold="true"></asp:BoundField>
                            <asp:BoundField HeaderText="Day" DataField="Day" Visible="true" HeaderStyle-Font-Bold="true"></asp:BoundField>
                            <asp:BoundField HeaderText="Date" DataField="attDate" Visible="true" DataFormatString="{0:dd-MMM-yyyy}" HeaderStyle-Font-Bold="true"></asp:BoundField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="lblNoDataFound" style="display: flex;justify-content: space-around;" runat="server" Text="No Data Found." Font-Bold="true" Font-Size="Medium" />
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnEmpId" runat="server" />
    <asp:HiddenField ID="hdntaskId" runat="server" />

      <script type="text/javascript">
          $(document).ready(function () {
              hidePunchButtonsOnWeekends();
          });

          function hidePunchButtonsOnWeekends() {
              var today = new Date();
              var day = today.getDay();
              if (day === 0 || day === 6) { // 0 is Sunday, 6 is Saturday
                  $("#<%= btnStartDay.ClientID %>").hide();
                  $("#<%= btnEndDay.ClientID %>").hide();
              }
          }
      </script>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            hidePunchButtonsOnWeekends();
            disablePunchInButtonIfAlreadyClicked();
        });

        function disablePunchInButtonIfAlreadyClicked() {
            if (localStorage.getItem('hasPunchedIn') === 'true') {
                $("#<%= btnStartDay.ClientID %>").prop('disabled', true);
            $("#<%= btnEndDay.ClientID %>").prop('disabled', false);
            }
        }

        $("#<%= btnStartDay.ClientID %>").click(function (event) {
            if (localStorage.getItem('hasPunchedIn') === 'true') {               
                alert("Punch-In can only be done once a day.");
                event.preventDefault();
                return false;
            } else {              
                localStorage.setItem('hasPunchedIn', 'true');
                $(this).prop('disabled', true);
                $("#<%= btnEndDay.ClientID %>").prop('disabled', false);
               }
           });

        function hidePunchButtonsOnWeekends() {
            var today = new Date();
            var day = today.getDay();
            if (day === 0 || day === 6) { // 0 is Sunday, 6 is Saturday
                $("#<%= btnStartDay.ClientID %>").hide();
        $("#<%= btnEndDay.ClientID %>").hide();
    }
</script>--%>
</asp:Content>
