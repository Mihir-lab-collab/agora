<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaveBalanceReport.aspx.cs" Inherits="Member_LeaveBalanceReport" MasterPageFile="~/Member/Admin.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css" />

    <%--<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>--%>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>
    <script src="../Member/js/LeaveBalance.js" type="text/javascript"></script>

    <style type="text/css">
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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblInventoryDetails" Text="Employee Leave Balance " runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <asp:Label ID="spndatetime" runat="server"></asp:Label>
                    </div>
                </div>
                <div id="gridLeaveBalance"></div>
            </div>
        </div>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Employee Leave Details</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table id="tdLDetails" runat="server" width="100%" class="manage_grid_a" style="border-top: none; border-bottom: none;">
                <tr>
                    <td width="25%"><b>
                        <asp:Label ID="Label1" runat="server"> Employee Id</asp:Label></b> </td>
                    <td width="15%">
                        <asp:Label ID="lblEmpId" runat="server"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                    </td>
                    <td width="15%">
                        <b>
                            <asp:Label ID="Label2" runat="server">Employee Name</asp:Label></b>
                    </td>
                    <td width="25%">
                        <asp:Label ID="lblEmpName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="leaveGridContainer">
                        </div>
                    </td>

                </tr>               
            </table>
            <div>&nbsp;</div>
        </div>
    </div>
</asp:Content>
