<%@ Page Title="" Language="C#" MasterPageFile="~/Member/Admin.master" AutoEventWireup="true" CodeFile="ProfileModule.aspx.cs" Inherits="Member_ProfileModule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
    <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />


    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script src="../Member/js/ProfileModule.js" type="text/javascript"></script>

    <style type="text/css">
        .k-detail-cell .k-tabstrip .k-content {
            padding: 0.2em;
        }

        .employee-details ul {
            list-style: none;
            font-style: italic;
            margin: 15px;
            padding: 0;
        }

            .employee-details ul li {
                margin: 0;
                line-height: 1.7em;
            }

        .employee-details label {
            display: inline-block;
            width: 90px;
            padding-right: 10px;
            text-align: right;
            font-style: normal;
            font-weight: bold;
        }

        .a_popbox {
            position: fixed;
            top: 0.1%;
            right: 0.1%;
            bottom: 0.1%;
            left: 0.1%;
            padding: 25px;
            z-index: 10010;
            background: url(../images/white_trans.png);
        }

        .manage_form {
            border-collapse: collapse;
            border: 1px solid #e7e7e7;
            background: #fff; /*width: 364px;*/
        }

            .manage_form th {
                text-align: right;
                border-bottom: 1px solid #e7e7e7;
                border-right: 1px solid #e7e7e7;
                padding: 6px 10px;
                vertical-align: top;
            }

            .manage_form td {
                text-align: left;
                border-bottom: 1px solid #e7e7e7;
                border-right: 1px solid #e7e7e7;
                padding: 7px 4px !important;
            }

            .manage_form .th_head {
                padding: 0 5px 0 15px;
                font-weight: bold;
            }

            .manage_form h2 {
                font-size: 18px;
            }

            .manage_form .tab-heading {
                background-color: #f9f9f9;
            }

                .manage_form .tab-heading h2 {
                    font-size: 14px;
                    font-weight: bold;
                    color: #88a245;
                }

        .a_popbox {
            position: fixed;
            top: 0.1%;
            right: 0.1%;
            bottom: 0.1%;
            left: 0.1%;
            padding: 25px;
            z-index: 10010;
            background: url(../images/white_trans.png);
        }

        .shortstring {
              border: solid 2px blue;
                white-space: nowrap;
                text-overflow: ellipsis;
                width: 100px;
                display: block;
                overflow: hidden
            }

        .k-grid tbody .k-button, .k-ie8 .k-grid tbody button.k-button {
            min-width: 0px;
        }

        a.SaveButtonClass:link{ 
              width: 40px;
              margin-top: -45px;
            }

    </style>

    <script type="text/javascript">
        function GetDataOnInsert() {

            var name = $('#TxtProfileName').val();
            var errName = $("#lblerrname");
            if (name == "") {
                errName.html("Please Enter Name.");
                return false;
            }
            else {
                errName.html("");
                document.getElementById("<%=hdnname.ClientID%>").value = name;
                return true;
            }

        }

        function moveKRA() {
            var oSrc = document.getElementById('<%= lstKRA.ClientID %>');
            var oDest = document.getElementById('<%= lstAddKRA.ClientID %>');
            var counterset = false;

            for (var i = 0; i < oSrc.options.length; i++) {
                var contains = false;
                if (oSrc.options[i].selected) {
                    for (var j = 0, ceiling = oDest.options.length; j < ceiling; j++) {

                        if (oDest.options[j].value == oSrc.options[i].value) {
                            contains = true;
                            break;
                        }
                    }
                    if (contains) {
                        alert("This KRA is already added");
                        counterset = true;
                    }
                    else {
                        var option = document.createElement("option");
                        oDest.appendChild(option);
                        option.value = oSrc.options[i].value;
                        option.text = oSrc.options[i].text;
                        option.title = oSrc.options[i].title;

                            //if (i % 2 == 0)
                            //{
                            //    option.style = "";
                            //}
                            //else
                            //{
                            //    option.style = "";
                            //    //if (counterset == false) {
                            //    //    option.style = "color:#2e2e2e; background:#cbc8c8;";
                            //    //}
                            //    //else {
                            //    //    option.style = "color:#2e2e2e; background:#eceaea;";
                            //    //}
                            //}
                        
                        //StyleSheet = "color:#2e2e2e; background:#eceaea;";
                    }
                }
            }
        }

        function removeKRA() {
            var oSrc = document.getElementById('<%= lstKRA.ClientID %>');
            var oDest = document.getElementById('<%= lstAddKRA.ClientID %>');
            var count = oDest.options.length;
            for (var i = 0; i < count; i++) {
                if (oDest.options[i].selected == true) {
                    try {
                        oDest.remove(i, null);
                        i--;
                    }
                    catch (error) {

                        oDest.remove(i);
                        i--;
                    }
                }
            }
        }

        function GetTextElements() {
            
            var arr = new Array();
            var lbox = document.getElementById('<%= lstAddKRA.ClientID %>');
            document.getElementById("<%=hftxtKRANames.ClientID%>").value = "";
            for (var i = 0; i < lbox.length; i++) {

                document.getElementById("<%=hftxtKRANames.ClientID%>").value += document.getElementById("<%=hftxtKRANames.ClientID%>").value != "" ? ',' + lbox.options[i].value : lbox.options[i].value;

            }
            return true;

        }
    </script>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div>
        <div id="example">
            <div id="AddProfilePopUp" class="a_popbox" style="display: none;">

                <div class="popup_wrap" style="top: 10%; left: 50%; margin-top: 1%; height: 225px; width: 350px; margin-left: -225px;">
                    <%--<div class="popup_wrap" style="padding-top: 10px; width: 280px; top: 10%; left: 40%; margin-top: 1%; height: auto; max-height: 300px; min-height: auto; overflow: auto;">--%>

                    <img src="Images/delete_ic.png" class="close-button" onclick="closeAddPopUP()" alt="Close" />
                    <div>
                        <table width="100%">
                            <tr>
                                <td colspan="2" align="center">
                                    <span id="Span1" style="font-size: large; font-weight: 100">Add Profiles</span>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table class="manage_form">
                            <tr>
                                <td>Name</td>
                                <td>
                                    <input style="display: none;" id="TxtProfileID" type="text" value="" /><%--Timesheet--%>

                                    <input id="TxtProfileName" type="text" value="" style="width: 180px" placeholder="Enter your name" required /><%--required validationmessage="Please Enter Name"--%>
                                    <span style="color: Red;">*</span>
                                    <span id="lblerrname" style="color: Red;"></span>
                   
                                </td>
                            </tr>
                            <tr>
                                <td>Location</td>
                                <td>
                                    <input id="Locations" style="width: 190px" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Reporting Manager</td>
                                <td>
                                    <input id="reportingmanager" style="width: 190px" />
                                </td>
                            </tr>
                            <tr>
                                <td>IsAdmin</td>
                                <td>
                                    <input id="ChkIsAdmin" type="checkbox" />
                                </td>
                            </tr>
                        </table>
                        <table class="manage_form" style="padding-top: 10px;">
                            <tr>
                                <td>                                    
                                    <asp:Button ID="BtnAdd" runat="server" Text="Save" class="k-button k-button-icontext k-grid-add" OnClientClick="javascript:return GetDataOnInsert(this.id);" /><%--"if GetDataOnInsert() return true; else return false;"--%>
                                    <asp:Button ID="BtnUpdate" runat="server" Text="Update" class="k-button k-button-icontext k-grid-add" OnClientClick="javascript:return GetDataOnInsert(this.id);" />
                                </td>
                                <td>
                                     <asp:Button ID="BtnClose" runat="server" Text="Close" class="k-button k-button-icontext k-grid-close" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <a id="BtnAddProfile" class="k-button k-button-icontext k-grid-add" href="#"><span class="k-icon k-add"></span>Add Profile</a>
            <table>
                <tr>
                    <td></td>
                </tr>
            </table>
            <div id="gridProfile">
            </div>

            <div id="PorfileModulePopuUp" class="a_popbox" style="display: none;">

                <div class="popup_wrap" style="padding-top: 10px; width: 700px; top: 10%; left: 25%; margin-top: 1%; height: auto; max-height: 600px; min-height: auto; overflow: auto;">


                    <img src="Images/delete_ic.png" class="close-button" onclick="closePorfileModulePopuUp()" alt="Close" />

                    <table>
                       <tr>
                            <td>
                                <strong style="font-size: 14px;">Profile Name: </strong>
                                <label for="ProfileName" style="vertical-align: middle; font-size: 14px;"></label>
                                <br />
                            </td>
                        </tr>
                    </table>

                    <div style="padding-top: 15px;">
                        <div id="grid"></div>
                    </div>
                </div>
            </div>


        </div>
        <script type="text/x-kendo-template" id="template">
                <div class="tabstrip">
                    <div>
                        <div class="Timesheet"></div>
                    </div>
                    
                </div>

        </script>
        <script type="text/x-kendo-template" id="template1">
                <div class="tabstrip">
                    <ul>
                        <li class="k-state-active">
                           Timesheet Details 
                        </li>
                       <%-- <li>
                            Project Details
                        </li>--%>
                    </ul>
                    <div>
                        <div class="Timesheet1"></div>
                    </div>

                </div>

        </script>
    </div>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="a_popbox" id="divKRANames" style="display: none;">

        <div class="popup_wrap" style="width: 900px; top: -20%; height:300px; left: 20%;">
            <div class="popup_head">
                <h3 style="margin-left: 376px;">KRA Names</h3>
                <input type="hidden" id="testd" name="testd" />
                <img src="images/delete_ic.png" alt="Close" class="close-button" onclick="closePopUP()" />
                <div class="clear">
                </div>
            </div>

            <style>
                select   { max-width:350px; width:100%; }
                select option  { overflow:hidden; text-overflow:ellipsis; color:#2e2e2e;background:#cbc8c8 url('images/bullte.png')  5px center no-repeat; height:5px; padding: 0.4em 0.6em 0.4em 20px;background-size: 9px; }
                select option:nth-child(even) { background-color:#eceaea;}               
            </style>
            <table style="width:100%">
                <tr>
                    <td style="width:40%">
                        <asp:ListBox ID="lstKRA" runat="server" SelectionMode="Multiple" Height="200" Font-Size="Small"></asp:ListBox>
                    </td>
                    <td align="center" style="width:20%">
                        <input type="button" id="btnTadd" style="width: 70px; margin-left: 1px;" runat="server" value="Add" class="small_button red_button open" onclick="moveKRA();"/>
                        <br />
                        <br />
                        <input type="button" id="btnTremove" style="width: 70px;margin-left: -2px; padding-left: 11px;" runat="server" value="Remove" class="small_button red_button open" onclick="removeKRA();" />
                    </td>
                    <td style="width:40%">
                        <asp:ListBox ID="lstAddKRA" size="4" runat="server" SelectionMode="Multiple" Height="200" Font-Size="Small"></asp:ListBox>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td style="width: 300px">
                        <asp:LinkButton ID="lnkSaveKRA" runat="server" CssClass="small_button red_button open SaveButtonClass" 
                            OnClientClick="javascript:return GetTextElements();javascript:return GetDataOnInsert(this.id);"
                            OnClick="lnkSaveKRA_Click"><span>Save</span></asp:LinkButton>
                    </td>

                    <td style="width: 300px">
                        <input type="button" class="small_button red_button open" style="margin-left: -82px; padding: 0px; width: 75px; margin-top: -45px;" value="Cancel" id="Button1" onclick="closePopUP();" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <asp:HiddenField ID="hdnProfileID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnname" runat="server" />
    <asp:HiddenField ID="hftxtKRANames" runat="server" />
</asp:Content>

