<%@ Page Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="Project.aspx.cs" Inherits="Customer_Project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://angular-ui.github.com/ng-grid/css/ng-grid.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/ng-grid/2.0.11/ng-grid.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.6.0/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.4.4/underscore-min.js"></script>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src="../Member/js/Milestone.js" type="text/javascript"></script>
    <script src="../Member/js/Project.js" type="text/javascript"> </script>
    <style type="text/css">
        .displayNone {
            display: none;
        }

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
        }

        .k-grid-content > table > tbody > .k-alt {
            background: #eceaea;
            padding: 0 !important;
            margin: 0 !important;
            white-space: pre-wrap;
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
        }

        .buttondel {
            background-image: url('images/delete.png');
        }

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
                background: #fff;
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

        div.k-windowAdd.mailbox {
            max-width: 610px;
            left: 50% !important;
            margin-left: -305px !important;
            height: auto;
        }
    </style>


    <script type="text/javascript">
        var GridMileStone = "#gridMileStone";

        function isNumberKeyOrDelete(event) {
            var charCode = (evt.which) ? evt.which : evt.keyCode
            if (charCode > 31 && (charCode < 46 || charCode > 57 || charCode == 8))
                return false;
            return true;
        }

        function CheckInsert() {
            if ($('#divAddPopup').is(':visible')) {
                if ($("#txtCustomer").val() == '')
                    alert('Please Select Customer');
                if ($("#txtProjectManager").val() == '')
                    alert('Please Select Project Manager');
                if ($("#txtAccountMgr").val() == '')
                    alert('Please Select Account Manager');
                if ($("#txtBA").val() == '')
                    alert('Please Select Business Analyst');
                if ($("#txtProjectType").val() == '')
                    alert('Please Select Project Type');
                if ($("#txtProjectType").val() == '1') {
                    if ($("#txtInitialProjectCost").is(":visible"))
                    {
                           if ($('#txtInitialProjectCost').val() == '')
                           {
                               alert('Please Input Initial Project Cost' + $('#txtInitialProjectCost').val());
                               return false;
                           }
                           //else
                           //{
                           //    return true;
                           //}
                                           
                    }
                    //else
                    //{     
                    //  return true;            
                    //}
                }
                
                checkEmail();
            }

            document.getElementById("<%=hftxtProjectTile.ClientID%>").value = $("#txtProjectTile").val();
            document.getElementById("<%=hftxtCustomer.ClientID%>").value = $("#txtCustomer").val();
            document.getElementById("<%=hftxtProjectManager.ClientID%>").value = $("#txtProjectManager").val();
            document.getElementById("<%=hftxtAccountMgr.ClientID%>").value = $("#txtAccountMgr").val();
            document.getElementById("<%=hftxtBA.ClientID%>").value = $("#txtBA").val();
            document.getElementById("<%=hftxtAppraisalAuthority.ClientID%>").value = $("#txtAppraisalAuthority").data("kendoMultiSelect").value().toString();
            document.getElementById("<%=hftxtProjectDescription.ClientID%>").value = $("#txtProjectDescription").val();
            document.getElementById("<%=hftxtProjectCost.ClientID%>").value = $('#txtProjectCost').val();
            document.getElementById("<%=hftxtInitialProjectCost.ClientID%>").value = $('#txtInitialProjectCost').val();
            document.getElementById("<%=hftxtPaymentCurrency.ClientID%>").value = $('#txtPaymentCurrency').val();
            document.getElementById("<%=hftxtProjectType.ClientID%>").value = $('#txtProjectType').val();
            document.getElementById("<%=hftxtExchangeRate.ClientID%>").value = $('#txtExchangeRate').val();
            document.getElementById("<%=hftxtDevelopmentTeam.ClientID%>").value = $("#txtDevelopmentTeam").data("kendoMultiSelect").value().toString();
            document.getElementById("<%=hfStartDate.ClientID%>").value = $("#txtStartDate").val();
            document.getElementById("<%=hdnReportDate.ClientID%>").value = $("#txtReportDate").val();
            document.getElementById("<%=hftxtActualCompletion.ClientID%>").value = $("#txtActualCompletion").val();
            document.getElementById("<%=hftxtotherEmailIds.ClientID%>").value = $("#txtotherEmailIds").val();
            document.getElementById("<%=hfmoduleids.ClientID%>").value = $("input[name=chkModuleID]:checked").map(function () { return this.value; }).get().join(",");
            document.getElementById("<%=hfReportDate.ClientID%>").value = $("#txtReportDate").val();
            document.getElementById("<%=hftxtStatus.ClientID%>").value = $("#txtStatus").data("kendoMultiSelect").value().toString();
            return true;
        }

        function clearPopup() {
            $('.popup.visible').addClass('transitioning').removeClass('visible');
            $('html').removeClass('overlay');

            setTimeout(function () {
                $('.popup').removeClass('transitioning');
            }, 200);
        }
        //Added by trupti
       
            function ChangeProjectType() {
                var pType = $('#txtProjectType').val();
                if (pType == '1') {
                    $('#txtInitialProjectCost').show();
                    $('#lblProjectCost').show();
                    

                }
                else {
                    $('#txtInitialProjectCost').hide();
                    $('#lblProjectCost').hide();
                    
                }


            }
       
        function checkEmail() {

            var email = $('[id$="txtotherEmailIds"]').val();
            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            var mailSpan = $("#lblmsgMailId");

            if (email != "") {

                var getEmail = email.split(',')

                for (var a in getEmail) {
                    var variable = getEmail[a]
                    if (!filter.test(variable)) {
                        alert('Please enter a valid email address');
                        mailSpan.html("*");
                        return false;
                    }
                    else {
                        mailSpan.html("");
                    }
                }
            }
        }



    </script>

    <script type="text/x-kendo-tmpl" id="myTemplateEdit">
    <div class="item click" data="${ModuleID}" style="width: 100%; margin: 0; padding: 0; overflow: hidden;">
        <input type="checkbox" name="chkModuleIDEdit" id="chk${ModuleID}" value="${ModuleID}" class="click" />
        <span class="checkbox">#:Menu#</span>
    </div>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                <span style="font-size: small;">Status</span>
                                <input id="txtStatus" multiple="multiple" name="txtStatus" style="width: 300px;" class="k-textbox" />
                                &nbsp;&nbsp;&nbsp;
                                        <input type="button" value="Search" runat="server" class="small_button white_button" onclick="SearchProjectByStatus();" />
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
        <div class="popup_wrap" style="width: 900px; top: -30%; left: 20%;height:480px;overflow:auto">
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
                                            <th class="required">Project Title </th>
                                            <td>
                                                <input id="txtProjectTile" type="text" name="txtProjectTile" style="width: 300px;"
                                                    required validationmessage="Please enter project Title"
                                                    class="k-textbox" />
                                            </td>
                                            <th class="required">Payment Currency</th>
                                            <td>
                                                <input id="txtPaymentCurrency" name="txtPaymentCurrency" style="width: 300px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Project Description</th>
                                            <td>
                                                <textarea id="txtProjectDescription" rows="4" cols="40" name="txtProjectDescription" style="width: 300px; resize: none;" class="k-textbox"></textarea>
                                            </td>
                                            <th class="required" hidden>Project Cost </th>
                                            <td valign="top" hidden>
                                                <input id="txtProjectCost" name="txtProjectCost" onkeyup="numericInput(this)" validationmessage="Please enter project cost"
                                                    style="width: 300px" class="k-textbox" />
                                            </td>
                                            <th class="required">Exchange Rate (In Rupee) </th>
                                            <td>
                                                <input id="txtExchangeRate" value="1" onkeyup="numericInput(this)" name="txtExchangeRate" style="width: 300px" class="k-textbox" />
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <th class="required">Customer</th>
                                            <td>
                                                <input id="txtCustomer" type="text" name="txtCustomer" style="width: 300px;" required validationmessage="Please Select customer" class="k-textbox" />
                                            </td>
                                            <th>Appraisal Authority </th>
                                            <td style="width: 300px;">
                                                <input id="txtAppraisalAuthority" multiple="multiple" data-placeholder="Select Appraisal Authority" name="txtAppraisalAuthority" style="width: 300px" class="k-textbox"/>
                                            </td>


                                        </tr>
                                        <tr>
                                         <th class="required">Project Type</th>
                                            <td>
                                                <input id="txtProjectType" type="text" name="txtProjectType" style="width: 300px;" required validationmessage="Please Select Project Type" onchange="ChangeProjectType()" class="k-textbox" />
                                            </td>
                                            <th id="lblProjectCost" style="display:none;">
                                                Initial Project Cost</th>
                                            <td style="width: 300px;">
