<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimesheetIncomplete.aspx.cs" Inherits="Member_TimesheetIncomplete" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/TimesheetIncomplete.js"></script>

    <style type="text/css">
        .Detailslbl {
            width: 300px;
        }
    </style>
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


                    <asp:Label ID="lblInventoryDetails" Text="Incompelete Timesheet" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>

                </div>
                <div class="grid_head">
                    <table width="100%">
                        <tr>

                            <td style="float: left;">
                                <div style="float: right;">
                            <font face="Verdana" size="2"><b>
                                <asp:LinkButton ID="prevMonth" Text="<<" CommandArgument="prev"
                                    runat="server" OnClick="PagerButtonClick" CausesValidation="False" />
                                <asp:Label ID="lblMonth" runat="server"></asp:Label>
                                <asp:LinkButton ID="nextMonth" Text=">>" CommandArgument="next" runat="server" OnClick="PagerButtonClick"
                                    CausesValidation="False" />
                            </b></font>
                                    </td>

                                    <td style="float: right;">
                                    <span>Emp ID</span>
                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="b_dropdown" Width="160">
                                    </asp:TextBox>
                                    <span id="spnSearch" runat="server" onclick="GetSearchReportDetails();" class="small_button white_button ">Apply Filter</span>
                                </div>
                            </td>

                        </tr>
                    </table>
                </div>
                <div id="grdReport"></div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
</asp:Content>
