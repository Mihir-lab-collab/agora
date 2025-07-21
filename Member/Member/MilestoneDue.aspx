<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="MilestoneDue.aspx.cs" Inherits="MilestoneDue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="js/MilstoneDue.js" type="text/javascript"></script>
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
    <div id="temp"></div>
    
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lbl" Text="Milestone Dues" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                     <%--<asp:Label ID="lblLocation" Text="" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>--%>
                    <div style="float: right">
                       <%-- <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>                                           
                                            <td>                                                
                                               Select Date:  <input type="text" id="txtFromDate" onkeypress="return false;" style="width: 140px" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td><span id="spn" runat="server" onclick="Search();" class="small_button white_button open">Search</span></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>--%>
                    </div>
                </div>
                <div id="gridMiles"></div>
            </div>
        </div>
    </div>

    <%--<div id="divAddPopupOverlay" runat="server"></div>--%>

    <asp:HiddenField ID="hdnKEID" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnLocationID" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnEventDate" runat="server" />
    <asp:HiddenField ID="hdnDescription" runat="server" />
    <asp:HiddenField ID="hdnTime" runat="server" />
</asp:Content>




