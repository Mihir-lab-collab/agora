<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProjectTeam.aspx.cs" Inherits="Member_ProjectTeam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://angular-ui.github.com/ng-grid/css/ng-grid.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-grid/2.0.11/ng-grid.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.6.0/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.4.4/underscore-min.js"></script>
    <script type="text/javascript" src="https://cdn.kendostatic.com/2012.2.710/js/cultures/kendo.culture.en-GB.min.js"></script>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    

    <%--<script src="../Member/js/Milestone.js" type="text/javascript"></script>--%>
    <script src="../Member/js/ProjectTeam.js" type="text/javascript"> </script>
    <style type="text/css">
         .displayNone { display: none; }

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

        .k-textbox {
            width: 11.8em;
        }

        #tickets {
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
            background: #eceaea;
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
            background: #eceaea;
            color: black;
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


        .buttondel {
            background-image: url('images/delete.png');
        }

        /*.buttoncopy{
            background-color:gray;
        }*/
        /*=========Popup css===========*/
        #popupbtn {
            padding: 10px;
            background: #267E8A;
            cursor: pointer;
            color: #FCFCFC;
            margin: 200px 0px 0px 200px;
        }


        .popup-overlay {
            width: 100%;
            height: 100%;
            position: fixed;
            display: none;
            background: rgba(0, 0, 0, .85);
            top: 0;
            left: 100%;
            opacity: 0.6;
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=0)";
            -webkit-transition: opacity .2s ease-out;
            -moz-transition: opacity .2s ease-out;
            -ms-transition: opacity .2s ease-out;
            -o-transition: opacity .2s ease-out;
            transition: opacity .2s ease-out;
            z-index: 9998;
        }

        .overlay .popup-overlay {
            left: 0;
            display: block;
            background: 10px solid rgba(0, 0, 0, .3);
        }

        .popup {
            position: fixed;
            top: 10%;
            left: 50%;
            z-index: 9999;
            display: none;
            padding: 10px;
            -webkit-transition: opacity .2s ease-out;
            -moz-transition: opacity .2s ease-out;
            -ms-transition: opacity .2s ease-out;
            -o-transition: opacity .2s ease-out;
            transition: opacity .2s ease-out;
        }

            .popup .popup-warp {
                background: #fff; /*opacity: 0; min-height: 150px; width: 500px; margin-left:-260px; */
                padding: 10px;
                position: relative;
                border: 1px solid #e9e9e9;
                border-radius: 20px;
                -webkit-border-radius: 20px;
                -moz-border-radius: 20px;
                behavior: url(pie/PIE.htc);
                border: 10px solid rgba(0, 0, 0, .3);
                -webkit-background-clip: padding-box; /* for Safari */
                background-clip: padding-box;
            }

            .popup.visible, .popup.transitioning {
                z-index: 9999;
                display: block;
            }

                .popup.visible .popup-warp {
                    opacity: 1;
                    -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
                    -webkit-transition: opacity .2s ease-out;
                    -moz-transition: opacity .2s ease-out;
                    -ms-transition: opacity .2s ease-out;
                    -o-transition: opacity .2s ease-out;
                    transition: opacity .2s ease-out;
                }

            .popup .popup-exit {
                background: url(../Member/images/close-icon.png) no-repeat top;
                height: 18px;
                width: 18px;
                overflow: hidden;
                text-indent: -9999px;
                position: absolute;
                right: 10px;
                cursor: pointer;
            }

                .popup .popup-exit:hover {
                    background-position: bottom;
                }

            .popup h2 {
                font-size: 16px;
                font-weight: 600;
                margin-bottom: 5px;
                color: #292929;
            }

            .popup p {
                font-weight: 400;
            }

        .importlist {
            padding: 15px 10px;
            overflow: auto;
        }

            .importlist li {
                float: left;
                padding: 10px 0px;
                min-width: 120px;
                text-align: center;
            }

                .importlist li > .radiodiv {
                    overflow: auto;
                    display: inline-block;
                    *display: inline;
                    zoom: 1;
                }

                    .importlist li > .radiodiv input {
                        margin: 2px 3px 0 0;
                        float: left;
                    }

                    .importlist li > .radiodiv label {
                        display: inline-block;
                        float: left;
                    }

        .popup .popup-warp > .note {
            font-size: 11px;
            margin-top: 20px;
        }

        .popup .popup-warp > .botbtndiv {
            text-align: center;
            margin: 20px 0 10px;
        }

            .popup .popup-warp > .botbtndiv .btn {
                margin: 0 5px;
            }

        input.ng-invalid-required {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        input.ng-invalid-pattern {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        .validdate {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }

        .k-textbox .k-icon {
            margin: -8px 0 0 -17px;
        }

        .center {
            text-align: center !important;
            margin: 0 auto;
            width: auto;
        }

        .text span.k-tooltip {
            display: table-caption;
            padding: 0 10px 0 0;
        }
    </style>
     <script type="text/javascript">
         function CheckInsert() {
             if ($('#divAddPopup').is(':visible')) {
                 if ($("#txtProjectTile").val() == '') {
                     alert('Please Select Project');
                     return false;
                 }
                 if ($("#txtEmployee").val() == '') {
                     alert('Please Select Employee');
                     return false;
                 }
                 if ($("#cboDiscount").val() == '' || $("#cboDiscount").val() == 'Select Discount') {
                     alert('Please Select Discount');
                     return false;
                 }
                 return true;
             }
         }
      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblproject" Text="Manage Projects" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                            </td>
                            <td align="right">
                            </td>
                            <td>
                                <div style="float: right;">
                                    <span id="spnproject" onclick="ShowAddPopup();" class="small_button white_button open">Add New Project</span>
                                    <span id="Spnpage1" style="font-size: small;">Show</span><input id="comboBox" /><span id="Spnpage2" style="font-size: small;">Records per Page</span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="gridProject"></div>
            </div>
        </div>
    </div>
    <div id="divAddPopupOverlay"></div>
    <div class="a_popbox" id="divAddPopup" style="display: none;">
        <div class="popup_wrap" style="width: 900px; top: -30%; left: 20%;">
            <img src="../Images/delete_ic.png" alt="Close" class="close-button" onclick="closeAddPopUP()" />
            <table width="100%">
                <tr>
                    <td colspan="2" align="center">
                        <span id="Span1" style="font-size: large; font-weight: 100">Add/Edit Project</span>
                    </td>
                </tr>
            </table>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlAddProfile" runat="server">
                                    <div id="example" class="k-content">
                                        <div id="tickets">

                                            <table class="manage_form" width="100%">
                                                <tr>
                                                    <th class="required">Project Title</th>
                                                    <td>
                                                        <input id="txtProjectTile" type="text" name="txtProjectTile" style="width: 300px;" required validationmessage="Please enter project Title"
                                                            class="k-textbox" />
                                                    </td>
                                                    <th class="required">Employee Name</th>
                                                    <td>
                                                        <input id="txtEmployee" name="txtEmployee" style="width: 300px" required validationmessage="Please select employee"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="required">Discount %</th>
                                                    <td>
                                                        <input id="cboDiscount" name="cboDiscount" type="text" style="width: 300px" required validationmessage="Please select Discount" class="k-textbox" />
                                                    </td>
                                                    <td>
                                                        <input id="chkIsActive" type="checkbox" runat="server" /><b>Isactive</b>
                                                    </td>
                                                </tr>
                                               <%-- <tr>
                                                    <th class="required">Customer</th>
                                                    <td>
                                                        <input id="txtCustomer" type="text" name="txtCustomer" style="width: 300px;" required validationmessage="Please Select customer" class="k-textbox" />
                                                    </td>
                                                    <th class="required">Start Date</th>
                                                    <td class="text">
                                                        <input id="txtStartDate" type="text" name="txtStartDate" style="width: 300px;" onkeyup="return false" onkeypress="return false"
                                                            required validationmessage="Please Select Start Date" class="k-textbox" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <th class="required">Project Manager </th>
                                                    <td>
                                                        <input id="txtProjectManager" type="text" name="txtProjectManager" style="width: 300px" required validationmessage="Please Select Project Manager" />
                                                    </td>
                                                    <th>Other EmailId</th>
                                                    <td valign="top">
                                                        <textarea id="txtotherEmailIds" rows="4" cols="40" name="txtotherEmailIds" style="width: 300px; resize: none;" class="k-textbox" onblur="checkEmail();"></textarea>
                                                    </td>
                                                </tr>
                                                <tr hidden>
                                                    <th class="required" hidden>Project Duration </th>
                                                    <td valign="top" hidden>
                                                        <input id="txtProjectDurationData" type="text" name="txtProjectDurationData" style="width: 100px;" validationmessage="Please enter project duration"
                                                            class="k-textbox" onkeyup="numericInput(this)" onkeypress="isNumberKeyOrDelete(event);" />
                                                        <input id="txtProjectDurationType" type="text" name="txtProjectDurationType" style="width: 150px;" validationmessage="Please enter project duration Type" />
                                                    </td>



                                                </tr>
                                                <tr>
                                                    <th>Development Team </th>
                                                    <td style="width: 300px;">
                                                        <input id="txtDevelopmentTeam" multiple="multiple" data-placeholder="Select Development team" name="txtDevelopmentTeam" />
                                                    </td>
                                                    <th></th>
                                                    <td>
                                                        <input id="chkInHouse" type="checkbox" runat="server" /><b>InHouse</b>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <input id="chkOnGoing" type="checkbox" runat="server" /><b>OnGoing</b>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <th class="required"></th>
                                                    <td>
                                                        <%--<input id="txtCustomer" type="text" name="txtCustomer" style="width: 300px;" required validationmessage="Please Select customer" class="k-textbox" />--%>
                                                   <%-- </td>
                                                    <th class="required">Report Date</th>
                                                    <td class="text">
                                                        <input id="txtReportDate" type="text" name="txtReportDate" style="width: 300px;" onkeyup="return false" onkeypress="return false"
                                                            required validationmessage="Please Select Report Date" class="k-textbox" />
                                                    </td>

                                                </tr>--%>
                                                <tr>
                                                    <td colspan="4" class="center">
                                                        <asp:Button ID="btnAddProjects" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClientClick="javascript:return CheckInsert() " Text="Save" OnClick="btnAddProjects_Click" />
                                                        <%--<asp:Button ID="btnAddMilstone" runat="server" CssClass="displayNone"  Text="Add Milestone" OnClick="btnAddMilstone_Click"/>--%>
                                                    </td>

                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
        </div>
    </div>
    <div id="divOverlay"></div>
    <asp:HiddenField ID="hdDiscount" runat="server" />
    <asp:HiddenField ID="hdempId" runat="server" />
    <asp:HiddenField ID="hdprojId" runat="server" />
    <asp:HiddenField ID="hdModifiedOn" runat="server" />
    <asp:HiddenField ID="hdInsertedOn" runat="server" />
    <asp:HiddenField ID="hdMode" runat="server" />
    <asp:HiddenField ID="hdMemberId" runat="server" />
    <asp:HiddenField ID="hdSessionProjId" runat="server" />
</asp:Content>

