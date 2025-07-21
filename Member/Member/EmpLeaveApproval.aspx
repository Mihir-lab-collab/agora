<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Member/Admin.master" CodeFile="EmpLeaveApproval.aspx.cs" Inherits="EmpLeaveApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Member/js/kendo.web.min.js"></script>
   <link href="../Member/Kendu/styles/kendo.common.min.css" rel="stylesheet" />
    <link href="../Member/Kendu/styles/kendo.default.min.css" rel="stylesheet" />
    <script src="../Member/js/kendo.all.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/console.js"></script>


    <%--Bellow links are for ng-grid --%>
    <%-- <link href="css/ng-grid.css" rel="stylesheet" />

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.0.2/angular.min.js"></script>
    <script src="js/1.3.7_angular.js"></script>
    <script src="js/ng-grid.js"></script>
    <script src="js/ng-grid-flexible-height.js"></script>--%>


    <script src="js/EmpLeaveApproval.js" type="text/javascript"></script>



    <style type="text/css">
        .btnHide {
            display: none;
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="temp"></div>

    <div class="content_wrap">
        <div class="gride_table">
            <div class="box_border">
                <div class="grid_head">
                    <asp:Label ID="lbl" Text="Leaves" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="float: right">
                        <table width="100%">
                            <tr>
                                <td align="right">
                                    <table>
                                        <tr>
                                            <td class="generate_btn" style="padding-right: 1px; /*float:left*/" >
                                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnShowList" class="small_button white_button open" Text="Generate Report" runat="server" OnClick="btnShowList_Click" />
                                                        <div class="k-widget k-windowAdd" id="divEmpPopup" runat="server" visible="false" style="padding-top: 10px; padding-right: 10px; min-width: 240px; min-height: 275px; top: 1%; left: 550px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

                                                            <div id="emp">

                                                                <div class="popup_head">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td colspan="2" align="center">
                                                                                <span id="span2" style="font-size: large; font-weight: 100">Select Employees</span>
                                                                                <asp:ImageButton ImageUrl="Images/delete_ic.png" ID="imgClose" runat="server" class="close-button" alt="Close" OnClick="imgClose_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <div class="clear">
                                                                    </div>
                                                                </div>

                                                                <table cellpadding="0" cellspacing="0" border="0"  style="width:100%" class="manage_form">
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <div id="divMsg" runat="server" visible="false" style="color:red;">please select employee</div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="200px">
                                                                            <input style="width: 25px;" type="checkbox" id="chkAll" />Select All                                                                            
                                                                            <asp:CheckBoxList ID="chkEmplist" runat="server"></asp:CheckBoxList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnOK" Width="80" class="small_button white_button open" Text="OK" runat="server" OnClick="btnOK_Click" />
                                                                            <asp:Button ID="btnClear" Width="80" class="small_button white_button open" Text="Clear" runat="server" OnClick="btnClear_Click" />
                                                                        </td>


                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnOK" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td style="/*display:block;*/">
                                                <input style="width: 25px;" type="checkbox" id="chkInclude" onclick="Search();" />Include Archive&nbsp;&nbsp;
                                            </td>
                                            <td style=" /*float:left*/" >Employee Name:
                                                <input type="text" id="txtName" style="width: 130px" />
                                            </td>
                                            <td>
                                                From date:
                                                <input type="text" id="txtFromDate" onkeypress="return false;" style="width: 125px" />
                                            </td>
                                            <td>
                                                To Date:
                                                <input type="text" id="txtFromTo" onkeypress="return false;" style="width: 125px" />
                                            </td>
                                            <td>
                                                <label>Leave Status: </label>
                                                <asp:DropDownList ID="ddlStatus" AppendDataBoundItems="true" runat="server" Width="100">
                                                    <asp:ListItem Value="0" Text="All" Selected="True" />
                                                    <asp:ListItem Value="a" Text="Approved" />
                                                    <asp:ListItem Value="r" Text="Rejected" />
                                                    <asp:ListItem Value="p" Text="Pending" />
                                                </asp:DropDownList>
                                             </td>
                                            <td>                                           
                                                <span id="spn" runat="server" onclick="Search();" class="small_button white_button open">Search</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="gridLeaves"></div>
            </div>
        </div>
    </div>

    <%--  <script id="command-template" type="text/x-kendo-template">
    # if(foo % 2 == 0) { #
        <a class="k-button k-grid-even">Even</a>
    # } else { #
        <a class="k-button k-grid-odd">Odd</a>
    # } #
</script>--%>

    <div id="divAddPopupOverlay" runat="server"></div>

    <div class="k-widget k-windowAdd" id="divAddPopup" style="display: none; padding-top: 10px; padding-right: 10px; min-width: 740px; min-height: 50px; top: 1%; left: 350px; z-index: 10003; opacity: 1; transform: scale(1); border: solid">

        <div id="Invoice_Form">

            <div class="popup_head">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <span id="span2" style="font-size: large; font-weight: 100">Leave Details</span>
                            <img src="Images/delete_ic.png" class="close-button" alt="Close" onclick="ClosePopUp()" />
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
            </div>

            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
                <tr>
                    <th style="vertical-align: middle;">Employee Name:   
                    </th>
                    <td width="200px">
                        <label id="lblName" title="" style="width: 250px" runat="server"></label>
                    </td>
                    <th>Leave Type:<br />
                        <br />
                        <br />
                        Leave Status:
                    </th>
                    <td>
                        <asp:DropDownList ID="ddlLeaveType" runat="server"></asp:DropDownList><br />
                        <br />
                        <asp:DropDownList ID="ddlleaveStatus" AppendDataBoundItems="true" runat="server" Width="100">
                            <asp:ListItem Value="a" Text="Approved" Selected="True" />
                            <asp:ListItem Value="r" Text="Rejected" />
                            <asp:ListItem Value="p" Text="Pending" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align: middle;">Employee ID: 
                    </th>
                    <td>
                        <label id="lblEmpID" title="" style="width: 250px" runat="server"></label>
                    </td>
                    <th>Admin Comments: 
                    </th>
                    <td>
                        <textarea id="txtAdminComment" runat="server" class="k-textbox" style="width: 100%; height: 80px; float: left;"></textarea>
                    </td>
                </tr>
                <tr>
                    <th style="vertical-align: middle;">Leave From:
                        <br />
                        <br />
                        <br />
                        Leave To: 
                    </th>
                    <td>
                        <input type="text" id="txtFDate" onkeypress="return false;" style="width: 140px" />
                        <br />
                        <br />
                        <input type="text" id="txtTDate" onkeypress="return false;" style="width: 140px" />
                    </td>
                    <th style="vertical-align: middle;">No Of Leaves Applied: 
                    </th>
                    <td>
                        <label id="lblleavesApplied" title="" style="width: auto" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <th>Reason of Leave:
                    </th>
                    <td>
                        <textarea id="txtReason" runat="server" class="k-textbox" disabled="disabled" style="width: 100%; height: 80px; float: left;"></textarea>
                    </td>
                    <th style="vertical-align: middle;">Leave Sanctioned By:
                    </th>
                    <td>
                        <label id="lblSanctionedBy" title="" style="width: auto" runat="server"></label>

                    </td>
                </tr>
                <tr>
                    <th>Leave Entry On:  
                    </th>
                    <td>
                        <label id="lblleaveEntry" title="" style="width: auto" runat="server"></label>
                    </td>
                    <th>Leave Sanctioned Date:
                    </th>
                    <td>
                        <label id="lblLeaveSanctionedDate" title="" style="width: auto" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="padding-left: 306px;">
                            <input type="button" id="btnSaveStatus" onclick="SaveStatus()" value="Submit" style="width: 100px; height: 30px;" />
                        </div>
                    </td>
                </tr>

            </table>
        </div>
    </div>


    <%-- <div id="grdEmpSkill" ng-app="myApp" ng-controller="Employee_Skill">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="manage_form">
            <tr>
                <td colspan="4">
                    <div id="divGSkill" runat="server">
                        <div id="ngGridSkill" class="gridStyle" style="height: 510px; width: 800px; display: none; overflow-y: auto; overflow-x: hidden" ng-grid="gridEmp_Skill">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>--%>

    <asp:HiddenField ID="hdnEmpID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnLocationID" runat="server" Value="0" />
    <asp:HiddenField ID="hdmEmpLeaveID" runat="server" />
    <asp:HiddenField ID="hdnLoginID" runat="server" />
     <asp:HiddenField ID="hdnHrProfileStatus" runat="server" />
    <asp:HiddenField ID="hdnTime" runat="server" />
</asp:Content>




