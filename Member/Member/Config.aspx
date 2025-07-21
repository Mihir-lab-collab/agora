<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Config.aspx.cs" Inherits="Member_Config" ValidateRequest="false" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Config.js" type="text/javascript"></script>
    <script type="text/javascript">
        var gridConfig = "#gridConfig";

        function GetDataOnInsert(Buttonid) {
            var configID = $("#txtConfigID").val();
            var category = $("#txtCategory").val();
            var name = $("#txtName").val();
            var comment = $("#txtComment").val();
            var configIDSpan = $("#lblerrconfigID");
            var categorySpan = $("#lblerrcategory");
            var nameSpan = $("#lblerrmsgName");
            var valueSpan = $("#lblerrmsgvalue");
            var commentSpan = $("#lblerrmsgcomment");


            if (configID == "") {
                configIDSpan.html("Please enter Config ID.");
                return false;
            }
            else {
                configIDSpan.html("");
            }
            if (name == "") {
                nameSpan.html("Please enter name.");
                return false;
            }
            else {
                nameSpan.html("");
            }

            if (name != "") {
                document.getElementById("<%=hfConfigID.ClientID%>").value = configID;
                document.getElementById("<%=hfCategory.ClientID%>").value = category;
                document.getElementById("<%=hfConfigName.ClientID%>").value = name;
                document.getElementById("<%=hfComment.ClientID%>").value = comment;

                return true;
            }
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on('click', '.DivShowEditor', function (e) { //  $('.DivShowEditor').click(function () {
                var NextDiv = $(this).next();
                var TxtObj = $(this).parent().find('textarea');
                var Text = TxtObj.val();
                TxtObj.kendoEditor();
                $(this).hide();
                NextDiv.show();
            });
            $(document).on('click', '.DivHideEditor', function (e) { //$('.DivHideEditor').click(function () {
                var PrevDiv = $(this).prev();
                var TxtObj = $(this).parent().find('textarea');
                var Text = TxtObj.val();
                var parentTable = TxtObj.parents('table:first');
                TxtObj.removeProp('data-role');
                TxtObj.removeProp('autocomplete');
                TxtObj.insertAfter($(this));
                TxtObj.show();
                // $('#temp').html(Text);
                // var HtmlVal = $('#temp').html();
                var ss = htmlDecode(Text);
                TxtObj.val(ss);
                parentTable.remove();
                $(this).hide();
                PrevDiv.show();
            });
        });
        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }
        function callEditor() {

            var value = $('[id$="txtValue"]');
            //     value.kendoEditor();
            var value1 = $('[id$="txtValue1"]');
            //     value1.kendoEditor();

            //if ($("#val").hasClass("k-editor-toolbar-wrap")) {
            //    $('[id$="txtValue"]').data('kendoEditor').wrapper.find("iframe").remove();
            //    $('[id$="txtValue"]').data('kendoEditor').destroy();
            //    var value = $('[id$="txtValue"]');
            //    value.kendoEditor();
            //}
            //else {
            //    var value = $('[id$="txtValue"]');
            //    value.kendoEditor();
            //}

        }
    </script>

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
   <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

   
    <div id="temp"></div>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup();callEditor();" class="small_button white_button open">Add New Config</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridConfig"></div>
            </div>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Config</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th>ID</th>
                    <td align="center">
                        <input id="txtConfigID" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrconfigID" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th>Category</th>
                    <td>
                        <input id="txtCategory" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrcategory" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Name
                    </th>
                    <td>
                        <input id="txtName" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgName" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Value
                    </th>
                    <td id="val">
                        <div style="width: 520px;">
                            <div class="DivShowEditor small_button red_button">HTML</div>
                            <div class="DivHideEditor small_button red_button" style="display: none;">HTML</div>
                            <textarea id="txtValue" runat="server" style="width: 500px; height: 100px;" rows="5" cols="70" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                            <span style="color: Red;"></span>
                            <span id="lblerrmsgvalue" style="color: Red;"></span>
                        </div>

                    </td>
                </tr>
                <tr>
                    <th>Value1
                    </th>
                    <td id="val1">
                        <div style="width: 520px;">
                            <div  class="DivShowEditor small_button red_button">HTML</div>
                            <div  class="DivHideEditor small_button red_button" style="display: none;">HTML</div>
                            <textarea id="txtValue1" runat="server" style="width: 500px; height: 100px;" rows="5" cols="70" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Comment
                    </th>
                    <td>
                        <textarea id="txtComment" style="width: 290px; height: 100px;" rows="5" cols="70" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgcomment" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveConfig" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveConfig_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="150%" class="manage_form">
            <tr id="trConfig" class="manage_bg">
            </tr>
            <tr>
                <th>ID
                </th>
                <td>
                    <input id="txtEditConfigID" type="text" style="width: 300px;" class="k-textbox" />

                </td>
            </tr>
            <tr>
                <th>Category
                </th>
                <td>
                    <input id="txtEditCategory" type="text" style="width: 300px;" class="k-textbox" />


                </td>
            </tr>

            <tr>
                <th>Name
                </th>
                <td>
                    <input id="txtEditName" type="text" style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Value
                </th>
                <td>
                        <div style="width: 520px;">
                            <div class="DivShowEditor small_button red_button">HTML</div>
                            <div class="DivHideEditor small_button red_button" style="display: none;">HTML</div>
                            <textarea id="txtEditValue" style="width: 500px; height: 100px;" rows="5" cols="70" class="k-textbox"></textarea>
                        </div>

                </td>
            </tr>
            <tr>
                <th>Value1
                </th>
                <td>
                        <div style="width: 520px;">
                            <div class="DivShowEditor small_button red_button">HTML</div>
                            <div class="DivHideEditor small_button red_button" style="display: none;">HTML</div>
                            <textarea id="txtEditValue1" style="width: 500px; height: 100px;" rows="5" cols="70" class="k-textbox"></textarea>
                        </div>
                </td>
            </tr>
            <tr>
                <th>Comment
                </th>
                <td>
                    <textarea id="txtEditComment" style="width: 290px; height: 100px;" rows="5" cols="70" class="k-textbox"></textarea>

                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <div id="tdUpdate"></div>
                </td>
            </tr>
        </table>

    </div>
    </script>
    <asp:HiddenField ID="hfConfigID" runat="server" />
    <asp:HiddenField ID="hfCategory" runat="server" />
    <asp:HiddenField ID="hfConfigName" runat="server" />
    <asp:HiddenField ID="hfValue" runat="server" />
    <asp:HiddenField ID="hfComment" runat="server" />
</asp:Content>

