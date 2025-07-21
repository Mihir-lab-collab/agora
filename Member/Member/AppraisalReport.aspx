<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="AppraisalReport.aspx.cs" Inherits="Member_AppraisalReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/AppraisalReport.js"></script>

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

    <script type="text/javascript" language="javascript">

        function PreLinkBtnClick() {
            PreLinkClick();
            return false;
        }

        function NextLinkBtnClick() {
            NextLinkClick();
            return false;
        }

    </script>

    <%--<script id="alt-template" type="text/x-kendo-template">
        <tr data-uid="#= uid #">
            <td colspan="6">
                <strong>#: KRANames #</strong>
            </td>
        </tr>
    </script>--%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">

                    <table width="100%">
                        <tr>
                            <td>
                                <span style="font-size: Medium; font-weight: bold;">Permormance Review Report</span><br />
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <strong style="font-size: 14px;">Quarter: </strong>
                                <label for="QuarterWiseRptDate" style="vertical-align: middle; font-size: 13px;"></label>
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td runat="server">
                                <asp:LinkButton ID="LnkbPrev" CssClass="ClcLnkbPrev" runat="Server" OnClientClick="JavaScript: return PreLinkBtnClick();" Text="Previous">
                                   <img class="LNKPRE_CLICK" src="images/button-previous.png" />
                                </asp:LinkButton>

                                <asp:LinkButton ID="LnkbNext" CssClass="ClsLnkbNext lnknexticon" runat="Server" OnClientClick="JavaScript: return NextLinkBtnClick();" Text="Next"> 
                                    <img id="NEXT_ICON" class="LNKNEXT_CLICK" src="images/button-next.png" />
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Previous</label>
                                <label class="lnknextlable">Next</label>
                            </td>
                        </tr>
                    </table>

                </div>

                <%--Get Appraisal Report Grid--%>

               
                <div id="GetAppRptGrid"></div>

            </div>
        </div>
    </div>

    <div id="divOverlay"></div>
    <div id="divAddPopupOverlay"></div>
    <div class="a_popbox" id="divAddPopup" style="display: none;">
        <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: 550px; width: 900px; margin-left: -450px;">
            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP()" />
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server">
                            <div id="example2" class="k-content">
                                <div id="tickets2">
                                    <table>
                                        <tr>
                                            <td>
                                                <strong style="font-size: 14px;">Quarter: </strong>
                                                <label for="QuarterDate" style="font-size: 14px;"></label>
                                                &nbsp;&nbsp;<strong style="font-size: 14px;">Employee Name: </strong>
                                                <label for="EmpName" style="font-size: 13px;"></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <br />
                                                <div class="ParentsClass" id="Parent">

                                                </div>
                                                <%--<div id="GetAllAppMngrRpt"></div>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>

             
            <div id="GetAuthApprRptGrid"></div>

        </div>

    </div>


    <asp:HiddenField ID="hndRptQuarterDate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnRptQuarterCounter" runat="server" ClientIDMode="Static" />

</asp:Content>
