<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="Member_NewEmployee" MasterPageFile="~/Member/Admin.master" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.common.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.rtl.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.min.css">
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.dataviz.default.min.css" />
    <link rel="stylesheet" href="https://cdn.kendostatic.com/2015.2.624/styles/kendo.mobile.all.min.css" />

    <%--<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>--%>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js"></script>
    <script src="https://cdn.kendostatic.com/2015.2.624/js/kendo.all.min.js"></script>

    <script type="text/javascript" src="../js/console.js"></script>
    <script src="js/Employee.js" type="text/javascript"></script>


    <%--    <script src="https://cdn.kendostatic.com/2015.2.624/js/jszip.min.js" type="text/javascript"></script>
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" /> 
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/require.js/2.1.16/require.js' type="text/javascript"></script>
    <script src="js/Employee.js" type="text/javascript"></script>--%>

    <script type="text/javascript">

        $(document).ready(function () {
            $('[id$="spanJoiningDate"]').css('display', 'none');
            var regx = /^[a-zA-Z0-9]{15}$/;

            $('[id$="txtAccountNo"]').focusout(function () {

                if ($('.red').length != 0) {
                    $('.red').html('');
                }

                var aac = $('[id$="txtAccountNo"]').val();
                if (!aac.length == 0) {
                    if (!regx.test(aac)) {
                        $('[id$="txtAccountNo"]').after('<div class="red" style="color:red; font-size:12px;text-align:left;">Enter Valid Account Number.</div>');

                    }

                }

            });

            // for IFSCCode

            var regxI = /^[a-zA-Z0-9]{11}$/;

            $('[id$="txtIFSCCode"]').focusout(function () {

                if ($('.red').length != 0) {
                    $('.red').html('');
                }

                var Ifac = $('[id$="txtIFSCCode"]').val();
                if (!Ifac.length == 0) {
                    if (!regxI.test(Ifac)) {
                        $('[id$="txtIFSCCode"]').after('<div class="red" style="color:red; font-size:12px;text-align:left;">Enter Valid IFSCCode </div>');

                    }

                }

            });


            var regx1 = /^[a-zA-Z ]*$/;

            $('[id$="txtEmpName"]').focusout(function () {

                if ($('.red').length != 0) {
                    $('.red').html('');
                }

                var name = $('[id$="txtEmpName"]').val();
                if (!name.length == 0) {
                    if (!regx1.test(name)) {
                        $('[id$="txtEmpName"]').after('<br/><div class="red" style="color:red; font-size:12px;text-align:left;">Enter Valid Name.</div>');

                    }
                }

            });

            $('[id$="btnSave"]').click(function () {
                $('[id$="hfQualification"]').val($('[id$="lstempQual"]').data("kendoMultiSelect").value().toString());
                $('[id$="hfSecondarySkills"]').val($('[id$="lstSecondarySkill"]').data("kendoMultiSelect").value().toString());
            });

            $('[id$="txtJoiningDate"]').blur(function () {
                var JoiningDate = $('[id$="txtJoiningDate"]').data("kendoDatePicker");
                var LeavingDate = $('[id$="txtLeavingDate"]').data("kendoDatePicker");
                LeavingDate.min(new Date(JoiningDate.value()));
            });


            $('[id$="txtLeavingDate"]').blur(function () {

                if ($('[id$="txtLeavingDate"]').val() == '')
                    $('[id$="ddlEmpStatus"]').val('Active');
                else
                    $('[id$="ddlEmpStatus"]').val('InActive');
            });


            //$('[id$="txtProfileID"]').blur(function () {
            //    $('[id$="hfProfileID"]').val($('[id$="txtProfileID"]').val());
            //});
            $('[id$="ddlProfile"]').blur(function () {
                $('[id$="hfProfileID"]').val($('[id$="ddlProfile"]').val());
            });

        });


        function CheckExistsEmailid() {
            var EmpID = 0;
            if ($('[id$="btnSave"]').val() != 'SAVE')
                EmpID = $('[id$="hdnempId"]').val();

            if ($('#ctl00_ContentPlaceHolder1_txtEmail').val() != '') {
                var Email = $('#ctl00_ContentPlaceHolder1_txtEmail').val()
                $.ajax({
                    type: "POST",
                    url: "Employee.aspx/GetExistsClient",
                    contentType: "application/json;charset=utf-8",
                    data: "{'EmailID':'" + Email + "','EmpID':' " + EmpID + "'}",
                    cache: false,
                    async: false,
                    dataType: "json",
                    success: function (data) {
                        if (data.d !== "0") {
                            alert('Email id already exist.');
                            $('#ctl00_ContentPlaceHolder1_txtEmail').focus();
                            $('#ctl00_ContentPlaceHolder1_hfValidate').val('1');
                            $('#ctl00_ContentPlaceHolder1_txtEmail').val('');
                            return false;
                        }
                        else
                            return true;

                    },
                    error: function (data) {
                        //alert("The call to the server side failed." + data.responseText);
                    }
                });
            }
            else
                return false;
        }

        function CheckADUserExists() {
            var EmpID = 0;
            if ($('[id$="btnSave"]').val() != "SAVE")
                EmpID = $('[id$="hdnempId"]').val();

            var ADUser = $('#ctl00_ContentPlaceHolder1_txtADUserName').val();
            $.ajax({
                type: "POST",
                url: "Employee.aspx/GetExistsADUserName",
                contentType: "application/json;charset=utf-8",
                data: "{'ADUserName':'" + ADUser + "','EmpID':' " + EmpID + "'}",
                cache: false,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d !== "0") {
                        alert('AD User name already exist.');
                        $('#ctl00_ContentPlaceHolder1_txtADUserName').focus();
                        $('#ctl00_ContentPlaceHolder1_hfValidate').val('1');
                        $('#ctl00_ContentPlaceHolder1_txtADUserName').val('');
                        return false;
                    }
                    else {
                        return true;
                    }
                },
                error: function (data) {
                    //alert("The call to the server side failed." + data.responseText);
                    return false;
                }
            });
        }

        //Added by Trupti on 20 Jully 2018

        function ShowPopUpInActive() {


            $('#divAddPopup').css('display', '');
            $('#divAddPopupOverlay').addClass('k-overlay');
            $('#divdoInactiveUsers').css('display', '');
            $('#divdoInactiveUsers').addClass('k-overlay');

        }


        //

        function Validate() {

            if ($('.red').length != 0) {
                $('.red').html('');
            }

            //Added By Nikhil Shetye for setting hidden filed value on 27/11/2017
            $('[id$="hfQualification"]').val($('[id$="lstempQual"]').data("kendoMultiSelect").value().toString());
            $('[id$="hfSecondarySkills"]').val($('[id$="lstSecondarySkill"]').data("kendoMultiSelect").value().toString());
            //End Nikhil Shetye

            if ($('[id$="txtEmpName"]').val() == '') {
                $('[id$="txtEmpName"]').after('<span class="red" style="color: red">*</span>');

                return false;
            }
            else if ($('[id$="ddlLocation"]').val() == 0) {
                $('[id$="ddlLocation"]').after('<span  class="red" style="color: red">*</span>');

                return false;
            }
            else if ($('[id$="txtEmail"]').val() == 0) {
                $('[id$="txtEmail"]').after('<span class="red" style="color: red">*</span>');

                return false;
            }
            else if ($('[id$="txtJoiningDate"]').val() == 0) {
                // $('[id$="txtJoiningDate"]').after('<span class="red" style="color: red">*</span>');
                $('[id$="spanJoiningDate"]').css('display', 'inline-block').html("*");
                return false;
            }
            else if ($('input[name="ctl00$ContentPlaceHolder1$rbtnGender"]:checked').length <= 0) {
                $('[id$="rbtnGender"]').after('<span class="red" style="color: red">*</span>');
                return false;
            }

            else if ($('[id$="ctl00_ContentPlaceHolder1_dropempExpyears"]').val() == "-Select Year-") {
                $('[id$="ctl00_ContentPlaceHolder1_dropempExpyears"]').after('<span class="red" style="color: red">*</span>');
                // alert('Please select Gender');
                return false;
            }
            else if ($('[id$="ctl00_ContentPlaceHolder1_dropempExpmonths"]').val() == "-Select Month-") {
                $('[id$="ctl00_ContentPlaceHolder1_dropempExpmonths"]').after('<span class="red" style="color: red">*</span>');
                // alert('Please select Gender');
                return false;
            }
            else if ($('[id$="ctl00_ContentPlaceHolder1_empSkill"]').val() == 0) {
                $('[id$="ctl00_ContentPlaceHolder1_empSkill"]').after('<span class="red" style="color: red">*</span>');
                // alert('Please select Gender');
                return false;
            }
            else if ($('[id$="txtADUserName"]').val() == '') {
                if ($('#ctl00_ContentPlaceHolder1_txtEmail').val() != '') {
                    document.getElementById("ctl00_ContentPlaceHolder1_txtEmail").addEventListener("keypress", CheckExistsEmailid(), false);
                    if ($('#ctl00_ContentPlaceHolder1_hfValidate').val() == "0" || $('#ctl00_ContentPlaceHolder1_hfValidate').val() == "") {
                    <%=Page.ClientScript.GetPostBackEventReference(btnSave,"") %>
                        return true;
                    }
                    else {
                        $('#ctl00_ContentPlaceHolder1_hfValidate').val('0');
                        return false;
                    }
                }
                else if ($('#ctl00_ContentPlaceHolder1_txtEmail').val() == '') {
                    $('#ctl00_ContentPlaceHolder1_txtEmail').focus();
                    return false;
                }
                else {
                    <%=Page.ClientScript.GetPostBackEventReference(btnSave,"") %>
                    return true;
                }
        }
        else {
            document.getElementById("ctl00_ContentPlaceHolder1_txtADUserName").addEventListener("keypress", CheckADUserExists(), false);
                // if ($('#ctl00_ContentPlaceHolder1_hfValidate').val() == "0") {
            if ($('#ctl00_ContentPlaceHolder1_hfValidate').val() == "0" || $('#ctl00_ContentPlaceHolder1_hfValidate').val() == "") {
                    <%=Page.ClientScript.GetPostBackEventReference(btnSave,"") %>
                    return true;
                }
                else {
                    $('#ctl00_ContentPlaceHolder1_hfValidate').val('0');
                    return false;
                }
            }
        }
    </script>

    <style type="text/css">
        .joining_date {
            position: relative;
            width: 200px;
        }

        /*headers*/

        .k-grid tbody .k-button, .k-ie8 .k-grid tbody button.k-button {
            min-width: 40px;
            min-height: 20px;
        }

        .ViewNote, .ViewNote:hover {
            background-image: url('images/note3.png');
            min-width: 10px;
            width: 15px;
            height: 22px;
            background-size: 20px;
            background-repeat: no-repeat;
        }


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

        .DivShowEditor, .DivHideEditor {
            float: right;
            margin-right: 20px;
            cursor: pointer;
        }
        /*for history grid [e]*/
    </style>
    <style type="text/css">
        .red {
            color: red;
            text-align: center;
            font-size: medium;
        }

        .black {
            color: black;
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lblInventoryDetails" Text="Employee Master" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <asp:DropDownList ID="dlstLeavingDate" runat="server" AutoPostBack="true" Width="160" CssClass="c_dropdown">

                                        <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                                        <asp:ListItem Text="Current" Value="Current" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Left" Value="Left"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                                <td align="right">

                                    <input type="button" id="btnRefresh" value="Edit Employee" class="small_button white_button open" onclick="Refresh();" style="display: none" />

                                </td>
                                <td align="right">

                                    <%--<asp:Button ID="Button2" runat="server" Text="Generate Report" class="small_button white_button open" CommandArgument="show" OnClientClick="openReportPopup();" /--%>
                                    <input type="Button" id="Button2" value="Generate Report" class="small_button white_button open" onclick="openReportPopup();" />
                                </td>
                                <td align="right">

                                    <span id="Span3" runat="server" onclick="ShowAddPopup();" class="small_button white_button open">Add Employee</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clear"></div>

                </div>
                <div id="gridEmployee"></div>
            </div>
        </div>
    </div>


    
    
    
    <div id="divAddPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 370px; min-height: 50px; top: 8%; left: 250px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Employee</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()" alt="Close" />
                <div class="clear">
                </div>
            </div>

            <asp:HiddenField ID="hdfProfile" ClientIDMode="Static" runat="server" />

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th rowspan="2">Photo:</th>
                    <td align="center" rowspan="2">
                        <div id="PhotoAttch">
                            <img alt="" id="photoImage" src="" runat="server" />
                        </div>
                        <br />
                        <asp:FileUpload ID="filePhotoUpload" runat="server"></asp:FileUpload>
                    </td>
                    <th>Employee Status:</th>
                    <td align="center">
                        <asp:DropDownList runat="server" ID="ddlEmpStatus" Style="width: 200px" Visible="true">
                            <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                            <asp:ListItem Text="InActive" Value="InActive"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>


                <tr>
                    <th><span style="color: red">*</span> Joining Date:</th>
                    <td align="center">
                        <asp:TextBox ID="txtJoiningDate" runat="server" CssClass="k-textbox joining_date" onkeyup="dateInput(this)" Width="200px" onkeydown="return false;" onkeypress="dateInput(this)"></asp:TextBox>

                        <span class="red" id="spanJoiningDate" style="color: red">*</span>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtJoiningDate" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                    </td>

                </tr>
                <tr>
                    <th><span style="color: red">*</span> Employee Name:</th>
                    <td align="center">

                        <asp:TextBox runat="server" ID="txtEmpName" Width="200px" CssClass="k-textbox"></asp:TextBox>
                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtEmpName" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>--%>
                        <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please Enter Valid Name" ControlToValidate="txtEmpName" ValidationExpression="[a-zA-Z ]*$"></asp:RegularExpressionValidator>--%>
                        <input type="button" id="btnTimeSheet" value="TimeSheet" onclick="opentimesheet();" class="small_button white_button open" />
                    </td>

                    <th>Leaving Date:</th>
                    <td align="center">
                        <input id="txtLeavingDate" runat="server" name="txtLeavingDate" onkeyup="dateInput(this)" visible="true" onkeypress="dateInput(this)" style="width: 200px" class="k-textbox" />
                    </td>

                </tr>
                <!--LWD Start-->
                <tr>
                    <th></th>
                    <td align="center">                        
                    </td>
                    <th>Expected LWD:</th>
                    <td align="center">
                        <input id="txtExpectedLWD" runat="server" name="txtExpectedLWD" onkeyup="dateInput(this)" onkeypress="dateInput(this)" style="width: 200px" class="k-textbox" />
                        <%--<input id="txtExpectedLWD" runat="server" name="txtExpectedLWD" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 200px" class="k-textbox" />--%>
                    </td>                    
                </tr>
                
                <!--LWD End-->
                <tr>

                    <th><span style="color: red">*</span> Location:</th>
                    <td align="center">

                        <asp:DropDownList runat="server" ID="ddlLocation" Style="width: 200px" onchange="return FillProfileData();">
                            <%--OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true"--%>
                        </asp:DropDownList>

                        <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                            ControlToValidate="ddlLocation" InitialValue="0" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>


                    </td>

                    <th><span style="color: red">*</span> Gender:</th>
                    <td align="center">
                        <asp:RadioButtonList ID="rbtnGender" name="rbtGender" RepeatDirection="Horizontal" runat="server" ValidationGroup="validate" Style="float: left;">
                            <asp:ListItem Value="Male" Text="Male"></asp:ListItem>
                            <asp:ListItem Value="Female" Text="Female"></asp:ListItem>
                        </asp:RadioButtonList>
                        <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="rbtnGender" ErrorMessage="*" ForeColor="Red" Display="Dynamic" ValidationGroup="validate">
                        </asp:RequiredFieldValidator>--%>
                    </td>

                </tr>

                <tr>

                    <th><span style="color: red">*</span> Email Id:</th>
                    <td align="center">
                        <asp:TextBox runat="server" ID="txtEmail" CssClass="k-textbox" onkeypress="return isChar(event,this);" Width="200px"></asp:TextBox>
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtEmail" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>--%>
                        <asp:RegularExpressionValidator ID="validateEmail" runat="server" ErrorMessage="Invalid email." ValidationGroup="validate" ControlToValidate="txtEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" /><asp:Button ID="btnSendMail" Text="Send Mail" Style="float: right" runat="server" Visible="true" CssClass="small_button white_button open" OnClick="btnSendMail_Click" /><%----%>

                    </td>

                    <th><span style="color: red">*</span> Experience:</th>
                    <td align="center">
                        <div class="tdbox">

                            <asp:DropDownList ID="dropempExpyears" runat="server" Width="100px">
                                <%--AppendDataBoundItems="true">--%>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="dropempExpyears" InitialValue="-Select Year-" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>

                        </div>
                        <div class="tdbox">

                            <asp:DropDownList ID="dropempExpmonths" runat="server" Width="100px">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="dropempExpmonths" InitialValue="-Select Month-" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" runat="server"></asp:RequiredFieldValidator>
                        </div>
                        <div class="tdbox" style="text-align: center">
                            <asp:Label ID="lblPExp" runat="server" Text="Previous Experience"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="lblPreExp" runat="server"></asp:Label>

                        </div>

                        <div style="margin: 22px 0 0 8px; float: left;">

                            <asp:HiddenField ID="hndExperienceStore" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Permanent Address:</th>
                    <td align="center">
                        <asp:TextBox runat="server" ID="txtAddress" TextMode="MultiLine" Height="57px" Width="200px"></asp:TextBox>
                        <input type="button" id="btnEmpHistory" style="float: right" value="Employee History" onclick="openEmpHistory();" class="small_button white_button open" />
                        <div class="clear"></div>
                    </td>
                    <th>Current Address:</th>
                    <td align="center">
                        <asp:TextBox runat="server" ID="txtCAddress" TextMode="MultiLine" Height="57px" Width="200px"></asp:TextBox>
                        <input type="checkbox" id="chkCopyAdd" onclick="CopyAddress()" />same as permanent address
                    </td>

                </tr>
                <tr>
                    <th>Probation Period:</th>
                    <td align="center">
                        <div class="tdbox">
                            <asp:DropDownList runat="server" ID="empProbationPeriod" Width="200px">
                                <asp:ListItem Value="0">0</asp:ListItem>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </td>
                    <th><span style="color: red">*</span>Designation</th>
                    <td align="center">
                        <asp:DropDownList ID="empSkill" runat="server" Width="200px"></asp:DropDownList>
                        <%--  <asp:RequiredFieldValidator runat="server" ID="rfvempSkill" ControlToValidate="empSkill" ErrorMessage="*" ForeColor="Red" ValidationGroup="validate"></asp:RequiredFieldValidator>--%>
                        <%-- <asp:RequiredFieldValidator runat="server" ID="rfvempSkill"
                            ControlToValidate="empSkill" InitialValue="0" ValidationGroup="validate" ErrorMessage="*" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <th>Contact Number:</th>
                    <td align="center">
                        <asp:TextBox runat="server" ID="txtContact" MaxLength="10" CssClass="k-textbox" onkeypress="return isChar(event,this);" Width="200px"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtContact" ErrorMessage="*" ForeColor="Red" runat="server" ValidationGroup="validate"></asp:RequiredFieldValidator>--%>
                        <asp:RegularExpressionValidator ID="RegularExpresphone2" Display="Dynamic" ControlToValidate="txtContact" runat="server" ErrorMessage="Enter Valid Phone Number." SetFocusOnError="True" ValidationExpression="^\d{10}$"></asp:RegularExpressionValidator>
                    </td>

                    <th>Primary Skill:</th>
                    <td align="center">
                        <asp:DropDownList ID="ddlPrimarySkill" runat="server" CssClass="b_dropdown" Style="margin-bottom: 30px;" AppendDataBoundItems="true">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>Employee BirthDate:</th>
                    <td align="center">
                        <input id="txtBirthDate" runat="server" name="txtBirthDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 200px" class="k-textbox" />
                    </td>


                    <th>Secondary Skill</th>
                    <td align="center">
                        <input id="lstSecondarySkill" runat="server" multiple="multiple" data-placeholder="Select Secondary Skills" name="lstSecondarySkill" style="width: 200px" />
                        <asp:HiddenField ID="hfSecondarySkills" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Anniversary Date:</th>
                    <td align="center">
                        <input id="txtAnniversaryDate" runat="server" name="txtAnniversaryDate" onkeyup="dateInput(this)" onkeydown="return false;" onkeypress="dateInput(this)" style="width: 200px" class="k-textbox" />
                    </td>

                    <th>Qualification:</th>
                    <td align="center">
                        <input id="lstempQual" runat="server" multiple="multiple" data-placeholder="Select Qualification" name="lstempQual" style="width: 200px" />
                        <asp:HiddenField ID="hfQualification" runat="server" />
                    </td>
                </tr>
                <tr>
                    <th>Account No.:</th>
                    <td align="center">
                        <%-- <input id="txtAccountNo" maxlength="15" runat="server" type="text" style="width: 200px;" onkeypress="return isChar(event,this);" class="k-textbox" />--%>
                        <input id="txtAccountNo" maxlength="15" runat="server" type="text" style="width: 200px;" class="k-textbox" />
                        <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtAccountNo" runat="server" ErrorMessage="Enter Valid Account Number." SetFocusOnError="True" ValidationExpression="^0[0-9a-zA-Z]{15}$"></asp:RegularExpressionValidator>--%>
                    </td>
                    <th>Notes:</th>
                    <td align="center">
                        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="57px" Width="250px"></asp:TextBox>
                    </td>
                </tr>
              
                <tr id="trIFSCCode" runat="server">
                    <th>IFSC Code:</th>
                    <td align="center">

                       <input id="txtIFSCCode" maxlength="11" runat="server" type="text" class="k-textbox" style="width: 200px" />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtIFSCCode" runat="server" ErrorMessage="Enter Valid IFSCCode" SetFocusOnError="True" ValidationExpression="^0[0-9a-zA-Z]{15}$"></asp:RegularExpressionValidator>--%>

                    </td>
                    <th>MSTeam ID:</th>
                    <td align="center">
                        <asp:TextBox ID="txtMSTeamID" runat="server" Width="200px" class="k-textbox"></asp:TextBox>
                    </td>
                </tr>
              
                <tr>
                    <th>PAN:</th>
                    <td align="center">
                        <input id="txtPan" maxlength="15" runat="server" type="text" style="width: 200px;" class="k-textbox" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorpan" Display="Dynamic" ControlToValidate="txtPan" runat="server" ErrorMessage="Enter Valid PAN." SetFocusOnError="True" ValidationExpression="[A-Za-z]{5}\d{4}[A-Za-z]{1}"></asp:RegularExpressionValidator>
                    </td>
                    <th>UAN:</th>
                    <td align="center">
                        <input id="txtUan" maxlength="12" runat="server" type="text" style="width: 200px;" class="k-textbox" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorUan" Display="Dynamic" ControlToValidate="txtUan" runat="server" ErrorMessage="Enter Valid UAN." SetFocusOnError="True" ValidationExpression="^\d{12}$"></asp:RegularExpressionValidator>
                    </td>
                </tr>

                <tr>
                    <th>EPF Account No.:</th>
                    <td align="center">
                        <input id="txtEpfacno" maxlength="16" runat="server" type="text" style="width: 200px;" class="k-textbox" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEpfac" Display="Dynamic" ControlToValidate="txtEpfacno" runat="server" ErrorMessage="Enter Valid EPF Account No" SetFocusOnError="True" ValidationExpression="^[a-zA-Z0-9-/]*$"></asp:RegularExpressionValidator>

                    </td>
                    <th>Appointment Letter:</th>
                    <td align="center">
                        <asp:FileUpload ID="fileAppointment" runat="server"></asp:FileUpload><br />
                        <br />
                        <label id="Label1" runat="server">Upload Only PDF formats.</label>
                        <br />
                        <asp:Label ID="Appointment" Visible="true" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <th>Previous Employer:</th>
                    <td align="center">
                        <asp:TextBox ID="txtPrevEmployer" runat="server" TextMode="MultiLine" Height="56px" Width="200px"></asp:TextBox>
                    </td>
                    <th>Attachment:</th>
                    <td align="center">
                        <asp:FileUpload ID="FileAttachment" runat="server"></asp:FileUpload><br />
                        <br />
                        <label id="lblMessage" runat="server">Upload Only /Doc/Docx/PDF formats.</label>
                        <br />
                        <asp:Label ID="lblUploadedName" Visible="true" runat="server"></asp:Label>
                    </td>

                </tr>

                <tr id="trProfileID" runat="server">
                    <th>Profile:</th>
                    <td align="center">

                        <asp:DropDownList runat="server" ID="ddlProfile" Style="width: 200px">
                            <%--AutoPostBack="true"--%>
                        </asp:DropDownList>
                    </td>
                    <th>AD User Name:</th>
                    <td align="center">
                        <input id="txtADUserName" maxlength="100" runat="server" type="text" class="k-textbox" style="width: 200px" />
                    </td>
                </tr>
                  <tr>
                    <th>Remote:</th>
                     <td align="center" colspan="3">
                      <asp:CheckBox ID="chkRemote" runat="server" AutoPostBack="false" OnCheckedChanged="chkRemote_CheckedChanged" />
                      <asp:HiddenField ID="hdfRemoteValue" runat="server" Value="0" />
                     </td>
               </tr>

             <%-- <tr id="trIFSCCode" runat="server">
                    <th>IFSC Code:</th>
                    <td align="center">

                       <input id="txtIFSCCode" maxlength="11" runat="server" type="text" class="k-textbox" style="width: 200px" />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" Display="Dynamic" ControlToValidate="txtIFSCCode" runat="server" ErrorMessage="Enter Valid IFSCCode" SetFocusOnError="True" ValidationExpression="^0[0-9a-zA-Z]{15}$"></asp:RegularExpressionValidator>--%>

                  </td>
                    <th></th>
                    <td align="center">
                    </td>
                </tr>--%>

                <tr>
                    <td colspan="2">
                        <span style="color: red; float: left;">Fields marked with * are mandatory</span><span class="clear"></span>
                    </td>
                    <th></th>
                    <td colspan="2">
                        <div id="Div1" runat="server" style="text-align: right">
                            <asp:Button ID="btnSave" runat="server" Text="SAVE" CausesValidation="true" ValidationGroup="validate" CssClass="small_button white_button" OnClick="btnSave_Click" OnClientClick="return Validate();" ClientIDMode="Static" UseSubmitBehavior="false" />
                            <asp:Button runat="server" ID="btnCancel" Text="CANCEL" OnClientClick="closeAddPopUP();" CssClass="small_button white_button open" CausesValidation="false" />
                            <asp:HiddenField ID="hfProfileID" runat="server" />
                        </div>



                    </td>
                </tr>
            </table>
        </div>
    </div>


     <%--  Added by Trupti on 20 Jully 2018 k-windowAdd
                                                                                                                
         --%>
    <div class="k-widget" id="divdoInactiveUsers" style="display: none; padding-top: 20px; padding-right: 20px; min-width: 270px; min-height: 50px; top: 40%; left: 40%; z-index: 10003; opacity: 1; transform: scale(1); width:500px; height:200px;" data-role="draggable" >
        <div>
            <div class="popup_head">
                
                <img src="Images/delete_ic.png" class="close-button" runat="server" onclick="closedoInactiveUsers();" alt="Close" />
                <div class="clear">
                    <center><b>Active Project Information</b></center>
                    <br />
                    <table>
                         <tr>
                    <td colspan="2">
                        
                        <span>
                           <b>Employee Name:</b>
                            <asp:Label ID="lbl_EmpName" runat="server" Text=""></asp:Label><br />
                           <b> Current Assigned Projects:</b><asp:Label ID="lbl_ProjectName" runat="server" Text=""></asp:Label><br />
                           <b> Project Managers:</b>
                            <asp:Label ID="lbl_projectManager" runat="server" Text=""></asp:Label>
                            <br />
                            <br /><b>Employee will Remove from Assigned Project List.<br />
                                Still do you want to Proceed ?</b></span><span class="clear"></span>
                    </td>
                    </tr>
                        <tr>
                    <td colspan="2">
                        <div id="Div2" runat="server" style="text-align: center">

                            <asp:Button ID="btn_yes" runat="server" Text="YES" CausesValidation="true" ValidationGroup="validate" CssClass="small_button white_button" OnClick="btn_yes_click"  ClientIDMode="Static" UseSubmitBehavior="false" />
                            <asp:Button ID="btn_no" runat="server" Text="NO" CausesValidation="true" ValidationGroup="validate" CssClass="small_button white_button" OnClick="btn_No_click"  ClientIDMode="Static" UseSubmitBehavior="false" />
                            
                            <%--<asp:HiddenField ID="hfDevTeam" runat="server"--%>
                        </div>



                    </td>
                </tr>
                    </table>
                </div>
            </div>
            </div>
        </div>
    <%--  for tool tip --%>
    <script id="template" type="text/x-kendo-template">
    <div>
        <div>CTC / mnth: #:ctcText#</div>
        <div>Gross / mnth: #:GrossText#</div>        
    </div>
    </script>
    <div id="divReportPopupOverlay" runat="server"></div>
    <div class="k-widget k-windowAdd" id="divGenerateReport" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 300px; min-height: 45s0px; top: 8%; left: 600px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div>
            <div class="popup_head">
                <h3>Generate Employee Report</h3>
                <img src="Images/delete_ic.png" class="close-button" onclick="closeReportPopUP()"
                    alt="Close" />
                <div class="clear">
                </div>
            </div>
            <div>

                <div class="multiselect">
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="empid" title="ID" />Employee ID<br />
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="empName" title="Name" />Name<br />
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="empAddress" title="Address" />Address<br />
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="empContact" title="Contact" />Contact<br />
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="empJoiningDate" title="JoiningDate" />JoiningDate<br />
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="Type" title="Experience" />Experience<br />
                    <%--empExperince--%>
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" disabled="disabled" value="Designation" title="Designation" />Designation<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IType" title="IntelegainExperience" />Intelegain Experience<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empLeavingDate" title="LeavingDate" />LeavingDate<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empProbationPeriod" title="ProbationPeriod" />Probation Period<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empNotes" title="Notes" />Notes<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empEmail" title="Email" />Email ID<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empGender" title="Gender" />Gender<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empTester" title="Is Tester" />Tester<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empAccountNo" title="AccountNo" />AccountNo<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empBDate" title="BirthDate" />Birth Date<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empADate" title="AnniversaryDate" />Aniversary Date<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="empPrevEmployer" title="PreviousEmployer" />Previous Employer<br />
                    <input type="checkbox" name="option[]" class="chkbox" checked="checked" value="AnnualCTC" title="AnnualCTC" />AnnualCTC<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="CTC" title="CTC" />CTC<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="Gross" title="Gross" />Gross<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="Net" title="Net" />Net<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsSuperAdmin" title="IsSuperAdmin" />IsSuperAdmin<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsAccountAdmin" title="IsAccountAdmin" />IsAccountAdmin<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsPayrollAdmin" title="IsPayrollAdmin" />IsPayrollAdmin<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsPM" title="IsPM" />IsPM<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsTester" title="IsTester" />IsTester<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsProjectReport" title="IsProjectReport" />IsProjectReport<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsProjectStatus" title="IsProjectStatus" />IsProjectStatus<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsLeaveAdmin" title="IsLeaveAdmin" />IsLeaveAdmin<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="IsActive" title="IsActive" />IsActive<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="Resume" title="Resume" />Resume<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="PrimarySkillDesc" title="PrimarySkill" />Primary Skill<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="Qualification" title="Qualification" />Qualification<br />
                    <input type="checkbox" name="option[]" class="chkbox" value="SecSkills" title="Secondary Skills" />Secondary Skills<br />
                     <input type="checkbox" name="option[]" class="chkbox" value="IFSCCode" title="IFSC Code" />IFSC Code<br />
                </div>
            </div>
            <div>
                <%-- <asp:Button ID="btnAddClmns" OnClientClick="FillEmployeeData()" Text="View" runat="server" CssClass="small_button white_button open" CausesValidation="false" />--%>
                <input type="button" id="btnAddClmns" onclick="FillEmployeeData()" value="View" class="small_button white_button open" />

                <asp:Button ID="btnCancelCoumns" OnClientClick="ResetEmployeeData()" Text="Cancel" runat="server" CssClass="small_button white_button open" CausesValidation="false" />
            </div>
        </div>
    </div>

    <div class="k-widget k-windowAdd" id="divTimeSheet" style="display: none; padding-top: 0px; padding-right: 4px; overflow: hidden; min-width: 240px; width: 246px; min-height: 520px; top: 39%; left: 609px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div class="popup_head" style="padding-left: 68px;">
            <h3>Timesheet</h3>
            <img src="Images/delete_ic.png" class="close-button" onclick="CloseTimeSheet();"
                alt="Close" />
            <div class="clear">
            </div>
        </div>
        <div id="gridTimeSheet" style="width: 248px; height: 520px; overflow: auto"></div>
    </div>
    <div class="k-widget k-windowAdd" id="divEmpHistory" style="display: none; padding-top: 0px; padding-right: 0; overflow: hidden; /*min-width: 240px; */ width: 70.6%; min-height: 320px; top: 39%; left: 250px; border-color: black; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
        <div class="popup_head" style="padding-left: 68px;">
            <h3>Employee History</h3>
            <input type="button" id="btnSaveAddress" style="float: right; margin-right: 20px;" value="Save" onclick="SaveApprovedData();" class="small_button white_button open" />
            <img src="Images/delete_ic.png" class="close-button" onclick="CloseEmpHistory();"
                alt="Close" />
            <div class="clear">
            </div>
        </div>
        <div id="gridEmpHistory" style="width: 100%; height: 320px; overflow: auto"></div>
        
    </div>
    <script id="popup-editor" type="text/x-kendo-template">
    </script>
    <asp:HiddenField ID="hdnDocName" runat="server" />
     <asp:HiddenField ID="hdnAppointmentname" runat="server" />

    <asp:HiddenField ID="hdnempId" runat="server" />
    <asp:HiddenField ID="hdnPrevExp" runat="server" />
    <asp:HiddenField ID="hfValidate" runat="server" />


</asp:Content>
