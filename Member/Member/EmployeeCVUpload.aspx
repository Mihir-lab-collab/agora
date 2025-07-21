<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="EmployeeCVUpload.aspx.cs" Inherits="Member_EmployeeCVUpload" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="https://cdn.kendostatic.com/2015.1.408/js/kendo.all.min.js" type="text/javascript"></script>




    <script src="js/EmployeeCV.js" type="text/javascript"></script>

    <script>




        function ValidateUpload() {

            if ($('#ctl00_ContentPlaceHolder1_FileAttachment').val() == "") {
                document.getElementById('dvfile').style.display = "block";
                document.getElementById('dvFormatMsg').style.display = "none";
                document.getElementById('dvMsg').style.display = "none";

                return false;
            }
            else {
                document.getElementById('dvfile').style.display = "none";
                var uploadControl = document.getElementById('<%= FileAttachment.ClientID %>');
                if (uploadControl.files[0].size > 2097152) {
                    document.getElementById('dvMsg').style.display = "block";
                    document.getElementById('dvFormatMsg').style.display = "none";
                    return false;
                }
                else {
                    document.getElementById('dvMsg').style.display = "none";

                }

                var fileExtension = ['doc', 'docx', 'pdf', 'PDF', 'DOC'];
                if ($.inArray($('#ctl00_ContentPlaceHolder1_FileAttachment').val().split('.').pop().toLowerCase(), fileExtension) == -1) {

                    document.getElementById('dvFormatMsg').style.display = "block";
                    document.getElementById('dvMsg').style.display = "none";
                    return false;

                }
                else {
                    document.getElementById('dvFormatMsg').style.display = "none";
                    <%=Page.ClientScript.GetPostBackEventReference(btnSave,"") %>
                    return true;
                }
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

        .PopupHeader {
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




        .checkbox {
            width: 20px !important;
        }

        .displayNone {
            display: none;
        }


        .Detailslbl {
            width: 300px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <br />
    <div class="k-widget k-windowAdd" id="divPopup" style="display: none; padding-top: 10px; padding-right: 50px; min-width: 760px; width: 765px; min-height: 300px; height: 400px; top: 30% !important; left: 40% !important; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">

        <div class="k-grid-header" style="height:20px; vertical-align:middle;padding:8px;width:730px;">
            <div>
                <table class="PopupHeader">
                    <tr>
                        <th style="width: 200px;text-align:left;">Address</th>
                        <th style="width: 160px;text-align:left;">Skill Matrix</th>
                        <th style="width: 150px;text-align:left;">Project Working On</th>
                        <th style="width: 150px;text-align:left;">Last Uploaded Date </th>
                        <th style="width: 110px;text-align:left;">Last Uploaded By </th>
                    </tr>
                </table>
            </div>
            <img src="Images/delete_ic.png" class="close-button" onclick="closeAdddPopUP()" alt="Close" />
            <div class="clear">
            </div>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" width="745px" class="manage_form">
            <tr>
                <td style="width: 135px;vertical-align: text-top">
                    <label id="lblAddress"></label>
                </td>
                <td style="width: 100px;vertical-align: text-top">
                    <label id="lblSkillMatrix"></label>
                </td>
                <td style="width: 100px;vertical-align: text-top">
                    <label id="lblProjectWorkingOn">

                    </label>
                    <%--  <div id="lblProjectWorkingOn"></div>--%>
                </td>
                <td style="width:95px;;vertical-align: text-top">
                    <label id="lblLastUpdatedate"></label>
                </td>
                <td style="width: 75px;;vertical-align: text-top">
                    <label id="lblLadtUpdateBy"></label>
                </td>
            </tr>
        </table>
    </div>

    <div id="GetApprGrid"></div>
    <div id="divAddPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 50px; min-width: 400px; width: 400px; min-height: 100px; height: 220px; top: 30% !important; left: 40% !important; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div class="popup_head">
            <h3>Upload CV</h3>
            <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()" alt="Close" />
            <div class="clear">
            </div>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr>
                <th>Employee Name:</th>
                <td>
                    <asp:Label runat="server" ID="lblempName"></asp:Label>
                </td>
            </tr>


            <tr>
                <th>Attachment:            </th>
                <%--<span style="color: Red;">*</span>--%>
                <td align="center">

                    <asp:FileUpload ID="FileAttachment" runat="server"></asp:FileUpload>
                    <br />

                    <div id="dvfile" style="color: red; width: 190px; padding: 3px; display: none;">
                        Please Select File to Upload.
                    </div>
                    <br />


                    <div id="dvMsg" style="color: red; width: 190px; padding: 3px; display: none;">
                        Maximum size allowed is 2 MB.
                    </div>

                    <div id="dvFormatMsg" style="color: red; width: 190px; padding: 3px; display: none;">
                        Upload Only Doc/Docx/PDF formats.
                    </div>
                    <asp:Label ID="lblUploadedName" Visible="false" runat="server"></asp:Label>
                </td>

            </tr>
            <tr>
                <td colspan="2">
                    <div align="center">
                        <asp:Button ID="btnSave" Width="70px" runat="server" Text="SAVE" CausesValidation="true" CssClass="small_button white_button" OnClick="btnSave_Click" OnClientClick="javascript:return ValidateUpload();" ClientIDMode="Static" UseSubmitBehavior="false" /><%--OnClientClick="javascript:return ValidateUpload();"--%>
                        <asp:Button ID="btnCancel" Width="70px" runat="server" Text="CANCEL" CausesValidation="true" CssClass="small_button white_button" OnClientClick="closeAddPopUP()" ClientIDMode="Static" UseSubmitBehavior="false" />

                    </div>
                </td>
            </tr>

        </table>

    </div>
    <asp:HiddenField ID="hdnempId" runat="server" />
    <asp:HiddenField ID="hdnDocName" runat="server" />
     <asp:HiddenField ID="hdnAppointmentname" runat="server" />
    <asp:HiddenField ID="hdnfile" runat="server" />

</asp:Content>
