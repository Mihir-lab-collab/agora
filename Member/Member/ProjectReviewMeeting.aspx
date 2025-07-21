<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectReviewMeeting.aspx.cs" Inherits="ProjectReviewMeeting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/ProjectReviewMeeting.js"></script>
    <style type="text/css">
        div.k-windowAdd {
            left: 10% !important;
        }

        /*headers*/
        .k-grid th.k-header,
        .k-grid-header {
            background: #252e34;
        }

            .k-grid th.k-header,
            .k-grid th.k-header .k-link {
                color: white;
            }
        /*rows*/
        .k-grid-content > table > tbody > tr {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }

        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*selection*/
        .k-grid table tr.k-state-selected {
            background: #f99;
            color: #fff;
        }
        /*for history grid [s]*/
        .k-alt {
            background: #cbc8c8;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/
        .stylelink {
            font-size: 17px;
            color: black;
        }

            .stylelink:hover {
                font-size: 14px;
                color: #F60;
            }

        .lnknexticon {
            margin-left: 60px;
        }

        .lnknextlable {
            margin-left: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <span style="font-size: Medium; font-weight: bold;">Team Meetings</span><br />
                                <span id="Span1" runat="server" style="float: right;" onclick="AddNewMeeting();" class="small_button white_button open">Add New Meeting</span>&nbsp;&nbsp;&nbsp;

                            </td>
                        </tr>
                    </table>
                    <div id="msgBox"></div>
                    <br />
                </div>
                <div id="Get_ProjectReviewMeetingGrid"></div>
            </div>
        </div>

    </div>
    <input type="hidden" id="hdnAdmin" runat="server" value="0" />
    <input type="hidden" id="hd_MeetingId" value="0" />
</asp:Content>


