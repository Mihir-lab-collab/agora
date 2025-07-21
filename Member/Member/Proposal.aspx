<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Proposal.aspx.cs" Inherits="Member_Proposal" EnableEventValidation="false" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <%--<script src="js/kendo.web.min.js"></script>
    <link href="../Proposals/css/kendo.common.min.css" rel="stylesheet" />
    <link href="../Proposals/css/kendo.default.min.css" rel="stylesheet" />
    <script src="../Proposals/js/kendo.all.min.js"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Proposals/js/ProjectList.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />


    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Proposal.js" type="text/javascript"></script>

    <script type="text/javascript">
        var GridProposalProjects = "#gridProposalPojects";

        function closePopUP() {
            $('#divPopUP').css('display', 'none');
            $('#divOverlay').removeClass("k-overlay").addClass("k-overlayDisplaynone");
        }
        function openPopUP() {
            $('#divPopUP').css('display', '');
            $('#divOverlay').addClass('k-overlay');
        }

        function GetDataOnInsert(Buttonid) {
            var ProjectTitle = $("#txtProjectTitle").val();
            var ProjectDesc = $("#txtProjectDesc").val().replace(new RegExp("\\n", "g"), "<br/>");
            var ClientMailId = $("#txtClientMailId").val();

            var ProjectTitleSpan = $("#lblerrmsgProjectTitle");
            var ProjectDescSpan = $("#lblerrmsgProjectDesc");
            var ClientMailIdSpan = $("#lblerrmsgClientMailId");

            if (ProjectTitle == "") {
                ProjectTitleSpan.html("Please enter project title.");
                return false;
            }
            else {
                ProjectTitleSpan.html("");
            }
            if (ProjectDesc == "") {
                ProjectDescSpan.html("Please enter project description.");
                return false;
            }
            else {
                ProjectDescSpan.html("");
            }
            if (ClientMailId == "") {
                ClientMailIdSpan.html("Please enter client mail-id.");
                return false;
            }
            else {
                ClientMailIdSpan.html("");
            }
            if (ProjectTitle != "" && ProjectDesc != "") {
                document.getElementById("<%=hfProjectTitle.ClientID%>").value = ProjectTitle;
                document.getElementById("<%=hfProjectDesc.ClientID%>").value = ProjectDesc;
                document.getElementById("<%=hfClientMailId.ClientID%>").value = ClientMailId;

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
                                            <span id="spn" runat="server" onclick="ShowAddPopup()" class="small_button white_button open">Add New Project</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridProposalPojects"></div>
            </div>
        </div>
    </div>


    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 90px; min-height: 50px; top: 10%; left: 456px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Add Project</h3>
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
                        <input id="txtProjectTitle" type="text" style="width: 300px;" onkeypress="return isChar(event,this);" onblur="return PasswordValidation();return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgProjectTitle" style="color: Red;"></span>
                    </td>
                </tr>
                <tr>
                    <th>Description
                    </th>
                    <td>
                        <textarea id="txtProjectDesc" style="width: 290px; height: 100px;" rows="5" cols="70" onblur="return GetDataOnInsert(this.id); " class="k-textbox"></textarea>
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgProjectDesc" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th>Email-id
                    </th>
                    <td>
                        <input id="txtClientMailId" type="text" style="width: 300px;" onblur="return GetDataOnInsert(this.id); " class="k-textbox" />
                        <span style="color: Red;">*</span>
                        <span id="lblerrmsgClientMailId" style="color: Red;"></span>
                    </td>
                </tr>

                <tr>
                    <th></th>
                    <td>
                        <asp:LinkButton ID="lnkSaveProject" runat="server" CssClass="small_button red_button open" OnClientClick="javascript:return GetDataOnInsert(this.id);" OnClick="lnkSaveProject_Click"><span>Save</span></asp:LinkButton>
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
                    <input id="txtEditProjectTitle" type="text" style="width: 300px;" class="k-textbox" />

                </td>
            </tr>
            <tr>
                <th>Project Description
                </th>
                <td>
                    <textarea id="txtEditProjectDesc" style="width: 290px; height: 100px;" rows="5" cols="70" class="k-textbox"></textarea>

                </td>
            </tr>

            <tr>
                <th>Client Email-id
                </th>
                <td>
                    <input id="txtEditClientMaildId" type="text" style="width: 300px;" class="k-textbox" />
                </td>
            </tr>
            <tr>
                <th>Status</th>
                <td>
                    <input id="drpStatusedit" style="width: 100px" /></td>
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
     <div id="window" style="text-align:center"></div>
    <script type="text/x-kendo-template" id="windowTemplate">        
    <button class="k-button" id="yesButton">Yes</button>
    <button class="k-button" id="noButton"> No</button>    
    </script>

    <asp:HiddenField ID="hfProjectTitle" runat="server" />
    <asp:HiddenField ID="hfProjectDesc" runat="server" />
    <asp:HiddenField ID="hfClientMailId" runat="server" />

</asp:Content>

