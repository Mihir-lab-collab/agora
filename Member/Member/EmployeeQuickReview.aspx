<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="EmployeeQuickReview.aspx.cs" Inherits="EmployeeQuickReview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/EmployeeQuickReview.js"></script>
     <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
     <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>
    <script type="text/javascript">
        function CheckOnUpdate(Buttonid) {
            var result = true;
            if ($("#txt_EditReviewText").val().trim() == "" || $("#txt_EditReviewText").val() == undefined) {
                $("#lbl_errReview").html("Review Cannot be empty.");
                result = false;
            } else {
                $('[id$="hf_Review"]').val($("#txt_EditReviewText").val());
                $("#txt_EditReviewText").val("");
                $('[id$="hf_EmpId"]').val($("#lblEmpCode").text());
                result = true;
            }
            return result;
        }
        function close_ReviewTextArea() {
            document.getElementById("div_ShowReviewTxtArea").style.display = "none";
            $("#txt_EditReviewText").val("");
            $("#lbl_errReview").html("");
        }
        function clear() {
            $("#txtEmployee").data('kendoDropDownList').value("")
            $("#txtReviewText").val("");
            $("#lblerrmsgEmp").html("");
            $("#lblerrmsgReview").html("");
        }
        function GetDataOnInsert(Buttonid) {
            var result = true;
            if ($("#txtEmployee").val() == "" || $("#txtEmployee").val() == undefined) {
                $("#lblerrmsgEmp").html("Please select employee.");
                result = false;
            }
            else if ($("#txtReviewText").val().trim() == "" || $("#txtReviewText").val() == undefined) {
                $("#lblerrmsgReview").html("Please enter Review Text.");
                $("#lblerrmsgEmp").html("");
                result = false;
            }
            else {
                $('[id$="hf_EmpId"]').val($("#txtEmployee").val());
                $('[id$="hf_Review"]').val($("#txtReviewText").val());
                clear();
                return true;
            }
            return result;
        }
    </script>
    <style type="text/css">
        div.k-windowAdd {
            left: 10% !important;
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <span style="font-size: Medium; font-weight: bold;">Employee Quick Review</span><br />
                                <span id="Span1" runat="server" style="float: right;" onclick="Show_AddReviewPopup();" class="small_button white_button open">Add Review</span>&nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    <div id="msgBox"></div>
                    <br />
                </div>
                <div id="Get_EmpQuickReviewGrid"></div>
            </div>
        </div>

    </div>
    <div id="divAddPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="div_AddReviewPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 400px; border-color: black; border-width: thin; min-height: 150px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);">
        <div>
            <div class="popup_head">
                <h3>Add Review</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeReviewPopup()" alt="Close" />
                <div class="clear">
                </div>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th class="required">Select Employee</th>
                    <td align="center">
                        <input id="txtEmployee" name="txt_Employee" style="width: 300px;" />
                        &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                        <span id="lblerrmsgEmp" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th class="required">Enter Review:</th>
                    <td align="center">
                        <textarea id="txtReviewText" rows="10" cols="200" style="width: 500px; height: 150px;"> </textarea>
                        &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                        <span id="lblerrmsgReview" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="btnSaveReview" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="Btn_SaveReview_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancelReviewAdd" onclick="closeReviewPopup();" />
                    </td>
                </tr>
            </table>
            <div>&nbsp;</div>
        </div>
    </div>
    
    <div class="k-widget k-windowAdd" id="div_EmpReviewDtlPopUp" style="display: none; padding-top: 10px; padding-right: 1px; min-width: 270px; 
min-height: 50px; top: 0%; left: 10%; z-index: 10003; opacity: 1; transform: scale(1); max-width: 1252px; width:80%; height: 890px; position: absolute;" data-role="draggable">
<%--width: 1261px; min-width: 400px; border-color: black; border-width: thin; min-height: 310px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);">--%>
        <div>
            <div class="popup_head">
                <h3>Employee wise Review List</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeEmpDetail()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <div>
                <b>Emp Code :
                    <label id="lblEmpCode"></label>
                </b>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <b>Emp Name :
                    <label id="lblEmpName"></label>
                </b>
                <br />
            </div>
            <div id="div_ShowReviewTxtArea" style="display: none;">
                <table>
                    <tr>
                        <th class="required">Edit Review here:</th>
                        <td align="center">
                            <textarea id="txt_EditReviewText" rows="10" cols="200" style="width: 500px; height: 150px;"> </textarea>
                            &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                            <span id="lbl_errReview" style="color: Red;"></span>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td>
                            <asp:Button ID="Update_Review" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return CheckOnUpdate(this.id);" OnClick="Btn_SaveReview_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <input type="button" class="small_button red_button open" value="Cancel" id="btn_CancelReviewEdit" onclick="close_ReviewTextArea();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="gridDetail_"></div>
        </div>
    </div>
    <asp:HiddenField ID="hf_EmpId" runat="server" />
    <asp:HiddenField ID="hf_Review" runat="server" />
    <asp:HiddenField ID="hdn_ReviewId" runat="server" />
</asp:Content>


