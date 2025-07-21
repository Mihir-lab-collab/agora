<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="PayAdd.aspx.cs" Inherits="Member_PayAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        var GridReport = "#grdPayAdd";

        function checkMonth() {

            var success = false;
            var month = $("[id$=dlMonth] option:selected").val();

            var monthSpan = $("#lblmsgMonth");
            if (month == 0) {
                monthSpan.html("Please select Month.");
                success = false;
                //return false;
            }
            else {
                monthSpan.html("");
                success = true;
                //return true;
            }
            return success;
        }
        function checkYear() {

            var Year = $("[id$=txtYear]").val()

            var yrSpan = $("#lblmsgYear");
            if (Year == "") {
                yrSpan.html("Please enter Year.");
                return false;
            }
            else {
                yrSpan.html("");
                return true;
            }
        }

        function checkValidation() {

            if (checkMonth() && checkYear())
                return true;
            else
                return false;
        }


    </script>

    <script type="text/javascript" src="js/PayAdd.js"></script>

    <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
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
        /*for history grid [e]*/

        /*for displaying text right aligned*/
        .k-grid .ra,
        .k-numerictextbox .k-input {
            text-align: right;
        }
        /*end*/

        /*for wrapping column header text*/
        .k-grid .k-grid-header .k-header .k-link {
            height: auto;
        }

        .k-grid .k-grid-header .k-header {
            white-space: normal;
        }
        /*end*/
        .grid_head table tr td span.active-set {
            margin-top: 10px;
        }

        /*#grdPayProcess {
            cursor: pointer;
        }*/
        .popup_head {
            background: url("images/table-title.png") repeat scroll left -1px rgba(0, 0, 0, 0) !important;
            margin-bottom: 0;
            border: 1px solid #ccc;
            border-top: none;
            border-bottom: none;
            padding-left: 12px;
        }

        .k-grid-content-locked {
            float: left;
            clear: both;
        }

        .k-grid-header-locked {
            float: left;
            clear: both;
        }

        /*.pay-comment {
            display: block;
            background: url("images/table-title.png") repeat scroll left -1px rgba(0, 0, 0, 0) !important;
            border: 1px solid #ccc;
            width: 100%;
        }*/
        div.k-grid-header .k-header {
            text-align: center;
            border-bottom: #c5c5c5 1px solid;
        }

        .k-grid-content {
            background: #fff;
            margin-top: -5px;
        }

        .changecolor {
            background: none !important;
            border: none;
        }

        .manage_form1 th, .manage_form1 td {
            border-top: 1px solid #e7e7e7;
            background: #f5f5f5;
        }

        .manage_form1 td {
            padding: 6px 20px !important;
        }

        .addpay {
            padding: 10px 10px 15px 10px;
            border-left: 1px solid #ccc;
            border-right: 1px solid #ccc;
            background: #eee;
        }

        .center {
            text-align: center;
        }

        input.chkbx[type="checkbox"], .k-grid-content table tr td input.chkbxq[type="checkbox"] {
            float: none;
            text-align: center;
            margin: 0 auto !important;
            padding: 0;
        }

        .addbgcolor {
            border: 1px red solid;
            background: pink;
        }
        .pink       { background-color:pink}//#f99;
        .blue       { background-color:#C6E2FF;}
        .grey       { background-color:#E9E9E9; }
        .green      { background-color:lightgreen;}

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                         
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblProjects" Text="Add Pay" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divinput" class="grid_head" style="padding: 10px 10px 85px 10px; display: block; overflow: hidden; background: none !important;">
                    <table width="100%" style="overflow: hidden;">
                        <tr>
                            <td>Month:
                                    <asp:DropDownList ID="dlMonth" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                                        Visible="true" AppendDataBoundItems="true" Width="200px" onchange="if (checkMonth()) getNoofDays(); else return false;">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                <span id="lblmsgMonth" style="color: Red;"></span>
                            </td>
                            <td>Year:
                                <asp:TextBox ID="txtYear" runat="server" MaxLength="4" onkeypress="return isNumber(event)" onblur="return checkYear();"></asp:TextBox>
                                <span id="lblmsgYear" style="color: Red;"></span>
                            </td>
<%--                            <td align="right">Location:
                                    <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                                        Visible="true" AppendDataBoundItems="true" Width="200px" OnSelectedIndexChanged="dlLocation_SelectedIndexChanged">
                                    </asp:DropDownList>
                            </td>--%>
                        </tr>
                        <tr style="padding-top: 15px; display: block;">
                            <td>
                                <span id="spnSubmit" runat="server" onclick="if (checkValidation()) CheckPayProcess(); else return false;" class="small_button white_button open">Submit</span>
                            </td>
                        </tr>
                    </table>
                </div>


                <div id="details" class="addpay" style="display: none;">
                    <div class="grid_head changecolor" style="padding: 10px 0px 25px 0px; height: auto; border-bottom: #888 1px solid!important; margin-bottom: 25px;">
                        <%-- Section 1- Pay details--%>
                        <table cellpadding="0" cellspacing="0" border="1" width="100%" class="manage_form1">
                            <tr class="manage_bg">
                            </tr>
                            <tr>
                                <th>Month
                                </th>
                                <td width="88%">
                                    <label id="lblMonth" class="Detailslbl" />
                                </td>
                            </tr>
                            <tr>
                                <th>Location
                                </th>
                                <td>
                                    <label id="lblLocation" class="Detailslbl" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <table style="overflow: hidden; padding-bottom: 10px;">
                        <tr>

                            <td>
                                <asp:Label ID="lblPC" Text="Pay Comment:" runat="server" Font-Bold="true" Font-Size="small"></asp:Label><br />
                                <br />
                                <%--  </td>
                                <td>--%>
                                <textarea id="txtPayComment" runat="server" rows="2" cols="70" style="height: 47px !important; min-height: 47px; max-height: 47px; font-size: 11pt; color: black; font-family: Verdana" maxlength="255"></textarea>
                            </td>
                            <td style="margin: 64px 0 0 15px; display: block;">
                                <span id="spnContinue" runat="server" class="small_button white_button open" onclick="GetCalPayDetails();">Continue</span>&nbsp;&nbsp;
                                    <span id="spnCancel" runat="server" class="small_button white_button open" onclick="CancelPayAdd();">Cancel</span>
                            </td>

                        </tr>
                    </table>
                    <div id="grdPayAdd" class="border"></div>

                </div>

                <div id="divCalculation" class="addpay" style="display: none;">
                    <table style="overflow: hidden;">
                        <tr>
                            <td>
                                <asp:Label ID="lblFinPC" Text="Pay Comment:" runat="server" Font-Bold="true" Font-Size="small"></asp:Label><br />
                                <br />
                                <asp:Label runat="server" ID="lblPayComment" Style="font-size: 11pt; color: black; font-family: Verdana"></asp:Label>
                            </td>
                        </tr>
                        <tr style="margin: 10px 0 20px; display: block;">
                            <td>
                                <span id="spnSubmitCal" runat="server" class="small_button white_button open" onclick="SaveData();">Submit</span>&nbsp;&nbsp;
                                    <span id="spnCancelCal" runat="server" class="small_button white_button open" onclick="CancelFinal();">Cancel</span>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <div id="grdPayProcessCal"></div>
                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnNoOfDays" runat="server" Value="0" />
    <asp:HiddenField ID="hdnJSONData" runat="server" />
    <asp:HiddenField ID="hdnCurrMonth" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCurrYear" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCurrLoc" runat="server" Value="0" />
</asp:Content>

