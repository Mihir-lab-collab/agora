<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report.aspx.cs" Inherits="Member_Report" MasterPageFile="~/Member/Admin.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/ReportBuilder.js"></script>
    <script src="js/google-jsapi.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    </script>


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

        .c_dropdown {
            width: 155px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblInventoryDetails" Text="Reports" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblReportName" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <input type="button" id="BtnCancelReport" onclick="Cancel()" value="Back" class="small_button white_button open">
                    </div>
                </div>
                <asp:Panel ID="pnlReport" runat="server">
                    <div style="overflow-x: scroll; height: 100%; width: 100%">
                        <div id="gridReport"></div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlRun" runat="server">
                    <div id="divRun" runat="server" visible="false" class="grid_head" style="padding: 10px 10px 25px 10px; display: block; overflow: hidden; background: none !important; height: auto;">
                       <%-- <img id="myImage" runat="server" onclick="changeImage()" src="images/plus_icon.png" style="float: right;" alt="" />--%>
                        <div style="float: left; width: auto; overflow: hidden; margin-right: 2%;">
                            <div id="RunControlsDiv" runat="server" style="float: left; width: 100%; overflow: hidden; padding-bottom: 15px;"></div>
                            <table width="100%" style="overflow: hidden;">
                                <tr style="display: block;">
                                    <td>
                                        <asp:TextBox ID="txtrunquery" runat="server" Style="display: none;" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
                                        <asp:TextBox ID="txtReportName" runat="server" Style="display: none;"></asp:TextBox>
                                        <asp:Button ID="btnRun" runat="server" Text="Run" OnClick="btnRun_Click" class="small_button white_button open" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <div id="ChartDivRunReport" runat="server" style="float: left; overflow: hidden;">
                            <asp:Literal ID="ltchartRun" runat="server"></asp:Literal>
                            <div id="chart_div_run"></div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlGrid" runat="server">
                    <div id="grid"></div>
                </asp:Panel>
                <asp:Button ID="btnRunReport" runat="server" Text="Button" OnClick="btnRunReport_Click" Style="display: none;" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnFieldCount" runat="server" Value="0" />
     <asp:HiddenField ID="hdnChartType" runat="server" Value="0" />
    <asp:HiddenField ID="hdnReportData" runat="server" Value="0" />
    <asp:HiddenField ID="hdnReportId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsNewAdd" runat="server" Value="0" />
    <asp:HiddenField ID="hdnChartRun" runat="server" Value="0" />
</asp:Content>
