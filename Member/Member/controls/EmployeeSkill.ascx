<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EmployeeSkill.ascx.cs" Inherits="Member_controls_EmployeeSkill" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%--Bellow links are for ng-grid --%>
<link href="css/ng-grid.css" rel="stylesheet" />

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
<script src="js/1.3.7_angular.js"></script>
<script src="js/ng-grid.js"></script>
<script src="js/ng-grid-flexible-height.js"></script>

<script src="js/EmployeeSkill.js" type="text/javascript"></script>
<script type="text/javascript">

    function mailMessage() {
        alert("mail sent successfully");
    }
    function CheckRSkillOnInsert(Buttonid) {
        var skill = $('[id$="txtSkill"]').val();
        var errSkill = $("#lblerrmsgRSkill");

        if (skill == "") {
            errSkill.html("Please enter Skill.");
            $("#lblerrmsgRSkill").css('display', '');
            return false;
        }
    }
</script>

<style type="text/css">
    .grid-align {
        text-align: center;
    }

    .ViewData {
        background-image: url('images/zoom.png');
        min-width: 10px;
        width: 20px;
        height: 20px;
        background-size: 20px;
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

    /*for displaying text right aligned*/
    .k-grid .ra,
    .k-numerictextbox .k-input {
        text-align: right;
    }
    /*end*/

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

    /*ng-grid*/
    .gridStyle {
        border: 1px solid rgb(212,212,212);
        width: 770px;
        height: 150px;
    }

    .buttondel {
        background-image: url('images/delete.png');
    }

    /*input.ng-dirty.ng-invalid {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }*/

    /*input.ng-invalid-required {
            border: 1px solid red;
            box-shadow: 0 0 10px red;
        }*/

    input.ng-invalid-pattern {
        border: 1px solid red;
        box-shadow: 0 0 10px red;
    }
</style>

<div id="grdEmpSkill" ng-app="myApp" ng-controller="Employee_Skill">
    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">

        <tr>
            <th style="padding-right: 0px;">Name:  
            </th>
            <td>
                <div style="float: left; padding-top: 0px;">
                    <div style="float: left; padding-top: 5px;">
                        <select id="ddlEmployee" style="padding-top: 5px;" runat="server" name="ddlEmployee">
                        </select>
                        <label id="lblEmpName" runat="server" visible="false" style="font-weight: bold; padding-top: 5px;"></label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                    <div id="divImgSkill" visible="false" runat="server" style="float: left; padding-top: 5px;">
                        <img src="images/add_Skill.png" style="height: 15px;" />
                    </div>
                    &nbsp;
                            <asp:HyperLink ID="lnkRecommendSkill" runat="server" OnClick="ShowRSkillPopup()" Visible="false">Recommend Skill</asp:HyperLink>

                    &nbsp;&nbsp;&nbsp;
                           <input type="button" id="btnBindSkill" value="Show Skills" ng-click="BindSkill()" runat="server" class="small_button white_button" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     Enter Skill :
                    <asp:TextBox ID="txtSearchEmpSkill" runat="server" Text=""></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;
                   <input type="button" id="spnSearch" value="SEARCH" runat="server" ng-click="BindSkill()" class="small_button white_button" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="chkAll" type="checkbox" ng-click="BindSkill()" /><label><b>View All Skills</b></label>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="button" id="btnEmp" visible="false" value="Show Employees" ng-click="ShowEmployee()" runat="server" class="small_button white_button" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                            
                    <input type="button" value="Save" ng-click="SaveEmployeeSkill()" runat="server" class="small_button white_button" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <div>
                    <div id="divGSkill" runat="server">
                        <div id="ngGridSkill" class="gridStyle" class="ag-fresh" style="height: 510px; width: 800px; display: none; overflow-y: auto; overflow-x: hidden" ng-grid="gridEmp_Skill">
                        </div>
                    </div>
                    <div id="divGEmp" runat="server" visible="false">
                        <div id="ngGridEmp" class="gridStyle" style="height: 510px; width: 800px; overflow-y: auto; overflow-x: hidden" ng-grid="gridEmp">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnLoginID" runat="server" Value="0" />
</div>
<div id="divAddPopupOverlay" runat="server"></div>
<div class="k-widget k-windowAdd" id="divRSkill" style="display: none; background-color: white; padding-top: 10px; padding-right: 10px; min-width: 335px; border-color: black; border-width: thin; min-height: 133px; top: 10%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1);" data-role="draggable">
    <div>
        <div class="popup_head">
            <h3>Recommend Skill</h3>
            <img src="Images/delete_ic.png" class="close-button" onclick="closeRSkillPopUP()"
                alt="Close" />
            <div class="clear">
            </div>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr>
                <th>Skill Name:</th>
                <td align="center">
                    <input id="txtSkill" runat="server" name="txtSkill" style="width: 200px;" validationmessage="Please enter skill name" class="k-textbox" />
                    &nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                    <span id="lblerrmsgRSkill" style="color: Red;"></span>
                </td>
            </tr>
            <tr>
                <th>Note:</th>
                <td align="center">
                    <textarea id="txtSkillNote" runat="server" name="txtSkillNote" style="width: 200px; height: 80px;" class="k-textbox"></textarea>
                    <%--&nbsp;&nbsp;&nbsp;<span style="color: Red;">*</span>
                    <span id="lblerrmsgRSkill" style="color: Red;"></span>--%>
                </td>
            </tr>
            <tr>
                <th></th>
                <td>
                    <asp:Button ID="btnMailSkill" runat="server" Text="Send Mail" CssClass="small_button red_button open" OnClientClick="javascript:return CheckRSkillOnInsert(this.id);" OnClick="btnMailSkill_Click" />
                    &nbsp;&nbsp;&nbsp;<input type="button" class="small_button red_button open" value="Cancel" id="btnCancelRSkill" onclick="closeRSkillPopUP();" />
                </td>
            </tr>

        </table>
    </div>
</div>
