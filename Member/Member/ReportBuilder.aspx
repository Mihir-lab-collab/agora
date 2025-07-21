<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportBuilder.aspx.cs" Inherits="Member_ReportBuilder" MasterPageFile="~/Member/Admin.master" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="js/ReportBuilder.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

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
                    <asp:Label ID="lblInventoryDetails" Text="Reports Builder" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblReportNameBui" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <input type="button" id="BtnAddReport" onclick="AddNewReport()" value="ADD" class="small_button white_button open">
                        <input type="button" id="BtnCancelReport" onclick="CancelReport()" value="Back" class="small_button white_button open">
                    </div>
                </div>

                <%-- Outer Kendo Grid--%>
                <asp:Panel ID="pnlReport" runat="server">
                    <div style="overflow-x: scroll; height: 100%; width: 100%">
                        <div id="gridReport"></div>
                    </div>
                </asp:Panel>

                <%--Report Details --%>
                <asp:Panel ID="pnlInput" runat="server">
                    <div id="divinput" class="grid_head" style="padding: 10px 10px 10px 10px; display: block; overflow: hidden; background: none !important; height: auto">
                        <table width="63%" style="overflow: hidden;">
                            <tr>
                                <td>Name:</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="reqChartName" runat="server" ErrorMessage="Chart name required" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Description:</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Input Field:</td>
                                <td>
                                    <select id="ddlInputFiled" name="ddlInputFiled" onchange="ddlonChange();">
                                        <option value="0">Select</option>
                                        <option value="1">1</option>
                                        <option value="2">2</option>
                                        <option value="3">3</option>
                                        <option value="4">4</option>
                                        <option value="5">5</option>
                                    </select>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

                <%-- Main Query and buttons--%>

                <asp:Panel ID="pnlQuery" runat="server">
                    <div id="divField" runat="server" class="grid_head" style="padding: 10px 10px 25px 10px; display: block; overflow: hidden; background: none !important; height: auto;">
                        <div class="head" style="float: left; width: auto; margin-right: 5%; overflow: hidden;">

                            <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>

                            <div id="ControlsDiv" runat="server"></div>
                            <table width="10%">
                                <tr>
                                    <td>Query:</td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox></td>
                                </tr>
                                <tr>

                                    <td>
                                        <asp:Label ID="lblChartType" runat="server" Text="Chart Type:"></asp:Label>
                                        <asp:DropDownList ID="dlstChartType" runat="server" Width="130" CssClass="c_dropdown">
                                            <%--OnSelectedIndexChanged="dlstChartType_SelectedIndexChanged">--%>
                                            <asp:ListItem Text="--Select Chart--" Value=""></asp:ListItem>
                                            <asp:ListItem Text="Single line chart" Value="Single_line_chart"></asp:ListItem>
                                            <asp:ListItem Text="2/3 line chart" Value="twobythree_line_chart"></asp:ListItem>
                                            <asp:ListItem Text="Pie Chart" Value="Pie_Chart"></asp:ListItem>
                                            <asp:ListItem Text="Bar Chart" Value="Bar_Chart"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                </tr>
                                <tr>
                                    <td style="width: 50%;">
                                        <asp:Label ID="lblMgmtReport" runat="server" Text="Management Report:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdbMgmtReport" RepeatDirection="Horizontal" runat="server" Style="float: left;">
                                            <asp:ListItem Value="yes" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="no" Text="No" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>

                                    </td>

                                </tr>
                                <tr style="padding-top: 15px; display: block;">
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Save" class="small_button white_button open" />
                                    </td>
                                </tr>
                            </table>

                            <%--  </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </div>

                    </div>
                </asp:Panel>

                <%-- Run Panel--%>
                <asp:Panel ID="pnlRun" runat="server">
                    <div id="divRun" runat="server" visible="true" class="grid_head" style="padding: 10px 10px 25px 10px; display: block; overflow: hidden; background: none !important; height: auto;">
                      <%-- <img id="myImage" runat="server" onclick="changeImage()" src="images/plus_icon.png" style="float: right;" alt="" />--%>
                        <div style="float: left; width: auto; overflow: hidden; margin-right: 2%;">
                            <div id="RunControlsDiv" runat="server" style="float: left; width: 100%; overflow: hidden; padding-bottom: 15px;"></div>
                            <table width="100%" style="overflow: hidden;">
                                <tr style="display: block;">
                                    <td>
                                        <asp:TextBox ID="txtrunquery" runat="server" Style="display: none;" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
                                        <asp:TextBox ID="txtReportName" runat="server" Style="display: none;"></asp:TextBox>
                                        <asp:Button ID="btnRun" runat="server" Text="Update" OnClick="btnRun_Click" class="small_button white_button open" />
                                    </td>

                                </tr>
                            </table>
                        </div>
                        <div id="ChartDivRun" runat="server" style="float: left; overflow: hidden;">
                            <asp:Literal ID="ltchartRun" runat="server"></asp:Literal>
                            <div id="chart_div_run"></div>
                        </div>
                    </div>
                </asp:Panel>

                <%-- Show Grid --%>

                <asp:Panel ID="pnlGrid" runat="server">
                    <div id="grid"></div>
                </asp:Panel>

                <asp:Button ID="btnReportDetails" runat="server" Text="Button" OnClick="btnReportDetails_Click" Style="display: none;" />
                <asp:Button ID="btnRunReport" runat="server" Text="Button" OnClick="btnRunReport_Click" Style="display: none;" />
                <asp:Button ID="btnDeleteReport" runat="server" Text="Button" OnClick="btnDeleteReport_Click" Style="display: none;" />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnFieldCount" runat="server" Value="0" />
    <asp:HiddenField ID="hdnReportData" runat="server" Value="0" />
    <asp:HiddenField ID="hdnReportId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsNewAdd" runat="server" Value="0" />
    <asp:HiddenField ID="hdnChart" runat="server" Value="0" />
    <asp:HiddenField ID="hdnChartRun" runat="server" Value="0" />
    <asp:HiddenField ID="hdnChartType" runat="server" Value="0" />

</asp:Content>
