<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectsReview.aspx.cs" Inherits="Member_ProjectsReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://angular-ui.github.com/ng-grid/css/ng-grid.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-grid/2.0.11/ng-grid.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.6.0/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.4.4/underscore-min.js"></script>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/ProjectsReview.js?18" type="text/javascript"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="scriptmanagerHome" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanelHome" runat="server">
        <ContentTemplate>
            <div class="content_wrap">
                <div class="gride_table">
                    <div class="grid_head">
                        <h2>Team Meeting</h2>
                        <button id="backButton" onclick="clk_backButton();" type="button" style="float: right;" class="small_button white_button open">Back</button>
                    </div>
                    <div id="InviteMeeting">
                        <table class="manage_form" width="100%">
                            <tr>
                                <th class="required">Meeting Date</th>
                                <td class="text">
                                    <input id="txtMeetingDate" type="text" name="txtMeetingDate" style="width: 300px;" onkeyup="return false" onkeypress="return false" class="k-textbox" />
                                </td>

                                <th>Agenda Topic</th>
                                <td style="width: 300px;" valign="top">
                                    <input id="txtAgendaTopic" type="text" name="txtAgendaTopic" style="width: 300px;" class="k-textbox" />
                                </td>
                                <th class="required">Type of Meeting</th>
                                <td style="width: 300px;" valign="top">
                                    <input id="txtMeetingType" type="text" name="txtProjectTile" style="width: 300px;" class="k-textbox" />
                                </td>
                            </tr>
                            <tr>
                                <th>Attendees</th>
                                <td style="width: 300px;" valign="top">
                                    <input id="txtAttendees" name="txtAttendees" />
                                </td>
                                <th class="required">Meeting called By </th>
                                <td>
                                    <input id="txtMeetingCalledBy" type="text" name="txtMeetingCalledBy" style="width: 300px" class="k-textbox" />
                                </td>

                                <th>Facilitator</th>

                                <td>
                                    <input id="txtFacilitator" type="text" name="txtFacilitator" style="width: 300px" class="k-textbox" />
                                </td>
                            </tr>
                            <tr>
                                <th>Time Alloted</th>
                                <td style="width: 300px;" valign="top">
                                    <input id="txtTimeAlloted" type="text" name="txtTimeAlloted" style="width: 300px" class="k-textbox" />
                                </td>
                                <th colspan="2" style="text-align: left;">
                                    <button id="btnAddProjectsReview" onclick="Save_MeetingDetails();" type="button" class="small_button white_button open">Invite Meeting</button>
                                    <button id="btnCancelProjectsReview" onclick="CancelMeeting();" type="button" class="small_button white_button open">Cancel Meeting</button>
                                </th>
                                <th colspan="2" style="text-align: right;">
                                    <button id="btn_SendMOM" onclick="SendMOM();" type="button" class="small_button white_button open">Send MOM</button>
                                </th>
                            </tr>
                        </table>
                    </div>
                    <div id="ConductMeeting" style="display: none;">
                        <div id="grdProjectDetails" style="overflow: auto;">
                            <div class="a_popbox" id="details" style="display: none;"></div>
                        </div>
                        <div id='grdTSBreakup' style="overflow: auto;"></div>
                        <div id="divAddPopupOverlay" runat="server"></div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="hdnAdmin" runat="server" value="0" />
    <asp:HiddenField ID="hdnProjectId" runat="server" />
    <input type="hidden" id="hd_MeetingId" name="MeetingId" value="0" />
</asp:Content>

