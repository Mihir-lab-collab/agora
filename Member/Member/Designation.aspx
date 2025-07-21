<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="Designation.aspx.cs" Inherits="Member_Designation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/Designation.js"></script>
    
    <script type="text/javascript">
        var gridConfig = "#gridDesignation";

        function GetDataOnInsert(Buttonid) {

            var txt = $("#txtDesig").val();
            var configIDSpan = $("#lblerrconfigID");

            if (txt == "") {
                configIDSpan.html("Please enter Designation.");
                return false;
            }
            else {
                configIDSpan.html("");
            }

            if (txt != "") {
                document.getElementById("<%=hdnDesignation.ClientID%>").value = txt;
                
                 return true;
             }
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
    <div id="temp"></div>
    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblInventoryDetails" Text="Designation" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="spn" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add New</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                        </div>
                </div>
                <div id="gridDesignation"></div>
            </div>
        </div>
    </div>
   
    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Designation</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            
            <table cellpadding="0" cellspacing="0"  border="0" width="100%" class="manage_form">
                <tr>
                    <th>Designation</th>
                    <td align="center">
                        <input id="txtDesig" type="text" style="width: 300px;" onkeypress="return isChar(event,this);"  class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrconfigID" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveDesignation" runat="server" Text="Save" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveDesignation_Click" />
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
            <div>&nbsp;</div>
        </div>
    </div>
    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trConfig" class="manage_bg">
            </tr>
            
            <tr>
                <th>Designation
                </th>
                <td>
                    <input id="txtEditDesignation" type="text" style="width: 300px;" class="k-textbox" required validationmessage="Designation cannot be blank." />

                </td>
            </tr>
            <tr>
                <td>
                   <span id="lblError style="color: Red;"></span>

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

    <asp:HiddenField ID="hdnDesigID" runat="server" />
    <asp:HiddenField ID="hdnDesignation"  runat="server"/>
</asp:Content>
