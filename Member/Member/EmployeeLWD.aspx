<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmployeeLWD.aspx.cs" Inherits="Member_EmployeeLWD" MasterPageFile="~/Member/Admin.master" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css"/>
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css" />

    
    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js" type="text/javascript"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/EmployeeLWD.js" type="text/javascript"></script>
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

        .DivShowEditor,.DivHideEditor  {
            float: right; margin-right: 20px; cursor: pointer;
        }
        /*for history grid [e]*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                   
                    <div class="clear"></div>

                </div>
                <div id="gridEmployeeLWD"></div>
            </div>
        </div>
    </div>
</asp:Content>