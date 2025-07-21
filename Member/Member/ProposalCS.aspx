<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProposalCS.aspx.cs" Inherits="Member_ProposalCS" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/ProposalCS.js" type="text/javascript"></script>

    <script type="text/javascript">
        var GridProposalCSProjects = "#gridProposalCSPojects";

        function closePopUP() {
            $('#divPopUP').css('display', 'none');
            $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
        }
        function openPopUP() {
            $('#divPopUP').css('display', '');
            $('#divOverlay').addClass('k-overlay');
        }

        function GetDataOnInsert(Buttonid) {
            var ProjectCSTitle = $("#txtProjectCSTitle").val();
            var ProjectCSDesc = $("#txtProjectCSDesc").val().replace(new RegExp("\\n", "g"), "<br/>");
            var ProjectCSUrl = $("#txtProjectCSUrl").val();

            var ProjectCSTitleSpan = $("#lblerrmsgProjectCSTitle");
            var ProjectCSDescSpan = $("#lblerrmsgProjectCSDesc");
            var ProjectCSUrlSpan = $("#lblerrmsgProjectCSUrl");


            if (ProjectCSTitle == "") {
                ProjectTitleSpan.html("Please enter project title.");
                return false;
            }
            else {
                ProjectCSTitleSpan.html("");
            }
            if (ProjectCSDesc == "") {
                ProjectCSDescSpan.html("Please enter project description.");
                return false;
            }
            else {
                ProjectCSDescSpan.html("");
            }

            if (ProjectCSUrl == "") {
                ProjectCSUrlSpan.html("Please enter project Url.");
                return false;
            }
            else {
                ProjectCSUrlSpan.html("");
            }
            if (ProjectCSTitle != "" && ProjectCSDesc != "") {
                document.getElementById("<%=hfProjectCSTitle.ClientID%>").value = ProjectCSTitle;
                document.getElementById("<%=hfProjectCSDesc.ClientID%>").value = ProjectCSDesc;
                document.getElementById("<%=hfProjectCSUrl.ClientID%>").value = ProjectCSUrl;
                return true;
            }
        }



    </script>

     <style type="text/css">
        .k-edit-form-container {
            width: 100%;
        }

        #details-container {
            padding: 10px;
        }

            #details-container h2 {
                margin: 0;
            }

            #details-container em {
                color: #8c8c8c;
            }

            #details-container dt {
                margin: 0;
                display: inline;
            }

        .ForeColor {
            color: red;
        }
    </style>
    <style type="text/css">
        .k-textbox {
            width: 11.8em;
        }

        #tickets {
            /*width: 510px;
                    height: 323px;
                    margin: 30px auto;
                    padding: 10px 20px 20px 170px;
                    background: url('../../content/web/validator/ticketsOnline.png') transparent no-repeat 0 0;*/
        }

            #tickets h3 {
                font-weight: normal;
                font-size: 1.4em;
                border-bottom: 1px solid #ccc;
            }

            #tickets ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
            }

            #tickets li {
                margin: 10px 0 0 0;
            }

        label {
            display: inline-block;
            width: 90px;
            text-align: right;
        }

        .required {
            font-weight: bold;
        }

        .accept, .status {
            padding-left: 90px;
        }

        .valid {
            color: green;
        }

        .invalid {
            color: red;
        }

        span.k-tooltip {
            margin-left: 6px;
        }

        .note.error span {
            background: transparent url(../images/error.png) 0px 0px no-repeat;
        }

        .note.check span {
            background: transparent url(../images/check.png) 0px 0px no-repeat;
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
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }


        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0!important;
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
            margin: 0!important;
            white-space: pre-wrap;
            /*vertical-align: top!important;*/
        }
        /*for history grid [e]*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                                            <span id="spn" runat="server" onclick="ShowAddPopup()" class="small_button white_button open">Add Case Study</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridProposalCSPojects"></div>
            </div>
        </div>
    </div>


    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add CS Project</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

                <tr>
                    <th>Name
                    </th>
                    <td>
                        <input id="txtProjectCSTitle" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgProjectCSTitle" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th>Project Url
                    </th>
                    <td>
                        <input id="txtProjectCSUrl" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgProjectCSUrl" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th>Description
                    </th>
                    <td>
                        <textarea id="txtProjectCSDesc" style="width: 290px; height: 100px;" rows="5" cols="70" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgProjectCSDesc" style="color: Red;"></span>
                    </td>
                </tr>
                 <tr>
                    <th style="width: 20%;">Attachment
                    </th>
                    <td style="padding: 0;">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <asp:LinkButton ID="lnkSaveCSProject" runat="server" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveCSProject_Click"><span>Save</span></asp:LinkButton>
                        &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancel" onclick="closeAddPopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>


    <script type="text/x-kendo-template" id="popup-editor">
   <div id="details-container">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr id="trProject" class="manage_bg">
            </tr>
            <tr>
                <th>Project Title
                </th>
                <td>
                    <input id="txtEditProjectCSTitle" type="text" style="width: 300px;" class="k-textbox" />

                </td>
            </tr>
            <tr>
                <th>Project Url
                </th>
                <td>
                    <textarea id="txtEditProjectCSUrl" type="text" style="width: 300px;" class="k-textbox" ></textarea>

                </td>
            </tr>
          <tr>
                <th>Project Url
                </th>
                <td>
                    <textarea id="txtEditProjectCSDesc" style="width: 290px; height: 100px;" rows="5" cols="70" class="k-textbox"></textarea>

                </td>
            </tr>
           <tr>
                    <th style="width: 20%;">Attachment
                    </th>
                    <td style="padding: 0;">
                        <asp:FileUpload ID="FileEditUpload" runat="server" />
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

    <asp:HiddenField ID="hfProjectCSTitle" runat="server" />
    <asp:HiddenField ID="hfProjectCSDesc" runat="server" />
     <asp:HiddenField ID="hfProjectCSUrl" runat="server" />
</asp:Content>

