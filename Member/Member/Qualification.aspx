<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Qualification.aspx.cs" Inherits="Member_Qualification" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/Qualification.js" type="text/javascript"></script>
     <script type="text/javascript">
         var gridQualification = "#gridQualification";

         function GetDataOnInsert(Buttonid) {
         
             var QualDesc = $("#txtQualDesc").val();
             var QualType = $("#drpQualType").val();

             var QualDescSpan = $("#lblerrmsgQualDesc");
             var QualTypeSpan = $("#lblmsgQualType");
       
             if (QualDesc == "") {
                 QualDescSpan.html("Please enter Qualification Description.");
                 return false;
             }
             else {
                 QualDescSpan.html("");
             }
             if (QualType == "") {
                 QualTypeSpan.html("Please select Type.");
                 return false;
             }
             else {
                 QualTypeSpan.html("");
             }

             if (QualDesc != "" && QualType != "") {
                 document.getElementById("<%=hdnQualDesp.ClientID%>").value = QualDesc;
                 document.getElementById("<%=hdnQualType.ClientID%>").value = QualType;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                                            <span id="spn" runat="server" onclick="ShowAddPopup();callEditor();" class="small_button white_button open">Add New Qualification</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridQualification"></div>
            </div>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 60px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Qualification</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

                 <tr>
                    <th>Description
                    </th>
                    <td>
                        <input id="txtQualDesc" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgQualDesc" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Type
                    </th>
                    <td>
                        <input id="drpQualType" style="width: 300px" onblur="return GetDataOnInsert(this.id);" />
                        <span style="color: Red;">*</span>
                        <span id="lblmsgQualType" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:Button ID="lnkSaveQualification" runat="server" Text="Save" CssClass="small_button red_button open" onblur="return GetDataOnInsert(this.empId);" OnClick="lnkSaveQualification_Click"/>
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <script type="text/x-kendo-template" id="popup-editor">
    <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trQual" class="manage_bg">
            </tr>
         
            <tr>
                <th>Description
                </th>
                <td>
                    <input id="txtEditQualDesp" type="text" data-bind="value:QualDesc"  required validationmessage="Please enter Description"  style="width: 300px;" class="k-textbox" />
                </td>
            </tr>

            <tr>
                <th>Type</th>
                <td><input  id="drpQualTypeedit" style="width: 300px"/></td>
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
     <asp:HiddenField ID="hdnQualID" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnQualDesp" runat="server" />
    <asp:HiddenField ID="hdnQualType" runat="server" />
</asp:Content>