<%--                                                <input id="txtCustomer" type="text" name="txtCustomer" style="width: 300px;" required validationmessage="Please Select customer" class="k-textbox" />--%>
                                                <input id="txtInitialProjectCost" onkeyup="numericInput(this)" name="txtInitialProjectCost" style="width: 300px;display:none;"  class="k-textbox"  />
                                                <%--<input id="txtInitialProjectCost" multiple="multiple" data-placeholder="Select Appraisal Authority" name="txtAppraisalAuthority" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="required">Start Date</th>
                                            <td class="text">
                                                <input id="txtStartDate" type="text" name="txtStartDate" style="width: 300px;" onkeyup="return false" onkeypress="return false"
                                                    required validationmessage="Please Select Start Date" class="k-textbox" />
                                            </td>
                                            <th class="required">Report Date</th>
                                            <td class="text">
                                                <input id="txtReportDate" type="text" name="txtReportDate" style="width: 300px;" onkeyup="return false" onkeypress="return false"
                                                    required validationmessage="Please Select Report Date" class="k-textbox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th class="required">Project Manager </th>
                                            <td>
                                                <input id="txtProjectManager" type="text" name="txtProjectManager" style="width: 300px"
                                                    required validationmessage="Please Select Project Manager" class="k-textbox" />
                                            </td>
                                            <th class="required">Account Manager </th>
                                            <td>
                                                <input id="txtAccountMgr" type="text" name="txtAccountMgr" style="width: 300px" required validationmessage="Please Select Account Manager" />
                                            </td>
                                            

                                        </tr>
                                        <tr>
                                         <th class="required">Business Analyst</th>
                                            <td>
                                                <input id="txtBA" type="text" name="txtBA" style="width: 300px"
                                                    required validationmessage="Please Select Project Manager" class="k-textbox" />
                                               
                                            </td>
                                            </tr>
                                        <tr hidden>
                                            <th class="required" hidden>Project Duration </th>
                                            <td valign="top" hidden>
                                                <input id="txtProjectDurationData" type="text" name="txtProjectDurationData" style="width: 100px;" validationmessage="Please enter project duration"
                                                    class="k-textbox" onkeyup="numericInput(this)" onkeypress="isNumberKeyOrDelete(event);" />
                                                <input id="txtProjectDurationType" type="text" name="txtProjectDurationType" style="width: 150px;" validationmessage="Please enter project duration Type" />
                                            </td>
                                            <th></th>
                                            <td>
                                                <input id="chkInHouse" type="checkbox" runat="server" /><b>InHouse</b>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                               <%-- <input id="chkOnGoing" type="checkbox" runat="server" /><b>OnGoing</b>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Development Team </th>
                                            <td style="width: 300px;" valign="top">
                                                <input id="txtDevelopmentTeam" multiple="multiple" data-placeholder="Select Development team" name="txtDevelopmentTeam" />
                                            </td>
                                            <th>Other EmailId</th>
                                            <td valign="top">
                                                <textarea id="txtotherEmailIds" rows="4" cols="40" name="txtotherEmailIds" style="width: 300px; resize: none;" class="k-textbox" onblur="checkEmail();"></textarea>
                                            </td>

                                        </tr>

                                        <tr>
                                            <th>TimeSheet Email?</th>
                                            <td style="width: 300px;">
                                                <asp:CheckBox ID="chkTSEmail" runat="server" />
                                            </td>
                                            <th>Send Email</th>
                                            <td valign="top">
                                                <asp:CheckBox ID="chkEmail" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>IsTracked?</th>
                                            <td style="width: 300px;">
                                                <asp:CheckBox ID="chkIsTracked" runat="server" />
                                            </td>
                                            <th>IsOngoing?</th>
                                            <td valign="top">
                                                <asp:CheckBox ID="chkOnGoing" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" class="center">
                                                <asp:Button ID="btnAddProjects" runat="server" CssClass="small_button white_button open" CausesValidation="false" OnClientClick="javascript:return CheckInsert() " Text="Save" OnClick="btnAddProjects_Click" />
                                                <asp:Button ID="btnAddMilstone" runat="server" CssClass="displayNone" Text="Add Milestone" OnClick="btnAddMilstone_Click" />
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

    <div id="divMail" class="k-widget k-windowAdd  mailbox" style="display: none; padding: 10px 10px 20px; min-width: 300px; min-height: 50px; top: 50%; left: 486px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">
        <div class="popup_head">
            <table width="100%">
                <tr>
                    <td colspan="2" align="center">
                        <span id="span2" style="font-size: large; font-weight: 100">Project Welcome Mail</span>
                        <img src="Images/delete_ic.png" class="close-button" alt="Close" onclick="CloseMailBox()" />
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
        </div>
        <table id="tblInvMail1" align="center" height="100" border="0" cellpadding="3"
            cellspacing="3" runat="server">
            <tr>
                <td>To:
                </td>
                <td>
                    <asp:TextBox ID="txtTo" size="60" runat="server"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td>Cc:
                </td>
                <td>
                    <asp:TextBox ID="txtCc" size="60" runat="server" Text="accounts@intelegain.com"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Bcc:
                </td>
                <td>
                    <asp:TextBox ID="txtBcc" size="60" runat="server" Text=""></asp:TextBox>

                </td>
                <td></td>
            </tr>
            <tr>
                <td>Subject:
                </td>
                <td>
                    <asp:TextBox ID="txtSubject" size="60" runat="server"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td valign="top" colspan="3">
                    <%--<asp:TextBox ID="txtEmail" runat="server" Rows="15" Width="400" TextMode="MultiLine"></asp:TextBox>--%>
                    <textarea id="txtEmail" runat="server" style="width: 515px; height: 250px;" rows="15" cols="70" class="k-textbox"></textarea>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="custbutton" colspan="2">
                    <center>
                        <asp:Button ID="btnSendEmail" runat="server" CssClass="small_button white_button open" Text="Send Email" OnClientClick=" return sendmail();" />
                    </center>
                </td>
            </tr>

        </table>
    </div>

    <asp:HiddenField ID="hdnEmpId" ClientIDMode="Static" runat="server" />
    <script id="popup-editor" type="text/x-kendo-template">
    </script>
    <asp:HiddenField ID="hdnProjectId" runat="server" />
    <asp:HiddenField ID="hfCustID" runat="server" />
    <asp:HiddenField ID="hftxtProjectTile" runat="server" />
    <asp:HiddenField ID="hftxtCustomer" runat="server" />
    <asp:HiddenField ID="hftxtProjectManager" runat="server" />
    <asp:HiddenField ID="hftxtAccountMgr" runat="server" />
    <asp:HiddenField ID="hftxtBA" runat="server" />
    

    <asp:HiddenField ID="hftxtAppraisalAuthority" runat="server" />

    <asp:HiddenField ID="hftxtProjectDescription" runat="server" />
    <asp:HiddenField ID="hftxtActualCompletion" runat="server" />
    <asp:HiddenField ID="hftxtProjectCost" runat="server" />
    <asp:HiddenField ID="hfStartDate" runat="server" />
    <asp:HiddenField ID="hdnReportDate" runat="server" />
    <asp:HiddenField ID="hftxtPaymentCurrency" runat="server" />
    <asp:HiddenField ID="hftxtProjectType" runat="server" />
    <asp:HiddenField ID="hftxtExchangeRate" runat="server" />
    <asp:HiddenField ID="hftxtDevelopmentTeam" runat="server" />
    <asp:HiddenField ID="hftxtCodeReviewTeam" runat="server" />
    <asp:HiddenField ID="hftxtotherEmailIds" runat="server" />
    <asp:HiddenField ID="hftxtInitialProjectCost" runat="server" />
    <asp:HiddenField ID="hfmoduleids" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="hftxtProjectDurationData" runat="server" />
    <asp:HiddenField ID="hftxtProjectDurationType" runat="server" />
    <asp:HiddenField ID="hftxtStatus" runat="server" />
    <asp:HiddenField ID="hfReportDate" runat="server" />
    <asp:HiddenField ID="hdnSendMail" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnshowHidediv" ClientIDMode="Static" runat="server" />

</asp:Content>



