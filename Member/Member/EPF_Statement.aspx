<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="EPF_Statement.aspx.cs" Inherits="Member_EPF_Statement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css">

    <%--<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>--%>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>

    <script type="text/javascript" src="../js/console.js"></script>

    <script src="../Member/js/EPFStatement.js" type="text/javascript"></script>

    <script type="text/javascript">
        function CheckEmptyDate() {

            if ($("#txtStartDate").val() == '') {
                alert("Please Select Month & Year");
                return false;
            }
            return true;
        }

    </script>


    <style type="text/css">
        .modalBackground {
            background: #827F7F;
            opacity: 0.8;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 20%;
        }

        .modalPopup {
            position: fixed;
            top: 46%;
            left: 45%;
            background-color: #ffffff;
            border-color: black;
            border-style: solid;
            border-width: 2px;
            height: 30px;
            padding-left: 10px;
            padding-top: 10px;
            width: 300px;
            z-index: 100001;
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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/
    </style>
    <style type="text/css">
        .red {
            color: red;
            text-align: center;
            font-size: medium;
        }

        .black {
            color: black;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">

                    <div style="float: right">
                        <asp:Button ID="btnExportToExcel" Text="ExportToExcel" OnClientClick="javascript:return CheckEmptyDate()" OnClick="btnEpf_Click" runat="server"></asp:Button>

                        <asp:Label ID="lblInventoryDetails" Text="Search  " runat="server" Font-Bold="true" Font-Size="small"></asp:Label>
                        <input id="txtStartDate" name="txtFromDate" onkeyup="return false" style="border-radius: 0;" onkeypress="return false"/>
                    </div>
                    <div class="clear"></div>

                </div>
                <div id="gridProject"></div>
                <div align="center" class="modalPopup" id="dvall">

                    <b>Please Select Month & Year for EPF Statement.</b>

                </div>
                <div class="modalBackground" id="dvbg" style="display: none"></div>
            </div>
        </div>

    </div>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
</asp:Content>

