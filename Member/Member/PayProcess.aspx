<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="PayProcess.aspx.cs" Inherits="Member_PayProcess" Culture="hi-IN" UICulture="hi-IN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--Bellow links are for kendo controls (do not change sequence)--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>

    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script src="https://cdn.kendostatic.com/2013.2.716/js/cultures/kendo.culture.en-IN.min.js"></script>

    <script type="text/javascript" language="javascript">

        var GridReport = "#grdPayProcess";



    </script>

    <script type="text/javascript" src="js/PayProcess.js"></script>

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
        .popup_head a   { cursor:pointer;}
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

       .k-grid-header{
            margin-bottom: -3px;
        }
    </style>

    <%--    <script>

        var culture = kendo.culture();
        console.log(culture.name); // outputs "en-US"
        console.log(kendo.format("{0:c}", 100000)); // outputs "$99.00" using the default en-US culture
        kendo.culture("en-IN"); // change the culture
        console.log(culture.name);
        console.log(kendo.format("{0:c}", 100000)); // outputs "£99.00"
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head" style="padding: 10px 10px 15px 10px; height: 115px; background: none !important;">
                    <table width="100%" style="overflow: hidden;">

                        <tr>
                            <td style="padding: 5px 0 15px;">
                                <asp:Label ID="lblPayroll" Text="Pay Process" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                           <%-- <td align="right">Location:
                                    <asp:DropDownList ID="dlLocation" runat="server" AutoPostBack="true" CssClass="c_dropdown"
                                        OnSelectedIndexChanged="dlLocation_SelectedIndexChanged" Visible="true" AppendDataBoundItems="true" Width="200px">
                                    </asp:DropDownList>
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="popup_head" style="background:none !important; border:none;">
                                    <a runat="server" id="lnkPrev" onclick="fncallGetPayProcessData(this.id);"><b><<</b></a>
                                    <asp:Label ID="lblmonthyear" runat="server" Font-Names="Arial" Font-Bold="true"></asp:Label>
                                    <a runat="server" id="lnkNext" onclick="fncallGetPayProcessData(this.id);"><b>>></b></a>
                            </td>
                        </tr>
                        <tr class="pay-comment">

                            <td>
                                <asp:Label ID="lblPC" Text="Pay Comment:" runat="server" Style="font-weight: bold; font-size: 11pt; color: black; font-family: Verdana"></asp:Label>
                                &nbsp;&nbsp;
                           <%-- </td>
                            <td>--%>
                                <asp:Label ID="lblPayComment" runat="server" Font-Bold="true" Font-Size="small"></asp:Label>
                                <%--<textarea id="txtPayComment" runat="server" rows="2" cols="70" style="height:47px !important; min-height:47px; max-height:47px;"></textarea>--%>
                                <%-- <asp:TextBox ID="txtValue" runat="server"></asp:TextBox>--%>
                            </td>
                            <td align="right">
                                <%--  <asp:Button ID="btnAdd" runat="server" Text="ADD" OnClick="btnAdd_Click"/>--%>
                              <div id="divBtnStmt" style="display:none;"> <asp:Button ID="btnBankStatement" CssClass="small_button white_button open" runat="server" Text="Bank Statement" OnClick="btnBankStatement_Click"/></div> 
                                <%--<span id="spnSearch" runat="server" onclick="GetSearchReportDetails();" class="small_button white_button open">Add</span>--%>
                            </td>
                        </tr>
                    </table>
                </div>

               <%-- <div class="popup_head" style="padding-top: 68px;">
                    <a runat="server" id="lnkPrev" onclick="fncallGetPayProcessData(this.id);"><b><<</b></a>
                    <asp:Label ID="lblmonthyear" runat="server" Font-Names="Arial"></asp:Label>
                    <a runat="server" id="lnkNext" onclick="fncallGetPayProcessData(this.id);"><b>>></b></a>
                    <div class="clear">
                    </div>
                </div>--%>

            <div id="grdPayProcess"></div>
            <div id="divAddPopupOverlay" runat="server"></div>
            <div id="divOverlay"></div>

        </div>
    </div>
    </div>
    <asp:HiddenField ID="hdLocationId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnMonth" runat="server" />
    <asp:HiddenField ID="hdnPayID" runat="server" />
    <asp:HiddenField ID="hdnComment" runat="server" />
</asp:Content>

